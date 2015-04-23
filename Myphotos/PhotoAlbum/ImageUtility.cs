using System;
using System.Drawing;
namespace Manning.PhotoAlbum
{
    public class ImageUtility
    {
        public static Rectangle ScaleToFit(Bitmap bmp, Rectangle targetArea)
        {
            Rectangle result = new Rectangle(targetArea.Location, targetArea.Size);

            
            if (result.Height * bmp.Width > result.Width * bmp.Height)
            {
                
                result.Height = result.Width * bmp.Height / bmp.Width;
                result.Y += (targetArea.Height - result.Height) / 2;
            }
            else
            {
               
                result.Width = result.Height * bmp.Width / bmp.Height;
                result.X += (targetArea.Width - result.Width) / 2;
            }

            return result;
        }
    }
}