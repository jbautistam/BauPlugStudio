using System;

using Bau.Libraries.DevConference.Admon.Application.ModelsManager;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Projects
{
	/// <summary>
	///		ViewModel de <see cref="TrackManagerModel"/>
	/// </summary>
	public class TrackManagerViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _path;
		private bool _isNew;

		public TrackManagerViewModel(TrackManagerModel model)
		{
			// Asigna el canal predeterminado si no se le ha pasado ninguno
			TrackManager = model;
			if (TrackManager == null)
			{
				TrackManager = new TrackManagerModel();
				_isNew = true;
			}
			Name = TrackManager.Title;
			Path = TrackManager.Path;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{ 
			bool isValidated = false;

				// Comprueba los datos
				if (string.IsNullOrEmpty(Name))
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del canal");
				else if (string.IsNullOrWhiteSpace(Path))
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el directorio donde se generan los archivos");
				else
					isValidated = true;
				// Devuelve el valor que indica si es correcto
				return isValidated;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{ 
			if (ValidateData())
			{ 
				// Pasa los datos de la vista al modelo
				if (_isNew)
					DevConferencesViewModel.Instance.TrackManager.TrackManagers.Add(TrackManager);
				TrackManager.Title = Name;
				TrackManager.Path = Path;
				// Graba los datos
				DevConferencesViewModel.Instance.TrackManager.Save();
				// Indica que no ha habido modificaciones y cierra el formulario
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Modelo
		/// </summary>
		public TrackManagerModel TrackManager { get; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Directorio donde se generan los archivos
		/// </summary>
		public string Path
		{
			get { return _path; }
			set { CheckProperty(ref _path, value); }
		}
	}
}
