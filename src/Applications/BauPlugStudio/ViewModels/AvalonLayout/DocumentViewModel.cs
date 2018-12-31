using System;
using System.Windows.Media;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;

namespace Bau.Applications.BauPlugStudio.ViewModels.AvalonLayout
{
	/// <summary>
	///		ViewModel de un documento
	/// </summary>
	public class DocumentViewModel : PaneViewModel
	{   
		// Constantes privadas
		private const string UpdatedSymbol = " *";
		// Conversor de iconos
		protected static ImageSourceConverter _imageConverter = new ImageSourceConverter();

		public DocumentViewModel(string windowID, string title, Xceed.Wpf.AvalonDock.Layout.LayoutContent layoutPane,
								 System.Windows.Controls.UserControl control)
								: base(windowID, title, layoutPane, control)
		{   
			// Asigna las propiedades
			IsDirty = false;
			// Asigna el icono
			//IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/document.png") as ImageSource;
		}

		/// <summary>
		///		Indica si el valor se ha modificado
		/// </summary>
		public bool IsDirty
		{
			get { return GetViewModel()?.IsUpdated ?? false; }
			set
			{
				BaseObservableObject viewModel = GetViewModel();

					if (viewModel != null && viewModel.IsUpdated != value)
					{ 
						// Cambia el valor que indica si se ha modificado
						viewModel.IsUpdated = value;
						OnPropertyChanged(nameof(IsDirty));
						// Cambia el título de la ventana
						if (viewModel.IsUpdated)
							Title += UpdatedSymbol;
						else if (Title.EndsWith(UpdatedSymbol))
							Title = Title.Left(Title.Length - UpdatedSymbol.Length);
					}
			}
		}

		/// <summary>
		///		Comando de grabación
		/// </summary>
		public BaseCommand SaveCommand
		{
			get 
			{
				BaseObservableObject viewModel = GetViewModel();

					if (viewModel != null)
						return (viewModel as Libraries.BauMvvm.ViewModels.Forms.Interfaces.IFormViewModel)?.SaveCommand; 
					else
						return null;
			}
		}
	}
}