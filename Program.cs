// Simple Financial Tracker by Rodrigo Bianchini - 2022.
// Project made with .Net 6.0. Implicit using is enabeled.
// Project made with the help of Tesseract OCR engine and Google Sheets Api.
// You can check the Google Documentantion on this link: https://developers.google.com/sheets/api/quickstart/dotnet?hl=en

using Tesseract;

namespace FinacialHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            // The imagePath variable recieves the Path where your image file is.
            // The textFileLogPath variable is the Path where the txt log file with the data collected from imagePath will be.
            var imagePath = @"<your image path goes here>/sampleImage.jpeg";
            var textFileLogPath = @"<your thex file path goes here>\Text File";

            ReadImage readImage = new ReadImage(textFileLogPath, imagePath, new TesseractEngine(@"tessdata", "por", EngineMode.Default));
            readImage.ReadImageFromUser();

            SheetConnector.Connect();
            SheetConnector.CreateEntry(readImage.FilterText());     
       }
        
    }
}