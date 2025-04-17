using Microsoft.AspNetCore.Mvc;
using ML_WebAPI.Models;
using ML_WebAPI.Services;

namespace ML_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageClassificationController : ControllerBase
    {
        [HttpPost("formFile")]
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

            PredictionResult predictionResult = ImageClassification.Result(image);

            return Ok(predictionResult);
        }

        [HttpPost("byte")]
        public IActionResult Post(byte[] image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image data is required.");

            PredictionResult predictionResult = ImageClassification.Result(image);

            return Ok(predictionResult);
        }
    }
}
