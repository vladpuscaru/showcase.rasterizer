
using System.Numerics;
using SkiaSharp;

namespace UV_Practical3
{
    public static class Generator
    {


        /**
         * Generates a greyscale version of an RGB surface
         * using the "Luminosity Method" [https://www.baeldung.com/cs/convert-rgb-to-grayscale]
         * 
         * grayscale = 0.3 * R + 0.59 * G + 0.11 * B
         */
        public static SKBitmap GenerateHeightMap(SKBitmap bitmap)
        {
            SKBitmap bitmapGrayscale = new SKBitmap(bitmap.Width, bitmap.Height, SKColorType.Gray8, SKAlphaType.Opaque);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    SKColor color = bitmap.GetPixel(x, y);

                    // Calculate the grayscale value using the luminosity method
                    byte grayValue = (byte)(0.3 * color.Red + 0.59 * color.Green + 0.11 * color.Blue);

                    // Set the pixel in the grayscale bitmap
                    bitmapGrayscale.SetPixel(x, y, new SKColor(grayValue, grayValue, grayValue));
                }
            }

            return bitmapGrayscale;
        }

        public static SKBitmap GenerateNormalMap(SKBitmap heightmap)
        {
            SKBitmap bitmapNormal = new SKBitmap(heightmap.Width, heightmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            for (int y = 1; y < heightmap.Height - 1; y++)
            {
                for (int x = 1; x < heightmap.Width - 1; x++)
                {
                    /**
                     * It's assumed that heightmap is a grayscale so Red, Green, Blue are the same
                     */
                    float gradientX = heightmap.GetPixel(x + 1, y - 1).Red + 2 * heightmap.GetPixel(x + 1, y).Red + heightmap.GetPixel(x + 1, y + 1).Red -
                                      heightmap.GetPixel(x - 1, y - 1).Red - 2 * heightmap.GetPixel(x - 1, y).Red - heightmap.GetPixel(x - 1, y + 1).Red;

                    float gradientY = heightmap.GetPixel(x - 1, y + 1).Red + 2 * heightmap.GetPixel(x, y + 1).Red + heightmap.GetPixel(x + 1, y + 1).Red -
                                      heightmap.GetPixel(x - 1, y - 1).Red - 2 * heightmap.GetPixel(x, y - 1).Red - heightmap.GetPixel(x + 1, y - 1).Red;

                    // Calculate the normal vector
                    Vector3 normal = Vector3.Normalize(new Vector3(-gradientX, -gradientY, 1));

                    // Map to RGB space
                    byte r = (byte)((normal.X + 1.0f) * 0.5f * 255.0f);
                    byte g = (byte)((normal.Y + 1.0f) * 0.5f * 255.0f);
                    byte b = (byte)((normal.Z + 1.0f) * 0.5f * 255.0f);

                    // Set the pixel in the grayscale bitmap
                    bitmapNormal.SetPixel(x, y, new SKColor(r, g, b));
                }
            }

            return bitmapNormal;
        }
    }
}