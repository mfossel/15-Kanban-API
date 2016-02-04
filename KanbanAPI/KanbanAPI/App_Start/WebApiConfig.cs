using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using KanbanAPI.Models;
using System.Web.Http.Cors;

namespace KanbanAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var rules = new EnableCorsAttribute("*", "*", "*");

            config.EnableCors(rules);


            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //SET API TO RETURN JSON INSTEAD OF XML
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            CreateMaps();
        }
        private static void CreateMaps()
        {
            Mapper.CreateMap<Card, CardsModel>();
            Mapper.CreateMap<list, listModel>();
        }
    }
}
