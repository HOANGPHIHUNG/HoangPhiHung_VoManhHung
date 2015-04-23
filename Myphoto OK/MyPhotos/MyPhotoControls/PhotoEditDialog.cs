using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PhotoAlbum;
using System.Collections.Specialized;

namespace MyPhotoControls
{
    public partial class PhotoEditDialog : MyPhotoControls.BaseEditDialog
    {
        private Photo photo;
        private Photo Photograph
        { get { return photo; } }
        
        private AlbumManager manager = null;
        private AlbumManager Manager
        { get { return manager; } }

        protected PhotoEditDialog()
        {
            InitializeComponent();
        }

        private void InitializeDialog(Photo photo)
        {
            this.photo = photo;
            ResetDialog();
        }

        public PhotoEditDialog(Photo photo) : this()
        {
            if (photo == null)
                throw new ArgumentNullException(
                "The photo parameter cannot be null");
            InitializeDialog(photo);
        }

        public PhotoEditDialog(AlbumManager mgr) : this()
        {
            if (mgr == null)
                throw new ArgumentNullException(
                    "The mgr parameter cannot be null");
            this.manager = mgr;
            InitializeDialog(mgr.Current);
        }

        protected override void ResetDialog()
        {
            // Fill combo box with photographers in album
            comboPhotographer.BeginUpdate();
            comboPhotographer.Items.Clear();
            if (Manager != null)
            {
                StringCollection coll = Manager.Photographers;
                foreach (string s in coll)
                    comboPhotographer.Items.Add(s);
            }
            else
                comboPhotographer.Items.Add(Photograph.Photographer);
            comboPhotographer.EndUpdate();

            Photo photo = Photograph;
            if (photo != null)
            {
                txtPhotoFile.Text = photo.FileName;
                txtCaption.Text = photo.Caption;
                dtpDateTaken.Value = photo.DateTaken;
                comboPhotographer.Text = photo.Photographer;
                txtNotes.Text = photo.Notes;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                SaveSettings();
            base.OnClosing(e);
        }

        private void SaveSettings()
        {
            Photo photo = Photograph;
            if (photo != null)
            {
                photo.Caption = txtCaption.Text;
                photo.Photographer = comboPhotographer.Text;
                photo.Notes = txtNotes.Text;
                photo.DateTaken = dtpDateTaken.Value;
            }
        }

        private void txtCaption_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(Char.IsLetterOrDigit(c)
                            || Char.IsWhiteSpace(c)
                            || Char.IsControl(c));
            if (e.Handled) Console.Beep();
        }

        private void txtCaption_TextChanged(object sender, EventArgs e)
        {
            Text = txtCaption.Text + " - Properties";
        }

        private void mskDateTaken_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                DialogResult result = MessageBox.Show(
                  "The Date Taken entry is invalid or "
                  + "in the future and may be ignored."
                  + " Do you wish to correct this?",
                  "Photo Properties",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);
                e.Cancel = (result == DialogResult.Yes);
            }
        }

        private void comboPhotographer_Leave(object sender, EventArgs e)
        {
            string person = comboPhotographer.Text;
            if (!comboPhotographer.Items.Contains(person))
                comboPhotographer.Items.Add(person);
        }
    }
}
