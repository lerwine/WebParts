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

namespace $safeprojectname$
{
    /// <summary>
    /// Description for OptionList
    /// </summary>
	public enum OptionList
	{
		[XmlEnum(Name = "Option One")]
		Option_One = 0,
		[XmlEnum(Name = "Option Two")]
		Option_Two = 1,
		[XmlEnum(Name = "Option Three")]
		Option_Three = 2
	};

	/// <summary>
    /// Description for $safeprojectname$.
	/// </summary>
	[DefaultProperty("Text"),
        ToolboxData("<{0}:$safeprojectname$ runat=server></{0}:$safeprojectname$>"),
		XmlRoot(Namespace = "$safeprojectname$")]
	public class $safeprojectname$ : Microsoft.SharePoint.WebPartPages.WebPart, System.Web.UI.INamingContainer
	{
		private const OptionList defaultOption = OptionList.Option_One;
		private const string defaultText = "";

		private string text = defaultText;
		private OptionList optionChoice;

        #region WEBPART PROPERTIES
		[Browsable(true),
			Category("Miscellaneous"),
			DefaultValue(defaultText),
			WebPartStorage(Storage.Personal),
			FriendlyName("Text"),
			Description("Text Property")]
		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		public bool ShouldSerializeText()
		{
			return true;
		}

		[Browsable(true),
			Category("Option Choices"),
		   DefaultValue(defaultOption),
			WebPartStorage(Storage.Shared),
			FriendlyName("Option Choices"),
			Description("Options for web part")]
		public OptionList OptionChoice
		{
			get
			{
				return this.optionChoice;
			}
			set
			{
				this.optionChoice = value;
			}
		}

		public bool ShouldSerializeOptionChoice()
		{
			return true;
		}
		#endregion

		/// <summary>
		///	Constructor for the class.
		///	</summary>
		public $safeprojectname$()
		{
		}

        protected override void CreateChildControls()
        {
            System.Web.UI.WebControls.Image image;

            image = new Image();
            image.AlternateText = "Resource Image";
            image.BorderWidth = new Unit("0px");
            image.Height = new Unit("16px");
            image.ID = this.Qualifier + "Image";
            image.ImageUrl = this.ClassResourcePath + "/ResourceImage.gif";

            this.Controls.Add((Control)image);
        }

		/// <summary>
		///	This method gets the custom tool parts for this Web Part by overriding the
		///	GetToolParts method of the WebPart base class. You must implement
		///	custom tool parts in a separate class that derives from 
		///	Microsoft.SharePoint.WebPartPages.ToolPart. 
		///	</summary>
		///<returns>An array of references to ToolPart objects.</returns>
		$nocustomtoolpane$public override ToolPart[] GetToolParts()
		$nocustomtoolpane${
		$nocustomtoolpane$	return new ToolPart[3] { new $commonprefix$ToolPart(), new CustomPropertyToolPart(), new WebPartToolPart() };
		$nocustomtoolpane$}

		/// <summary>
		/// Render this Web Part to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderWebPart(HtmlTextWriter output)
		{
            Control control;

			output.Write(SPEncode.HtmlEncode(Text));

            if ((control = this.FindControl(this.Qualifier + "Image")) != null)
                control.RenderControl(output);

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
                stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("$safeprojectname$." +
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
