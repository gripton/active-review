using System;
using System.Configuration;
using System.Threading;
using ARR.API.Controllers;
using ARR.API.Models;
using ARR.IntegrationTests.API;
using ARR.UnitTests;
using Autofac;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace ARR.IntegrationTests
{
    public class BaseIntegrationTest : BaseUnitTest, IDisposable
    {
        private readonly IDocumentStore _store;
        
        public BaseIntegrationTest()
        {
            _store = new DocumentStore
                {
                    Url = ConfigurationManager.AppSettings["RavenUrl"],
                    ApiKey = ConfigurationManager.AppSettings["RavenKey"]
                };

            _store.Initialize();

            IndexCreation.CreateIndexes(typeof(BaseIntegrationTest).Assembly, _store);    
        }

        protected IContainer Setup()
        {
            // Map AutoMapper
            AutoMapper.Mapper.AddProfile<IndexMappingProfile>();

            // Create the container builder.
            var builder = new ContainerBuilder();

            builder
                .RegisterInstance(_store)
                .As<IDocumentStore>();


            builder.RegisterModule(new TestApplicationModule());

            // Register the Web API controllers.
            builder
                .RegisterType<ReviewIndexController>()
                .AsSelf();

            // Register the Web API controllers.
            builder
                .RegisterType<ReviewSessionController>()
                .AsSelf();

            // Build the container.
            var container = builder.Build();

            return container;
        }

        

        public void Dispose()
        {
            // We will definately have to move the unit-like test to a unit test library
            while (_store.DatabaseCommands.GetStatistics().StaleIndexes.Length != 0)
            {
                Thread.Sleep(10);
            }

            _store.DatabaseCommands.DeleteByIndex("AllDocumentsById", new IndexQuery());
        }
    }

    public class AllDocumentsById : AbstractIndexCreationTask
    {
        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition
            {
                Name = "AllDocumentsById",
                Map = "from doc in docs let DocId = doc[\"@metadata\"][\"@id\"] select new {DocId};"
            };
        }
    }
}