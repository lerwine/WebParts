using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace HtmlBulletedList
{
	public enum HtmlListType
	{
		[XmlEnum("Ordered")]
		Ordered,
		[XmlEnum("Unordered")]
		Unordered,
		[XmlEnum("Decimal")]
		Decimal,
		[XmlEnum("Decimal with Leading Zero")]
		DecimalLz,
		[XmlEnum("Roman Upper Case")]
		RomanUc,
		[XmlEnum("Roman Lower Case")]
		RomanLc,
		[XmlEnum("Greek Upper Case")]
		GreekUc,
		[XmlEnum("Greeek Lower Case")]
		GreekLc,
		[XmlEnum("Latin Upper Case")]
		LatinUc,
		[XmlEnum("Latin Lower Case")]
		LatinLc,
		[XmlEnum("Alpha Upper Case")]
		AlphaUc,
		[XmlEnum("Alpha Lower Case")]
		AlphaLc,
		[XmlEnum("Circle")]
		Circle,
		[XmlEnum("Disc")]
		Disc,
		[XmlEnum("Square")]
		Square,
		[XmlEnum("None")]
		None
	};

	[Category("Appearance"),
		DefaultProperty("HtmlListType"),
		ToolboxData("<{0}:HtmlBulletedList runat=server></{0}:HtmlBulletedList>")]
	public class HtmlBulletedList : WebControl
	{
		private HtmlListType listType = HtmlListType.Unordered;
		public HtmlListItemCollection Items;

		public HtmlBulletedList()
		{
			this.Items = new HtmlListItemCollection(this);
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(HtmlListType.Ordered)]
		[Localizable(true)]
		public HtmlListType HtmlListType
		{
			get
			{
				return this.listType;
			}

			set
			{
				this.listType = value;

				switch (this.listType)
				{
					case HtmlListType.AlphaLc:
						this.Style["list-style-type"] = "alpha-lower";
						break;
					case HtmlListType.AlphaUc:
						this.Style["list-style-type"] = "alpha-upper";
						break;
					case HtmlListType.Circle:
						this.Style["list-style-type"] = "circle";
						break;
					case HtmlListType.Decimal:
						this.Style["list-style-type"] = "decimal";
						break;
					case HtmlListType.DecimalLz:
						this.Style["list-style-type"] = "decimal-leading-zero";
						break;
					case HtmlListType.Disc:
						this.Style["list-style-type"] = "disc";
						break;
					case HtmlListType.GreekLc:
						this.Style["list-style-type"] = "greek-lower";
						break;
					case HtmlListType.GreekUc:
						this.Style["list-style-type"] = "greek-upper";
						break;
					case HtmlListType.LatinLc:
						this.Style["list-style-type"] = "latin-lower";
						break;
					case HtmlListType.LatinUc:
						this.Style["list-style-type"] = "latin-upper";
						break;
					case HtmlListType.None:
						this.Style["list-style-type"] = "none";
						break;
					case HtmlListType.RomanLc:
						this.Style["list-style-type"] = "roman-lower";
						break;
					case HtmlListType.RomanUc:
						this.Style["list-style-type"] = "roman-upper";
						break;
					case HtmlListType.Square:
						this.Style["list-style-type"] = "square";
						break;
					default:
						if (this.Style["list-style-type"] != null)
							this.Style.Remove("list-style-type");
						break;
				}
			}
		}

		protected override string TagName
		{
			get
			{
				return (this.HtmlListType == HtmlListType.Circle || this.HtmlListType == HtmlListType.Disc || this.HtmlListType == HtmlListType.Unordered ||
					this.HtmlListType == HtmlListType.Square || this.HtmlListType == HtmlListType.None) ? "ul" : "ol";
			}
		}

		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return (this.HtmlListType == HtmlListType.Circle || this.HtmlListType == HtmlListType.Disc || this.HtmlListType == HtmlListType.Unordered ||
					this.HtmlListType == HtmlListType.Square || this.HtmlListType == HtmlListType.None) ? HtmlTextWriterTag.Ul : HtmlTextWriterTag.Ol;
			}
		}
	}
}
