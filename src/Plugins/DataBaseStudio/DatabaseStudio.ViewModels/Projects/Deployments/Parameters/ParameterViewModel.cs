using System;
using System.Collections.ObjectModel;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models.Deployment;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments.Parameters
{
	/// <summary>
	///		ViewModel para los datos de un parámetro
	/// </summary>
	public class ParameterViewModel : ControlItemViewModel
	{
		// Variables privadas
		private string _key, _value;
		private ObservableCollection<ControlItemViewModel> _items;
		private ControlItemViewModel _selectedItem;

		public ParameterViewModel(ParameterModel parameter) : base(parameter?.GlobalId, parameter)
		{
			// Carga el combo de tipos
			LoadComboTypes();
			// Asigna las propiedades
			if (parameter == null)
			{
				parameter = new ParameterModel();
				Key = string.Empty;
			}
			else
				Key = parameter.GlobalId;
			SelectType(parameter.Type);
			Value = parameter.Value;
		}

		/// <summary>
		///		Carga la lista de tipos
		/// </summary>
		public void LoadComboTypes()
		{
			// Crea el combo
			ComboTypes = new ObservableCollection<ControlItemViewModel>();
			// Añade los tipos
			ComboTypes.Add(new ControlItemViewModel("Cadena", ParameterModel.ParameterType.String.ToString()));
			ComboTypes.Add(new ControlItemViewModel("Entero", ParameterModel.ParameterType.Integer.ToString()));
			ComboTypes.Add(new ControlItemViewModel("Decimal", ParameterModel.ParameterType.Decimal.ToString()));
			ComboTypes.Add(new ControlItemViewModel("Fecha", ParameterModel.ParameterType.DateTime.ToString()));
			ComboTypes.Add(new ControlItemViewModel("Lógico", ParameterModel.ParameterType.Boolean.ToString()));
			// Selecciona el primer elemento (la cadena)
			SelectedComboType = ComboTypes[0];
		}

		/// <summary>
		///		Selecciona un tipo
		/// </summary>
		public void SelectType(ParameterModel.ParameterType type)
		{
			foreach (ControlItemViewModel item in ComboTypes)
				if ((item.Tag.ToString().GetEnum(ParameterModel.ParameterType.String) == type))
					SelectedComboType = item;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		public bool ValidateData(out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Comprueba los datos
			if (string.IsNullOrEmpty(Key))
				error = "Introduzca la clave del parámetro";
			// Devuelve el valor que indica si los datos son correctos
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Obtiene los datos del parámetro
		/// </summary>
		public ParameterModel GetParameter()
		{
			return new ParameterModel
							{
								GlobalId = Key,
								Type = SelectedType,
								Value = Value
							};
		}

		/// <summary>
		///		Clave del parámetro
		/// </summary>
		public string Key
		{
			get { return _key; }
			set { CheckProperty(ref _key, value); }
		}

		/// <summary>
		///		Valor del parámetro
		/// </summary>
		public string Value
		{
			get { return _value; }
			set { CheckProperty(ref _value, value); }
		}

		/// <summary>
		///		Conexiones
		/// </summary>
		public ObservableCollection<ControlItemViewModel> ComboTypes
		{
			get { return _items; }
			set { CheckObject(ref _items, value); }
		}

		/// <summary>
		///		Elemento seleccionado
		/// </summary>
		public ControlItemViewModel SelectedComboType
		{
			get { return _selectedItem; }
			set { CheckProperty(ref _selectedItem, value); }
		}

		/// <summary>
		///		Conexión seleccionada
		/// </summary>
		public ParameterModel.ParameterType SelectedType
		{
			get { return (SelectedComboType?.Tag as string).GetEnum(ParameterModel.ParameterType.String); }
		}
	}
}
