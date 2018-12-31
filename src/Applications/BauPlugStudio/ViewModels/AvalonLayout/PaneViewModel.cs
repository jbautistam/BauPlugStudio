using System;
using System.Windows.Media;

using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Applications.BauPlugStudio.ViewModels.AvalonLayout
{
	/// <summary>
	///		ViewModel para los paneles
	/// </summary>
	public class PaneViewModel : BaseObservableObject
	{   
		// Variables privadas
		private string _title, _windowID, _propertiesMessage;
		private bool _isSelected, _isActive;
		private System.Windows.Controls.UserControl _contentControl;
		private BaseObservableObject _contentViewModel;

		public PaneViewModel(string windowID, string title, Xceed.Wpf.AvalonDock.Layout.LayoutContent layoutPane,
							 System.Windows.Controls.UserControl contentControl)
		{
			WindowID = windowID;
			Title = title;
			LayoutPane = layoutPane;
			ContentControl = contentControl;
		}

		/// <summary>
		///		Obtiene la implementación de FormView del formulario
		/// </summary>
		public IFormView GetFormView()
		{
			if (ContentControl is IFormView formView)
				return formView;
			else
				return null;
		}

		/// <summary>
		///		Obtiene el ViewModel del formulario
		/// </summary>
		public BaseObservableObject GetViewModel()
		{
			return GetFormView()?.FormView?.ViewModel;
		}

		/// <summary>
		///		Panel al que se asocia la vista
		/// </summary>
		public Xceed.Wpf.AvalonDock.Layout.LayoutContent LayoutPane { get; }

		/// <summary>
		///		Identificador de la ventana
		/// </summary>
		public string WindowID
		{
			get { return _windowID; }
			set { CheckProperty(ref _windowID, value); }
		}

		/// <summary>
		///		Título del panel
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		Origen del icono
		/// </summary>
		public ImageSource IconSource { get; protected set; }

		/// <summary>
		///		Indica si el panel está seleccionado
		/// </summary>
		public bool IsSelected
		{
			get { return _isSelected; }
			set { CheckProperty(ref _isSelected, value); }
		}

		/// <summary>
		///		Indica si el panel está activo
		/// </summary>
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (CheckProperty(ref _isActive, value))
					ContentControl.Focus();
			}
		}

		/// <summary>
		///		Estado en la barra de mensaje
		/// </summary>
		public string PropertiesMessage
		{
			get { return _propertiesMessage; }
			set { CheckProperty(ref _propertiesMessage, value); }
		}

		/// <summary>
		///		Control de usuario que muestra la vista
		/// </summary>
		public System.Windows.Controls.UserControl ContentControl 
		{ 
			get { return _contentControl; }
			set
			{
				if (CheckObject(ref _contentControl, value))
					ContentViewModel = GetFormView()?.FormView.ViewModel;
			}
		}

		/// <summary>
		///		ViewModel del control
		/// </summary>
		public BaseObservableObject ContentViewModel
		{
			get { return _contentViewModel; }
			set { CheckObject(ref _contentViewModel, value); }
		}
	}
}
