using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing; // for Bitmap class
using System.IO; // for Path class
using System.Collections.Specialized;

namespace PhotoAlbum
{
    public class AlbumManager
    {
        private StringCollection _photographers = null;
        public StringCollection Photographers
        {
            get
            {
                if (Album.HasChanged || _photographers == null)
                {
                    _photographers = new StringCollection();
                    foreach (Photo p in Album)
                    {
                        // Make sure we add each person only once
                        string person = p.Photographer;
                        if (person != null && person.Length > 0
                            && !_photographers.Contains(person))
                        {
                            _photographers.Add(person);
                        }
                    }
                }
                return _photographers;
            }
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
        private string pwd;
        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public AlbumManager()
        {
            album = new PhotoAlbum();
        }

        public AlbumManager(string name) : this()
        {
            this.name = name;
            album = AlbumStorage.ReadAlbum(name);
            if (Album.Count > 0)
                Index = 0;
        }

        public AlbumManager(string name, string pwd) : this()
        {
            this.name = name;
            album = AlbumStorage.ReadAlbum(name, pwd);
            Password = pwd;
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
            get {
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
            AlbumStorage.WriteAlbum(Album, FullName, Password);
        }

        public void Save(string name, bool overwrite)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name != FullName && AlbumExists(name) && !overwrite)
                throw new ArgumentException("An album with this name exists");
            AlbumStorage.WriteAlbum(Album, name, Password);
            FullName = name;
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

        public void MoveItemBackward(int index)
        {
            if (index <= 0 || index >= Album.Count)
                throw new IndexOutOfRangeException();
            // Remove photo and reinsert at prior position
            Photo photo = Album[index];
            Album.RemoveAt(index);
            Album.Insert(index - 1, photo);
        }

        public void MoveItemForward(int index)
        {
            if (index < 0 || index > Album.Count - 1)
                throw new IndexOutOfRangeException();
            // Remove photo and reinsert at subsequent pos
            Photo photo = Album[index];
            Album.RemoveAt(index);
            Album.Insert(index + 1, photo);
        }
    }
}
