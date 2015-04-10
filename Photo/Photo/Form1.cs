using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PhotoAlbum;


namespace Photo
{
    public partial class frMain : Form
    {
        public frMain()
        {
            InitializeComponent();
            NewAlbum();
            mnuView.DropDown = ctxMenuphoto;
            
        }
        
       private void SaveAlbum(string name)
        {
            Manager.Save(name, true);
        }
        private void SaveAlbum()
{
if (String.IsNullOrEmpty(Manager.FullName))
    SaveAsAlbum(); // Force user to select name
else
{
    // Save the album under the existing name
    SaveAlbum(Manager.FullName);
}
}

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuFileSave_Click
(object sender, EventArgs e)
        {
            SaveAlbum();
        }
        protected void menuFileSaveAs_Click
(object sender, System.EventArgs e)
        {
            SaveAsAlbum();
        }



        private void SaveAsAlbum()
{
SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Save Album";
dlg.DefaultExt = "abm";
dlg.Filter = "Album files (*.abm)|*.abm|"
+ "All files|*.*";
dlg.InitialDirectory
= AlbumManager.DefaultPath;
dlg.RestoreDirectory = true;
if (dlg.ShowDialog() == DialogResult.OK)
{
SaveAlbum(dlg.FileName);
// Update title bar to include new name
SetTitleBar();
}
dlg.Dispose();
}
        private void SetTitleBar()
        {
            Version ver = new Version(
            Application.ProductVersion);
            string name = Manager.FullName;
            this.Text = String.Format(
            "{2} - MyPhotos {0:0}.{1:0}",
            ver.Major, ver.Minor,
            String.IsNullOrEmpty(name)
            ? "Untitled" : name);
        }
        


        protected void mnuFileOpen_Click(object sender, System.EventArgs e)
        {
            // Allow user to select a new album
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Album";
            dlg.Filter = "Album files (*.abm)|*.abm|"
            + "All files (*.*)|*.*";
            dlg.InitialDirectory
            = AlbumManager.DefaultPath;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // TODO: save any existing album.
                // Open the new album
                // TODO: handle invalid album file
                Manager = new AlbumManager(dlg.FileName);
                DisplayAlbum();
            }
            dlg.Dispose();
        }

        private void ProcessPhoto(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string enumVal = item.Tag as string;
            if (enumVal != null)
            {
                pbxPhoto.SizeMode = (PictureBoxSizeMode)
                    Enum.Parse(typeof(PictureBoxSizeMode), enumVal);
            }
        }

        private void mnuImage_DropDownOpening(object sender, EventArgs e)
        {
            ToolStripDropDownItem parent = (ToolStripDropDownItem)sender;
            if (parent != null)
            {
                string enumVal = pbxPhoto.SizeMode.ToString();
                foreach (ToolStripMenuItem item in parent.DropDownItems)
                {
                    item.Enabled = (pbxPhoto.Image != null);
                    item.Checked = item.Tag.Equals(enumVal);
                }
            }
        }
        private void SetstatusStrip(string path)
        {
            if (pbxPhoto.Image != null)
            {
                sttInfo.Text == Manager.Current.Caption;
                sttImageSize.Text = string.Format("{0:#} x {1:#}", pbxPhoto.Image.Width, pbxPhoto.Image.Height);
                statusAlbumPos.Text
= String.Format(" {0:0}/{1:0} ",
Manager.Index + 1,
Manager.Album.Count);
            }

            else
            {
                sttInfo.Text = null;
                sttImageSize.Text = null;
                sttAlbumPos.Text = null;
            }
        }
        private AlbumManager manager;
            
        private AlbumManager Manager
            { 
                get { return manager; }
                    set { manager = value; }
            }
       
        private void SetStatusStrip()
            {
                if (pbxPhoto.Image != null)
            {
                sttInfo.Text = Manager.Current.FileName;
                sttImageSize.Text = string.Format("{0:#} x {1:#}", pbxPhoto.Image.Width, pbxPhoto.Image.Height);
            }
            
                else
                {
                sttInfo.Text = null;
                sttImageSize.Text = null;
                sttAlbumPos.Text = null;
                
                }
        }
        
            private void DisplayAlbum()
               {
                pbxPhoto.Image = Manager.CurrentImage;
                SetStatusStrip();
                SetTitleBar();
               }

            private void mnuNew_Click (object sender, System.EventArgs e)
               {
                        NewAlbum();
               }

            private void NewAlbum()
            {
                // TODO: clean up, save existing album
                Manager = new AlbumManager();
                DisplayAlbum();
            }

            private void EditAdd_Click(object sender, EventArgs e)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Add Photos";
                dlg.Multiselect = true;
                dlg.Filter
                = "Image Files (JPEG, GIF, BMP, etc.)|"
                + "*.jpg;*.jpeg;*.gif;*.bmp;"
                + "*.tif;*.tiff;*.png|"
                + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
                + "GIF files (*.gif)|*.gif|"
                + "BMP files (*.bmp)|*.bmp|"
                + "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|"
                + "PNG files (*.png)|*.png|"
                + "All files (*.*)|*.*";
                dlg.InitialDirectory
                = Environment.CurrentDirectory;
                if (dlg.ShowDialog() == DialogResult.OK)
                  {
                    string[] files = dlg.FileNames;
                    int index = 0;
                    foreach (string s in files)
                    {
                        Photography photo = new Photo(s);
                    // Add the file (if not already present)
                    index = Manager.Album.IndexOf(photo);
                    if (index < 0)
                    Manager.Album.Add(photo);
                    else
                    photo.Dispose(); // photo already there
                    }
                    Manager.Index = Manager.Album.Count - 1;
                  }
                dlg.Dispose();
DisplayAlbum();
}

            private void menuEditRemove_Click(
    object sender, EventArgs e)
            {
                if (Manager.Album.Count > 0)
{
Manager.Album.RemoveAt(Manager.Index);
DisplayAlbum();
}
}

            private void mnuNext_Click(object sender, EventArgs e)
            {
                if (Manager.Index < Manager.Album.Count - 1)
                {
                Manager.Index++;
                DisplayAlbum();
                }
            }
            private void menuPrevious_Click(
    object sender, EventArgs e)
            {
                if (Manager.Index > 0)
{
Manager.Index--;
DisplayAlbum();
}
}
            private void ctxMenuPhoto_Opening(
    object sender, CancelEventArgs e)
            {
                menuNext.Enabled
= (Manager.Index < Manager.Album.Count - 1);
                menuPrevious.Enabled = (Manager.Index > 0);
}

            }

            }
    }
    

