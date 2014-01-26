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
	class GDToolPart : ToolPart, System.Web.UI.INamingContainer
	{
		private System.Xml.XmlDocument xmlDocMenuData;
		private XmlNode editingNode;
		private string strEditingNode;
		private bool bNeedsReLoad = false;
		private int nDeletePosition = 0;

		public GDToolPart()
		{
			this.Init += new EventHandler(GDToolPart_Init);
			this.EnableViewState = true;
			this.strEditingNode = "/menu";
			this.AllowMinimize = true;
			this.FrameType = FrameType.Default;
			this.Title = "Menu Items";
		}

		private void GDToolPart_Init(object sender, EventArgs e)
		{
			System.Xml.XmlNode node;

			this.xmlDocMenuData = ((MenuTree)(this.ParentToolPane.SelectedWebPart)).XmlDocument;

			this.editingNode = this.xmlDocMenuData.SelectSingleNode(this.strEditingNode);
		}

		protected override void LoadViewState(object savedState)
		{
			this.strEditingNode = Convert.ToString(((object[])(savedState))[0]).Trim();
			this.xmlDocMenuData = (System.Xml.XmlDocument)(((object[])(savedState))[1]);
			this.editingNode = this.xmlDocMenuData.SelectSingleNode(this.strEditingNode);
			this.nDeletePosition = Convert.ToInt32(((object[])(savedState))[2]);
			base.LoadViewState(((object[])(savedState))[3]);
		}

		protected override object SaveViewState()
		{
			return new object[] { this.strEditingNode, this.xmlDocMenuData, this.nDeletePosition, base.SaveViewState() };
		}

		protected override void CreateChildControls()
		{
			XmlNode node;
			Button button;
			TextBox textBox;

			button = new Button();
			button.ID = "DeleteConfirm";
			button.Text = "Yes";
			button.CommandName = "Delete";
			button.CommandArgument = this.nDeletePosition.ToString();
			button.Command += new CommandEventHandler(button_Command);
			this.Controls.Add(button);

			button = new Button();
			button.ID = "DeleteDeny";
			button.Text = "No";
			button.CommandName = "NoDelete";
			button.CommandArgument = this.nDeletePosition.ToString();
			button.Command += new CommandEventHandler(button_Command);
			this.Controls.Add(button);

			this.createAncestorDataGrid();

			textBox = new TextBox();
			textBox.ID = "Display";
			textBox.Width = new Unit("180px");
			if ((node = this.editingNode.SelectSingleNode("child::display")) != null)
				textBox.Text = node.InnerText.Trim();
			else
				textBox.Text = " * Display text not found * ";
			this.Controls.Add(textBox);

			textBox = new TextBox();
			textBox.ID = "URL";
			textBox.Width = new Unit("180px");
			if ((node = this.editingNode.SelectSingleNode("child::url")) != null)
				textBox.Text = node.InnerText.Trim();
			else
				textBox.Text = " * URL text not found * ";
			this.Controls.Add(textBox);

			button = new Button();
			button.Text = "Save";
			button.CommandName = "Save";
			button.CommandArgument = "";
			button.Command += new CommandEventHandler(button_Command);
			button.ID = "SaveItem";
			this.Controls.Add(button);

			this.createChildDataGrid();

			textBox = new TextBox();
			textBox.ID = "NewDisplay";
			textBox.Width = new Unit("180px");
			textBox.Text = "";
			this.Controls.Add(textBox);

			textBox = new TextBox();
			textBox.ID = "NewURL";
			textBox.Width = new Unit("180px");
			textBox.Text = "";
			this.Controls.Add(textBox);

			button = new Button();
			button.Text = "Add New Item";
			button.CommandName = "New";
			button.Command += new CommandEventHandler(button_Command);
			button.ID = "NewItem";
			this.Controls.Add(button);
		}

		private void createChildDataGrid()
		{
			System.Data.DataTable itemTable;
			System.Data.DataRow dr;
			XmlNode node;
			XmlNodeList items;
			DataGrid dataGrid;
			ButtonColumn col;
			System.Data.DataView dv;

			items = this.editingNode.SelectNodes("child::menuItem");

			if (items.Count < 1)
				return;

			itemTable = new System.Data.DataTable();
			itemTable.Columns.Add(new System.Data.DataColumn("Position", typeof(Int32)));
			itemTable.Columns.Add(new System.Data.DataColumn("Display", typeof(string)));
			itemTable.Columns.Add(new System.Data.DataColumn("URL", typeof(string)));

			for (int n = 0; n < items.Count; n++)
			{
				dr = itemTable.NewRow();
				dr[0] = n + 1;
				if ((node = items.Item(n).SelectSingleNode("display")) == null)
					dr[1] = " * Display text not found * ";
				else
					dr[1] = node.InnerText;
				if ((node = items.Item(n).SelectSingleNode("url")) == null)
					dr[2] = " * URL text not found * ";
				else
					dr[2] = node.InnerText;
				itemTable.Rows.Add(dr);
			}

			dv = new System.Data.DataView(itemTable);

			dataGrid = new DataGrid();
			dataGrid.Caption = "Current Level Items";
			dataGrid.ID = "ChildItems";
			dataGrid.AllowPaging = false;
			dataGrid.AllowSorting = false;
			dataGrid.AutoGenerateColumns = false;
			dataGrid.BorderStyle = BorderStyle.Solid;
			dataGrid.BorderWidth = new Unit("1px");
			dataGrid.ShowHeader = false;
			dataGrid.ShowFooter = false;
			col = new ButtonColumn();
			col.ButtonType = ButtonColumnType.LinkButton;
			col.DataTextField = "Display";
			col.CommandName = "Select";
			dataGrid.Columns.Add(col);
			col = new ButtonColumn();
			col.ButtonType = ButtonColumnType.LinkButton;
			col.Text = "[delete]";
			col.CommandName = "Delete";
			dataGrid.Columns.Add(col);
			dataGrid.DataKeyField = "Position";
			dataGrid.GridLines = GridLines.Horizontal;
			dataGrid.ItemCommand += new DataGridCommandEventHandler(child_ItemCommand);
			dataGrid.DataSource = dv;
			dataGrid.DataBind();
			this.Controls.Add(dataGrid);
		}

		private void createAncestorDataGrid()
		{
			System.Data.DataTable itemTable;
			System.Data.DataRow dr;
			System.Data.DataView ancestorDataView;
			XmlNode node;
			XmlNodeList items;
			DataGrid dataGrid;
			ButtonColumn col;

			if (this.editingNode.Name == "menu")
				return;

			itemTable = new System.Data.DataTable();
			itemTable.Columns.Add(new System.Data.DataColumn("Step", typeof(Int32)));
			itemTable.Columns.Add(new System.Data.DataColumn("Display", typeof(string)));
			itemTable.Columns.Add(new System.Data.DataColumn("Text", typeof(string)));
			itemTable.Columns.Add(new System.Data.DataColumn("URL", typeof(string)));

			items = this.editingNode.SelectNodes("ancestor::menuItem");

			dr = itemTable.NewRow();
			dr[0] = items.Count + 1;
			dr[1] = "Top Level";
			dr[2] = "... [Up to Top Level]";
			dr[3] = "";
			itemTable.Rows.Add(dr);

			for (int n = items.Count; n > 0; n--)
			{
				dr = itemTable.NewRow();
				dr[0] = n;
				if ((node = items.Item(n - 1).SelectSingleNode("display")) == null)
				{
					dr[1] = " * Display text not found * ";
					dr[2] = "... [Up]";
				}
				else
				{
					dr[1] = node.InnerText;
					dr[2] = "... [Up to " + node.InnerText + "]";
				}
				if ((node = items.Item(n - 1).SelectSingleNode("url")) == null)
					dr[3] = " * URL text not found * ";
				else
					dr[3] = node.InnerText;
				itemTable.Rows.Add(dr);
			}

			ancestorDataView = new System.Data.DataView(itemTable);

			dataGrid = new DataGrid();
			dataGrid.ID = "AncestorItems";
			dataGrid.AllowPaging = false;
			dataGrid.AllowSorting = false;
			dataGrid.AutoGenerateColumns = false;
			col = new ButtonColumn();
			col.ButtonType = ButtonColumnType.LinkButton;
			col.DataTextField = "Text";
			col.CommandName = "Up";
			dataGrid.Columns.Add(col);
			dataGrid.DataKeyField = "Step";
			dataGrid.GridLines = GridLines.None;
			dataGrid.ItemCommand += new DataGridCommandEventHandler(ancestors_ItemCommand);
			dataGrid.ShowFooter = false;
			dataGrid.ShowHeader = false;
			dataGrid.DataSource = ancestorDataView;
			dataGrid.DataBind();
			this.Controls.Add(dataGrid);
		}

		private void button_Command(object sender, CommandEventArgs e)
		{
			XmlNode node, subNode;
			Control control;

			switch (e.CommandName)
			{
				case "Save":
					if ((node = this.editingNode.SelectSingleNode("child::display")) != null &&
							(control = this.FindControl("Display")) != null)
						node.InnerText = ((TextBox)control).Text.Trim();
					if ((node = this.editingNode.SelectSingleNode("child::url")) != null &&
							(control = this.FindControl("URL")) != null)
						node.InnerText = ((TextBox)control).Text.Trim();
					this.bNeedsReLoad = true;
					break;
				case "New":
					node = this.xmlDocMenuData.CreateNode("element", "menuItem", "");
					subNode = this.xmlDocMenuData.CreateNode("element", "display", "");
					if ((control = this.FindControl("NewDisplay")) != null)
					{
						subNode.InnerText = ((TextBox)control).Text.Trim();
						((TextBox)control).Text = "";
					}
					else
						subNode.InnerText = " * Display text not found * ";
					node.AppendChild(subNode);
					subNode = this.xmlDocMenuData.CreateNode("element", "url", "");
					if ((control = this.FindControl("NewURL")) != null)
					{
						subNode.InnerText = ((TextBox)control).Text.Trim();
						((TextBox)control).Text = "";
					}
					else
						subNode.InnerText = " * Display text not found * ";
					node.AppendChild(subNode);
					this.editingNode.AppendChild(node);
					this.bNeedsReLoad = true;
					break;
				case "Delete":
					if ((node = this.editingNode.SelectSingleNode("child::menuItem[position()=" +
						Convert.ToString(this.nDeletePosition) + "]")) != null)
						this.editingNode.RemoveChild(node);
					this.nDeletePosition = 0;
					this.bNeedsReLoad = true;
					break;
				case "NoDelete":
					this.nDeletePosition = 0;
					this.bNeedsReLoad = true;
					break;
			}
		}

		private void ancestors_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			XmlAttribute attrNode;

			if (e.CommandName != "Up")
				return;

			if (this.editingNode.Name != "menu")
				this.editingNode.Attributes.RemoveNamedItem("current");

			for (int i = 0; i < Convert.ToInt32(((DataGrid)source).DataKeys[e.Item.ItemIndex]); i++)
			{
				if (this.editingNode.Name == "menu")
					break;
				this.editingNode = this.editingNode.ParentNode;
			}
			this.strEditingNode = this.BuildXpath(this.editingNode);
			attrNode = this.xmlDocMenuData.CreateAttribute("current");
			attrNode.InnerText = "true";
			this.editingNode.Attributes.Append(attrNode);
			this.bNeedsReLoad = true;
		}

		private void child_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			XmlNode node;

			switch (e.CommandName)
			{
				case "Select":
					if ((node = this.editingNode.SelectSingleNode("child::menuItem[position()=" +
						Convert.ToString(((DataGrid)source).DataKeys[e.Item.ItemIndex]) + "]")) != null)
						this.editingNode = node;
					this.strEditingNode = this.BuildXpath(this.editingNode);
					this.bNeedsReLoad = true;
					break;
				case "Delete":
					this.nDeletePosition = Convert.ToInt32(((DataGrid)source).DataKeys[e.Item.ItemIndex]);
					this.bNeedsReLoad = true;
					break;
			}
		}

		public override void ApplyChanges()
		{
			((MenuTree)(this.ParentToolPane.SelectedWebPart)).XmlDocument = this.xmlDocMenuData;

			base.ApplyChanges();
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (this.bNeedsReLoad)
			{
				this.Controls.Clear();
				this.CreateChildControls();
			}

			base.OnPreRender(e);
		}

		protected override void RenderToolPart(System.Web.UI.HtmlTextWriter output)
		{
			Control control;

			this.EnsureChildControls();

			if (this.nDeletePosition > 0)
			{
				this.renderDeleteTable(output);
				return;
			}

			if ((control = this.FindControl("AncestorItems")) != null)
			{
				control.RenderControl(output);
				output.RenderBeginTag(HtmlTextWriterTag.Hr);
				output.RenderEndTag();
			}

			if (this.editingNode.Name == "menu")
			{
				output.AddAttribute(HtmlTextWriterAttribute.Align, "center");
				output.RenderBeginTag(HtmlTextWriterTag.Div);
				output.RenderBeginTag(HtmlTextWriterTag.Strong);
				output.Write("Top Level");
				output.RenderEndTag();
				output.RenderEndTag();
			}
			else
			{
				this.renderCurrentItemTable(output);
				output.RenderBeginTag(HtmlTextWriterTag.Hr);
				output.RenderEndTag();
			}

			if ((control = this.FindControl("ChildItems")) != null)
			{
				control.RenderControl(output);
				output.RenderBeginTag(HtmlTextWriterTag.Hr);
				output.RenderEndTag();
			}

			this.renderNewItemTable(output);
		}

		private void renderDeleteTable(HtmlTextWriter output)
		{
			Control control;
			XmlNode node;

			output.AddAttribute(HtmlTextWriterAttribute.Border, "1");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			output.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			#region Caption

			output.RenderBeginTag(HtmlTextWriterTag.Caption);

			output.RenderBeginTag(HtmlTextWriterTag.Strong);
			if ((node = this.editingNode.SelectSingleNode("child::menuItem[position()=" +
				Convert.ToString(this.nDeletePosition) + "]/display")) != null)
				output.Write("Deleting: " + node.InnerText);
			else
				output.Write("Deleteing unknown item");
			output.RenderEndTag();
			if ((node = this.editingNode.SelectSingleNode("child::menuItem[position()=" +
				Convert.ToString(this.nDeletePosition) + "]/url")) != null)
				output.Write(" (" + node.InnerText + ")");

			output.RenderEndTag();

			#endregion

			#region Row
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			#region Contents

			output.Write("Deleting this item cannot be undone.");
			output.RenderBeginTag(HtmlTextWriterTag.P);
			output.Write("This will also delete any descendants of this menu item as well.");
			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.P);
			output.RenderBeginTag(HtmlTextWriterTag.Strong);
			output.Write("Are you sure?");
			output.RenderEndTag();
			output.RenderEndTag();

			if ((control = this.FindControl("DeleteConfirm")) != null)
				control.RenderControl(output);

			if ((control = this.FindControl("DeleteDeny")) != null)
				control.RenderControl(output);

			#endregion

			output.RenderEndTag();
			output.RenderEndTag();

			#endregion

			output.RenderEndTag();
		}

		private void renderCurrentItemTable(HtmlTextWriter output)
		{
			Control control;
			XmlNode node;

			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);

			output.RenderBeginTag(HtmlTextWriterTag.Caption);
			output.RenderBeginTag(HtmlTextWriterTag.Strong);
			if ((node = this.editingNode.SelectSingleNode("child::display")) != null)
				output.Write(SPEncode.HtmlEncode(node.InnerText));
			else
				output.Write(" * Text not found * ");
			output.RenderEndTag();
			output.RenderEndTag();

			if ((control = this.FindControl("Display")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("Text:");
				output.RenderEndTag();
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			if ((control = this.FindControl("URL")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("URL:");
				output.RenderEndTag();
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			if ((control = this.FindControl("SaveItem")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			output.RenderEndTag();
		}

		private void renderNewItemTable(HtmlTextWriter output)
		{
			Control control;

			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.RenderBeginTag(HtmlTextWriterTag.Table);
			if ((control = this.FindControl("NewDisplay")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("Text:");
				output.RenderEndTag();
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			if ((control = this.FindControl("NewURL")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				output.Write("URL:");
				output.RenderEndTag();
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			if ((control = this.FindControl("NewItem")) != null)
			{
				output.RenderBeginTag(HtmlTextWriterTag.Tr);
				output.AddAttribute(HtmlTextWriterAttribute.Align, "right");
				output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
				output.RenderBeginTag(HtmlTextWriterTag.Td);
				control.RenderControl(output);
				output.RenderEndTag();
				output.RenderEndTag();
			}
			output.RenderEndTag();
		}

		private string BuildXpath(XmlNode node)
		{
			XmlNodeList nodeList;

			if (node == null)
				return null;

			if (node.Name == "#document" || node.ParentNode.Name == "#document")
				return "/" + node.Name;

			nodeList = node.SelectNodes("preceding-sibling::" + node.Name);

			return this.BuildXpath(node.ParentNode) + "/" + node.Name + "[position()=" + Convert.ToString(nodeList.Count + 1) + "]";
		}
	}
}
