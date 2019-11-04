using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MLSection
{
    public class Reader
    {
        private const string _assets = @"../resources/vehicles";
        private string _assembly = typeof(Reader).Assembly.Location;
        private string _assetPath => Path.Combine(_assembly, _assets); 

        public async Task<IEnumerable<ImagedVehicle>> DownloadHttpImages(IEnumerable<Vehicle> vehicles)
        {
            var processedVehicles = new List<ImagedVehicle>();
            foreach (var vehicle in vehicles)
            {
                var processing = new ImagedVehicle();
                processing.Label = $"{vehicle.make}.{vehicle.model}.{vehicle.year}";
                using (var client = new WebClient())
                {
                    foreach (var image in vehicle.vehiclePhotos)
                        processing.Images.Add(client.DownloadData(new Uri(image)));
                }
                processedVehicles.Add(processing);
            }
            return processedVehicles;
        }



        public Task ReadDiskImages()
        {
            var list = new List<ImagedVehicle>();
            foreach (var vehicle in LoadInMemoryImagesFromDirectory())
            {
                list.Add(vehicle);
            }
            return Task.CompletedTask;
        }

        private IEnumerable<ImagedVehicle> LoadInMemoryImagesFromDirectory()
        {
            var Memory = new List<ImagedVehicle>();
            var types = Directory.GetDirectories(_assetPath);
            foreach (var vehicle in types)
            {
                var classification = new ImagedVehicle() { Label = new DirectoryInfo(vehicle).Name };
                Memory.Add(classification);
            }
            foreach (var classification in Memory)
            {
                var assets = Path.Combine(_assetPath, classification.Label);
                var files = Directory.GetFiles(assets, "*", searchOption: SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if ((Path.GetExtension(file) != ".jpg") && (Path.GetExtension(file) != ".png"))
                        continue;

                    var bytes = File.ReadAllBytes(file);
                    classification.Images.Add(bytes);
                }
                yield return classification;
            }
        }
    }
}
