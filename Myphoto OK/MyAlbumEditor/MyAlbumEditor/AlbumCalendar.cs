using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PhotoAlbum;
using MyPhotoControls;

namespace MyAlbumEditor
{
    public partial class AlbumCalendar : UserControl
    {
        private AlbumManager _manager = null;
        public AlbumManager Manager
        {
            get { return _manager; }
            set
            {
                _manager = value;
                UpdateCalendar();
            }
        }

        public AlbumCalendar()
        {
            InitializeComponent();
        }

        private void UpdateCalendar()
        {
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            calendarDates.RemoveAllBoldedDates();

            if (Manager == null)
            {
                minDate = DateTime.Now;
                maxDate = DateTime.Now.AddMonths(2);
            }
            else
            {
                foreach (Photo p in Manager.Album)
                {
                    DateTime date = p.DateTaken;
                    calendarDates.AddBoldedDate(date);
                    if (date < minDate) minDate = date;
                    if (date > maxDate) maxDate = date;
                }
            }

            calendarDates.MinDate = minDate;
            calendarDates.MaxDate = maxDate;
            calendarDates.TodayDate = minDate;
            calendarDates.SelectionStart = minDate;
            calendarDates.UpdateBoldedDates();
        }

        private void calendarDates_MouseDown(object sender, MouseEventArgs e)
        {
            // Ignore click if no album selected
            if (Manager == null) return;

            MonthCalendar.HitTestInfo info = calendarDates.HitTest(e.X, e.Y);
            if (info.HitArea == MonthCalendar.HitArea.Date)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                cms.ShowImageMargin = false;
                // See if any photos at date
                for (int i = 0; i < Manager.Album.Count; i++)
                {
                    Photo p = Manager.Album[i];
                    if (p.DateTaken.Date == info.Time.Date)
                    {
                        ToolStripItem menuItem = cms.Items.Add(p.FileName);
                        menuItem.Tag = i;
                        menuItem.Click += cmsItem_Click;
                    }
                }
                if (cms.Items.Count > 0) 
                    cms.Show(calendarDates, e.Location);
            }
        }

        private void cmsItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item != null && item.Tag is int)
            {
                Manager.Index = (int)item.Tag;
                using (PhotoEditDialog dlg = new PhotoEditDialog(Manager))
                {
                    DateTime oldDate = Manager.Current.DateTaken;
                    if (dlg.ShowDialog() == DialogResult.OK
                        && oldDate != Manager.Current.DateTaken)
                    {
                        // DateTaken was modified
                        UpdateCalendar();
                    }
                }
            }
        }
    }
}
