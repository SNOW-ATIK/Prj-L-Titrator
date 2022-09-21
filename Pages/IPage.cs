using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_Titrator.Pages
{
    interface IPage
    {
        void SetMargin(Padding margin);
        void SetDock(DockStyle dockStyle);
        void SetVisible(bool visible);
        void ShowSubPage(string subPageName);
        void PagingNext();
        void PagingPrev();
    }
}
