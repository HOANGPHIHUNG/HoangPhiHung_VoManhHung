using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyPhotoControls
{
    public class ScrollablePictureBox : PictureBox
{
private bool _allowScrollBars = true;
public bool AllowScrollBars
{
get { return _allowScrollBars; }
set
{
if (_allowScrollBars != value)
{
// Force a redraw when value changes
_allowScrollBars = value;
Invalidate();
}
}
}
}

  public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
            {
                if (Scroll != null)
                Scroll(this, e);
            }

        private VScrollBar _vbar = new VScrollBar();
        private Control _vbarContainer = new Control();
        private HScrollBar HBar {
            get { return _hbar; } }
        private VScrollBar VBar {
            get { return _vbar; } }
        private Control VContainer {
            get { return _vbarContainer; } }

                public ScrollablePictureBox()
            {
            // Initialize horizontal scroll bar
            HBar.Visible = false;
            HBar.Dock = DockStyle.Bottom;
            HBar.Minimum = 0;
            HBar.Maximum = 1000;
            HBar.Scroll += HandleScroll;
            // Initialize vertical scroll bar container
            VContainer.Visible = false;
            VContainer.Width = VBar.Width;
            VContainer.Height = Height;
            VContainer.Dock = DockStyle.Right;
            // Initialize vertical scroll bar
            VBar.Top = 0;
            VBar.Left = 0;
            VBar.Height = VContainer.Height - HBar.Height;
            VBar.Anchor = AnchorStyles.Top
            | AnchorStyles.Bottom
            | AnchorStyles.Left
            | AnchorStyles.Right;
            VBar.Minimum = 0;
            VBar.Maximum = 1000;
            VBar.Scroll += HandleScroll;
            }

            VContainer.Controls.Add(VBar);
            Controls.Add(HBar);
            Controls.Add(VContainer);
            DoubleBuffered = true;
}
private void HandleScroll(
object sender, ScrollEventArgs e)
{
}

