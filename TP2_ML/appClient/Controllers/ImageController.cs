using Microsoft.AspNetCore.Mvc;
using appClient.Models;

namespace appClient.Controllers
{
    public class ImageController : Controller
    {
        // GET: ImageController/Create
        public ActionResult Create()
        {
            return View("UploadImage");
        }

        // POST: ImageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(IFormFile Image, string Title)
        {
            if (Image != null && Image.Length > 0)
            {
                var response = await SendToApi(Image, Title);

                if (response.IsSuccessStatusCode)
                {
                    // Traitement en cas de succès
                    return RedirectToAction("Create");
                }
                else
                {
                    // Gestion des erreurs
                    ViewBag.ErrorMessage = "Échec de l'envoi à l'API ML_WebAPI.";
                    return View("Error");
                }
            }

            ViewBag.ErrorMessage = "Aucune image détectée.";
            return View("Error");
        }


        public async Task<HttpResponseMessage> SendToApi(IFormFile image, string title)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://localhost:7278/ImageClassification/byte";

                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    var imageData = memoryStream.ToArray();

                    // Crée le contenu pour l'envoi
                    var content = new MultipartFormDataContent
                    {
                        { new ByteArrayContent(imageData), "Image", image.FileName }
                    };

                    // Envoi les données
                    var response = await httpClient.PostAsync(apiUrl, content);

                    return response;
                }
            }
        }
    }
}
