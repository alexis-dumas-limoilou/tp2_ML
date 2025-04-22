using Microsoft.AspNetCore.Mvc;
using appClient.Models;
using System.Reflection;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace appClient.Controllers
{
    public class ImageController : Controller
    {
        private readonly HttpClient _httpClient;

        public ImageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: ImageController/Create
        public ActionResult Create()
        {
            var model = new Image();
            if (TempData["Content"] != null)
            {
                string content = TempData["Content"].ToString();

                // Désérialiser les données si c'est du JSON
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                ViewBag.Type = data.predictedLabel; // Récupère l'étiquette prédite
                ViewBag.Confiance = data.confidence; // Récupère la confiance
            }

            return View(model);

        }

            // POST: ImageController/Create
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Image model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageData != null && model.ImageData.Length > 0)
                {
                    var response = await SendToApi(model.ImageData);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        TempData["content"] = content;

                        // Traitement en cas de succès
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        // Gestion des erreurs
                        ViewBag.ErrorMessage = "Échec de l'envoi à l'API ML_WebAPI.";
                        var errorViewModel = new ErrorViewModel
                        {
                            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                        };

                        return View("Error", errorViewModel);
                    }
                }
                return View("UploadImage");
            }

            ViewBag.ErrorMessage = "Aucune image détectée.";
            return View("Error", model);
        }
        public async Task<HttpResponseMessage> SendToApi(IFormFile image)
        {
                var apiUrl = "https://localhost:7278/ImageClassification/formFile";

                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    var imageData = memoryStream.ToArray();
                    
                    var byteArrayContent = new ByteArrayContent(imageData);
                    byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);

                    // Crée le contenu pour l'envoi
                    var content = new MultipartFormDataContent
                    {
                        { byteArrayContent, "imageFile", image.FileName }
                    };
                
                    // Envoi les données
                    var response = await _httpClient.PostAsync(apiUrl, content);

                    return response;
            }
        }
    }
}