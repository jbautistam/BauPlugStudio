using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.DevConference.Admon.Application.ModelsManager;
using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Projects
{
	/// <summary>
	///		ViewModel de <see cref="TrackModel"/>
	/// </summary>
	public class TrackViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _description, _url;
		private bool _enabled, _isNew;

		public TrackViewModel(TrackManagerModel trackManager, TrackModel model)
		{
			TrackManager = trackManager;
			Track = model;
			if (Track == null)
			{
				Track = new TrackModel();
				_isNew = true;
			}
			// Inicializa las propiedades
			Name = Track.Title;
			Description = Track.Description;
			Url = Track.Url;
			Enabled = Track.Enabled;
			// Indica que no ha habido modificaciones
			IsUpdated = false;
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
				else if (Url.GetUrl() == null)
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca una URL correcta");
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
				Track.Title = Name;
				Track.Description = Description;
				Track.Url = Url;
				Track.Enabled = Enabled;
				// Añade el elemento si es necesario
				if (_isNew)
					TrackManager.Tracks.Add(Track);
				// Indica que no hay modificaciones
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Manager padre
		/// </summary>
		private TrackManagerModel TrackManager { get; }

		/// <summary>
		///		Canal
		/// </summary>
		private TrackModel Track { get; }

		/// <summary>
		///		Nombre del canal
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción del canal
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Url del canal
		/// </summary>
		public string Url
		{
			get { return _url; }
			set { CheckProperty(ref _url, value); }
		}

		/// <summary>
		///		Indica si el proyecto está activo
		/// </summary>
		public bool Enabled
		{
			get { return _enabled; }
			set { CheckProperty(ref _enabled, value); }
		}
	}
}
