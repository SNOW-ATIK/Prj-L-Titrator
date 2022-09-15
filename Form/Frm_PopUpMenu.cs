using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_Titrator_Alpha.Controls
{
    public partial class Frm_PopUpMenu : Form
    {
        private const int BtnHeight = 60;

        public delegate void SubMenuClicked(string clickedMenuName);
        public event SubMenuClicked SubMenuClickedEvent;

        private int CntOfSubMenus = 0;

        public Frm_PopUpMenu(string[] subMenus, Point menuTopLeftCorner)
        {
            InitializeComponent();

            CntOfSubMenus = subMenus.Length;

            this.StartPosition = FormStartPosition.Manual;

            this.Height = BtnHeight * CntOfSubMenus + 4;
            this.Location = new Point(menuTopLeftCorner.X, menuTopLeftCorner.Y - BtnHeight * CntOfSubMenus - 5);

            for (int i = 0; i < CntOfSubMenus; i++)
            {
                Button btn = new Button();
                btn.TabStop = false;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Size = new Size(this.Width - 6, BtnHeight - 2);
                btn.Margin = new Padding(1, 1, 1, 1);
                btn.Font = new Font("Consolas", 14f, FontStyle.Bold);
                btn.Text = subMenus[i];
                btn.Click += MenuClicked;
                btn.MouseDown += Btn_MouseDown;
                btn.Location = new Point(2, BtnHeight * i + 2);
                panel1.Controls.Add(btn);
            }

            this.LostFocus += Frm_PopUpMenu_LostFocus;
            this.Focus();
            this.TopMost = true;
        }

        public void SetShowLocation(Point menuTopLeftCorner)
        {
            this.Location = new Point(menuTopLeftCorner.X, menuTopLeftCorner.Y - BtnHeight * CntOfSubMenus - 5);
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            SubMenuClickedEvent?.Invoke(btn.Text);
        }

        private void Frm_PopUpMenu_LostFocus(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void MenuClicked(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
