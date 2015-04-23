using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing; // for Bitmap class
using System.IO; // for Path class

namespace Manning.PhotoAlbum
{
    public class AlbumManager
    {
        static private string defaultPath;
        static public string DefaultPath
        {
            get { return defaultPath; }
            set { defaultPath = value; }
        }

         static AlbumManager()
        {
            defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Albums";
        }

        private int pos = -1;
        private string _name = String.Empty;
        private PhotoAlbum album;

        public  AlbumManager()
        {
            album = new PhotoAlbum();
        }
        public AlbumManager(string name,string pwd): this()
        {

            _name = name;
            album = AlbumStorage.ReadAlbum(name);
            Password = pwd;
            if (Album.Count > 0)
                Index = 0;
        }

        public PhotoAlbum Album
        {
            get { return album; }
        }
        public string FullName
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string ShortName
        {
            get
            {
                if (String.IsNullOrEmpty(FullName))
                    return null;
                else
                    return Path.
                    GetFileNameWithoutExtension(FullName);
            }
        }
        public   Photograph Current
        {
            get
            {
                if (Index < 0 || Index >= Album.Count)
                    return null;
                return Album[pos];
            }
        }
        public Bitmap CurrentImage
        {
            get
            {
                if (Current == null)
                    return null;
                return Current.Image;
            }
        }

        public int Index
        {
            get
            {
                int count = Album.Count;
                if (pos >= count)
                    pos = count - 1;
                return pos;
            }
            set
            {
                if (value < 0 || value >= Album.Count)
                    throw new IndexOutOfRangeException(
                    "The given index is out of bounds");
                pos = value;
            }
        }

        static public bool AlbumExists(string name)
        {
            return File.Exists(name);
            
        }

        public void Save()
        {
           if (FullName == null)
            throw new InvalidOperationException("Unable to save album with no name");
            AlbumStorage.WriteAlbum(Album, FullName,Password);
            // TODO: Implement Save method
          
        }
       
        public void Save(string name, bool overwrite)
        {
           
            if (name == null)
                throw new ArgumentNullException("name");
            if (name != FullName && AlbumExists(name) && !overwrite)
                throw new ArgumentException( "An album with this name exists");
            AlbumStorage.WriteAlbum(Album, name,Password);
            FullName = name;
        }

        public bool mnuEditRemove()
        {
            if (Index >= Album.Count)
                return false;
            Index++;
            return true;
        }

        public bool MovePrev()
        {
            if (Index <= 0)
                return false;
            Index--;
            return true;
        }
        private string pwd;
        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }
    }
}
