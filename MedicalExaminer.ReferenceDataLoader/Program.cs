using System;
using MedicalExaminer.ReferenceDataLoader.Loaders;

namespace MedicalExaminer.ReferenceDataLoader
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var locationsLoader = new LocationsLoader(args);
                locationsLoader.Load().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loading failed. Exception raised:");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}