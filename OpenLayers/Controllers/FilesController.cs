using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpenLayers.Controllers
{
    public class FilesController : Controller
    {
        private readonly Utils.ShapeFile shapeFile;

        public FilesController()
        {
            shapeFile = new Utils.ShapeFile();
        }


        // GET: Files
        public ContentResult GetPerdefinedShapeFiles()
        {
            return Content(shapeFile.LoadPredefinedFiles());
        }

        [HttpPost]
        public ContentResult LoadPerdefinedShapeFile(string id = "")
        {
            return Content(shapeFile.LoadPredefinedFile(id));
        }
    }
}