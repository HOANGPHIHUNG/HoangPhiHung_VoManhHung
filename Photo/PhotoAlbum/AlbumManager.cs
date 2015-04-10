using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; // for Bitmap class
using System.IO; // for Path class

namespace PhotoAlbum
{
    public static class AlbumStorage
    {
        static public void WriteAlbum(PhotoAlbum album, string path);
        static public PhotoAlbum ReadAlbum(string path);


        static public bool AlbumExists(string name)
        {
            return File.Exists(name);
        }
    }
    public class AlbumManager
    {
        static public bool AlbumExists(string name)
        {
            return File.Exists(name);
        }
        static private string defaultPath;
        static public string DefaultPath
        {
            get { return defaultPath; }
            set { defaultPath = value; }
        }

        static AlbumManager()
        {
            defaultPath = Environment.GetFolderPath(
            Environment.SpecialFolder.Personal)
            + @"\Albums";
        }

        private int pos = -1;
        private string name = String.Empty;
        private PhotoAlbum album;

        public AlbumManager()
        {
            album = new PhotoAlbum();
        }
        public AlbumManager(string name) : this()
        {
            this.name = name;
            // TODO: load the album
            throw new NotImplementedException();
        }

        public PhotoAlbum Album
        {
            get { return album; }
        }
        public string FullName
        {
            get { return name; }
            private set { name = value; }
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
        public Photo Current
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
            // TODO: implement AlbumExists method
            return false;
        }

        public void Save()
        {
            // TODO: Implement Save method
            throw new NotImplementedException();
        }
        public void Save(string name, bool overwrite)
        {
            // TODO: Implement Save(name) method
            throw new NotImplementedException();
        }

        public bool MoveNext()
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

        public AlbumManager(string name): this()
        {
            //name = name;
            album = AlbumStorage.ReadAlbum(name);
            if (Album.Count > 0)
                Index = 0;
        }

        public void Save()
        {
            if (FullName == null)
                throw new InvalidOperationException( "Unable to save album with no name");
            AlbumStorage.WriteAlbum(Album, FullName);
        }
        public void Save(string name, bool overwrite)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name != FullName && AlbumExists(name) && !overwrite)
                throw new ArgumentException(
                "An album with this name exists");
            AlbumStorage.WriteAlbum(Album, name);
            FullName = name;
        }
    
    
    
    
    
    }
        }


