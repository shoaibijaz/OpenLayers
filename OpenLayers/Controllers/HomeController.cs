using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenLayers.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Index2()
        {

            return View();
        }

        public ActionResult Leaf()
        {
            return View();
        }

        public ActionResult ReadShapeFile()
        {
            DotSpatial.Data.Shapefile sf = DotSpatial.Data.Shapefile.OpenFile(@"D:\Shoaib\BOUNDARY\plan 24-11-15.shp");

            var featuresList = new List<Feature>();

            foreach (DotSpatial.Data.IFeature item in sf.Features)
            {
                var type = item.FeatureType.ToString();

                if (item.FeatureType == DotSpatial.Topology.FeatureType.Polygon)
                {
                    var coordinates = new List<GeographicPosition>();
                    item.Coordinates.ToList().ForEach(c => coordinates.Add(new GeographicPosition(c.Y,c.X)));

                    var geometry = new Polygon(new List<LineString> { new LineString(coordinates) });

                    featuresList.Add(new Feature(geometry));
                }
                else if (item.FeatureType == DotSpatial.Topology.FeatureType.Point)
                {
                    var point = new GeographicPosition(item.Coordinates[0].Y, item.Coordinates[0].X);

                    var geometry = new Point(point);

                    featuresList.Add(new Feature(geometry));
                }
            }

            var featureCollection = new FeatureCollection(featuresList);

            var serializedData = JsonConvert.SerializeObject(featureCollection, Formatting.Indented);

            return Content(serializedData);
        }

    }
}