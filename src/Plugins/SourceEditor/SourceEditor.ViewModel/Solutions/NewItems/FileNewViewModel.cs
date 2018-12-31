using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems
{
	/// <summary>
	///		ViewModel de <see cref="FileModel"/> para creación de un archivo
	/// </summary>
	public class FileNewViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private ProjectModel _project;
		private FileModel _folderParent;
		private string _name;
		private ListViewModel<ListItems.FileDefinitionListItemViewModel> _definitions;
		private bool _addExternalFile;

		public FileNewViewModel(ProjectModel project, FileModel folderParent)
		{
			_project = project;
			_folderParent = folderParent;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{ 
			// Inicializa la colección
			FilesDefinition = new ListViewModel<ListItems.FileDefinitionListItemViewModel>();
			// Añade las definiciones
			foreach (AbstractDefinitionModel definition in _project.Definition.FilesDefinition)
				if (definition is FileDefinitionModel)
					FilesDefinition.Add(new ListItems.FileDefinitionListItemViewModel(definition as FileDefinitionModel));
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del archivo");
				else if (FilesDefinition.SelectedItem == null && !AddExternalFile)
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el tipo del archivo");
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
				Application.Bussiness.Solutions.FileFactory factory = new Application.Bussiness.Solutions.FileFactory();

					// Crea el archivo
					if (AddExternalFile)
						File = factory.Create(null, _project, _folderParent, _name);
					else
						File = factory.Create(FilesDefinition.SelectedItem.FileDefinition, _project, _folderParent, _name);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Definiciones de archivos
		/// </summary>
		public ListViewModel<ListItems.FileDefinitionListItemViewModel> FilesDefinition
		{
			get { return _definitions; }
			set { CheckObject(ref _definitions, value); }
		}

		/// <summary>
		///		Archivo creado
		/// </summary>
		public FileModel File { get; private set; }

		/// <summary>
		///		Indica si se va a añadir un archivo externo
		/// </summary>
		public bool AddExternalFile
		{
			get { return _addExternalFile; }
			set { CheckProperty(ref _addExternalFile, value); }
		}
	}
}
