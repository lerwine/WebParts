using System;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using Microsoft.SharePoint.Utilities;

namespace $safeprojectname$
{
	class $commonprefix$ToolPart : ToolPart, System.Web.UI.INamingContainer
	{
		public $commonprefix$ToolPart()
		{
			//this.Init += new EventHandler($commonprefix$ToolPart_Init);
			this.EnableViewState = true;
			this.AllowMinimize = true;
			this.FrameType = FrameType.Default;
			this.Title = "$toolparttitle$";
		}

		//private void $commonprefix$ToolPart_Init(object sender, EventArgs e)
		//{
		//    (($safeprojectname$)(this.ParentToolPane.SelectedWebPart)).OptionChoice
		//}

		//protected override object SaveViewState()
		//{
		//    return new object[] { base.SaveViewState() };
		//}

		//protected override void LoadViewState(object savedState)
		//{
		//    base.LoadViewState(((object[])(savedState))[0]);
		//}

		protected override void RenderToolPart(System.Web.UI.HtmlTextWriter output)
		{
			this.EnsureChildControls();
		}
	}
}
