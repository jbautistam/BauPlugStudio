using System;

using Bau.Libraries.LibDataStructures.Base;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.DatabaseStudio.Models.Deployment;

namespace Bau.Libraries.DatabaseStudio.ViewModels.Projects.Deployments.Parameters
{
	/// <summary>
	///		ListView de conexiones a base de datos
	/// </summary>
	public class ParameterListViewModel : ControlListViewModel
	{
		public ParameterListViewModel()
		{
			NewItemCommand = new BaseCommand(parameter => OpenParameter(null));
			DeleteItemCommand = new BaseCommand(parameter => DeleteParameter(),
												parameter => SelectedItem != null)
								.AddListener(this, nameof(SelectedItem));

		}

		/// <summary>
		///		Carga los parámetros
		/// </summary>
		public void LoadItems(BaseModelCollection<ParameterModel> parameters)
		{
			foreach (ParameterModel parameter in parameters)
				Items.Add(new ParameterViewModel(parameter));
		}

		/// <summary>
		///		Abre el formulario de modificación de una conexión
		/// </summary>
		private void OpenParameter(ParameterModel parameter)
		{
			Items.Add(new ParameterViewModel(parameter));
		}

		/// <summary>
		///		Borra el parámetro seleccionado
		/// </summary>
		private void DeleteParameter()
		{
			if (SelectedItem != null && MainViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea borrar el parámetro seleccionado?"))
				Items.Remove(SelectedItem);
		}

		/// <summary>
		///		Comprueba los datos
		/// </summary>
		internal bool ValidateData(out string error)
		{
			// Inicializa los argumentos de salida
			error = string.Empty;
			// Comprueba los parámetros
			foreach (ControlItemViewModel item in Items)
				if (string.IsNullOrEmpty(error) && item is ParameterViewModel parameterViewModel)
					parameterViewModel.ValidateData(out error);
			// Devuelve el valor que indica si es correcto
			return string.IsNullOrEmpty(error);
		}

		/// <summary>
		///		Obtiene una lista de parámetros
		/// </summary>
		internal BaseModelCollection<ParameterModel> GetParameters()
		{
			BaseModelCollection<ParameterModel> parameters = new BaseModelCollection<ParameterModel>();

				// Asigna los parámetros
				foreach (ControlItemViewModel item in Items)
					if (item is ParameterViewModel parameterViewModel)
						parameters.Add(parameterViewModel.GetParameter());
				// Devuelve los parámetros
				return parameters;
		}

		/// <summary>
		///		Comando para crear un elemento
		/// </summary>
		public BaseCommand NewItemCommand { get; }

		/// <summary>
		///		Comando para borrar un elemento
		/// </summary>
		public BaseCommand DeleteItemCommand { get; }
	}
}
