﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Our.Umbraco.GMaps.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.GMaps.UmbracoV10.Controllers
{
    public class MapTestController : UmbracoApiController
    {
        private readonly IContentService contentService;

        public MapTestController(IContentService contentService)
        {
            this.contentService = contentService;
        }

        public IActionResult CreateMapEntry()
        {
            double lat = -35.23989947459226;
            double lng = 149.149934680426;
            Map gmap = new()
            {
                Address = new Address
                {
                    Coordinates = new Location
                    {
                        Latitude = lat,
                        Longitude = lng
                    }
                },
                MapConfig = new MapConfig
                {
                    Zoom = 15,
                    CenterCoordinates = new Location
                    {
                        Latitude = lat,
                        Longitude = lng
                    }
                }
            };

            string json = JsonConvert.SerializeObject(gmap);

            //Hack to get zoom to an int. Probably bug that's a string in model.
            //If a string the map won't show up and there is an error saying that zoom is not an int.
            json = json.Replace("\"zoom\":\"15\"", "\"zoom\":15");

            var testContent = contentService.GetRootContent();
            foreach (var n in testContent)
            {
                n.SetValue("singleMap", json);
                contentService.SaveAndPublish(n);
            }

            return Ok();
        }
    }
}
