using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ATIK;

namespace L_Titrator.Controls
{
    public partial class Frm_PopUpMenu : Form
    {
        private const int BtnHeight = 56;
        private const int Cmp_Margin = 4;

        public delegate void SubMenuClicked(string clickedMenuName);
        public event SubMenuClicked SubMenuClickedEvent;

        private int CntOfSubMenus = 0;

        private string[] SubMenuNames;
        private string[] SubMenuTags;

        private List<Button> BtnList = new List<Button>();
        private List<bool> ShownBtnIdx = new List<bool>();

        public Frm_PopUpMenu(string[] subMenuName, string[] subMenuTag, Point menuTopLeftCorner)
        {
            InitializeComponent();

            SubMenuNames = subMenuName;
            SubMenuTags = subMenuTag;

            CntOfSubMenus = subMenuName.Length;

            this.StartPosition = FormStartPosition.Manual;

            this.Height = (BtnHeight + Cmp_Margin) * CntOfSubMenus + Cmp_Margin;
            this.Location = new Point(menuTopLeftCorner.X, menuTopLeftCorner.Y - this.Height - 2);

            for (int i = 0; i < CntOfSubMenus; i++)
            {
                Button btn = new Button();
                btn.TabStop = false;
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.White;
                btn.Location = new Point(Cmp_Margin, i * (BtnHeight + Cmp_Margin) + Cmp_Margin);
                btn.Size = new Size(this.Width - 2 * Cmp_Margin, BtnHeight);
                btn.Font = new Font("Consolas", 14f, FontStyle.Bold);
                btn.Text = subMenuName[i];
                btn.Tag = subMenuTag[i];
                btn.Click += MenuClicked;
                btn.MouseDown += Btn_MouseDown;
                pnl_View.Controls.Add(btn);
                BtnList.Add(btn);
                ShownBtnIdx.Add(true);
            }

            this.LostFocus += Frm_PopUpMenu_LostFocus;
            this.Focus();
            this.TopMost = true;
        }

        public void SetShowLocation(Point menuTopLeftCorner)
        {
            if (SubMenuNames.Contains("CONFIG") == true)
            {
                if (GlbVar.CurrentAuthority == UserAuthority.Admin)
                {
                    BtnList[0].Visible = true;
                    ShownBtnIdx[0] = true;
                    CntOfSubMenus = SubMenuNames.Length;
                }
                else
                {
                    BtnList[0].Visible = false;
                    ShownBtnIdx[0] = false;
                    CntOfSubMenus = SubMenuNames.Length - 1;
                }
            }
            this.Height = (BtnHeight + Cmp_Margin) * CntOfSubMenus + Cmp_Margin;
            int cntShow = 0;
            for (int i = 0; i < BtnList.Count; i++)
            {
                if (ShownBtnIdx[i] == true)
                {
                    BtnList[i].Location = new Point(Cmp_Margin, cntShow * (BtnHeight + Cmp_Margin) + Cmp_Margin);
                    ++cntShow;
                }
            }
            this.Location = new Point(menuTopLeftCorner.X, menuTopLeftCorner.Y - this.Height - 2);
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            SubMenuClickedEvent?.Invoke((string)btn.Tag);
        }

        private void Frm_PopUpMenu_LostFocus(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void MenuClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Frm_PopUpMenu_SizeChanged(object sender, EventArgs e)
        {
        }
    }
}
