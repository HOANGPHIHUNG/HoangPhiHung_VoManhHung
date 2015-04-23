using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Manning.PhotoAlbum;
namespace Manning.PhotoControl
{
    public partial class AlbumEditDialog : Manning.PhotoControl.BaseEditDialog
    {
  
        private AlbumManager _manager;
        private AlbumManager Manager
        { get { return _manager; } }
        private void cbxPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = cbxPassword.Checked;
            txtPassword.Enabled = enabled;
            lblConfirm.Enabled = enabled;
            txtConfirm.Enabled = enabled;
            // If enabled, assign focus
            if (enabled)
                txtPassword.Focus();
        }
        public AlbumEditDialog(AlbumManager mgr)
        {
            if (mgr == null)
                throw new ArgumentException("AlbumManager cannot be null");

            InitializeComponent();

            _manager = mgr;
            ResetDialog();
        }
        protected override void ResetDialog()
        {
            Manning.PhotoAlbum.PhotoAlbum  album = Manager.Album;
            txtAlbumFile.Text = Manager.FullName;
            txtTile.Text = album.Title;
            // Assign radio button
            switch (album.PhotoDescriptor)
            {
                case Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.Caption:
                    rbtnCaption.Checked = true;
                    break;
                case Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.DateTaken:
                    rbtnDateTaken.Checked = true;
                    break;
                case Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.FileName:
                    rbtnFileName.Checked = true;
                    break;
            }
            // Assign check box
            string pwd = Manager.Password;
            cbxPassword.Checked
            = (pwd != null && pwd.Length > 0);
            txtPassword.Text = pwd;
            txtConfirm.Text = pwd;
        }
        private bool ValidPassword()
        {
            if (cbxPassword.Checked)
                return (txtPassword.TextLength > 0
                && txtConfirm.Text == txtPassword.Text);
            else
                return true;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (!ValidPassword())
                {
                    DialogResult result = MessageBox.Show(
                    "The current password is blank "
                    + " or the two password entries "
                    + "do not match.",
                    "Invalid Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                    e.Cancel = true;
                }
                if (!e.Cancel)
                    SaveSettings();
            }
        }
        private void SaveSettings()
        {
            Manning.PhotoAlbum.PhotoAlbum album = Manager.Album;
            if (album != null)
            {
                album.Title = txtTile.Text;
                if (rbtnCaption.Checked)
                    album.PhotoDescriptor = Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.Caption;
                else if (rbtnDateTaken.Checked)
                    album.PhotoDescriptor = Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.DateTaken;
                else if (rbtnFileName.Checked)
                    album.PhotoDescriptor = Manning.PhotoAlbum.PhotoAlbum.DescriptorOption.FileName;
                if (cbxPassword.Checked && ValidPassword())
                    Manager.Password = txtPassword.Text;
                else
                    Manager.Password = null;
            }
        }

        private void txtTile_TextChanged(object sender, EventArgs e)
        {
            Text = txtTile.Text + " - Album Properties";
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.TextLength > 0)
                errorProvider1.SetError(txtPassword, "");
            else
                errorProvider1.SetError(txtPassword,
                "The assignedpassword cannot be blank");
        }

        private void txtConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirm.Text == txtPassword.Text)
                errorProvider1.SetError(txtConfirm, "");
            else
                errorProvider1.SetError(txtConfirm,
                "The password and confirmation entries "
                + "do not match");
        }
    }
}
