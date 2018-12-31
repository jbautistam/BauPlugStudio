using System;

using Bau.Libraries.SourceEditor.Model.Definitions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.ListItems
{
	/// <summary>
	///		ViewModel para definiciones de archivos
	/// </summary>
	public class FileDefinitionListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public FileDefinitionListItemViewModel(FileDefinitionModel file)
		{
			Tag = FileDefinition = file;
			Name = file.Name;
			Extension = file.Extension;
		}

		/// <summary>
		///		Nombre del tipo de archivo
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Extensión
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		///		Definición del archivo
		/// </summary>
		public FileDefinitionModel FileDefinition { get; }
	}
}
