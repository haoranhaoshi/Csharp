using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Framework.SkinMgr.ZHSkin
{
    public partial class ZHMenuStrip : MenuStrip
    {
        public static ToolStripMenuItem lastMenuItem = null;
        public static ToolStripMenuItem activeMenuItem = null;

        public ZHMenuStrip()
        {
            this.Renderer = new ZHRender();
            this.ForeColor = Color.FromArgb(241, 240, 240);
            this.Height = 54;
            this.BackgroundImage = Properties.Resources.Rectangle;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.AutoSize = false;
            this.Padding = new Padding(0, 0, 0, 0);
            this.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            if ((Screen.PrimaryScreen.Bounds.Width - 1225) / 2 > (82 + 22))
            {
                ToolStripMenuItem menuItem1 = new ToolStripMenuItem();
                menuItem1.Image = Properties.Resources.Icon;
                menuItem1.DisplayStyle = ToolStripItemDisplayStyle.Image;
                menuItem1.ImageScaling = ToolStripItemImageScaling.None;
                menuItem1.Margin = new System.Windows.Forms.Padding(22, 0, (Screen.PrimaryScreen.Bounds.Width - 1225) / 2 - 82 - 22, 0);
                menuItem1.ImageAlign = ContentAlignment.MiddleCenter;
                this.Items.Add(menuItem1);
            }            
        }

        public static ToolStripMenuItem ActiveMenuItem
        {
            get
            {
                return activeMenuItem;
            }
            set
            {
                activeMenuItem = value;
                SetImage(activeMenuItem);
            }
        }

        public ToolStripMenuItem FindCurrentItem(string text)
        {
            return FindCurrentItem(text, null);
        }

        public ToolStripMenuItem FindCurrentItem(string text, ToolStripMenuItem item)
        {
            ToolStripItemCollection tsic = this.Items;
            if (item != null)
            {
                tsic = item.DropDownItems;
            }

            foreach (ToolStripMenuItem tsmi in tsic)
            {
                if (tsmi.Text.Equals(text))
                {
                    return tsmi;
                }

                if (tsmi.DropDownItems.Count != 0)
                {
                    ToolStripMenuItem ts = FindCurrentItem(text, tsmi);
                    if (ts != null)
                    {
                        return ts;
                    }
                }
            }

            return null;
        }

        public void SetImageByText(string text)
        {
            ToolStripMenuItem tsmi = FindCurrentItem(text);
            if (tsmi != null)
            {
                ActiveMenuItem = tsmi;
            }
        }

        public static void SetImage(ToolStripMenuItem activeMenuItem)
        {
            if (activeMenuItem.DropDownItems.Count == 0)
            {
                if (lastMenuItem != null)
                {
                    GetTopItem(lastMenuItem).Image = Properties.Resources.Transparent;  
                }
                GetTopItem(activeMenuItem).Image = Properties.Resources.Triangle;             
                lastMenuItem = activeMenuItem;
            }
        }

        private static ToolStripMenuItem GetTopItem(ToolStripMenuItem item)
        {
            while (item.OwnerItem != null)
            {
                item = (ToolStripMenuItem)item.OwnerItem;
            }
            return item;
        }


    }
}
