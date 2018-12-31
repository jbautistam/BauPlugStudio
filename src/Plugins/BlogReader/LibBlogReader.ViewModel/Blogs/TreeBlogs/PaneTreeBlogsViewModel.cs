using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Menus;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using System.Collections.ObjectModel;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs
{
	/// <summary>
	///		ViewModel para el árbol de blogs
	/// </summary>
	public class PaneTreeBlogsViewModel : BauMvvm.ViewModels.Forms.BasePaneViewModel
	{   
		// Constantes privadas
		private const string OpmlFilter = "Archivos OPML (*.opml)|*.opml|Todos los archivos (*.*)|*.*";
		// Variables privadas
		private TreeBlogsViewModel _tree;

		public PaneTreeBlogsViewModel()
		{
			Folder = BlogReaderViewModel.Instance.BlogManager.File;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			MenuGroupViewModel menuGroup;

				// Inicializa los comandos
				OpenOpmlCommand = new BaseCommand("Abrir archivo OPML", parameter => ExecuteAction(nameof(OpenOpmlCommand), parameter));
				NewFolderCommand = new BaseCommand("Nueva carpeta",
												   parameter => ExecuteAction(nameof(NewFolderCommand), parameter),
												   parameter => CanExecuteAction(nameof(NewFolderCommand), parameter))
										.AddListener(this, nameof(TreeBlogsViewModel.SelectedNode));
				NewBlogCommand = new BaseCommand("Nuevo blog",
												 parameter => ExecuteAction(nameof(NewBlogCommand), parameter),
												 parameter => CanExecuteAction(nameof(NewBlogCommand), parameter))
										.AddListener(this, nameof(TreeBlogsViewModel.SelectedNode));
				DownloadCommand = new BaseCommand("Descargar",
												  parameter => ExecuteAction(nameof(DownloadCommand), parameter),
												  parameter => CanExecuteAction(nameof(DownloadCommand), parameter))
										.AddListener(this, nameof(TreeBlogsViewModel.SelectedNode));
				SeeNewsCommand = new BaseCommand("Ver noticias",
												 parameter => ExecuteAction(nameof(SeeNewsCommand), parameter),
												 parameter => CanExecuteAction(nameof(SeeNewsCommand), parameter))
										.AddListener(this, nameof(TreeBlogsViewModel.SelectedNode));
				DeleteCommand.AddListener(this, nameof(TreeBlogsViewModel.SelectedNode));
				// Añade los menús de la ventana principal
				menuGroup = MenuCompositionData.Menus.Add("Nuevo", MenuGroupViewModel.TargetMenuType.MainMenu,
														  MenuGroupViewModel.TargetMainMenuItemType.FileOpenItems);
				menuGroup.MenuItems.Add("Archivo OPML", BlogReaderViewModel.Instance.GetIconRoute(BlogReaderViewModel.IconIndex.NewBlog),
										OpenOpmlCommand);
				menuGroup = MenuCompositionData.Menus.Add("Nuevo", MenuGroupViewModel.TargetMenuType.MainMenu,
														  MenuGroupViewModel.TargetMainMenuItemType.FileNewItems);
				menuGroup.MenuItems.Add("Nueva carpeta", BlogReaderViewModel.Instance.GetIconRoute(BlogReaderViewModel.IconIndex.NewFolder),
										NewFolderCommand);
				menuGroup.MenuItems.Add("Nuevo blog", BlogReaderViewModel.Instance.GetIconRoute(BlogReaderViewModel.IconIndex.NewBlog),
										NewBlogCommand);
				// Inicializa el árbol
				Tree = new TreeBlogsViewModel(Folder);
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		public void LoadNodes()
		{
			Tree.LoadNodes();
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(OpenOpmlCommand):
						OpenFileOpml();
					break;
				case nameof(NewFolderCommand):
						OpenFormUpdateFolder(null);
					break;
				case nameof(NewBlogCommand):
						OpenFormUpdateBlog(null);
					break;
				case nameof(DownloadCommand):
						DownloadItems();
					break;
				case nameof(SeeNewsCommand):
						SeeNews();
					break;
				case nameof(PropertiesCommand):
						if (Tree.SelectedNode != null)
						{
							if (Tree.SelectedNode is FolderNodeViewModel folderNode)
								OpenFormUpdateFolder(folderNode.Folder);
							else if (Tree.SelectedNode is BlogNodeViewModel blogNode)
								OpenFormUpdateBlog(blogNode.Blog);
						}
					break;
				case nameof(SaveCommand):
						SaveOpml();
					break;
				case nameof(DeleteCommand):
						if (Tree.SelectedNode != null)
						{
							if (Tree.SelectedNode is FolderNodeViewModel folderNode)
								DeleteFolder(folderNode.Folder);
							else if (Tree.SelectedNode is BlogNodeViewModel blogNode)
								DeleteBlog(blogNode.Blog);
						}
					break;
				case nameof(RefreshCommand):
						Refresh();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewFolderCommand):
				case nameof(NewBlogCommand):
					return Tree.IsSelectedFolder || Tree.SelectedNode == null;
				case nameof(PropertiesCommand):
				case nameof(DeleteCommand):
				case nameof(DownloadCommand):
				case nameof(SeeNewsCommand):
					return Tree.SelectedNode != null;
				case nameof(SaveCommand):
				case nameof(RefreshCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Abre un archivo OPML
		/// </summary>
		private void OpenFileOpml()
		{
			string fileName = BlogReaderViewModel.Instance.DialogsController.OpenDialogLoad(null, OpmlFilter);

				if (!fileName.IsEmpty() && System.IO.File.Exists(fileName))
				{ 
					// Carga el archivo
					BlogReaderViewModel.Instance.BlogManager.LoadOpml(fileName);
					// Graba la configuración
					BlogReaderViewModel.Instance.BlogManager.Save();
					// Actualiza el árbol
					Refresh();
				}
		}

		/// <summary>
		///		Guarda el archivo como OPML
		/// </summary>
		private void SaveOpml()
		{
			string fileName = BlogReaderViewModel.Instance.DialogsController.OpenDialogSave(null, OpmlFilter);

				if (!fileName.IsEmpty())
					BlogReaderViewModel.Instance.BlogManager.SaveOpml(fileName);
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta
		/// </summary>
		private void OpenFormUpdateFolder(FolderModel folder)
		{
			FolderModel parent = null;

				// Obtiene la carpeta seleccionada
				if (folder == null)
				{
					if (Tree.SelectedNode is FolderNodeViewModel node)
						parent = node.Folder;
					else
						parent = Folder;
				}
				// Abre la ventana de modificación
				if (BlogReaderViewModel.Instance.ViewsController.OpenUpdateFolderView
										(new FolderViewModel(parent, folder)) == SystemControllerEnums.ResultType.Yes)
					Refresh();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una carpeta
		/// </summary>
		private void OpenFormUpdateBlog(BlogModel blog)
		{
			FolderModel folder = null;

				// Obtiene la carpeta a la que se añade el blog
				if (blog == null)
				{
					if (Tree.SelectedNode is FolderNodeViewModel node)
						folder = node.Folder;
					else
						folder = Folder;
				}
				else
					folder = blog.Folder;
				// Abre la ventana de modificación
				if (BlogReaderViewModel.Instance.ViewsController.OpenUpdateBlogView
											(new BlogViewModel(folder, blog)) == SystemControllerEnums.ResultType.Yes)
					Refresh();
		}

		/// <summary>
		///		Borra una carpeta
		/// </summary>
		private void DeleteFolder(FolderModel folder)
		{
			if (folder != null &&
				BlogReaderViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar la carpeta '{folder.Name}'?"))
			{ 
				BlogsModelCollection blogs = folder.GetBlogsRecursive();

					// Borra los directorios de los blogs
					KillPaths(blogs);
					// Borra la carpeta
					Folder.Delete(folder);
					// Borra la carpeta
					BlogReaderViewModel.Instance.BlogManager.Save();
					// Actualiza
					Refresh();
			}
		}

		/// <summary>
		///		Borra un blog
		/// </summary>
		private void DeleteBlog(BlogModel blog)
		{
			if (blog != null &&
				BlogReaderViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar el blog '{blog.Name}'?"))
			{ 
				// Borra el directorio del blog
				KillPaths(new BlogsModelCollection { blog } );
				// Borra el blog
				Folder.Delete(blog);
				// Graba los datos
				BlogReaderViewModel.Instance.BlogManager.Save();
				// Actualiza
				Refresh();
			}
		}

		/// <summary>
		///		Elimina los directorios de una serie de blogs
		/// </summary>
		private void KillPaths(BlogsModelCollection blogs)
		{
			foreach (BlogModel blog in blogs)
				LibCommonHelper.Files.HelperFiles.KillPath(System.IO.Path.Combine(BlogReaderViewModel.Instance.PathBlogs, blog.Path));
		}

		/// <summary>
		///		Descarga los elementos
		/// </summary>
		private void DownloadItems()
		{
			if (Tree.SelectedNode is BaseNodeViewModel node && node != null)
			{
				BlogsModelCollection blogs = node.GetBlogs();

					// Descarga los blogs
					new Application.Services.Reader.RssDownload(BlogReaderViewModel.Instance.BlogManager).Download(true, blogs);
			}
		}

		/// <summary>
		///		Muestra las noticias
		/// </summary>
		private void SeeNews()
		{
			BlogReaderViewModel.Instance.ViewsController.OpenSeeNewsBlog(Tree.SelectedNode as BaseNodeViewModel);
		}

		/// <summary>
		///		Chequea un nodo
		/// </summary>
		public void CheckNode(int? id)
		{
			Tree.CheckNode(id);
		}

		/// <summary>
		///		Obtiene los nodos chequeados
		/// </summary>
		public System.Collections.Generic.List<int> GetCheckedNodes()
		{
			return Tree.GetCheckedNodes();
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		public void Refresh()
		{   
			// Recalcula el número de elementos
			BlogReaderViewModel.Instance.BlogManager.File.GetNumberNotRead();
			// Llama al método base
			LoadNodes();
		}

		/// <summary>
		///		Carpeta base
		/// </summary>
		public FolderModel Folder { get; }

		/// <summary>
		///		Arbol
		/// </summary>
		public TreeBlogsViewModel Tree
		{
			get { return _tree; }
			set { CheckObject(ref _tree, value); }
		}

		/// <summary>
		///		Comando de abrir archivo OPML
		/// </summary>
		public BaseCommand OpenOpmlCommand { get; private set; }

		/// <summary>
		///		Comando de nueva carpeta
		/// </summary>
		public BaseCommand NewFolderCommand { get; private set; }

		/// <summary>
		///		Comando de nuevo blog
		/// </summary>
		public BaseCommand NewBlogCommand { get; private set; }

		/// <summary>
		///		Comando de descarga
		/// </summary>
		public BaseCommand DownloadCommand { get; private set; }

		/// <summary>
		///		Comando de ver el contenido de un blog
		/// </summary>
		public BaseCommand SeeNewsCommand { get; private set; }
	}
}
