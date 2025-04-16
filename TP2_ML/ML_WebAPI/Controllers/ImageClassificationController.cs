using Microsoft.AspNetCore.Mvc;

namespace ML_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageClassificationController : ControllerBase
    {
        [HttpPost()]
        public IActionResult Post(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Image data is required.");

            var supportedTypes = new[] { "image/jpeg", "image/png", "image/bmp" };
            if (!supportedTypes.Contains(imageFile.ContentType.ToLower()))
                return BadRequest("Unsupported file format.");

            byte[] image;
            using (var memoryStream = new MemoryStream())
            {
                imageFile.CopyTo(memoryStream);
                image = memoryStream.ToArray();
            }

            var imageData = new MLImagesModel.ModelInput()
            {
                ImageSource = image
            };

            var result = MLImagesModel.Predict(imageData);

            return Ok(result.PredictedLabel);
        }
    }
}
