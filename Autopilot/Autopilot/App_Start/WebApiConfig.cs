using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Net.Http.Headers;

namespace Autopilot
{
   
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            ////globally registering Cors
            //var cors = new EnableCorsAttribute("http://localhost:4200", "*", "*");



            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            //var cors = new EnableCorsAttribute("*", "*", "*");


            // Web API configuration and services by diksha
            // config.EnableCors();




            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

      
    }
}
