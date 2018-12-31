using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.DevConference.Admon.Application.ModelsManager;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Explorer
{
	/// <summary>
	///		ViewModel para el árbol del explorador
	/// </summary>
	public class TreeTracksViewModel : BaseObservableObject
	{
		// Constantes privadas
		private const int EntriesRssNumber = 100;
		// Variables privadas
		private TreeExplorerNodeViewModel _selectedNode;
		private BaseModel _bufferCopy;

		public TreeTracksViewModel()
		{ 
			NewTrackManagerCommand = new BaseCommand(parameter => ExecuteAction(nameof(NewTrackManagerCommand), parameter),
												parameter => CanExecuteAction(nameof(NewTrackManagerCommand), parameter))
											.AddListener(this, nameof(SelectedNode));
			NewTrackCommand = new BaseCommand(parameter => ExecuteAction(nameof(NewTrackCommand), parameter),
												parameter => CanExecuteAction(nameof(NewTrackCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			NewCategoryCommand = new BaseCommand(parameter => ExecuteAction(nameof(NewCategoryCommand), parameter),
											   parameter => CanExecuteAction(nameof(NewCategoryCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			NewEntryCommand = new BaseCommand(parameter => ExecuteAction(nameof(NewEntryCommand), parameter),
												parameter => CanExecuteAction(nameof(NewEntryCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			OpenCommand = new BaseCommand(parameter => ExecuteAction(nameof(OpenCommand), parameter),
										  parameter => CanExecuteAction(nameof(OpenCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			SeeVideoCommand = new BaseCommand(parameter => ExecuteAction(nameof(SeeVideoCommand), parameter),
											  parameter => CanExecuteAction(nameof(SeeVideoCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			ProcessCommand = new BaseCommand(parameter => ExecuteAction(nameof(ProcessCommand), parameter),
											 parameter => CanExecuteAction(nameof(ProcessCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			CopyCommand = new BaseCommand(parameter => ExecuteAction(nameof(CopyCommand), parameter),
										  parameter => CanExecuteAction(nameof(CopyCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			CutCommand = new BaseCommand(parameter => ExecuteAction(nameof(CutCommand), parameter),
										  parameter => CanExecuteAction(nameof(CutCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			PasteCommand = new BaseCommand(parameter => ExecuteAction(nameof(PasteCommand), parameter),
										   parameter => CanExecuteAction(nameof(PasteCommand), parameter))
									.AddListener(this, nameof(SelectedNode))
									.AddListener(this, nameof(BufferCopy));
			DeleteCommand = new BaseCommand(parameter => ExecuteAction(nameof(DeleteCommand), parameter),
										   parameter => CanExecuteAction(nameof(DeleteCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
			ImportCommand = new BaseCommand(parameter => ExecuteAction(nameof(ImportCommand), parameter),
											parameter => CanExecuteAction(nameof(ImportCommand), parameter))
									.AddListener(this, nameof(SelectedNode));
		}

		/// <summary>
		///		Ejecuta un comando
		/// </summary>
		private void ExecuteAction(string action, object parameter)
		{ 
			switch (action)
			{	
				case nameof(NewTrackManagerCommand):
						OpenFormTrackManager(null, true);
					break;
				case nameof(NewTrackCommand):
						OpenFormTrack(null, true);
					break;
				case nameof(NewCategoryCommand):
						OpenFormCategory(null, true);
					break;
				case nameof(NewEntryCommand):
						OpenFormEntry(null, true);
					break;
				case nameof(OpenCommand):
						ShowDetails(SelectedNode);
					break;
				case nameof(SeeVideoCommand):
						ShowVideo(SelectedNode.Tag as EntryModel);
					break;
				case nameof(CopyCommand):
						Copy(SelectedNode, false);
					break;
				case nameof(CutCommand):
						Copy(SelectedNode, true);
					break;
				case nameof(PasteCommand):
						Paste(SelectedNode);
					break;
				case nameof(DeleteCommand):
						if (SelectedNode != null)
							switch (SelectedNode?.NodeType)
							{	
								case Controllers.GlobalEnums.NodeTypes.TrackManager:
										DeleteTrackManager(SelectedNode.Tag as TrackManagerModel);
									break;
								case Controllers.GlobalEnums.NodeTypes.Track:
										DeleteTrack(SelectedNode.Tag as TrackModel);
									break;
								case Controllers.GlobalEnums.NodeTypes.Category:
										DeleteCategory(SelectedNode.Tag as CategoryModel);
									break;
								case Controllers.GlobalEnums.NodeTypes.Entry:
										DeleteEntry(SelectedNode.Tag as EntryModel);
									break;
							}
					break;
				case nameof(ImportCommand):
						ImportEntries();
					break;
				case nameof(ProcessCommand):
						ProcessTracks();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar un comando
		/// </summary>
		private bool CanExecuteAction(string action, object parameter)
		{ 
			Controllers.GlobalEnums.NodeTypes? nodeType = SelectedNode?.NodeType;

				switch (action)
				{	
					case nameof(NewTrackManagerCommand):
						return true;
					case nameof(NewTrackCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.TrackManager;
					case nameof(NewCategoryCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.Track;
					case nameof(NewEntryCommand):
					case nameof(ImportCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.Category;
					case nameof(SeeVideoCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.Entry;
					case nameof(DeleteCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.TrackManager ||
									nodeType == Controllers.GlobalEnums.NodeTypes.Track ||
									nodeType == Controllers.GlobalEnums.NodeTypes.Category ||
									nodeType == Controllers.GlobalEnums.NodeTypes.Entry;
					case nameof(ProcessCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.TrackManager ||
							   nodeType == Controllers.GlobalEnums.NodeTypes.Track;
					case nameof(CopyCommand):
					case nameof(CutCommand):
						return nodeType == Controllers.GlobalEnums.NodeTypes.Track ||
								nodeType == Controllers.GlobalEnums.NodeTypes.Category ||
							    nodeType == Controllers.GlobalEnums.NodeTypes.Entry;
					case nameof(PasteCommand):
						return BufferCopy != null && 
								   (nodeType == Controllers.GlobalEnums.NodeTypes.TrackManager ||
									nodeType == Controllers.GlobalEnums.NodeTypes.Track||
								    nodeType == Controllers.GlobalEnums.NodeTypes.Category ||
								    nodeType == Controllers.GlobalEnums.NodeTypes.Entry);
					default:
						return false;
			}
		}

		/// <summary>
		///		Carga los datos
		/// </summary>
		public void Load()
		{ 
			List<IHierarchicalViewModel> nodesExpanded = GetNodesExpanded(Children);
			TreeExplorerNodeViewModel previousSelected;

				// Guarda el nodo seleccionado
				previousSelected = SelectedNode;
				// Limpia los nodos
				Children.Clear();
				// Carga el nodo de canal
				foreach (TrackManagerModel trackManager in DevConferencesViewModel.Instance.TrackManager.TrackManagers)
					Children.Add(new TreeExplorerNodeViewModel(this, null, trackManager.Title, trackManager, true, true, MvvmColor.DarkSlateBlue));
				// Expande de nuevo los nodos abiertos anteriormente
				ExpandNodes(Children, nodesExpanded);
				// Selecciona de nuevo el nodo
				if (previousSelected != null)
					SelectedNode = previousSelected;
		}

		/// <summary>
		///		Obtiene recursivamente los elementos expandidos del árbol (para poder recuperarlos posteriormente y dejarlos abiertos de nuevo)
		/// </summary>
		private List<IHierarchicalViewModel> GetNodesExpanded(ObservableCollection<IHierarchicalViewModel> nodes)
		{
			List<IHierarchicalViewModel> expanded = new List<IHierarchicalViewModel>();

				// Recorre los nodos obteniendo los seleccionados
				foreach (IHierarchicalViewModel node in nodes)
				{ 
					// Añade el nodo si se ha expandido
					if (node.IsExpanded)
						expanded.Add(node);
					// Añade los nodos hijo
					if (node.Children != null && node.Children.Count > 0)
						expanded.AddRange(GetNodesExpanded(node.Children));
				}
				// Devuelve la colección de nodos
				return expanded;
		}

		/// <summary>
		///		Expande los nodos que se le pasan en la colección <param name="nodesExpanded" />
		/// </summary>
		private void ExpandNodes(ObservableCollection<IHierarchicalViewModel> nodes, List<IHierarchicalViewModel> nodesExpanded)
		{ 
			if (nodes != null)
				foreach (IHierarchicalViewModel node in nodes)
					if (CheckIsExpanded(node, nodesExpanded))
					{ 
						// Expande el nodo
						node.IsExpanded = true;
						// Expande los hijos
						ExpandNodes(node.Children, nodesExpanded);
					}
		}

		/// <summary>
		///		Comprueba si un nodo estaba en la lista de nodos abiertos
		/// </summary>
		private bool CheckIsExpanded(IHierarchicalViewModel node, List<IHierarchicalViewModel> nodesExpanded)
		{ 
			// Recorre la colección
			foreach (IHierarchicalViewModel nodeExpanded in nodesExpanded)
				if ((nodeExpanded as TreeExplorerNodeViewModel)?.Text == (node as TreeExplorerNodeViewModel)?.Text)
					return true;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return false;
		}

		/// <summary>
		///		Muestra la ventana de detalles
		/// </summary>
		private void ShowDetails(TreeExplorerNodeViewModel selectedNode)
		{
			switch (selectedNode?.Tag)
			{
				case TrackManagerModel node:
						OpenFormTrackManager(node, false);
					break;
				case TrackModel node:
						OpenFormTrack(node, false);
					break;
				case CategoryModel node:
						OpenFormCategory(node, false);
					break;
				case EntryModel node:
						OpenFormEntry(node, false);
					break;
			}
		}

		/// <summary>
		///		Abre el formulario de un canal
		/// </summary>
		private void OpenFormTrackManager(TrackManagerModel manager, bool isNew)
		{ 
			if (DevConferencesViewModel.Instance.ViewsController.OpenPropertiesTrackManager(new Projects.TrackManagerViewModel(manager)) == SystemControllerEnums.ResultType.Yes)
				UpdateTracks();
		}

		/// <summary>
		///		Abre el formulario de un proyecto
		/// </summary>
		private void OpenFormTrack(TrackModel track, bool isNew)
		{ 
			TrackManagerModel trackManager = GetParentNodeOfType<TrackManagerModel>();

				if (trackManager != null &&
						DevConferencesViewModel.Instance.ViewsController.OpenPropertiesTrack(new Projects.TrackViewModel(trackManager, track)) == SystemControllerEnums.ResultType.Yes)
					UpdateTracks();
		}

		/// <summary>
		///		Abre el formulario de un <see cref="CategoryModel"/>
		/// </summary>
		private void OpenFormCategory(CategoryModel category, bool isNew)
		{ 
			TrackModel track = GetParentNodeOfType<TrackModel>();

				if (track != null &&
						DevConferencesViewModel.Instance.ViewsController.OpenPropertiesCategory(new Projects.CategoryViewModel(track, category)) == SystemControllerEnums.ResultType.Yes)
					UpdateTracks();
		}

		/// <summary>
		///		Abre el formulario de un <see cref="EntryModel"/>
		/// </summary>
		private void OpenFormEntry(EntryModel entry, bool isNew)
		{
			CategoryModel category = GetParentNodeOfType<CategoryModel>();

				if (category != null &&
						DevConferencesViewModel.Instance.ViewsController.OpenPropertiesEntry(new Projects.EntryViewModel(category, entry)) == 
									SystemControllerEnums.ResultType.Yes)
					UpdateTracks();
		}

		/// <summary>
		///		Muestra el vídeo
		/// </summary>
		private void ShowVideo(EntryModel entry)
		{
			if (entry != null && !string.IsNullOrEmpty(entry.UrlVideo))
				DevConferencesViewModel.Instance.ViewsController.ShowWebBrowser(entry.UrlVideo);
		}

		/// <summary>
		///		Procesa los administradores de canales o canales
		/// </summary>
		private void ProcessTracks()
		{
			if (SelectedNode != null)
			{
				if (SelectedNode?.Tag is TrackManagerModel trackManager)
				{
					// Procesa todas las pistas
					ProcessTrackManager(trackManager);
					// Mensaje para el usuario
					DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Ha finalizado el proceso de los canales");
				}
				else
				{
					TrackManagerModel trackNodeManager = GetParentNodeOfType<TrackManagerModel>();

						if (trackNodeManager != null)
						{
							TrackModel track = GetParentNodeOfType<TrackModel>();

								// Procesa la pista
								if (track != null)
									ProcessTrack(trackNodeManager, track);
								// Mensaje para el usuario
								DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Ha finalizado el proceso de los canales");
						}
				}
			}
		}

		/// <summary>
		///		Procesa el manager de canales
		/// </summary>
		private void ProcessTrackManager(TrackManagerModel trackManager)
		{
			foreach (TrackModel track in trackManager.Tracks)
				ProcessTrack(trackManager, track);
		}

		/// <summary>
		///		Procesa el canal
		/// </summary>
		private void ProcessTrack(TrackManagerModel trackManager, TrackModel track)
		{
			EntryModelCollection lastEntries = GetLastEntries(track);
			TrackModel trackCloned = track.Clone();

				// Crea el directorio de destino
				LibCommonHelper.Files.HelperFiles.MakePath(trackManager.Path);
				// Borra las entradas que no estén entre las últimas
				foreach (CategoryModel category in trackCloned.Categories)
					for (int index = category.Entries.Count - 1; index >= 0; index--)
						if (lastEntries.FirstOrDefault(item => item.Id.EqualsIgnoreCase(category.Entries[index].Id)) == null)
							category.Entries.RemoveAt(index);
				// Graba las entradas
				LibCommonHelper.Files.HelperFiles.SaveTextFile(System.IO.Path.Combine(trackManager.Path, GetLocalFileName(track)),
															   new DevConference.Application.FeedsManager().GetEntriesXml(trackCloned));
		}

		/// <summary>
		///		Obtiene el nombre del archivo local
		/// </summary>
		private string GetLocalFileName(TrackModel track)
		{
			string fileName = "";

				// Obtiene el nombre de la Url
				if (!string.IsNullOrWhiteSpace(track.Url))
				{
					string[] urlParts = track.Url.Split('/');

						if (urlParts.Length > 0)
							fileName = urlParts[urlParts.Length - 1];
				}
				// Si no se ha obtenido ningún nombre, lo coge del nombre del canal
				if (string.IsNullOrWhiteSpace(fileName))
					fileName = track.Title;
				// Le añade la extensión
				if (!fileName.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
					fileName += ".xml";
				// Normaliza el nombre y lo devuelve
				return LibCommonHelper.Files.HelperFiles.Normalize(fileName, false);
		}

		/// <summary>
		///		Obtiene las últimas entradas de la web
		/// </summary>
		private EntryModelCollection GetLastEntries(TrackModel track)
		{
			EntryModelCollection entries = new EntryModelCollection();

				// Busca las entradas de todas las categorías
				foreach (CategoryModel category in track.Categories)
					foreach (EntryModel entry in category.Entries)
						entries.Add(entry);
				// Ordena por fecha de creación descendente
				entries.Sort((first, second) => -1 * first.CreatedAt.CompareTo(second.CreatedAt));
				// Obtiene sólo las primeras n entradas
				while (entries.Count > EntriesRssNumber)
					entries.RemoveAt(entries.Count - 1);
				// Devuelve la colección de entradas
				return entries;
		}

		/// <summary>
		///		Obtiene el nodo o el nodo padre de tipo seleccionado
		/// </summary>
		private TypeData GetParentNodeOfType<TypeData>() where TypeData : BaseModel
		{
			IHierarchicalViewModel node = SelectedNode;

				// Obtiene el nodo de pista
				while (node != null && !((node as TreeExplorerNodeViewModel)?.Tag is TypeData))
					if (node.Parent != null)
						node = node.Parent;
				// Devuelve el nodo seleccionado
				if ((node as TreeExplorerNodeViewModel)?.Tag is TypeData data)
					return data;
				else
					return null;
		}

		/// <summary>
		///		Copia un elemento en el buffer
		/// </summary>
		private void Copy(TreeExplorerNodeViewModel selectedNode, bool cut)
		{
			Controllers.GlobalEnums.NodeTypes nodeType = selectedNode?.NodeType ?? Controllers.GlobalEnums.NodeTypes.TrackManager;

				if (nodeType == Controllers.GlobalEnums.NodeTypes.Track || nodeType == Controllers.GlobalEnums.NodeTypes.Category || 
					nodeType == Controllers.GlobalEnums.NodeTypes.Entry)
				{
					BufferCopy = selectedNode.Tag as BaseModel;
					MustCut = cut;
				}
		}

		/// <summary>
		///		Pega un elemento del buffer
		/// </summary>
		private void Paste(TreeExplorerNodeViewModel nodeToPaste)
		{
			// Pega los datos sobre el nodo
			if (nodeToPaste != null && BufferCopy != null)
			{
				bool pasted = false;

					// Pega el nodo
					if (nodeToPaste.NodeType == Controllers.GlobalEnums.NodeTypes.TrackManager && BufferCopy is TrackModel track)
					{
						// Pega los datos
						PasteTrackToTrackManager(track, (TrackManagerModel) nodeToPaste.Tag);
						// Indica que se ha pegado
						pasted = true;
					}
					else if (BufferCopy is EntryModel entry)
					{
						if (nodeToPaste.NodeType == Controllers.GlobalEnums.NodeTypes.Category)
						{
							// Pega los datos
							PasteEntryToCategory(entry, (CategoryModel) nodeToPaste.Tag);
							// Indica que se ha pegado
							pasted = true;
						}
					}
					else if (nodeToPaste.NodeType == Controllers.GlobalEnums.NodeTypes.Track && BufferCopy is CategoryModel category)
					{
						// Pega los datos
						PasteCategoryToTrack(category, (TrackModel) nodeToPaste.Tag);
						// Indica que se ha pegado
						pasted = true;
					}
					// Graba y actualiza
					if (pasted)
						UpdateTracks();
			}
			// Vacía el buffer de copia
			BufferCopy = null;
		}

		/// <summary>
		///		Pega un canal a un manager de canales
		/// </summary>
		private void PasteTrackToTrackManager(TrackModel track, TrackManagerModel manager)
		{
			TrackModel newTrack = track.Clone();

				// Limpia el ID del canal
				newTrack.Id = null;
				// Limpia los IDs
				foreach (CategoryModel category in track.Categories)
				{
					// Limpia el ID de la categoría
					category.Id = null;
					// Limpia los IDs de las entradas
					foreach (EntryModel entry in category.Entries)
						entry.Id = null;
				}
				// Añade el nuevo canal
				manager.Tracks.Add(newTrack);
				// Borra el canal inicial si se debía cortar
				if (MustCut)
					manager.Tracks.Remove(track);
		}

		/// <summary>
		///		Pega una entrada sobre la categoría
		/// </summary>
		private void PasteEntryToCategory(EntryModel entry, CategoryModel category)
		{
			EntryModel newEntry = entry.Clone();

				// Quita el ID de la entrada clonada para que se cree de nuevo
				newEntry.Id = null;
				// Añade la entrada
				category.Entries.Add(newEntry);
				// Borra la entrada inicial si se debía cortar
				if (MustCut)
					DevConferencesViewModel.Instance.TrackManager.RemoveEntry(entry);
		}

		/// <summary>
		///		Pega la categoría en un canal
		/// </summary>
		private void PasteCategoryToTrack(CategoryModel category, TrackModel track)
		{
			CategoryModel newCategory = category.Clone(track);

				// Elimina los ID
				newCategory.Id = null;
				foreach (EntryModel entry in newCategory.Entries)
					entry.Id = null;
				// Añade la catgoría al canal
				track.Categories.Add(newCategory);
				// Borra la categoría inicial si se debía cortar
				if (MustCut)
					category.Track.Categories.Remove(category);
		}

		/// <summary>
		///		Importa un archivo de entradas
		/// </summary>
		private void ImportEntries()
		{
			string fileName = DevConferencesViewModel.Instance.DialogsController.OpenDialogLoad(null, "Archivos XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*");

				// Importa las entradas
				if (!string.IsNullOrWhiteSpace(fileName) && System.IO.File.Exists(fileName))
				{
					CategoryModel category = (SelectedNode?.Tag as CategoryModel);

						if (category == null)
							DevConferencesViewModel.Instance.ControllerWindow.ShowMessage("Seleccione una categoría");
						else
						{
							EntryModelCollection entries = DevConferencesViewModel.Instance.TrackManager.LoadExchangeFileEntries(fileName);

								// Actualiza el autor con el nombre de la categoría
								foreach (EntryModel entry in entries)
									if (string.IsNullOrWhiteSpace(entry.Authors))
										entry.Authors = category.Title;
								// Elimina las entradas que ya estén en la categoría
								for (int index = entries.Count - 1; index >= 0; index--)
									if (category.Entries.SearchByUrlVideo(entries[index].UrlVideo) != null)
										entries.RemoveAt(index);
								// Añade las entradas y graba el proyecto
								if (entries.Count > 0)
								{
									category.Entries.AddRange(entries);
									UpdateTracks();
								}
						}
				}
		}

		/// <summary>
		///		Borra los datos de un manager de canales
		/// </summary>
		private void DeleteTrackManager(TrackManagerModel trackManager)
		{
			if (DevConferencesViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar el administrador de canales '{trackManager.Title}'?"))
			{
				// Borra el canal
				DevConferencesViewModel.Instance.TrackManager.RemoveTrackManager(trackManager);
				// Actualiza el árbol
				UpdateTracks();
			}
		}

		/// <summary>
		///		Borra los datos de un canal
		/// </summary>
		private void DeleteTrack(TrackModel track)
		{
			if (DevConferencesViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar el canal '{track.Title}'?"))
			{
				// Borra el canal
				DevConferencesViewModel.Instance.TrackManager.RemoveTrack(track);
				// Actualiza el árbol
				UpdateTracks();
			}
		}

		/// <summary>
		///		Borra los datos de un <see cref="CategoryModel"/>
		/// </summary>
		private void DeleteCategory(CategoryModel category)
		{ 
			if (DevConferencesViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar la categoría '{category.Title}'?"))
			{ 
				// Borra la categoría del canal
				category.Track.Categories.Remove(category);
				// Actualiza el árbol
				UpdateTracks();
			}
		}

		/// <summary>
		///		Borra los datos de un <see cref="Models.Servers.Accounts.AccountModel"/>
		/// </summary>
		private void DeleteEntry(EntryModel entry)
		{
			if (DevConferencesViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar la entrada '{entry.Title}'?"))
			{
				// Borra la cuenta de la colección
				DevConferencesViewModel.Instance.TrackManager.RemoveEntry(entry);
				// Actualiza el árbol
				UpdateTracks();
			}
		}

		/// <summary>
		///		Modifica el archivo de canales y actualiza el árbol
		/// </summary>
		private void UpdateTracks()
		{
			DevConferencesViewModel.Instance.TrackManager.Save();
			Load();
		}

		/// <summary>
		///		Nodo seleccionado
		/// </summary>
		public TreeExplorerNodeViewModel SelectedNode
		{	
			get { return _selectedNode; }
			set { CheckObject(ref _selectedNode, value); }
		}

		/// <summary>
		///		Buffer que almacena un elemento para instrucciones de copiar / pegar
		/// </summary>
		private BaseModel BufferCopy
		{
			get { return _bufferCopy; }
			set { CheckObject(ref _bufferCopy, value); }
		}

		/// <summary>
		///		Nodos
		/// </summary>
		public ObservableCollection<IHierarchicalViewModel> Children { get; } = new ObservableCollection<IHierarchicalViewModel>();

		/// <summary>
		///		Indica si se debe cortar el nodo al copiar / pegar
		/// </summary>
		private bool MustCut { get; set; }

		/// <summary>
		///		Creación de un administrador de canales
		/// </summary>
		public BaseCommand NewTrackManagerCommand { get; }

		/// <summary>
		///		Creación de un nuevo canal
		/// </summary>
		public BaseCommand NewTrackCommand { get; }

		/// <summary>
		///		Comando de nueva categoría
		/// </summary>
		public BaseCommand NewCategoryCommand { get; }

		/// <summary>
		///		Comando de nuevo artículo
		/// </summary>
		public BaseCommand NewEntryCommand { get; }

		/// <summary>
		///		Comando para abrir las propiedades de un elemento
		/// </summary>
		public BaseCommand OpenCommand { get; }

		/// <summary>
		///		Comando para ver un vídeo
		/// </summary>
		public BaseCommand SeeVideoCommand { get; }

		/// <summary>
		///		Comando para copiar las propiedades de un elemento
		/// </summary>
		public BaseCommand CopyCommand { get; }

		/// <summary>
		///		Comando para cortar las propiedades de un elemento
		/// </summary>
		public BaseCommand CutCommand { get; }

		/// <summary>
		///		Comando para pegar las propiedades de un elemento
		/// </summary>
		public BaseCommand PasteCommand { get; }

		/// <summary>
		///		Comando para borrar las propiedades de un elemento
		/// </summary>
		public BaseCommand DeleteCommand { get; }

		/// <summary>
		///		Comando para importar un archivo
		/// </summary>
		public BaseCommand ImportCommand { get; }

		/// <summary>
		///		Comando para procesar un canal o un administrador
		/// </summary>
		public BaseCommand ProcessCommand { get; }
	}
}
