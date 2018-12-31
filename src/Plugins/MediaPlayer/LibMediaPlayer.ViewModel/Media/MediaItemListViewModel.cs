using System;

using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Tools.Media
{
	/// <summary>
	///		ViewModel para un elemento del reproductor de medios
	/// </summary>
	public class MediaItemListViewModel : BauMvvm.ViewModels.BaseObservableObject
	{ 
		// Variables privadas
		private string _group, _title, _fullFileName, _fileName, _url, _textFile;
		private bool _selected, _canDownload;
		private DateTime _createdAt;

		public MediaItemListViewModel(MediaFileModel file)
		{
			File = file;
			Group = file.Album.Name;
			Title = file.Name;
			Url = file.Url;
			FileName = file.FileName;
			CreatedAt = file.CreatedAt;
			if (!string.IsNullOrWhiteSpace(file.FileName))
			{
				FullFileName = file.FileName;
				TextFile = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
			}
			else
			{
				TextFile = FullFileName = file.Url;
				CanDownload = true;
			}
		}

		/// <summary>
		///		Archivo multimedia
		/// </summary>
		internal MediaFileModel File { get; }

		/// <summary>
		///		Grupo
		/// </summary>
		public string Group
		{
			get { return _group; }
			set { CheckProperty(ref _group, value); }
		}

		/// <summary>
		///		Título
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		Nombre completo del archivo
		/// </summary>
		public string FullFileName
		{
			get { return _fullFileName; }
			set { CheckProperty(ref _fullFileName, value); }
		}

		/// <summary>
		///		Nombre del archivo para mostrar en la pantalla
		/// </summary>
		public string TextFile
		{
			get { return _textFile; }
			set { CheckProperty(ref _textFile, value); }
		}

		/// <summary>
		///		Url del archivo
		/// </summary>
		public string Url
		{
			get { return _url; }
			set { CheckProperty(ref _url, value); }
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Fecha de alta
		/// </summary>
		public DateTime CreatedAt
		{
			get { return _createdAt; }
			set { CheckProperty(ref _createdAt, value); }
		}

		/// <summary>
		///		Indica si el archivo se puede descargar
		/// </summary>
		public bool CanDownload
		{
			get { return _canDownload; }
			set { CheckProperty(ref _canDownload, value); }
		}

		/// <summary>
		///		Indica si el elemento está seleccionado
		/// </summary>
		public bool IsSelected
		{
			get { return _selected; }
			set { CheckProperty(ref _selected, value); }
		}
	}
}
