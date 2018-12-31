using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		ViewModel para la lista de páginas asociadas <see cref="PageListItemViewModel"/>
	/// </summary>
	public class PagesListViewModel : MVVM.ViewModels.ListItems.ListViewUpdatableViewModel<PageListItemViewModel>
	{
		public PagesListViewModel(BauMvvm.ViewModels.BaseObservableObject form, ProjectModel project, DocumentModel document)
		{
			FormParent = form;
			Project = project;
			Document = document;
		}

		/// <summary>
		///		Carga los datos
		/// </summary>
		public void LoadData()
		{ 
			// Limpia la lista
			ListItems.Clear();
			// Carga los elementos en la lista
			foreach (FileModel child in Document.ChildPages)
				Add(new PageListItemViewModel(child));
		}

		/// <summary>
		///		Crea un nuevo elemento
		/// </summary>
		protected override bool NewItem()
		{
			FilesModelCollection files = new FilesModelCollection(Project);
			bool isUpdated = false;

				// Obtiene los archivos de la lista
				foreach (PageListItemViewModel item in ListItems)
					files.Add(item.File);
				// Abre la ventana de selección de documentos
				if (DocWriterViewModel.Instance.ViewsController.SelectFilesProject
										(Project, FileModel.DocumentType.Document,
										 files, out FilesModelCollection filesSelected) == SystemControllerEnums.ResultType.Yes)
				{ 
					// Limpia la lista
					ListItems.Clear();
					// Añade los elementos
					foreach (FileModel file in filesSelected)
						ListItems.Add(new PageListItemViewModel(file));
					// Indica que ha habido modificaciones
					isUpdated = true;
				}
				// Devuelve el valor que indica si se ha dado alguno de alta
				return isUpdated;
		}

		/// <summary>
		///		Modifica un elemento -> En este caso simplemente implementa la interface
		/// </summary>
		protected override bool UpdateItem(PageListItemViewModel selectedItem)
		{
			return false;
		}

		/// <summary>
		///		Borra un elemento
		/// </summary>
		protected override bool DeleteItem(PageListItemViewModel selectedItem)
		{
			bool deleted = false;

				// Borra el elemento
				if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea quitar esta página?"))
				{ 
					// Elimina el elemento
					Document.ChildPages.RemoveByID(selectedItem.File.GlobalId);
					// Actualiza la lista e indica que se ha borrado
					Refresh();
					deleted = true;
				}
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Obtiene los archivos de la lista de páginas
		/// </summary>
		internal FilesModelCollection GetPages()
		{
			FilesModelCollection files = new FilesModelCollection(Project);

				// Obtiene los archivos de la lista
				foreach (PageListItemViewModel item in ListItems)
					files.Add(item.File);
				// Devuelve la colección de listas
				return files;
		}

		/// <summary>
		///		Actualiza los elementos
		/// </summary>
		protected override void Refresh()
		{   
			// Actualiza los elementos
			LoadData();
			// Llama al método base
			base.Refresh();
		}

		/// <summary>
		///		Formulario padre
		/// </summary>
		public BauMvvm.ViewModels.BaseObservableObject FormParent { get; }

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Documento al que se asocian los archivos
		/// </summary>
		public DocumentModel Document { get; }
	}
}
