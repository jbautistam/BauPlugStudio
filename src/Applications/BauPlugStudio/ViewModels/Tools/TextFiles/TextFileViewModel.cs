using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Applications.BauPlugStudio.ViewModels.Tools.TextFiles
{
	/// <summary>
	///		ViewModel con los datos de un archivo de texto
	/// </summary>
	public class TextFileViewModel : Bau.Libraries.BauMvvm.ViewModels.Forms.BaseFormViewModel
	{ 
		// Variables privadas
		private string _content;

		public TextFileViewModel(string fileName, string template = null)
		{
			FileName = fileName;
			LoadFile(fileName, template);
		}

		/// <summary>
		///		Carga los datos del archivo de texto
		/// </summary>
		private void LoadFile(string fileName, string template)
		{ 
			// Carga el archivo en el editor de texto
			if (!System.IO.File.Exists(fileName) && !template.IsEmpty() && System.IO.File.Exists(template))
				Content = Libraries.LibCommonHelper.Files.HelperFiles.LoadTextFile(template);
			else
				Content = Libraries.LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName);
			// Si está vacío y tenemos plantilla, la carga
			if (Content.IsEmpty() && !template.IsEmpty() && System.IO.File.Exists(template))
				Content = Libraries.LibCommonHelper.Files.HelperFiles.LoadTextFile(template);
			// Indica que aún no se ha hecho ninguna modificación
			base.IsUpdated = false;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		private void Save()
		{ 
			// Graba el archivo
			Libraries.LibCommonHelper.Files.HelperFiles.SaveTextFile(FileName, Content);
			// Indica que no hay modificaciones pendientes
			base.IsUpdated = false;
		}

		/// <summary>
		///		Borra el archivo
		/// </summary>
		private void Delete()
		{
			if (Globals.HostController.ControllerWindow.ShowQuestion("¿Realmente desea borrar este archivo?"))
			{ 
				// Borra el archivo
				Libraries.LibCommonHelper.Files.HelperFiles.KillFile(FileName);
				// Cierra la ventana
				base.IsUpdated = false;
				base.Close(Libraries.BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes);
			}
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(SaveCommand):
						Save();
					break;
				case nameof(DeleteCommand):
						Delete();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(DeleteCommand):
				case nameof(SaveCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Título del documento
		/// </summary>
		public string Title
		{
			get { return System.IO.Path.GetFileName(FileName); }
		}

		/// <summary>
		///		Contenido
		/// </summary>
		public string Content
		{
			get { return _content; }
			set { CheckProperty(ref _content, value); }
		}

		/// <summary>
		///		Archivo
		/// </summary>
		protected string FileName { get; }
	}
}
