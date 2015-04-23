using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PhotoAlbum;
using MyPhotoControls;

namespace MyPhotos
{
    public partial class Form1 : Form
    {
        private const int WM_KEYDOWN = 0x100;

        private AlbumManager manager;
        private AlbumManager Manager
        {
            get { return manager; }
            set { manager = value; AssignSelectDropDown(); }
        }

        private PixelDialog dlgPixel = null;
        private PixelDialog PixelForm
        {
            get { return dlgPixel; }
            set { dlgPixel = value; }
        }

        public Form1()
        {
            InitializeComponent();

            NewAlbum();
            flybyProvider.SetFlybyText(mnuSave, "Save the current album");
        }

        private void ProcessPhoto(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            string enumVal = item.Tag as string;
            if (enumVal != null)
                pbxPhoto.SizeMode = (PictureBoxSizeMode)
                    Enum.Parse(typeof(PictureBoxSizeMode), enumVal);
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

        private void SetStatusStrip()
        {
            if (pbxPhoto.Image != null)
            {
                sttInfo.Text = this.Manager.Current.Caption;
                sttImageSize.Text = string.Format("{0:#} x {1:#}",
                                                 pbxPhoto.Image.Width,
                                                 pbxPhoto.Image.Height);
                sttAlbumPos.Text = String.Format(" {0:0}/{1:0} ",
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

        private void DisplayAlbum()
        {
            pbxPhoto.Image = Manager.CurrentImage;
            SetStatusStrip();
            SetTitleBar();

            Point p = pbxPhoto.PointToClient(Form.MousePosition);
            UpdatePixelDialog(p.X, p.Y);
        }

        private void NewAlbum()
        {
            // TODO: clean up, save existing album
            if (Manager == null || SaveAndCloseAlbum())
            {
                // Album closed, create a new one
                Manager = new AlbumManager();
                DisplayAlbum();
            }
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            NewAlbum();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string path = null;
            string password = null;
            if (AlbumController.OpenAlbumDialog(ref path, ref password))
            {
                // Close any existing album
                if (!SaveAndCloseAlbum())
                    return;  // Close canceled
                
                try
                {
                    // Open the new album
                    Manager = new AlbumManager(path, password);
                }
                catch (AlbumStorageException aex)
                {
                    string msg = String.Format("Unable to open album file {0}\n({1})",
                                                path, aex.Message);
                    MessageBox.Show(msg, "Unable to Open");
                    Manager = new AlbumManager();
                }
                DisplayAlbum();
            }
        }

        private void SaveAlbum(string name)
        {
            try
            {
                Manager.Save(name, true);
            }
            catch (AlbumStorageException aex)
            {
                string msg = String.Format("Unable to save album {0} ({1})\n\n"
                                            + "Do you wish to save the album "
                                            + "under an alternate name?",
                                            name, aex.Message);
                DialogResult result = MessageBox.Show(msg, "Unable to Save",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Error,
                                                      MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                    SaveAsAlbum();
            }
        }

        private void SaveAlbum()
        {
            if (string.IsNullOrEmpty(Manager.FullName))
                SaveAsAlbum(); // Force user to select name
            else // Save the album under the existing name
                SaveAlbum(Manager.FullName);
        }

        private bool SaveAndCloseAlbum()
        {
            if (Manager.Album.HasChanged)
            {
                DialogResult result = AlbumController.AskForSave(Manager);

                if (result == DialogResult.Yes)
                    SaveAlbum();
                else if (result == DialogResult.Cancel)
                    return false;  // do not close
            }
            // Close the album and return true
            if (Manager.Album != null)
                Manager.Album.Dispose();
            Manager = new AlbumManager();
            SetTitleBar();
            return true;
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveAlbum();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsAlbum();
        }

        private void SaveAsAlbum()
        {
            string path = null;
            if (AlbumController.SaveAlbumDialog(ref path))
            {
                SaveAlbum(path);
                //Update title bar to include new name
                SetTitleBar();
            }
        }

        private void SetTitleBar()
        {
            Version ver = new Version(Application.ProductVersion);
            string name = Manager.FullName;
            this.Text = String.Format("{2} - My Photos {0:0}.{1:0}",
                ver.Major, ver.Minor, String.IsNullOrEmpty(name)?"Untitled":name);
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected void mnuAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Add Photos";
            dlg.Multiselect = true;
            dlg.Filter = "Image Files (JPEG, GIF, BMP, etc.)|"
                            + "*.jpg;*.jpeg;*.gif;*.bmp;"
                            + "*.tif;*.tiff;*.png|"
                            + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|" 
                            + "GIF files (*.gif)|*.gif|"
                            + "BMP files (*.bmp)|*.bmp|"
                            + "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|" 
                            + "PNG files (*.png)|*.png|"
                            + "All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.CurrentDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = dlg.FileNames;
                int index = 0;
                foreach (string s in files)
                {
                    Photo photo = new Photo(s);
                    // Add the file (if not already present)
                    index = Manager.Album.IndexOf(photo);
                    if (index < 0)
                        Manager.Album.Add(photo);
                    else
                        photo.Dispose();  // photo already there
                }
                Manager.Index = Manager.Album.Count - 1;
            }
            dlg.Dispose();
            DisplayAlbum();
        }

        private void mnuRemove_Click(object sender, EventArgs e)
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

        private void mnuPrevious_Click(object sender, EventArgs e)
        {
            if (Manager.Index > 0)
            {
                Manager.Index--;
                DisplayAlbum();
            }
        }

        private void ctxMenuPhoto_Opening(object sender, CancelEventArgs e)
        {
            mnuNext.Enabled = (Manager.Index < Manager.Album.Count - 1);
            mnuPrevious.Enabled = (Manager.Index > 0);
            mnuPhotoPros.Enabled = (Manager.Current != null);
            mnuAlbumPros.Enabled = (Manager.Album != null);
            mnuSlideShow.Enabled = (Manager.Album != null && Manager.Album.Count > 0);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (SaveAndCloseAlbum() == false)
                e.Cancel = true;
            else
                e.Cancel = false;

            base.OnFormClosing(e);
        }

        private void mnuPixelData_Click(object sender, EventArgs e)
        {
            if (PixelForm == null || PixelForm.IsDisposed)
            {
                PixelForm = new PixelDialog();
                PixelForm.Owner = this;
            }
            PixelForm.Show();
            Point p = pbxPhoto.PointToClient(Form.MousePosition);
            UpdatePixelDialog(p.X, p.Y);
            UpdatePixelButton(true);
        }

        private void UpdatePixelDialog(int x, int y)
        {
            if (PixelForm != null && PixelForm.Visible)
            {
                Bitmap bmp = Manager.CurrentImage;
                PixelForm.Text = (Manager.Current == null
                                    ? "Pixel Data"
                                    : Manager.Current.Caption);
                if (bmp == null || !pbxPhoto.DisplayRectangle.Contains(x, y))
                    PixelForm.ClearPixelData();
                else
                    PixelForm.UpdatePixelData(x, y, bmp,
                                              pbxPhoto.DisplayRectangle,
                                              new Rectangle(0, 0, bmp.Width, bmp.Height),
                                              pbxPhoto.SizeMode);
            }
        }

        private void pbxPhoto_MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePixelDialog(e.X, e.Y);
        }

        private void mnuPhotoPros_Click(object sender, EventArgs e)
        {
            if (Manager.Current == null)
                return;
            using (PhotoEditDialog dlg = new PhotoEditDialog(Manager))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    DisplayAlbum();
            }
        }

        private void mnuAlbumPros_Click(object sender, EventArgs e)
        {
            if (Manager.Album == null)
                return;
            
            using (AlbumEditDialog dlg = new AlbumEditDialog(Manager))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    DisplayAlbum();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '+':
                    mnuNext.PerformClick();
                    e.Handled = true;
                    break;
                case '-':
                    mnuPrevious.PerformClick();
                    e.Handled = true;
                    break;
            }
            // Invoke the base method
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    mnuPrevious.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.PageDown:
                    mnuNext.PerformClick();
                    e.Handled = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == WM_KEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        mnuNext.PerformClick();
                        return true;
                    case Keys.Shift | Keys.Tab:
                        mnuPrevious.PerformClick();
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void mnuSlideShow_Click(object sender, EventArgs e)
        {
            using (SlideShowDialog dlg = new SlideShowDialog(Manager))
            {
                dlg.ShowDialog();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.tsbNew.Tag = mnuNew;
            this.tsbOpen.Tag = mnuOpen;
            this.tsbSave.Tag = mnuSave;
            this.tsbPrint.Tag = mnuPrint;
            this.tsbCut.Tag = mnuCut;
            this.tsbCopy.Tag = mnuCopy;
            this.tsbPaste.Tag = mnuPaste;
            tsbPrevious.Tag = mnuPrevious;
            tsbNext.Tag = mnuNext;
            this.tsbHelp.Tag = mnuAbout;
            toolStripMain.ImageList = imageListArrows;
            tsbPrevious.ImageIndex = 0;
            tsbNext.ImageIndex = 1;
            // Set up toolStripDialogs 
            tsbAlbumProps.Tag = mnuAlbumPros;
            tsbPhotoProps.Tag = mnuPhotoPros;
            tsbPixelData.Tag = tsbPixelData.Image;
            // Set up toolStripImages
            tsdImage.DropDown = mnuImage.DropDown;
            base.OnLoad(e);
        }

        private void tbs_Click(object sender, EventArgs e)
        {
            // Ensure sender is a menu item
            ToolStripItem item = sender as ToolStripItem;
            if (item != null)
            {
                ToolStripMenuItem mi = item.Tag as ToolStripMenuItem;
                if (mi != null)
                    mi.PerformClick();
            }
        }

        private void tsbPixelData_Click(object sender, EventArgs e)
        {
            Form f = PixelForm;
            if (f == null || f.IsDisposed || !f.Visible)
                mnuPixelData.PerformClick();
            else
                f.Hide();
            UpdatePixelButton(PixelForm.Visible);
        }

        private void UpdatePixelButton(bool visible)
        {
            tsbPixelData.Checked = visible;
            if (visible)
                tsbPixelData.Image = tsbPixelData2.Image;
            else
                tsbPixelData.Image = (Image)tsbPixelData.Tag;
        }

        protected override void OnActivated(EventArgs e)
        {
            if (dlgPixel != null)
                UpdatePixelButton(dlgPixel.Visible);

            base.OnActivated(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Visible = false;
            if (Owner != null)
                Owner.Activate();

            base.OnFormClosed(e);
        }

        private void AssignSelectDropDown()
        {
            ToolStripDropDown drop = new ToolStripDropDown();
            PhotoAlbum.PhotoAlbum a = Manager.Album;
            for (int i = 0; i < a.Count; i++)
            {
                PictureBox box = new PictureBox(); 
                box.SizeMode = PictureBoxSizeMode.Zoom;
                box.Image = a[i].Image;
                box.Dock = DockStyle.Fill;
                ToolStripControlHost host = new ToolStripControlHost(box);
                host.AutoSize = false;
                host.Size = new Size(tssSelect.Width, tssSelect.Width);
                host.Tag = i;
                host.Click += delegate(object o, EventArgs e)
                {
                    int x = (int)(o as ToolStripItem).Tag;
                    Manager.Index = x;
                    drop.Close();
                    DisplayAlbum();
                };
                drop.Items.Add(host);
            }
            if (drop.Items.Count > 0)
            {
                tssSelect.DropDown = drop;
                tssSelect.DefaultItem = drop.Items[0];
            }
        }
    }
}
