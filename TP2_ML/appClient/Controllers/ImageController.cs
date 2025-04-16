using Microsoft.AspNetCore.Mvc;
using appClient.Models;
namespace appClient.Controllers
{
    public class ImageController : Controller
    {
        // GET: ImageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IFormFile Image, string Title)
        {
            if (Image != null && Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Image.CopyTo(memoryStream);
                    var imageData = memoryStream.ToArray();

                    // Sauvegarder les données dans la base ou autre traitement
                    var model = new Image
                    {
                        ImageData = imageData
                    };

                    // Exemple : sauvegarder dans une base de données
                    // _dbContext.Images.Add(model);
                    // _dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
