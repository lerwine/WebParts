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
	/// Description for MenuTypeEnum
	/// </summary>
	public enum MenuTypeEnum
	{
		[XmlEnum("Basic Nested List")]
		Basic_Nested_List = 0,
		[XmlEnum("Collapsable Nested List")]
		Collapsable_List = 1,
		[XmlEnum("Horizontal Pull-down")]
		Horizontal_Menu = 2,
		[XmlEnum("Vertical Fly-out")]
		Vertical_Menu = 3
	};

	/// <summary>
	/// Description for MenuTree.
	/// </summary>
	[DefaultProperty("ShowTitle"),
		ToolboxData("<{0}:MenuTree runat=server></{0}:MenuTree>"),
		XmlRoot(Namespace = "MenuTree")]
	public class MenuTree : Microsoft.SharePoint.WebPartPages.WebPart, System.Web.UI.INamingContainer
	{
		private const MenuTypeEnum defaultMenuType = MenuTypeEnum.Basic_Nested_List;
		private const bool defaultShowTitle = false;
		private const bool defaultAutoFlyOut = true;
		private const string defaultMenuData = "";

		private bool showTitle = defaultShowTitle;
		private MenuTypeEnum menuType = defaultMenuType;
		private string menuData = defaultMenuData;
		private bool autoFlyOut = defaultAutoFlyOut;

		#region WEBPART PROPERTIES
		[Browsable(true),
			Category("Menu Options"),
			DefaultValue(defaultShowTitle),
			WebPartStorage(Storage.Shared),
			FriendlyName("Show Title"),
			Description("Show title inline")]
		public bool ShowTitle
		{
			get
			{
				return showTitle;
			}

			set
			{
				showTitle = value;
			}
		}

		public bool ShouldSerializeShowTitle()
		{
			return true;
		}

		[Browsable(true),
			Category("Menu Options"),
			DefaultValue(defaultMenuType),
			WebPartStorage(Storage.Shared),
			FriendlyName("Menu Type"),
			Description("Type of menu to display")]
		public MenuTypeEnum MenuType
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

		[Browsable(true),
		Category("Menu Options"),
	    DefaultValue(defaultAutoFlyOut),
		WebPartStorage(Storage.Shared),
		FriendlyName("Auto-open menus on hover"),
		Description("If not checked, a mouse click is required to open a menu item. Applies to Vertical and Horizontal menus.")]
		public bool AutoFlyOut
		{
			get
			{
				return this.autoFlyOut;
			}
			set
			{
				this.autoFlyOut = value;
			}
		}

		[Browsable(false),
		    Category("Internal"),
		    DefaultValue(defaultMenuData),
			WebPartStorage(Storage.Shared),
			FriendlyName("Menu Data"),
			Description("XML Menu Data")]
		public string MenuData
		{
			get
			{
				return this.menuData;
			}
			set
			{
				this.menuData = value;
			}
		}

		public bool ShouldSerializeMenuData()
		{
			return true;
		}
		#endregion

		public System.Xml.XmlDocument XmlDocument
		{
			get
			{
				System.Xml.XmlDocument xmlDoc;
				System.Xml.XmlElement rootNode;
				xmlDoc = new System.Xml.XmlDocument();

				if (this.menuData == null || this.menuData.Trim() == "")
				{
					rootNode = xmlDoc.CreateElement("menu");
					xmlDoc.AppendChild(rootNode);
				}
				else
				{

					try
					{
						xmlDoc.LoadXml(this.menuData.Trim());
					}
					catch
					{
						xmlDoc = new System.Xml.XmlDocument();
						rootNode = xmlDoc.CreateElement("menu");
						xmlDoc.AppendChild(rootNode);
					}
				}

				return xmlDoc;
			}
			set
			{
			}
		}

		/// <summary>
		///	Constructor for the class.
		///	</summary>
		public MenuTree()
		{
		}

		protected override void CreateChildControls()
		{
			System.Xml.XmlDocument xmlDoc;
			System.Xml.XmlNode rootNode;
			Control control;

			xmlDoc = this.XmlDocument;

			if ((rootNode = xmlDoc.SelectSingleNode("/menu")) == null)
				return;

			if (this.menuType == MenuTypeEnum.Basic_Nested_List || this.menuType == MenuTypeEnum.Collapsable_List)
				control = (Control)(this.BuildMenuList(rootNode));
			else
				control = (Control)(this.BuildMenuTable(rootNode));

			this.Controls.Add(control);
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
			return new ToolPart[2] { new GDToolPart(), new WebPartToolPart() };
		}

		/// <summary>
		/// Render this Web Part to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderWebPart(HtmlTextWriter output)
		{
			this.EnsureChildControls();

			output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
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

		private HtmlBulletedList.HtmlBulletedList BuildSubmenu(System.Xml.XmlNode parentNode)
		{
			HtmlBulletedList.HtmlBulletedList subMenu, childMenu;
			HtmlBulletedList.HtmlListItem item;
			System.Xml.XmlNodeList childNodes;
			System.Web.UI.HtmlControls.HtmlGenericControl g;
			System.Web.UI.HtmlControls.HtmlImage image;
			System.Web.UI.HtmlControls.HtmlAnchor anchor;
			System.Xml.XmlNode itemNode;
			string strUrl;

			childNodes = parentNode.SelectNodes("child::menuItem");

			subMenu = new HtmlBulletedList.HtmlBulletedList();
			subMenu.Attributes.Add("class", "ms-WPMenu");

			if (this.menuType == MenuTypeEnum.Basic_Nested_List)
				subMenu.HtmlListType = HtmlBulletedList.HtmlListType.Unordered;
			else
			{
				subMenu.HtmlListType = HtmlBulletedList.HtmlListType.None;
				subMenu.Style.Add("display", "none");
				if (this.menuType == MenuTypeEnum.Horizontal_Menu || this.menuType == MenuTypeEnum.Vertical_Menu)
					subMenu.Style.Add("position", "absolute");
			}

			foreach (System.Xml.XmlNode node in childNodes)
			{
				item = new HtmlBulletedList.HtmlListItem();
				childMenu = this.BuildSubmenu(node);

				if (childMenu.Items.Count > 0 && this.menuType == MenuTypeEnum.Collapsable_List)
				{
					image = new System.Web.UI.HtmlControls.HtmlImage();
					image.Attributes.Add("class", this.Qualifier + "ExpandImage");
					image.Height = 11;
					image.Width = 11;
					image.Src = "/_layouts/images/TPEXP.GIF";
					item.Controls.Add(image);
				}
				try
				{
					if ((itemNode = node.SelectSingleNode("url")) == null || (strUrl = itemNode.InnerText.Trim()) == "#")
						strUrl = "";
				}
				catch
				{
					strUrl = "";
				}

				try
				{
					if ((itemNode = node.SelectSingleNode("display")) == null)
					{
						g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
						g.InnerText = " * Text not found * ";
						item.Controls.Add(g);
					}
					else
					{
						if (strUrl == "")
							item.InnerText = itemNode.InnerText.Trim();
						else
						{
							anchor = new System.Web.UI.HtmlControls.HtmlAnchor();
							anchor.HRef = strUrl;
							anchor.InnerText = itemNode.InnerText.Trim();
							item.Controls.Add(anchor);
						}
					}
				}
				catch (Exception exc)
				{
					g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
					g.InnerText = " * Xml select error: " + exc.ToString() + " * ";
					item.Controls.Add(g);
				}

				if (childMenu.Items.Count > 0)
					item.Controls.Add(childMenu);

				subMenu.Items.Add(item);
			}

			return subMenu;
		}

		private HtmlBulletedList.HtmlBulletedList BuildMenuList(System.Xml.XmlNode topLevelNode)
		{
			System.Web.UI.HtmlControls.HtmlGenericControl g;
			System.Web.UI.HtmlControls.HtmlAnchor anchor;
			HtmlBulletedList.HtmlListItem item;
			HtmlBulletedList.HtmlBulletedList subMenu, childMenu;
			System.Xml.XmlNodeList childNodes;
			System.Xml.XmlNode itemNode;
			string strUrl;

			childNodes = topLevelNode.SelectNodes("child::menuItem");

			subMenu = new HtmlBulletedList.HtmlBulletedList();
			subMenu.ID = this.Qualifier + "Nav";

			foreach (System.Xml.XmlNode node in childNodes)
			{
				item = new HtmlBulletedList.HtmlListItem();

				try
				{
					if ((itemNode = node.SelectSingleNode("url")) == null || (strUrl = itemNode.InnerText.Trim()) == "#")
						strUrl = "";
				}
				catch
				{
					strUrl = "";
				}

				try
				{
					if ((itemNode = node.SelectSingleNode("display")) == null)
					{
						g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
						g.InnerText = " * Text not found * ";
						item.Controls.Add(g);
					}
					else
					{
						if (strUrl == "")
							item.InnerText = itemNode.InnerText.Trim();
						else
						{
							anchor = new System.Web.UI.HtmlControls.HtmlAnchor();
							anchor.HRef = strUrl;
							anchor.InnerText = itemNode.InnerText.Trim();
							item.Controls.Add(anchor);
						}
					}
				}
				catch (Exception exc)
				{
					g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
					g.InnerText = " * Xml select error: " + exc.ToString() + " * ";
					item.Controls.Add(g);
				}

				childMenu = this.BuildSubmenu(node);

				if (childMenu.Items.Count > 0)
					item.Controls.Add(childMenu);

				subMenu.Items.Add(item);
			}


			return subMenu;
		}

		private System.Web.UI.HtmlControls.HtmlTable BuildMenuTable(System.Xml.XmlNode topLevelNode)
		{
			System.Web.UI.HtmlControls.HtmlTable table;
			System.Web.UI.HtmlControls.HtmlTableRow row;
			System.Web.UI.HtmlControls.HtmlTableCell cell;
			System.Web.UI.HtmlControls.HtmlGenericControl g;
			System.Web.UI.HtmlControls.HtmlAnchor anchor;
			System.Web.UI.HtmlControls.HtmlImage image;
			HtmlBulletedList.HtmlBulletedList submenu;
			System.Xml.XmlNodeList childNodes;
			System.Xml.XmlNode itemNode;
			string strUrl;

			childNodes = topLevelNode.SelectNodes("child::menuItem");

			table = new System.Web.UI.HtmlControls.HtmlTable();
			table.ID = this.Qualifier + "Nav";
			table.Attributes["class"] = (this.menuType == MenuTypeEnum.Horizontal_Menu) ? "ms-toolbar" : "ms-nav";

			row = new System.Web.UI.HtmlControls.HtmlTableRow();

			if (this.showTitle)
			{
				if (this.menuType == MenuTypeEnum.Vertical_Menu)
				{
					cell = new System.Web.UI.HtmlControls.HtmlTableCell("th");
					cell.InnerText = this.Title;
					row.Controls.Add(cell);
					table.Rows.Add(row);
					row = new System.Web.UI.HtmlControls.HtmlTableRow();
				}
				else
				{
					cell = new System.Web.UI.HtmlControls.HtmlTableCell();
					g = new System.Web.UI.HtmlControls.HtmlGenericControl("strong");
					g.InnerText = this.Title;
					cell.Controls.Add(g);
					row.Cells.Add(cell);
				}
			}
			if (childNodes.Count < 1)
				return table;


			foreach (System.Xml.XmlNode node in childNodes)
			{
				cell = new System.Web.UI.HtmlControls.HtmlTableCell();
				cell.Style.Add("position", "relative");

				try
				{
					if ((itemNode = node.SelectSingleNode("url")) == null || (strUrl = itemNode.InnerText.Trim()) == "#")
						strUrl = "";
				}
				catch
				{
					strUrl = "";
				}

				try
				{
					if ((itemNode = node.SelectSingleNode("display")) == null)
					{
						g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
						g.InnerText = " * Text not found * ";
						cell.Controls.Add(g);
					}
					else
					{
						if (strUrl == "")
							cell.InnerText = itemNode.InnerText.Trim();
						else
						{
							anchor = new System.Web.UI.HtmlControls.HtmlAnchor();
							anchor.HRef = strUrl;
							anchor.InnerText = itemNode.InnerText.Trim();
							cell.Controls.Add(anchor);
						}
					}
				}
				catch (Exception exc)
				{
					g = new System.Web.UI.HtmlControls.HtmlGenericControl("em");
					g.InnerText = " * Xml select error: " + exc.ToString() + " * ";
					cell.Controls.Add(g);
				}

				submenu = this.BuildSubmenu(node);

				if (submenu.Items.Count > 0)
				{
					image = new System.Web.UI.HtmlControls.HtmlImage();
					cell.Controls.Add(submenu);

					if (this.menuType == MenuTypeEnum.Horizontal_Menu)
					{
						image.Height = 12;
						image.Width = 9;
						image.Src = "/_layouts/images/TPMAX1.GIF";
					}
					else
					{
						image.Height = 13;
						image.Width = 13;
						image.Src = "/_layouts/images/TPMIN.GIF";
					}
					cell.Controls.Add(image);
				}

				row.Cells.Add(cell);
				if (this.menuType == MenuTypeEnum.Vertical_Menu)
				{
					table.Rows.Add(row);
					row = new System.Web.UI.HtmlControls.HtmlTableRow();
				}
			}

			if (this.menuType == MenuTypeEnum.Horizontal_Menu)
				table.Rows.Add(row);

			return table;
		}
	}
}
