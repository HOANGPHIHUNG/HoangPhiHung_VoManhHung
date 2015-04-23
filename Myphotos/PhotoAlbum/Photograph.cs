using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Manning.PhotoAlbum
{
    public class Photograph : IDisposable
    {
        private string _fileName;
        private Bitmap bitmap;

        private string caption = "";
        private string photographer = "";
        private DateTime dateTaken = DateTime.Now;
        private string notes = "";
        private bool hasChanged = true;

        public Photograph(string fileName)
        {
            _fileName = fileName;
            bitmap = null;

            caption = System.IO.Path.
                GetFileNameWithoutExtension(fileName);
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public Bitmap Image
        {
            get
            {
                if (bitmap == null)
                {
                    bitmap = new Bitmap(_fileName);
                }
                return bitmap;
            }
        }

        public bool HasChanged
        {
            get { return hasChanged; }
            internal set { hasChanged = value; }
        }

        public string Caption
        {
            get { return caption; }
            set
            {
                if (caption != value)
                {
                    caption = value;
                    HasChanged = true;
                }
            }
        }

        public string Photographer
        {
            get { return photographer; }
            set
            {
                if (photographer != value)
                {
                    photographer = value;
                    HasChanged = true;
                }
            }
        }

        public DateTime DateTaken
        {
            get { return dateTaken; }
            set
            {
                if (dateTaken != value)
                {
                    dateTaken = value;
                    HasChanged = true;
                }
            }
        }

        public string Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    HasChanged = true;
                }
            }
        }

    
        public override bool Equals(object obj)
        {
            if (obj is Photograph)
            {
                Photograph p = (Photograph)obj;
                return (FileName.Equals(p.FileName,
                StringComparison.
                InvariantCultureIgnoreCase));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FileName.ToLowerInvariant().GetHashCode();
        }

        public override string ToString()
        {
            return FileName;
        }

     
        public void ReleaseImage()
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        public void Dispose()
        {
            ReleaseImage();
        }
    }
}
