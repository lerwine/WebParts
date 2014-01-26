using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;


namespace $safeprojectname$
{
    /// <summary>
    /// Description for $safeprojectname$.
    /// </summary>
    [DefaultProperty("Text"),
        ToolboxData("<{0}:WebPart1 runat=server></{0}:WebPart1>"),
        XmlRoot(Namespace = "$safeprojectname$")]
    public class WebPart1 : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private const string defaultText = "";
        private string text = defaultText;

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
        #endregion

        /// <summary>
        ///	This method gets the custom tool parts for this Web Part by overriding the
        ///	GetToolParts method of the WebPart base class. You must implement
        ///	custom tool parts in a separate class that derives from 
        ///	Microsoft.SharePoint.WebPartPages.ToolPart. 
        ///	</summary>
        /// <returns>An array of references to ToolPart objects.</returns>
        //		public override ToolPart[] GetToolParts()
        //		{
        //			ToolPart[] toolparts = new ToolPart[2];
        //			WebPartToolPart wptp = new WebPartToolPart();
        //			CustomPropertyToolPart custom = new CustomPropertyToolPart();
        //			toolparts[0] = wptp;
        //			toolparts[1] = custom;
        //			return toolparts;
        //		}


        /// <summary>
        /// Render this Web Part to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void RenderWebPart(HtmlTextWriter output)
        {
            output.Write(text);
        }

    }
}
