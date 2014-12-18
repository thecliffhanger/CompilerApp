using WebServer.Helpers;
using EntityModel;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using EntityDAL;

namespace WebServer.Filters
{
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext context)
        {
            try
            {
                var value = new LogItem() { Id = Guid.NewGuid(), LogType = "Error", Source = "WebServer", LogText = context.Exception.StackTrace };
                using (var db = new LogDBContext())
                {
                    if (value != null)
                    {
                        value.Source = "Server";
                        value.Created = DateTime.Now;
                        db.LogItems.Add(value);
                        db.SaveChanges();                       
                    }
                }                
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry("WebServer", context.Exception.StackTrace);
                //EventLog.WriteEntry("WebServer", ex.Message);
            }
            throw new HttpResponseException(
            new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred!"),
                ReasonPhrase = "Please check Log server for more details."
            });
        }
    }

}