using System;
using System.Collections.Generic;
using System.Threading;
using ARR.API.Controllers;
using ARR.API.Models;
using ARR.Data.Entities;
using ARR.IntegrationTests.API;
using Autofac;
using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace ARR.IntegrationTests
{
    public class BaseIntegrationTest : IDisposable
    {
        private readonly IDocumentStore _store;
        
        public BaseIntegrationTest()
        {
            _store = new DocumentStore
                {
                    Url = "https://aeo.ravenhq.com/databases/AppHarbor_48e97815-70ea-43bc-ac81-4229e1cc4454",
                    ApiKey = "3d9f210f-2fdc-4eb5-a350-5d4cc3a1e226"
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

        protected ReviewSession NewReviewSession()
        {
            var session = new ReviewSession();
            session.Title = "Session 1";
            session.Creator = "test@test.com";
            
            var requirements1 = new Requirement();
            requirements1.Content = "This is requirement 1";

            var requirements2 = new Requirement();
            requirements2.Content = "This is requirement 2";

            var requirements3 = new Requirement();
            requirements3.Content = "This is requirement 3";


            session.Requirements = new List<Requirement> { requirements1, requirements2, requirements3 };

            var question1 = new Question { Content = "This is question 1" };
            var question2 = new Question { Content = "This is question 2" };

            session.Questions = new List<Question> { question1, question2 };

            return session;
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