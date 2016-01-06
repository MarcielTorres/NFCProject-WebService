using System;
using System.Web;

namespace NFCProject.Webservice
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            String domainname = Convert.ToString(HttpContext.Current.Request.Headers["Origin"]);

            if (!String.IsNullOrEmpty(domainname))
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", domainname);
            }
            
            String allowedmethods = "POST, PUT, DELETE, GET";
            String headers = Convert.ToString(HttpContext.Current.Request.Headers["Access-Control-Request-Headers"]);
            String accesscontrolmaxage = "1728000";
            String contenttypeforoptionsrequest = "application/json";


            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //These headers are handling the "pre-flight" OPTIONS call sent by the browser
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", allowedmethods);
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", headers);
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", accesscontrolmaxage);
                HttpContext.Current.Response.AddHeader("ContentType", contenttypeforoptionsrequest);
                HttpContext.Current.Response.End();
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private bool IsAllowedDomain(String Domain)
        {
            if (string.IsNullOrEmpty(Domain)) return false;
            string[] alloweddomains = { "http://192.168.0.70:8001", "http://localhost", "http://localhost:3229", "http://127.0.0.1" }; // you can place comma separated domains here.
            foreach (string alloweddomain in alloweddomains)
            {
                if (Domain.ToLower() == alloweddomain.ToLower())
                    return true;
            }
            return false;
        }
    }
}