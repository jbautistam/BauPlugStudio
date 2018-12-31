using System;
using System.Windows.Controls;
using System.Windows.Input;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.WebCurator.ViewModel.WebSites;

namespace Bau.Libraries.WebCurator.Plugin.Views.WebSites
{
	/// <summary>
	///		Ventana para mostrar los datos de un proyecto de WebCurator
	/// </summary>
	public partial class ProjectView : UserControl, IFormView
	{
		public ProjectView(ProjectViewModel viewModel)
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = viewModel;
			FormView = new BaseFormView(ViewModel);
			// Asigna los manejadores de eventos
			pthSource.Changed += (sender, evntArgs) =>
										{
											if (!pthSource.PathName.IsEmpty())
											{
												ViewModel.PathImagesSources = ViewModel.PathImagesSources.AddWithSeparator(pthSource.PathName, Environment.NewLine, false);
												pthSource.PathName = null;
											}
										};
			fnRssSource.Changed += (sender, evntArgs) =>
										{
											if (!fnRssSource.FileName.IsEmpty() &&
															(ViewModel.FilesRssSources.IsEmpty() || ViewModel.FilesRssSources.IndexOf(fnRssSource.FileName) < 0))
											{
												ViewModel.FilesRssSources = ViewModel.FilesRssSources.AddWithSeparator(fnRssSource.FileName, Environment.NewLine, false);
												fnRssSource.FileName = null;
											}
										};
			fnSentences.Changed += (sender, evntArgs) =>
										{
											if (!fnSentences.FileName.IsEmpty())
											{
												ViewModel.FilesXMLSentences = ViewModel.FilesXMLSentences.AddWithSeparator(fnSentences.FileName, Environment.NewLine, false);
												fnSentences.FileName = null;
											}
										};
		}

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public ProjectViewModel ViewModel { get; }

		private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				ViewModel.UpdateProjectTargetCommand.Execute(null);
		}
	}
}