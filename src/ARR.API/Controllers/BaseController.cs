using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using ARR.Core.Authorization;
using ARR.ReviewSessionManagement.Exceptions;

namespace ARR.API.Controllers
{
    public class BaseController : ApiController
    {
        protected Dictionary<Type, HttpStatusCode> CodeMap
        {
            get
            {
                return new Dictionary<Type, HttpStatusCode>
                    {
                        {typeof (InvalidOperationException), HttpStatusCode.Forbidden},
                        {typeof (SessionNotFoundException), HttpStatusCode.NotFound},
                        {typeof (AuthorizationException), HttpStatusCode.Unauthorized},
                    };
            }
        }

        protected HttpResponseMessage GetResponse(string content = null)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(string.IsNullOrEmpty(content)? "" : content);

            return response;    
        }

        protected HttpResponseMessage GetResponse(Exception e)
        {
            var map = CodeMap;
            var exType = e.GetType();

            var response = Request.CreateErrorResponse(
                (map.ContainsKey(exType)) ? map[exType] : HttpStatusCode.InternalServerError, e);

            return response;
        }
    }
}