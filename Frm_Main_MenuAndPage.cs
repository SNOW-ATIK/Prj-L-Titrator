using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using L_Titrator.Pages;
using L_Titrator.Controls;
using ATIK;

namespace L_Titrator
{
    public partial class Frm_Main : Form
    {
        private enum MenuAndPage
        {
            NONE,
            MAIN,
            SETTING,
            DEVICE,
            LIFE_TIME,
            HISTORY,
        }

        private MenuAndPage MenuCurrent = MenuAndPage.NONE;

        private Frm_PopUpMenu PopupMenu_Setting;
        private Frm_PopUpMenu PopupMenu_History;
        private Dictionary<MenuAndPage, Button> Dic_Menu = new Dictionary<MenuAndPage, Button>();
        private Dictionary<MenuAndPage, IPage> Dic_Page = new Dictionary<MenuAndPage, IPage>();

        private Page_Main PageMain = new Page_Main();
        private Page_Setting PageSetting = new Page_Setting();
        private Page_Device PageDevice = new Page_Device();
        private Page_LifeTime PageLifeTime = new Page_LifeTime();
        private Page_History PageHistory = new Page_History();

        private void InitMenuAndPage()
        {
            // Init Menu
            Dic_Menu.Add(MenuAndPage.MAIN, btn_Main);
            Dic_Menu.Add(MenuAndPage.SETTING, btn_Setting);
            Dic_Menu.Add(MenuAndPage.DEVICE, btn_Device);
            Dic_Menu.Add(MenuAndPage.LIFE_TIME, btn_LifeTime);
            Dic_Menu.Add(MenuAndPage.HISTORY, btn_History);

            Dic_Menu.Values.ToList().ForEach(btn => btn.BackColor = PreDef.MenuBG_Unselect);

            CreateSubMenus();

            // Init Page
            Dic_Page.Add(MenuAndPage.MAIN, PageMain);
            Dic_Page.Add(MenuAndPage.SETTING, PageSetting);
            Dic_Page.Add(MenuAndPage.DEVICE, PageDevice);
            Dic_Page.Add(MenuAndPage.LIFE_TIME, PageLifeTime);
            Dic_Page.Add(MenuAndPage.HISTORY, PageHistory);

            Dic_Page.Values.ToList().ForEach(page =>
            {
                page.SetMargin(new Padding(1, 1, 1, 1));
                page.SetDock(DockStyle.Fill);
                page.SetVisible(false);
                pnl_MainView.Controls.Add((UserControl)page);
            });

            PageMain.Init();
            PageSetting.Init();
            PageDevice.Init();
            PageLifeTime.Init();

            // Set Main as Start
            MenuCurrent = MenuAndPage.MAIN;
            Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Select;
            Dic_Page[MenuCurrent].SetVisible(true);

            // Register UserControl's Events
            usrCtrl_Login1.UserAuthorityChangedEvent += UsrCtrl_Login1_UserAuthorityChangedEvent;
        }

        private void UsrCtrl_Login1_UserAuthorityChangedEvent(UserAuthority authority)
        {
            btn_Main.PerformClick();
        }

        private void CreateSubMenus()
        {
            Point menuLoc = PointToScreen(tbl_MainMenu.Location);

            Point settingLoc = new Point(menuLoc.X + btn_Setting.Location.X, menuLoc.Y + btn_Setting.Location.Y);
            PopupMenu_Setting = new Frm_PopUpMenu(new string[] { "CONFIG", "OPTION", "RECIPE" }, settingLoc);
            PopupMenu_Setting.Tag = MenuAndPage.SETTING;
            PopupMenu_Setting.SubMenuClickedEvent += PopupMenu_SubMenuClicked;

            Point historyLoc = new Point(menuLoc.X + btn_History.Location.X, menuLoc.Y + btn_History.Location.Y);
            PopupMenu_History = new Frm_PopUpMenu(new string[] { "ALARM", "DATA" }, historyLoc);
            PopupMenu_History.Tag = MenuAndPage.HISTORY;
            PopupMenu_History.SubMenuClickedEvent += PopupMenu_SubMenuClicked;
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            MenuAndPage clickedMenu = (MenuAndPage)Enum.Parse(typeof(MenuAndPage), (string)btn.Tag);
            switch (clickedMenu)
            {
                case MenuAndPage.MAIN:
                    break;

                case MenuAndPage.SETTING:
                case MenuAndPage.HISTORY:
                    Point menuLoc = PointToScreen(tbl_MainMenu.Location);
                    Point subMenuLoc = new Point();
                    if (clickedMenu == MenuAndPage.SETTING)
                    {
                        subMenuLoc.X = menuLoc.X + btn_Setting.Location.X;
                        subMenuLoc.Y = menuLoc.Y + btn_Setting.Location.Y;
                        PopupMenu_Setting.SetShowLocation(subMenuLoc);
                        PopupMenu_Setting.Show();
                    }
                    else
                    {
                        subMenuLoc.X = menuLoc.X + btn_History.Location.X;
                        subMenuLoc.Y = menuLoc.Y + btn_History.Location.Y;
                        PopupMenu_History.SetShowLocation(subMenuLoc);
                        PopupMenu_History.Show();
                    }
                    break;
            }

            if (MenuCurrent != clickedMenu)
            {
                if (MenuCurrent == MenuAndPage.SETTING || MenuCurrent == MenuAndPage.HISTORY)
                {
                    // Restore to default text (Remove sub-menu name)
                    Dic_Menu[MenuCurrent].Text = Convert.ToString(Dic_Menu[MenuCurrent].Tag);
                }

                if (clickedMenu == MenuAndPage.SETTING || clickedMenu == MenuAndPage.HISTORY)
                {
                }
                else
                {
                    if (MenuCurrent != MenuAndPage.NONE)
                    {

                        // Hide previous
                        Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Unselect;
                        Dic_Page[MenuCurrent].SetVisible(false);

                        // Switch variable
                        MenuCurrent = clickedMenu;

                        // Show current
                        Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Select;
                        Dic_Page[MenuCurrent].SetVisible(true);
                    }
                }
            }
        }

        private void PopupMenu_SubMenuClicked(string subMenu)
        {
            if (MenuCurrent == MenuAndPage.NONE) 
            {
                return;
            }

            Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Unselect;
            MenuAndPage clickedMenu = MenuAndPage.NONE;
            if (subMenu == "CONFIG" || subMenu == "OPTION" || subMenu == "RECIPE")
            {
                clickedMenu = MenuAndPage.SETTING;
            }
            else if (subMenu == "ALARM" || subMenu == "DATA")
            {
                clickedMenu = MenuAndPage.HISTORY;
            }
            string pageName = $"{Convert.ToString(clickedMenu)}_{subMenu}";


            // Hide previous
            MenuAndPage PrevPage = MenuCurrent;
            Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Unselect;
            if (PrevPage != clickedMenu)
            {
                Dic_Page[MenuCurrent].SetVisible(false);
            }

            // Switch variable
            MenuCurrent = clickedMenu;

            // Show current
            Dic_Menu[MenuCurrent].BackColor = PreDef.MenuBG_Select;
            Dic_Menu[MenuCurrent].Text = $"{Convert.ToString(Dic_Menu[MenuCurrent].Tag)}\r\n▶ {subMenu}";
            Dic_Page[MenuCurrent].ShowSubPage(subMenu);
            if (PrevPage != clickedMenu)
            {
                Dic_Page[MenuCurrent].SetVisible(true);
            }

            var rcpPage = (Page_Setting)Dic_Page[MenuCurrent];
            if (subMenu == "RECIPE")
            {
                rcpPage.Show_RightMenu(false);
                rcpPage.Show_BottomMenu(false);
            }
            else
            {
                rcpPage.Show_RightMenu(true);
                rcpPage.Show_BottomMenu(true);
            }
        }
    }
}
