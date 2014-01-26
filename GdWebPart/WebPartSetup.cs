using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;

namespace GdWebPart
{
	public class WebPartSetup : IWizard
	{
		#region IWizard Members

		public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(EnvDTE.Project project)
		{
		}

		public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
		{
		}

		public void RunFinished()
		{
		}

		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			UserInputForm form;
			Guid guid;

			form = new UserInputForm();
			form.ShowDialog();

			guid = Guid.NewGuid();

			replacementsDictionary.Add("$guid$", guid.ToString());
			replacementsDictionary.Add("$commonprefix$", form.CommonPrefix);
			replacementsDictionary.Add("$webparttitle$", form.WebPartTitle);
			replacementsDictionary.Add("$webpartdescription$", form.WebPartDescription);
			replacementsDictionary.Add("$assemblytitle$", form.AssemblyDescription);
			replacementsDictionary.Add("$assemblydescription$", form.AssemblyDescription);
			replacementsDictionary.Add("$company$", form.Company);
			replacementsDictionary.Add("$product$", form.Product);
			replacementsDictionary.Add("$copyright$", form.CopyRight);
			replacementsDictionary.Add("$trademark$", form.TradeMark);
			replacementsDictionary.Add("$assemblyversionmajor$", form.AssemblyVersionMajor);
			replacementsDictionary.Add("$assemblyversionminor$", form.AssemblyVersionMinor);
			replacementsDictionary.Add("$assemblyversionbuild$", form.AssemblyBuild);
			replacementsDictionary.Add("$assemblyversionrevision$", form.AssemblyRevision);
			replacementsDictionary.Add("$assemblyversion$", form.AssemblyVersion);
			replacementsDictionary.Add("$fileversionmajor$", form.FileVersionMajor);
			replacementsDictionary.Add("$fileversionminor$", form.FileVersionMinor);
			replacementsDictionary.Add("$fileversionbuild$", form.FileBuild);
			replacementsDictionary.Add("$fileversionrevision$", form.FileRevision);
			replacementsDictionary.Add("$fileversion$", form.FileVersion);
			replacementsDictionary.Add("$toolparttitle$", form.ToolpaneTitle);
			replacementsDictionary.Add("$nocustomtoolpane$", (form.NeedsCustomToolPane) ? "" : "//");
		}

		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}

		#endregion
	}
}
