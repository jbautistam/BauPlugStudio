using System;
using System.Windows;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.WebCurator.ViewModel.WebSites;

namespace Bau.Libraries.WebCurator.Plugin.Views.WebSites
{
	/// <summary>
	///		Formulario para mantenimiento de <see cref="ProjectTargetModel"/>
	/// </summary>
	public partial class ProjectTargetView : Window
	{
		public ProjectTargetView(ProjectTargetViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = viewModel;
			DataContext = ViewModel;
			ViewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
			// Inicializa el manejador de eventos para el control de selección de archivos de secciones
			fnSectionWithPages.Changed += (sender, evntArgs) =>
											{
												if (!fnSectionWithPages.FileName.IsEmpty())
												{
													ViewModel.SectionWithPages = ViewModel.SectionWithPages.AddWithSeparator(fnSectionWithPages.FileName, Environment.NewLine, false);
													fnSectionWithPages.FileName = null;
												}
											};
			fnSectionMenus.Changed += (sender, evntArgs) =>
											{
												if (!fnSectionMenus.FileName.IsEmpty())
												{
													ViewModel.SectionMenus = ViewModel.SectionMenus.AddWithSeparator(fnSectionMenus.FileName, Environment.NewLine, false);
													fnSectionMenus.FileName = null;
												}
											};
		}

		/// <summary>
		///		ViewModel del formulario
		/// </summary>
		public ProjectTargetViewModel ViewModel { get; }
	}
}