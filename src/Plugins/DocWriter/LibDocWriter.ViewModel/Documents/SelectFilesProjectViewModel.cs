using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		ViewModel para seleccionar archivos
	/// </summary>
	public class SelectFilesProjectViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private TreeDocumentsViewModel _treeFiles;

		public SelectFilesProjectViewModel(ProjectModel project, FileModel.DocumentType idDocumentType, FilesModelCollection filesSelected)
		{ 
			// Inicializa las propiedades
			Project = project;
			FilesSelected = filesSelected;
			IDDocumentType = idDocumentType;
			// Carga por primera vez el combo de proyectos
			LoadTreeFiles(project);
		}

		/// <summary>
		///		Carga el árbol de archivo
		/// </summary>
		private void LoadTreeFiles(ProjectModel project)
		{
			if (project != null)
			{
				if (TreeFiles != null && TreeFiles.Files != null)
					TreeFiles.Files.Clear();
				TreeFiles = new TreeDocumentsViewModel(this, IDDocumentType, project.File, FilesSelected);
			}
		}

		/// <summary>
		///		Comprueba los datos de la ventana
		/// </summary>
		private bool ValidateData()
		{
			return true;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Guarda los archivos seleccionados
				FilesSelected = TreeFiles.GetIsCheckedFiles();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Tipo de documentos que se muestran en el árbol
		/// </summary>
		public FileModel.DocumentType IDDocumentType { get; }

		/// <summary>
		///		Archivos seleccionados
		/// </summary>
		public FilesModelCollection FilesSelected { get; private set; }

		/// <summary>
		///		Arbol de archivos
		/// </summary>
		public TreeDocumentsViewModel TreeFiles
		{
			get { return _treeFiles; }
			private set { CheckObject(ref _treeFiles, value); }
		}
	}
}
