using System.Reflection;
using Template;
using SkiaSharp;

namespace UV_Practical3
{
    public static class IOUtils
    {
        public static string ASSETS_FOLDER = "assets";
        public static string OUTPUT_FOLDER = "generated";

        public static List<Surface> ReadAllTextures()
        {
            string executablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
            string folderPath = Path.Combine(executablePath, ASSETS_FOLDER)!;

            List<Surface> textures = new List<Surface>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.jpg"))
            {
                Console.WriteLine("Reading " + file);
                textures.Add(new Surface(file));
            }
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.jpeg"))
            {
                Console.WriteLine("Reading " + file);
                textures.Add(new Surface(file));
            }

            return textures;
        }

        public static void SaveSurface(string fileName, SKBitmap bitmap)
        {
            string outputFolder = GetOutputFolder();
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // Save the bitmap to a JPG file
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            using (var stream = System.IO.File.OpenWrite(outputFolder + Path.DirectorySeparatorChar + fileName))
            {
                data.SaveTo(stream);
                Console.WriteLine("Saved output to " + outputFolder + Path.DirectorySeparatorChar + fileName);
            }
        }

        private static string GetOutputFolder()
        {
            return Utils.GetPathFromSrc(ASSETS_FOLDER + Path.DirectorySeparatorChar + OUTPUT_FOLDER);
        }
    }
}