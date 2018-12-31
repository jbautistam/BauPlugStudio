using System;

using Bau.Libraries.BauMvvm.ViewModels;

namespace Bau.Applications.BauPlugStudio.ViewModels.AvalonLayout
{
	/// <summary>
	///		ViewModel vacío --> borra los comandos predeterminados
	/// </summary>
	public class EmptyViewModel : DocumentViewModel
	{
		public EmptyViewModel() : base(null, "Sin título", null, null)
		{
			NewCommand = new BaseCommand(parameter => ExecuteAction(parameter), parameter => CanExecuteAction(parameter));
			SaveCommand = new BaseCommand(parameter => ExecuteAction(parameter), parameter => CanExecuteAction(parameter));
			DeleteCommand = new BaseCommand(parameter => ExecuteAction(parameter), parameter => CanExecuteAction(parameter));
			PropertiesCommand = new BaseCommand(parameter => ExecuteAction(parameter), parameter => CanExecuteAction(parameter));
			RefreshCommand = new BaseCommand(parameter => ExecuteAction(parameter), parameter => CanExecuteAction(parameter));
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		public void ExecuteAction(object parameter)
		{
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		public bool CanExecuteAction(object parameter)
		{
			return false;
		}

		/// <summary>
		///		Comando para crear un nuevo elemento
		/// </summary>
		public BaseCommand NewCommand { get; }

		/// <summary>
		///		Comando para grabar un elemento
		/// </summary>
		public new BaseCommand SaveCommand { get; }

		/// <summary>
		///		Comando para borrar un elemento
		/// </summary>
		public BaseCommand DeleteCommand { get; }

		/// <summary>
		///		Comando para mostrar las propiedades del elemento
		/// </summary>
		public BaseCommand PropertiesCommand { get; }

		/// <summary>
		///		Comando para actualizar las propiedades del elemento
		/// </summary>
		public BaseCommand RefreshCommand { get; }
	}
}
