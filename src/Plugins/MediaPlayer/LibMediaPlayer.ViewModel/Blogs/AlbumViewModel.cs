using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel de <see cref="MediaAlbumModel"/>
	/// </summary>
	public class AlbumViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private MediaFolderModel _parent;
		private MediaAlbumModel _album;
		private string _name, _description;

		public AlbumViewModel(MediaFolderModel parent, MediaAlbumModel album)
		{
			if (album == null)
			{
				_album = new MediaAlbumModel();
				_album.Folder = parent;
			}
			else
				_album = album;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Parent = _album.Folder;
			Name = _album.Name;
			Description = _album.Description;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					MediaPlayerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del blog");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Asigna la carpeta
				if (_parent != null && !_parent.Albums.Exists(_album.GlobalId))
					_parent.Albums.Add(_album);
				// Asigna los datos del formulario al objeto
				_album.Folder = Parent;
				_album.Name = Name;
				_album.Description = Description;
				// Graba el objeto
				MediaPlayerViewModel.Instance.MediaManager.Save();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Carpeta padre
		/// </summary>
		public MediaFolderModel Parent
		{
			get { return _parent; }
			set { CheckObject(ref _parent, value); }
		}

		/// <summary>
		///		Nombre del álbum
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Descripción del álbum
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}
	}
}
