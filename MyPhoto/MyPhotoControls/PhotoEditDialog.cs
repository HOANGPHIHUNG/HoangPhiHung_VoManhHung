using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PhotoAlbum;
using MyPhotoControls;

namespace MyPhotoControls
{
    public partial class PhotoEditDialog : MyPhotoControls.BaseEditDialog
    {
        private Photograph photo;
        private Photograph Photo
        { get { return photo; } }
        private AlbumManager manager = null;
        private AlbumManager Manager
        { get { return manager; } }

        protected PhotoEditDialog()
        {
            // Call required by Windows Form Designer.
            InitializeComponent();
        }

        private void InitializeDialog(Photo photographer)
        {
            photo = photo;
            ResetDialog();
        }

        public PhotoEditDialog(Photograph photo): this()
        {
            if (photo == null)
                throw new ArgumentNullException("The photo parameter cannot be null");
            InitializeDialog(photo);
        }

        public PhotoEditDialog(AlbumManager mgr): this()
        {
            if (mgr == null)
                throw new ArgumentNullException(
                "The mgr parameter cannot be null");
            manager = mgr;
            InitializeDialog(mgr.Current);
        }

        protected override void ResetDialog()
        {
            Photograph photo = Photo;
            if (photo != null)
            {
                txtPhotoFile.Text = photo.FileName;
                txtCaption.Text = photo.Caption;
                txtDateTaken.Text
                = photo.DateTaken.ToString();
                txtPhotographer.Text
                = photo.Photographer;
                txtNotes.Text = photo.Notes;
            }
        }

        private void InitializeDialog(Photograph photo)
        {
            throw new NotImplementedException();
        }
        public PhotoEditDialog()
        {
            InitializeComponent();
        }
    }
}
