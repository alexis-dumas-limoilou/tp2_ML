namespace ML_WebAPI.Models
{
    public class PredictionResult
    {
        public string PredictedLabel { get; set; }
        public float Confidence {  get; set; } 
    }
}
