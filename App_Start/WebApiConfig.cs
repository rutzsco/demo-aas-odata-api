using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;

using Demo.Api.Models;

namespace Demo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // New code:
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<PerformanceIndicator>("PerformanceIndicator");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());
        }
    }
}
