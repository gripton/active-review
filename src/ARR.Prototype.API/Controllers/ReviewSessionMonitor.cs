using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;

namespace ARR.Prototype.API.Controllers
{
    public interface IReviewSessionMonitor
    {

    }
    public class ReviewSessionMonitor : IReviewSessionMonitor
    {
        public ReviewSessionMonitor(IDocumentStore store){
        

        }
    }
}