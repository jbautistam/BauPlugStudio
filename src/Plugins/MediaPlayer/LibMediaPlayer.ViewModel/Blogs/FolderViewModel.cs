using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMediaPlayer.Model;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel de <see cref="MediaFolderModel"/>
	/// </summary>
	public class FolderViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{
		// Variables privadas
		private MediaFolderModel _parent, _folder;
		private string _name;

		public FolderViewModel(MediaFolderModel parent, MediaFolderModel folder)
		{
			_parent = parent;
			_folder = folder;
			if (_parent == null)
				_parent = MediaPlayerViewModel.Instance.MediaManager.File;
			if (_folder == null)
				_folder = new MediaFolderModel();
			else
				_parent = folder.Parent;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Name = _folder.Name;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					MediaPlayerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la carpeta");
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
				if (_parent != null && !_parent.Folders.Exists(_folder.GlobalId))
					_parent.Folders.Add(_folder);
				_folder.Parent = _parent;
				// Asigna los datos del formulario al objeto
				_folder.Name = Name;
				// Graba el objeto
				MediaPlayerViewModel.Instance.MediaManager.Save();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del blog
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}
	}
}
