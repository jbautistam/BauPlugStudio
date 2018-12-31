using System;

using Bau.Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.EditorInstruction
{
	/// <summary>
	///		ViewModel para una instrucción del editor en un listView
	/// </summary>
	public class EditorInstructionListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public EditorInstructionListItemViewModel(EditorInstructionModel instruction)
		{
			Name = instruction.Name;
			Code = instruction.Code;
			Text = Name;
			Tag = instruction;
		}

		/// <summary>
		///		Nombre de la instrucción
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Código
		/// </summary>
		public string Code { get; set; }
	}
}
