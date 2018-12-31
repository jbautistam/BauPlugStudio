using System;

using Bau.Libraries.BauMvvm.ViewModels.Media;
using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.DevConferences.Admon.ViewModel.Controllers;
using Bau.Libraries.DevConference.Admon.Application.ModelsManager;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Explorer
{
	/// <summary>
	///		Nodo del árbol del explorador
	/// </summary>
	public class TreeExplorerNodeViewModel : ControlHierarchicalViewModel, IHierarchicalViewModel
	{	
		public TreeExplorerNodeViewModel(TreeTracksViewModel trvTree, IHierarchicalViewModel parent, string text, object tag, 
										 bool lazyLoad, bool isBold = false, MvvmColor foreground = null) 
							: base(parent, text, tag, lazyLoad, foreground)
		{ 
			// Asigna las propiedades básicas
			Tree = trvTree;
			ToolTipText = text;
			IsBold = isBold;
			// Asigna el tipo de nodo
			if (tag is TrackManagerModel)
				NodeType = GlobalEnums.NodeTypes.TrackManager;
			else if (tag is TrackModel)
				NodeType = GlobalEnums.NodeTypes.Track;
			else if (tag is CategoryModel)
				NodeType = GlobalEnums.NodeTypes.Category;
			else if (tag is EntryModel)
				NodeType = GlobalEnums.NodeTypes.Entry;
		}

		/// <summary>
		///		Carga los nodos hijo
		/// </summary>
		public override void LoadChildrenData()
		{ 
			if (Tag is TrackManagerModel manager)
				LoadTracks(manager);
			else if (Tag is TrackModel track)
				LoadCategories(track);
			else if (Tag is CategoryModel category)
				LoadEntries(category);
		}

		/// <summary>
		///		Carga los canales
		/// </summary>
		private void LoadTracks(TrackManagerModel manager)
		{
			// Ordena los canales
			manager.Tracks.Sort((first, second) => first.Title.CompareTo(second.Title));
			// Añade los ndos
			foreach (TrackModel track in manager.Tracks)
				AddNode(track.Title, track, true, true, MvvmColor.Red);
		}

		/// <summary>
		///		Carga las categorías
		/// </summary>
		private void LoadCategories(TrackModel track)
		{ 
			// Ordena las categorías
			track.Categories.Sort((first, second) => first.Title.CompareTo(second.Title));
			// Añade los nodos
			foreach (CategoryModel category in track.Categories)
				AddNode(category.Title, category, false, true, MvvmColor.Navy);
		}

		/// <summary>
		///		Carga las entradas
		/// </summary>
		private void LoadEntries(CategoryModel category)
		{
			// Ordena las entradas
			category.Entries.Sort((first, second) => first.Title.CompareTo(second.Title));
			// Añade los nodos
			foreach (EntryModel entry in category.Entries)
				AddNode(entry.Title, entry, false, false);
		}

		/// <summary>
		///		Obtiene el color dependiendo de si el elemento está activo o no
		/// </summary>
		private MvvmColor GetColor(bool enabled, MvvmColor colorEnabled)
		{ 
			return enabled ? colorEnabled : MvvmColor.Gray;
		}

		/// <summary>
		///		Añade un nodo hijo
		/// </summary>
		private TreeExplorerNodeViewModel AddNode(string text, object tag, bool isBold, bool lazyLoad, MvvmColor foreground = null)
		{ 
			TreeExplorerNodeViewModel node = new TreeExplorerNodeViewModel(Tree, this, text, tag, lazyLoad, isBold, foreground);

				// Añade el nodo
				Children.Add(node);
				// Devuelve el nodo enviado
				return node;
		}

		/// <summary>
		///		Tipo del nodo
		/// </summary>
		public GlobalEnums.NodeTypes NodeType { get; }

		/// <summary>
		///		Texto del toolTip
		/// </summary>
		public string ToolTipText { get; }

		/// <summary>
		///		Indica si hay algún error en el elemento asociado al nodo
		/// </summary>
		public bool HasError { get; }

		/// <summary>
		///		Arbol padre del elemento
		/// </summary>
		private TreeTracksViewModel Tree { get; }
	}
}
