using System;

using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;
using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.BauMvvm.ViewModels;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.EditorInstruction
{
	/// <summary>
	///		ViewModel para el formulario de mantenimiento de instrucciones del editor
	/// </summary>
	public class ListInstructionsViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{
		public ListInstructionsViewModel()
		{ 
			// Inicializa las propiedades
			Instructions = new ListViewModel<EditorInstructionListItemViewModel>();
			LoadListInstructions();
			// Inicializa los comandos 
			//? Después de cambiar las instrucciones porque si no el AddListener daría un error (Instructions == null)
			NewInstructionCommand = new BaseCommand("Nueva instrucción",
													parameter => ExecuteAction(nameof(NewInstructionCommand), parameter),
													parameter => CanExecuteAction(nameof(NewInstructionCommand), parameter));
			UpdateInstructionCommand = new BaseCommand("Modificar instrucción",
													   parameter => ExecuteAction(nameof(UpdateInstructionCommand), parameter),
													   parameter => CanExecuteAction(nameof(UpdateInstructionCommand), parameter))
												.AddListener(Instructions, nameof(Instructions.SelectedItem));
			DeleteInstructionCommand = new BaseCommand("Borrar instrucción",
													   parameter => ExecuteAction(nameof(DeleteInstructionCommand), parameter),
													   parameter => CanExecuteAction(nameof(DeleteInstructionCommand), parameter))
												.AddListener(Instructions, nameof(Instructions.SelectedItem));
			// Indica que no ha habido modificaciones
			IsUpdated = false;
		}

		/// <summary>
		///		Carga la lista de instrucciones
		/// </summary>
		private void LoadListInstructions()
		{
			EditorInstructionModelCollection instructions = new EditorInstructionBussiness().Load(DocWriterViewModel.Instance.FileNameEditorInstructions);

				// Limpia la lista de instrucciones
				Instructions.ListItems.Clear();
				// Añade las instrucciones
				foreach (EditorInstructionModel instruction in instructions)
					Instructions.Add(new EditorInstructionListItemViewModel(instruction));
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		private void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewInstructionCommand):
						OpenUpdateForm(null);
					break;
				case nameof(UpdateInstructionCommand):
						if (Instructions.SelectedItem != null && Instructions.SelectedItem.Tag is EditorInstructionModel)
							OpenUpdateForm(Instructions.SelectedItem.Tag as EditorInstructionModel);
					break;
				case nameof(DeleteInstructionCommand):
						DeleteInstruction(Instructions.SelectedItem?.Tag as EditorInstructionModel);
					break;
				case nameof(SaveCommand):
						Save();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		private bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(NewInstructionCommand):
				case nameof(SaveCommand):
					return true;
				case nameof(UpdateInstructionCommand):
				case nameof(DeleteInstructionCommand):
					return Instructions.SelectedItem != null;
				default:
					return false;
			}
		}

		/// <summary>
		///		Abre el formulario de modificación de una instrucción
		/// </summary>
		private void OpenUpdateForm(EditorInstructionModel instruction)
		{
			if (DocWriterViewModel.Instance.ViewsController.OpenFormEditorInstruction(instruction) == BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes)
			{
				LoadListInstructions();
				IsUpdated = true;
			}
		}

		/// <summary>
		///		Borra una instrucción
		/// </summary>
		private void DeleteInstruction(EditorInstructionModel instruction)
		{
			if (instruction != null &&
					  DocWriterViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar la instrucción '{instruction.Name}'?"))
			{
				EditorInstructionModelCollection instructions;

					// Carga las instrucciones
					instructions = new EditorInstructionBussiness().Load(DocWriterViewModel.Instance.FileNameEditorInstructions);
					// Elimina la instrucción actual
					instructions.RemoveByName(instruction.Name);
					// Graba las instrucciones
					new EditorInstructionBussiness().Save(DocWriterViewModel.Instance.FileNameEditorInstructions, instructions);
					// Actualiza la lista e indica que ha habido modificaciones
					LoadListInstructions();
					IsUpdated = true;
			}
		}

		/// <summary>
		///		Graba los datos de la instrucción
		/// </summary>
		protected override void Save()
		{
			RaiseEventClose(IsUpdated);
		}

		/// <summary>
		///		Instrucciones
		/// </summary>
		public ListViewModel<EditorInstructionListItemViewModel> Instructions { get; }

		/// <summary>
		///		Comando nueva instrucción
		/// </summary>
		public BaseCommand NewInstructionCommand { get; }

		/// <summary>
		///		Comando modificar instrucción
		/// </summary>
		public BaseCommand UpdateInstructionCommand { get; }

		/// <summary>
		///		Comando borrar instrucción
		/// </summary>
		public BaseCommand DeleteInstructionCommand { get; }
	}
}
