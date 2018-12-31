using System;

using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Projects
{
	/// <summary>
	///		ViewModel de <see cref="CategoryModel"/>
	/// </summary>
	public class CategoryViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name;
		private bool _isNew;

		public CategoryViewModel(TrackModel track, CategoryModel category)
		{
			// Asigna los objetos
			Track = track;
			Category = category;
			if (Category == null)
			{
				Category = new CategoryModel(track);
				_isNew = true;
			}	
			// Inicializa el ViewModel
			Name = Category.Title;
			IsUpdated = false;
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool isValidated = false;

				// Comprueba los datos
				if (string.IsNullOrEmpty(Name))
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la categoría");
				else
					isValidated = true;
				// Devuelve el valor que indica si los datos son correctos
				return isValidated;
		}

		/// <summary>
		///		Graba los datos de la categoría
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Asigna las propiedades básicas de la categoría
				Category.Title = Name;
				// Graba los datos y actualiza el árbol
				if (_isNew)
					Track.Categories.Add(Category);
				DevConferencesViewModel.Instance.TrackManager.Save();
				// Indica que no hay modificaciones pendientes
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Canal al que se asocia la categoría
		/// </summary>
		private TrackModel Track { get; }

		/// <summary>
		///		Categoría
		/// </summary>
		private CategoryModel Category { get; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}
	}
}
