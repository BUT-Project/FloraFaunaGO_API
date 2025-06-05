using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FloraFauna_GO_Dto.Response
{
    public class AnimalIdentificationResultDto
    {
        public List<Predictions> Predictions { get; set; }
    }

    public class Predictions
    {
        public string FilePath { get; set; }

        public Classifications Classifications { get; set; }

        public List<Detection> Detections { get; set; }

        public string Prediction { get; set; }
        public double PredictionScore { get; set; }

        public string PredictionSource { get; set; }

        public string ModelVersion { get; set; }
    }

    public class Classifications
    {
        public List<string> Classes { get; set; }
        public List<double> Scores { get; set; }
    }

    public class Detection
    {
        public string Category { get; set; }

        public string Label { get; set; }

        public double Confidence { get; set; }

        public List<double> BoundingBox { get; set; }
    }

}
