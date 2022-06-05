using System.Text.RegularExpressions;
using Tesseract;

namespace FinacialHelper
{
    internal class ReadImage
    {
        public string textFileLogPath;
        public string imagePath;
        private readonly TesseractEngine _engine;

        public ReadImage(string textFileLogPath, string imagePath, TesseractEngine engine)
        {
            this.textFileLogPath = textFileLogPath;
            this.imagePath = imagePath;
            _engine = engine;
        }
        // This Method tries to open the image that will be passed by the constructor and it reads its content and save it in a text format.
        // Then, it will create a Directory with the Path provided by the user via constructor and it will write the text collected from the image into a text file.
        public void ReadImageFromUser()
        {
            try
            {
                using (var image = Pix.LoadFromFile(imagePath))
                {
                    using (var page = _engine.Process(image))
                    {
                        var text = page.GetText();

                        if (!Directory.Exists(textFileLogPath))
                            Directory.CreateDirectory(textFileLogPath);

                        using (var sw = new StreamWriter(Path.Combine(textFileLogPath, "convertedText.txt")))
                        {
                            foreach (var line in text)
                            {
                                sw.Write(line);
                            }

                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        // This Method is responsible to Filter the text from the text File.
        // First, it will try to open the text file and read its content. Then, it will try to match the Regex patterns and, if pattern is found, it will add the results to a List.
        public List<object> FilterText()
        {
            string? line;
            var matchesDate = new Regex(@"(\d+\/\d+\/\d+)");
            var matchesValue = new Regex(@"(\d+\,\d{2})");
            var matchesTime = new Regex(@"(\d+\:\d+)");
            var resultList = new List<object>();

            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(textFileLogPath, "convertedText.txt")))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        var date = matchesDate.Match(line);
                        var value = matchesValue.Match(line);
                        var time = matchesTime.Match(line);
                        if (date.Success)
                        {
                            string dateFound = date.Groups[0].Value;
                            resultList.Add(dateFound);
                        }
                        if (value.Success)
                        {
                            string valueFound = value.Groups[0].Value;
                            resultList.Add(valueFound);
                        }
                        if (time.Success)
                        {
                            string timeFound = time.Groups[0].Value;
                            resultList.Add(timeFound);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }
            return resultList;
        }
    }
}
