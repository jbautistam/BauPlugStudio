using System;

using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Projects
{
	/// <summary>
	///		ViewModel de <see cref="EntryModel"/>
	/// </summary>
	public class EntryViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _name, _summary, _authors, _urlImage, _urlVideo, _urlWebSite, _urlSlides;
		private bool _isNew;

		public EntryViewModel(CategoryModel category, EntryModel entry)
		{
			// Asigna los objetos
			Category = category;
			if (entry == null)
			{
				Entry = new	EntryModel();
				_isNew = true;
			}
			else
				Entry = entry;
			// Inicializa las propiedades
			Name = Entry.Title;
			Summary = Entry.Summary;
			Authors = Entry.Authors;
			UrlImage = Entry.UrlImage;
			UrlVideo = Entry.UrlVideo;
			UrlWebSite = Entry.UrlWebSite;
			UrlSlides = Entry.UrlSlides;
			// Inicializa el ViewModel
			IsUpdated = false;
		}

		/// <summary>
		///		Comprueba los datos introducidos en el formulario
		/// </summary>
		private bool ValidateData()
		{
			bool isValidated = false;

				// Comprueba los datos
				if (string.IsNullOrWhiteSpace(Name))
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la categoría");
				else if (string.IsNullOrWhiteSpace(UrlVideo))
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la URL del vídeo");
				else
					isValidated = true;
				// Devuelve el valor que indica si los datos son correctos
				return isValidated;
		}

		/// <summary>
		///		Graba los datos de la entrada
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Asigna las propiedades básicas de la entrada
				Entry.Title = Name;
				Entry.Summary = Summary;
				Entry.Authors = Authors;
				Entry.UrlImage = UrlImage;
				Entry.UrlVideo = UrlVideo;
				Entry.UrlWebSite = UrlWebSite;
				Entry.UrlSlides = UrlSlides;
				// Graba los datos y actualiza el árbol
				if (_isNew)
					Category.Entries.Add(Entry);
				// Indica que no hay modificaciones pendientes y cierra
				IsUpdated = false;
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Categoría a la que pertenece la entrada
		/// </summary>
		private CategoryModel Category { get; }

		/// <summary>
		///		Entrada
		/// </summary>
		private EntryModel Entry { get; }

		/// <summary>
		///		Nombre
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción de la entrada
		/// </summary>
		public string Summary
		{
			get { return _summary; }
			set { CheckProperty(ref _summary, value); }
		}

		/// <summary>
		///		Autor / autores
		/// </summary>
		public string Authors
		{
			get { return _authors; }
			set { CheckProperty(ref _authors, value); }
		}

		/// <summary>
		///		Url de la imagen
		/// </summary>
		public string UrlImage
		{
			get { return _urlImage; }
			set { CheckProperty(ref _urlImage, value); }
		}

		/// <summary>
		///		Url del vídeo
		/// </summary>
		public string UrlVideo
		{
			get { return _urlVideo; }
			set { CheckProperty(ref _urlVideo, value); }
		}

		/// <summary>
		///		Url del sitio web
		/// </summary>
		public string UrlWebSite
		{
			get { return _urlWebSite; }
			set { CheckProperty(ref _urlWebSite, value); }
		}

		/// <summary>
		///		Url de las slides
		/// </summary>
		public string UrlSlides
		{
			get { return _urlSlides; }
			set { CheckProperty(ref _urlSlides, value); }
		}

		/// <summary>
		///		Comprueba si se ha modificado alguna de las propiedades
		/// </summary>
		public bool IsDataUpdated
		{
			get { return IsUpdated; }
		}
	}
}
