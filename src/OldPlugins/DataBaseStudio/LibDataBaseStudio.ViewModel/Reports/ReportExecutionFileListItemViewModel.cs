using System;

using Bau.Libraries.LibDataBaseStudio.Model.Reports;

namespace Bau.Libraries.LibDataBaseStudio.ViewModel.Reports
{
	/// <summary>
	///		ViewModel para los elementos del listView de archivos asociados a un informe
	/// </summary>
	public class ReportExecutionFileListItemViewModel : MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public ReportExecutionFileListItemViewModel(BauMvvm.ViewModels.BaseObservableObject form, ReportExecutionFileModel file) : base(form)
		{
			File = file;
			Text = file.GlobalId;
			Tag = file;
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public ReportExecutionFileModel File { get; private set; }

		/// <summary>
		///		Tipo de archivo
		/// </summary>
		public string FileType
		{
			get
			{
				switch (File.IDType)
				{
					case ReportExecutionFileModel.FileType.Font:
						return "Fuente";
					case ReportExecutionFileModel.FileType.Image:
						return "Imagen";
					default:
						return "Estilo";
				}
			}
		}

		/// <summary>
		///		Nobre de archivo
		/// </summary>
		public string FileName
		{
			get { return File.FileName; }
		}
	}
}
