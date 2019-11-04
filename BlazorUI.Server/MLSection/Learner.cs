using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.ML.DataOperationsCatalog;


namespace MLSection
{
    public class Learner
    {
        public void Learn(IEnumerable<ImagedVehicle> vehicles)
        {
            MLContext mlContext = new MLContext(seed: "DealerOn Greenlight".GetHashCode());

            /// Data

            List<UnlearnedImage> dataSet = new List<UnlearnedImage>();
            foreach (var vehicle in vehicles)
            {
                foreach (var image in vehicle.Images)
                {
                    var imageToLearn = new UnlearnedImage() { Label = vehicle.Label, Image = image };
                    dataSet.Add(imageToLearn);
                }
            }
            IDataView fullImageSet = mlContext.Data.LoadFromEnumerable(dataSet);
            IDataView fullShuffledSet = mlContext.Data.ShuffleRows(fullImageSet);
            TrainTestData trainTestData = mlContext.Data.TrainTestSplit(fullShuffledSet, testFraction: 0.2);

            IDataView trainDataView = trainTestData.TrainSet;
            IDataView testDataView = trainTestData.TestSet;

            /// Pipeline

            var pipeline = mlContext.Transforms.Conversion
                .MapValueToKey(outputColumnName: "LabelAsKey",
                               inputColumnName: "Label",
                               keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                .Append(mlContext.Model.ImageClassification("ImagePath", "LabelAsKey",
                arch: ImageClassificationEstimator.Architecture.InceptionV3,
                epoch: 100,
                batchSize: 30,
                metricsCallback: (metrics) => Console.WriteLine(metrics)));


            /// Train
           
            ITransformer trainedModel = pipeline.Fit(trainDataView);

            /// Evaluate
            
            IDataView predictionsDataView = trainedModel.Transform(testDataset);

            var metrics = mlContext.MulticlassClassification.Evaluate(
                predictionsDataView, 
                labelColumnName: "LabelAsKey", 
                predictedLabelColumnName: "PredictedLabel");
        }
    }
}
