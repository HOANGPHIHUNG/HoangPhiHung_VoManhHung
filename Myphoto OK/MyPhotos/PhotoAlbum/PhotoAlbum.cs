using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PhotoAlbum
{
    public class PhotoAlbum : Collection<Photo>, IDisposable
    {
        public enum DescriptorOption { FileName, Caption, DateTaken }
        
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                HasChanged = true;
            }
        }

        private DescriptorOption descriptor;
        public DescriptorOption PhotoDescriptor
        {
            get { return descriptor; }
            set
            {
                descriptor = value;
                HasChanged = true;
            }
        }

        private void ClearSettings()
        {
            title = null;
            descriptor = DescriptorOption.Caption;
        }

        private bool hasChanged = false;
        public bool HasChanged
        {
            get {
                if (hasChanged) return true;
                foreach (Photo p in this)
                    if (p.HasChanged) return true;
                return false;
            }
            set 
            {
                hasChanged = value;
                if (value == false)
                    foreach (Photo p in this)
                        p.HasChanged = false;
            }
        }

        public PhotoAlbum()
        {
            ClearSettings();
        }

        public void Dispose()
        {
            ClearSettings();
            foreach (Photo p in this)
                p.Dispose();
        }

        public string GetDescription(Photo photo)
        {
            switch (PhotoDescriptor)
            {
                case DescriptorOption.Caption:
                    return photo.Caption;
                case DescriptorOption.DateTaken:
                    return photo.DateTaken.ToShortDateString();
                case DescriptorOption.FileName:
                    return photo.FileName;
            }
            throw new ArgumentException(
                "Unrecognized photo descriptor option.");
        }

        public string GetDescription(int index)
        {
            return GetDescription(this[index]);
        }

        public Photo Add(string fileName)
        {
            Photo p = new Photo(fileName);
            base.Add(p);
            return p;
        }

        protected override void ClearItems()
        {
            if (Count > 0)
            {
                Dispose();
                base.ClearItems();
                HasChanged = true;
            }
        }

        protected override void InsertItem(int index, Photo item)
        {
            base.InsertItem(index, item);
            HasChanged = true;
        }

        protected override void RemoveItem(int index)
        {
            Items[index].Dispose();
            base.RemoveItem(index);
            hasChanged = true;
        }

        protected override void SetItem(int index, Photo item)
        {
            base.SetItem(index, item);
            HasChanged = true;
        }

        public string GetDescriptorFormat()
        {
            switch (PhotoDescriptor)
            {
                case DescriptorOption.Caption: return "c";
                case DescriptorOption.DateTaken: return "d";
                case DescriptorOption.FileName:
                default:
                    return "f";
            }
        }
    }
}
