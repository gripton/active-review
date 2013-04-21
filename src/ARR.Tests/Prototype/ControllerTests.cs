using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARR.API.Controllers;
using ARR.Data.Entities;
using ARR.Prototype.API.Controllers;
using Autofac;
using NUnit.Framework;

using Raven.Client;
using Raven.Client.Document;

namespace ARR.Tests.Prototype
{
    public class ControllerTests
    {
        [Test]
        public void Get_Succeeds()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TestModule());

            var container = builder.Build();

            var manager = container.Resolve<IReviewSessionManager>();
            var sessions = manager.ListCreated();
        }

        [Test]
        public void Post_Succeeds()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TestModule());

            var container = builder.Build();

            var session = new ReviewSession();
            session.Name = "Session 1";

            var requirements1 = new Requirement();
            requirements1.Content = "This is requirement 1";
            requirements1.Created = DateTime.UtcNow;

            var requirements2 = new Requirement();
            requirements2.Content = "This is requirement 2";
            requirements2.Created = DateTime.UtcNow;

            var requirements3 = new Requirement();
            requirements3.Content = "This is requirement 3";
            requirements3.Created = DateTime.UtcNow;

            session.Requirements = new List<Requirement> { requirements1, requirements2, requirements3 };

            var question1 = new Question { Content = "This is question 1" };
            var question2 = new Question { Content = "This is question 2" };

            session.Questions = new List<Question> { question1, question2 };

            var manager = container.Resolve<IReviewSessionManager>();
            manager.Process(session);
        }

    }
}
