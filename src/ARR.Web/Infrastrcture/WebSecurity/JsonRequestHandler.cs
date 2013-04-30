using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Reflection;
using System.Web.SessionState;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public abstract class JsonRequestHandler : IHttpHandler, IRequiresSessionState
    {

        public abstract void ProcessRequest(HttpContext context);

        protected void SetResponse(HttpContext context, string jsonResponse)
        {
            context.Response.Cache.SetExpires(DateTime.Now);
            context.Response.ContentType = "application/json";
            context.Response.Write(jsonResponse);
            context.Response.End();
        }

        protected void SetResponse(HttpContext context, 
			Dictionary<string, string> attributes)
        {
            SetResponse(context, GetJsonObject(attributes));
        }

        protected void SetResponse<T>(HttpContext context, T obj)
        {
            SetResponse(context, GetJsonObject<T>(obj));
        }

        protected T CreateObjectFromRequest<T>(HttpContext context) where T: new()
        {
            T ob = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string value = context.Request.Params[property.Name];
                if (value == null)
                {
                    continue;
                }

                if (property.PropertyType != typeof(string) && value == string.Empty)
                {
                    continue;
                }

                object convertedValue = Convert.ChangeType(value, property.PropertyType);
                if (convertedValue == null)
                {
                    continue;
                }

                property.SetValue(ob, convertedValue, null);
            }
            return ob;
        }

        protected string GetJsonObject(Dictionary<string, string> attributes)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            bool firstTime = true;
            foreach (string key in attributes.Keys)
            {
                if (!firstTime)
                {
                    jsonBuilder.Append(",");
                }

                string name = key;
                object value = attributes[key];
                jsonBuilder.Append("\"" + name + "\":" + value.ToString());
                firstTime = false;
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        protected string GetJsonCollection<T>(IEnumerable<T> collection)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");

            bool first = true;
            foreach (T item in collection)
            {
                if (!first)
                {
                    jsonBuilder.Append(",");
                }
                jsonBuilder.Append(GetJsonObject<T>(item));
                first = false;
            }

            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }

        protected string GetJsonObject<T>(T obj)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            bool firstTime = true;
            foreach (PropertyInfo property in properties)
            {
                if (!firstTime)
                {
                    jsonBuilder.Append(",");
                }

                string name = property.Name;
                object value = property.GetValue(obj, null);
                jsonBuilder.Append("\"" + name + "\":\"" + value.ToString() + "\"");
                firstTime = false;
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

