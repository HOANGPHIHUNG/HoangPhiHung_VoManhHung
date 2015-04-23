namespace MyAlbumEditor
{
    partial class AlbumCalendar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.calendarDates = new System.Windows.Forms.MonthCalendar();
            this.SuspendLayout();
            // 
            // calendarDates
            // 
            this.calendarDates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calendarDates.Location = new System.Drawing.Point(0, 0);
            this.calendarDates.MaxSelectionCount = 1;
            this.calendarDates.Name = "calendarDates";
            this.calendarDates.ShowToday = false;
            this.calendarDates.TabIndex = 0;
            this.calendarDates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.calendarDates_MouseDown);
            // 
            // AlbumCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.calendarDates);
            this.Name = "AlbumCalendar";
            this.Size = new System.Drawing.Size(285, 189);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MonthCalendar calendarDates;
    }
}
