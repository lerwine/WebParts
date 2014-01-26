using System;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using Microsoft.SharePoint.Utilities;

namespace MenuTree
{
	class MenuTreeToolPart : ToolPart, System.Web.UI.INamingContainer
	{
		public MenuTreeToolPart()
		{
			this.Init += new EventHandler(MenuTreeToolPart_Init);
			this.EnableViewState = true;
			this.AllowMinimize = true;
			this.FrameType = FrameType.Default;
			this.Title = "Menu Options";
		}

		protected override void AddedControl(Control control, int index)
		{
			base.AddedControl(control, index);
		}
		private void MenuTreeToolPart_Init(object sender, EventArgs e)
		{
		    // ((MenuTree)(this.ParentToolPane.SelectedWebPart)).MenuType
		}

		protected override object SaveViewState()
		{
			return new object[] { base.SaveViewState() };
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(((object[])(savedState))[0]);
		}

		protected override void RenderToolPart(System.Web.UI.HtmlTextWriter output)
		{
			this.EnsureChildControls();
		}
	}
}
