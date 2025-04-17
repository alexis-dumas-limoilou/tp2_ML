using ML_WebAPI.Models;

namespace ML_WebAPI.Services
{
    public static class ImageClassification
    {
        public static PredictionResult Result(byte[] image)
        {
            var imageData = new MLImagesModel.ModelInput()
            {
                ImageSource = image
            };

            var result = MLImagesModel.Predict(imageData);

            PredictionResult predictionResult = new PredictionResult
            {
                PredictedLabel = result.PredictedLabel,
                Confidence = result.Score.Max()
            };

            return predictionResult;
        }
    }
}
