using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HtmlBulletedList
{
	[DefaultProperty("InnerText")]
	[ToolboxData("<{0}:HtmlListItem runat=server></{0}:HtmlListItem>")]
	public class HtmlListItem : WebControl
	{
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string InnerText
		{
			get
			{
				String s = (String)ViewState["InnerText"];
				return ((s == null) ? String.Empty : s);
			}

			set
			{
				ViewState["InnerText"] = (value == null) ? String.Empty : value;
				ViewState["InnerHtml"] = System.Web.HttpUtility.HtmlEncode((string)(ViewState["InnerText"]));
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Localizable(true)]
		public string InnerHtml
		{
			get
			{
				String s = (String)ViewState["InnerHtml"];
				return ((s == null) ? String.Empty : s);
			}

			set
			{
				ViewState["InnerHtml"] = (value == null) ? String.Empty : value;
				ViewState["InnerText"] = System.Web.HttpUtility.HtmlDecode((string)(ViewState["InnerHtml"]));
			}
		}

		protected override void RenderContents(HtmlTextWriter output)
		{
			output.Write(this.InnerHtml);
			this.RenderChildren(output);
		}

		protected override string TagName
		{
			get
			{
				return "li";
			}
		}

		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return System.Web.UI.HtmlTextWriterTag.Li;
			}
		}
	}
}
