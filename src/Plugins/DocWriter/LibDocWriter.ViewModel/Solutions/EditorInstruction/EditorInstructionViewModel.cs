using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction;
using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.EditorInstruction
{
	/// <summary>
	///		ViewModel para el formulario de mantenimiento de instrucciones del editor
	/// </summary>
	public class EditorInstructionViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private string _previousName, _name, _code;

		public EditorInstructionViewModel(EditorInstructionModel instruction)
		{
			if (instruction != null)
			{
				_previousName = instruction.Name;
				Name = instruction.Name;
				Code = instruction.Code;
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (Name.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la instrucción");
				else if (Code.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el código de la instrucción");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos de la instrucción
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				EditorInstructionModelCollection instructions;

					// Carga las instrucciones
					instructions = new EditorInstructionBussiness().Load(DocWriterViewModel.Instance.FileNameEditorInstructions);
					// Si es una modificación, cambia los datos
					if (!_previousName.IsEmpty())
						instructions.RemoveByName(_previousName);
					// Añade la instrucción
					instructions.Add(new EditorInstructionModel { Name = Name, Code = Code });
					// Graba las instrucciones
					new EditorInstructionBussiness().Save(DocWriterViewModel.Instance.FileNameEditorInstructions, instructions);
					// Cierra la ventana
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Título de la instrucción
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Código de la instrucción
		/// </summary>
		public string Code
		{
			get { return _code; }
			set { CheckProperty(ref _code, value); }
		}
	}
}
