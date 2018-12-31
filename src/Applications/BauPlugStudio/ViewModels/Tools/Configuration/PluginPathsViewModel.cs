using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Applications.BauPlugStudio.Controllers.Plugins;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Applications.BauPlugStudio.ViewModels.Tools.Configuration
{
	/// <summary>
	///		ViewModel para los directorios de plugins
	/// </summary>
	public class PluginPathsViewModel : Bau.Libraries.BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private ListViewModel<PluginPathItemViewModel> _paths;

		public PluginPathsViewModel()
		{
			// Inicializa los comandos
			NewPathCommand = new BaseCommand(parameter => NewPathPlugin());
			DeletePathCommand = new BaseCommand(parameter => DeletePathPlugin(),
												parameter => PathPlugins.SelectedItem != null);
			// Inicializa las propiedades
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{ 
			// Inicializa las propiedades
			PathPlugins = new ListViewModel<PluginPathItemViewModel>();
			LoadListProjects();
			// Indica que no ha habido modificaciones
			IsUpdated = false;
		}

		/// <summary>
		///		Carga la lista de directorios
		/// </summary>
		private void LoadListProjects()
		{
			PluginPathModelCollection paths = new PluginPathModelCollection();

				// Carga los directorios
				paths.Load();
				// Añade los directorios
				foreach (PluginPathModel path in paths)
					AddPath(path);
		}

		/// <summary>
		///		Añade un directorio a la lista
		/// </summary>
		public void AddPath(PluginPathModel path)
		{
			if (!path.Path.IsEmpty() && System.IO.Directory.Exists(path.Path))
				PathPlugins.Add(new PluginPathItemViewModel(path));
		}

		/// <summary>
		///		Obtiene el proyecto destino seleccionado
		/// </summary>
		private PluginPathItemViewModel GetSelectedPath()
		{
			PluginPathItemViewModel item = PathPlugins.SelectedItem;

				if (item == null)
					return null;
				else
					return item;
		}

		/// <summary>
		///		Añade un directorio a la lista
		/// </summary>
		public void NewPathPlugin()
		{
			if (Globals.HostController.DialogsController.OpenDialogSelectPath(null, out string path) == SystemControllerEnums.ResultType.Yes)
			{
				if (!path.IsEmpty() && System.IO.Directory.Exists(path))
					AddPath(new PluginPathModel(path, true));
			}
		}

		/// <summary>
		///		Borra el proyecto destino
		/// </summary>
		private void DeletePathPlugin()
		{
			PluginPathItemViewModel target = GetSelectedPath();

				if (target != null &&
					Globals.HostController.ControllerWindow.ShowQuestion($"¿Realmente desea quitar '{target.Text}'?\nDirectorio: {target.Path}"))
				{ 
					// Borra el directorio
					PathPlugins.ListItems.Remove(target);
					// Indica que ha habido modificaciones
					IsUpdated = true;
				}
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			PluginPathModelCollection paths = new PluginPathModelCollection();

				// Añade los directorios
				foreach (PluginPathItemViewModel pathViewModel in PathPlugins.ListItems)
					paths.Add(new PluginPathModel(pathViewModel.Path, pathViewModel.IsChecked));
				// Graba los plugins
				paths.Save();
				// Cierra el formulario
				RaiseEventClose(true);
		}

		/// <summary>
		///		Proyectos destino
		/// </summary>
		public ListViewModel<PluginPathItemViewModel> PathPlugins
		{
			get { return _paths; }
			set { CheckObject(ref _paths, value); }
		}

		/// <summary>
		///		Comando de nuevo directorio
		/// </summary>
		public BaseCommand NewPathCommand { get; }

		/// <summary>
		///		Comando de borrar directorio
		/// </summary>
		public BaseCommand DeletePathCommand { get; }
	}
}
