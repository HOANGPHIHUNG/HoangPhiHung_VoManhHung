using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Manning.PhotoControl
{
    [ProvideProperty("FlybyText", typeof(ToolStripMenuItem))]
    public class FlybyTextProvider
        : Component, IExtenderProvider
    {
        public FlybyTextProvider(IContainer container)
        {
            container.Add(this);
        }

        private Hashtable _flybyTable = new Hashtable();
        private ToolStripStatusLabel _statusLabel = null;
        private string _currentText = null;
        private bool _set = false;

        private Hashtable FlybyTable { get { return _flybyTable; } }

        private string CurrentStatusText
        {
            get { return _currentText; }
            set { _currentText = value; }
        }

        private bool IsSet
        {
            get { return _set; }
            set { _set = value; }
        }

        public ToolStripStatusLabel StatusLabel
        {
            get { return _statusLabel; }
            set { _statusLabel = value; }
        }

        public bool CanExtend(object extendee)
        {
            return (extendee is ToolStripMenuItem);
        }

        public void SetFlybyText(ToolStripMenuItem item, string text)
        {
            if (text == null || text.Length == 0)
            {
                // Clear the item's text, if necessary
                if (FlybyTable.Contains(item))
                {
                    FlybyTable.Remove(item);
                    item.MouseHover -= OnMouseHover;
                    item.MouseLeave -= OnMouseLeave;
                    item.MouseDown -= OnMouseDown;
                }
            }
            else
            {
                // Write or overwrite the item's text
                FlybyTable[item] = text;
                item.MouseHover += OnMouseHover;
                item.MouseLeave += OnMouseLeave;
                item.MouseDown += OnMouseDown;
            }
        }

        public string GetFlybyText(ToolStripMenuItem item)
        {
            return FlybyTable[item] as string;
        }

        private void ShowFlyby(object item)
        {
            string flybyText = FlybyTable[item] as string;
            if (flybyText != null && StatusLabel != null)
            {
                if (!IsSet)
                {
                    CurrentStatusText = StatusLabel.Text;
                    IsSet = true;
                }
                StatusLabel.Text = flybyText;
            }
        }

        private void RevertFlyby(object item)
        {
            if (StatusLabel != null && IsSet)
            {
                StatusLabel.Text = CurrentStatusText;
                CurrentStatusText = null;
                IsSet = false;
            }
        }

        private void OnMouseHover(object sender, EventArgs e)
        {
            // Display flyby text on hover if assigned
            ShowFlyby(sender);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            // Revert to status text when mouse leaves
            RevertFlyby(sender);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            // Revert to status text when mouse pressed
            RevertFlyby(sender);
        }
    }
}