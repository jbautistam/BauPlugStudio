using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel de <see cref="FolderModel"/>
	/// </summary>
	public class FolderViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{
		// Variables privadas
		private FolderModel _parent, _folder;
		private string _name;

		public FolderViewModel(FolderModel parent, FolderModel folder)
		{
			_parent = parent;
			_folder = folder;
			if (_parent == null)
				_parent = BlogReaderViewModel.Instance.BlogManager.File;
			if (_folder == null)
				_folder = new FolderModel();
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
					BlogReaderViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la carpeta");
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
				BlogReaderViewModel.Instance.BlogManager.Save();
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
