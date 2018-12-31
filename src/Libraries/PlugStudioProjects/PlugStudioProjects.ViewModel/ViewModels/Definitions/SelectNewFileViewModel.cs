using System;
using System.Collections.ObjectModel;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.PlugStudioProjects.ViewModels.Definitions
{
	/// <summary>
	///		ViewModel para la selección de un nuevo archivo
	/// </summary>
	public class SelectNewFileViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{
		// Variables privadas
		private ObservableCollection<ControlItemViewModel> _definitions;
		private ControlItemViewModel _selectedItem;
		private string _fileName;

		public SelectNewFileViewModel(BauMvvm.ViewModels.Controllers.IHostSystemController controllerWindow, Models.ProjectItemDefinitionModel definition)
		{
			// Asigna las propiedades
			ControllerWindow = controllerWindow;
			Definitions = new ObservableCollection<ControlItemViewModel>();
			// Carga la lista de archivo
			LoadFilesList(definition);
		}

		/// <summary>
		///		Carga la lista de archivos
		/// </summary>
		private void LoadFilesList(Models.ProjectItemDefinitionModel definition)
		{
			foreach (Models.ProjectItemDefinitionModel fileDefinition in definition.Select(Models.ProjectItemDefinitionModel.ItemType.File))
				Definitions.Add(new ControlItemViewModel(fileDefinition.Name, fileDefinition));
		}

		/// <summary>
		///		Comprueba los datos seleccionados
		/// </summary>
		private bool ValidateData()
		{
			bool validated = false;

				// Comprueba los datos
				if (SelectedDefinition == null)
					ControllerWindow.ShowMessage("Seleccione un tipo de archivo");
				else if (string.IsNullOrEmpty(FileName))
					ControllerWindow.ShowMessage("Introduzca el nombre de archivo");
				else
					validated = true;
				// Devuelve el valor que indica si los datos son correctos
				return validated;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
				RaiseEventClose(true);
		}

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		private BauMvvm.ViewModels.Controllers.IHostSystemController ControllerWindow { get; }

		/// <summary>
		///		Lista de definiciones
		/// </summary>
		public ObservableCollection<ControlItemViewModel> Definitions
		{
			get { return _definitions; }
			set { CheckObject(ref _definitions, value); }
		}

		/// <summary>
		///		Elemento seleccionado
		/// </summary>
		public ControlItemViewModel SelectedItem
		{
			get { return _selectedItem; }
			set { CheckObject(ref _selectedItem, value); }
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Definición seleccionada
		/// </summary>
		public Models.ProjectItemDefinitionModel SelectedDefinition
		{
			get { return SelectedItem?.Tag as Models.ProjectItemDefinitionModel; }
		}
	}
}
