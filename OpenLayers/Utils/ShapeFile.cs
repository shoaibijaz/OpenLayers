using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OpenLayers.Utils
{
    public class ShapeFile
    {
        private string folderPath = HttpContext.Current.Server.MapPath("~/TempFiles");

        public string LoadPredefinedFiles()
        {
            var files = new List<string>();

            foreach (var file in Directory.GetFiles(folderPath, "*.shp"))
            {
                files.Add(Path.GetFileNameWithoutExtension(file));
            }

            return JsonConvert.SerializeObject(files);
        }

        public string LoadPredefinedFile(string file)
        {
            try
            {
                var path = Path.Combine(folderPath, file + ".shp");
                var response = "";

                if (File.Exists(path))
                {

                    DotSpatial.Data.Shapefile sf = DotSpatial.Data.Shapefile.OpenFile(path);

                    var featuresList = new List<Feature>();

                    foreach (DotSpatial.Data.IFeature item in sf.Features)
                    {
                        if (item.FeatureType == DotSpatial.Topology.FeatureType.Polygon)
                        {
                            var coordinates = new List<GeographicPosition>();
                            item.Coordinates.ToList().ForEach(c => coordinates.Add(new GeographicPosition(c.Y, c.X)));

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

                    response = JsonConvert.SerializeObject(featureCollection, Formatting.Indented);

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}