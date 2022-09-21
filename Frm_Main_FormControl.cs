using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using L_Titrator.Pages;
using L_Titrator.Controls;

namespace L_Titrator
{
    public partial class Frm_Main : Form
    {
        private void btn_Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static Point mousePoint;
        private void lbl_AppTitle_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.NoMove2D;
        }

        private void lbl_AppTitle_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void lbl_AppTitle_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void lbl_AppTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Location = new Point(this.Left - (mousePoint.X - e.X), this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void lbl_AppTitle_DoubleClick(object sender, EventArgs e)
        {
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            this.Location = new Point(screenSize.Width / 2 - this.Size.Width / 2, screenSize.Height / 2 - this.Size.Height / 2);
        }
    }
}
