using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GdWebPart
{
	public partial class UserInputForm : Form
	{
		public string WebPartTitle
		{
			get
			{
				return this.wpTitleTextBox.Text.Trim();
			}
			set
			{
				this.wpTitleTextBox.Text = value;
			}
		}

		public string WebPartDescription
		{
			get
			{
				return this.wpDescriptionTextBox.Text.Trim();
			}
			set
			{
				this.wpDescriptionTextBox.Text = value;
			}
		}

		public bool NeedsCustomToolPane
		{
			get
			{
				return this.needCustomToolPaneCheckBox.Checked;
			}
			set
			{
				this.needCustomToolPaneCheckBox.Checked = value;
			}
		}

		public string CommonPrefix
		{
			get
			{
				return this.commonPrefixTextBox.Text.Trim();
			}
			set
			{
				this.commonPrefixTextBox.Text = value;
			}
		}

		public string AssemblyTitle
		{
			get
			{
				return this.assyTitleTextBox.Text.Trim();
			}
			set
			{
				this.assyTitleTextBox.Text = value;
			}
		}

		public string AssemblyDescription
		{
			get
			{
				return this.assyDescriptionTextBox.Text.Trim();
			}
			set
			{
				this.assyDescriptionTextBox.Text = value;
			}
		}

		public string Company
		{
			get
			{
				return this.companyTextBox.Text.Trim();
			}
			set
			{
				this.companyTextBox.Text = value;
			}
		}

		public string Product
		{
			get
			{
				return this.productTextBox.Text.Trim();
			}
			set
			{
				this.productTextBox.Text = value;
			}
		}

		public string CopyRight
		{
			get
			{
				return this.copyrightTextBox.Text.Trim();
			}
			set
			{
				this.copyrightTextBox.Text = value;
			}
		}

		public string TradeMark
		{
			get
			{
				return this.trademarkTextBox.Text.Trim();
			}
			set
			{
				this.trademarkTextBox.Text = value;
			}
		}

		public string AssemblyVersionMajor
		{
			get
			{
				return this.assyVersMajorTextBox.Text.Trim();
			}
			set
			{
				this.assyVersMajorTextBox.Text = value;
			}
		}

		public string AssemblyVersionMinor
		{
			get
			{
				return this.assyVersMinorTextBox.Text.Trim();
			}
			set
			{
				this.assyVersMinorTextBox.Text = value;
			}
		}

		public string AssemblyBuild
		{
			get
			{
				return this.assyBuildTextBox.Text.Trim();
			}
			set
			{
				this.assyBuildTextBox.Text = value;
			}
		}

		public string AssemblyRevision
		{
			get
			{
				return this.assyRevisionTextBox.Text.Trim();
			}
			set
			{
				this.assyRevisionTextBox.Text = value;
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return this.assyVersMajorTextBox.Text.Trim() + "." + this.assyVersMinorTextBox.Text.Trim() +
					"." + this.assyBuildTextBox.Text.Trim() + "." + this.assyRevisionTextBox.Text.Trim();
			}
		}

		public string FileVersionMajor
		{
			get
			{
				return this.fileVersMajorTextBox.Text.Trim();
			}
			set
			{
				this.fileVersMajorTextBox.Text = value;
			}
		}

		public string FileVersionMinor
		{
			get
			{
				return this.fileVersMinorTextBox.Text.Trim();
			}
			set
			{
				this.fileVersMinorTextBox.Text = value;
			}
		}

		public string FileBuild
		{
			get
			{
				return this.fileBuildTextBox.Text.Trim();
			}
			set
			{
				this.fileBuildTextBox.Text = value;
			}
		}

		public string FileRevision
		{
			get
			{
				return this.fileRevisionTextBox.Text.Trim();
			}
			set
			{
				this.fileRevisionTextBox.Text = value;
			}
		}

		public string FileVersion
		{
			get
			{
				return this.fileVersMajorTextBox.Text.Trim() + "." + this.fileVersMinorTextBox.Text.Trim() +
					"." + this.fileBuildTextBox.Text.Trim() + "." + this.fileRevisionTextBox.Text.Trim();
			}
		}

		public string ToolpaneTitle
		{
			get
			{
				return this.toolpaneTitleTextBox.Text.Trim();
			}
			set
			{
				this.toolpaneTitleTextBox.Text = value;
			}
		}

		public UserInputForm()
		{
			InitializeComponent();
		}

		private void wpTitleTextBox_Leave(object sender, EventArgs e)
		{
			if (this.assyTitleTextBox.Text == "")
				this.assyTitleTextBox.Text = this.wpTitleTextBox.Text.Trim();
		}

		private void wpDescriptionTextBox_Leave(object sender, EventArgs e)
		{
			if (this.assyDescriptionTextBox.Text == "")
				this.assyDescriptionTextBox.Text = this.wpDescriptionTextBox.Text.Trim();
		}

		private void numberFieldChanged(object sender, EventArgs e)
		{
			this.validateNumberTextBox((TextBox)sender);
		}

		private void needCustomToolPaneCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.toolpaneTitleTextBox.Enabled = this.needCustomToolPaneCheckBox.Checked;
		}

		private void continueButton_Click(object sender, EventArgs e)
		{
			if ((this.commonPrefixTextBox.Text = this.commonPrefixTextBox.Text.Trim()) == "")
			{
				MessageBox.Show("You must specify a prefix to use.");
				return;
			}
			if (this.needCustomToolPaneCheckBox.Checked &&
				(this.toolpaneTitleTextBox.Text = this.toolpaneTitleTextBox.Text.Trim()) == "")
			{
				MessageBox.Show("Toolpane must have a title.");
				return;
			}

			if (!this.validateNumberTextBox(this.assyVersMajorTextBox) ||
					!this.validateNumberTextBox(this.assyVersMinorTextBox) ||
					!this.validateNumberTextBox(this.assyBuildTextBox) ||
					!this.validateNumberTextBox(this.assyRevisionTextBox) ||
					!this.validateNumberTextBox(this.fileVersMajorTextBox) ||
					!this.validateNumberTextBox(this.fileVersMinorTextBox) ||
					!this.validateNumberTextBox(this.fileBuildTextBox) ||
					!this.validateNumberTextBox(this.fileRevisionTextBox))
				return;

			this.Close();
		}

		private bool validateNumberTextBox(TextBox textbox)
		{
			uint numVal;

			if ((textbox.Text = textbox.Text.Trim()) == "")
			{
				MessageBox.Show(textbox.AccessibleDescription + " required.");
				return false;
			}
			try
			{
				numVal = Convert.ToUInt32(textbox.Text);
				textbox.Text = numVal.ToString();
				return true;
			}
			catch
			{
				MessageBox.Show("Invalid " + textbox.AccessibleDescription);
			}

			return false;
		}
	}
}