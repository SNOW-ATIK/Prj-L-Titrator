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

namespace L_Titrator.Pages
{
    public partial class Page_Config : UserControl, IPage, IParamSetting
    {
        public Page_Config()
        {
            InitializeComponent();
        }

        public void SetDock(DockStyle dockStyle)
        {
            this.Dock = dockStyle;
        }

        public void SetMargin(Padding margin)
        {
            this.Margin = margin;
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
        }

        public void ShowSubPage(string subPageName)
        {
            throw new NotImplementedException();
        }

        public void PagingNext()
        {
            throw new NotImplementedException();
        }

        public void PagingPrev()
        {
            throw new NotImplementedException();
        }

        public void Restore()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.Restore());
        }

        public void UpdateChangedStatus()
        {
            ParamPageUtil.GetAll_IComps(this).ForEach(cmp => cmp.UpdateNamePlate());
        }

        public void SaveAllParams(bool askSave)
        {
            ParamPageUtil.GetAll_IParams(this).ForEach(prm => prm?.Save(true));
            UpdateChangedStatus();
        }

        public bool CheckParamChanged()
        {
            int changedCounts = 0;
            ParamPageUtil.GetAll_IParams(this).ForEach(prm =>
            {
                if (prm != null && prm.ValueObject.Equals(prm.ValueObject_Original) == false)
                {
                    ++changedCounts;
                }
            });
            return (changedCounts != 0);
        }
    }
}
