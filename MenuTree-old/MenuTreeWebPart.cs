using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;

namespace MenuTree
{
	/// <summary>
	/// Description for MenuType
	/// </summary>
	public enum MenuType { Basic = 0, Collapsible = 1, Horizontal = 2, Vertical = 3 };

	/// <summary>
	/// Description for MenuTree.
	/// </summary>
	[DefaultProperty("MenuData"),
		ToolboxData("<{0}:MenuTree runat=server></{0}:MenuTree>"),
		XmlRoot(Namespace = "MenuTree")]
	public class MenuTree : Microsoft.SharePoint.WebPartPages.WebPart, System.Web.UI.INamingContainer
	{
		private const MenuType defaultMenuType = MenuType.Basic;
		private const string defaultMenuData = "";

		private string menuData = defaultMenuData;
		private MenuType menuType;

		[Browsable(false),
			Category("Menu Options"),
			DefaultValue(defaultMenuData),
			WebPartStorage(Storage.Shared),
			FriendlyName("MenuData"),
			Description("XML Menu Data")]
		public string MenuData
		{
			get
			{
				return menuData;
			}

			set
			{
				menuData = value;
			}
		}

		public bool ShouldSerializeMenuData()
		{
			return true;
		}

		[Browsable(true),
			Category("Menu Options"),
			DefaultValue(defaultMenuType),
			WebPartStorage(Storage.Shared),
			FriendlyName("Menu Options"),
			Description("Select menu type")]
		public MenuType MenuType
		{
			get
			{
				return this.menuType;
			}
			set
			{
				this.menuType = value;
			}
		}

		public bool ShouldSerializeMenuType()
		{
			return true;
		}

		/// <summary>
		///	Constructor for the class.
		///	</summary>
		public MenuTree()
		{
		}

		protected override void CreateChildControls()
		{
			HtmlBulletedList.HtmlBulletedList list;
			HtmlBulletedList.HtmlListItem item;

			list = new HtmlBulletedList.HtmlBulletedList();
			item = new HtmlBulletedList.HtmlListItem();
			item.InnerText = (this.BrowserDesignMode) ? "Design Mode" : "Not design mode";
			list.Items.Add(item);

			item = new HtmlBulletedList.HtmlListItem();
			item.InnerText = this.menuType.ToString();
			list.Items.Add(item);

			list.ID = this.Qualifier + "Activity";

			this.Controls.Add(list);
		}

		/// <summary>
		///	This method gets the custom tool parts for this Web Part by overriding the
		///	GetToolParts method of the WebPart base class. You must implement
		///	custom tool parts in a separate class that derives from 
		///	Microsoft.SharePoint.WebPartPages.ToolPart. 
		///	</summary>
		///<returns>An array of references to ToolPart objects.</returns>
		public override ToolPart[] GetToolParts()
		{
			return new ToolPart[2] { new MenuTreeToolPart(), new WebPartToolPart() };
		}

		/// <summary>
		/// Render this Web Part to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderWebPart(HtmlTextWriter output)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Type, "menuData/javascript");
			output.AddAttribute("Language", "JavaScript1.2");
			output.RenderBeginTag(HtmlTextWriterTag.Script);
			output.Write("<!--\r\n");
			try
			{
				output.Write(this.GetEmbeddedFileContents("Embedded.js"));
			}
			catch (Exception exc)
			{
				output.Write("/* " + SPEncode.HtmlEncode(exc.Message) + " */");
			}
			output.Write("// -->");
			output.RenderEndTag();
		}

		public string GetEmbeddedFileContents(string strResourceFile)
		{
			System.IO.StreamReader sr = null;
			string strText;
			System.IO.Stream stream = null;

			try
			{
				stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MenuTree." +
					strResourceFile);
				sr = new System.IO.StreamReader(stream);
				strText = this.ReplaceTokens(sr.ReadToEnd());
			}
			catch
			{
				throw;
			}
			finally
			{
				if (sr != null) sr.Close();
				if (stream != null) stream.Close();
			}

			return strText;
		}
	}
}
