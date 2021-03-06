﻿using System;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel con los datos de un archivo de DocWriter
	/// </summary>
	public class FileViewModel : BaseFormViewModel
	{ 
		// Variables privadas
		private string _title, _content;

		public FileViewModel(FileModel file)
		{
			File = file;
			LoadFile(file);
		}

		/// <summary>
		///		Carga los datos del archivo de texto
		/// </summary>
		private void LoadFile(FileModel file)
		{ 
			// Asigna el título inicial
			Title = file.Title;
			// Carga el archivo en el editor de texto
			Content = LibCommonHelper.Files.HelperFiles.LoadTextFile(file.FullFileName);
			// Indica que aún no se ha hecho ninguna modificación
			IsUpdated = false;
		}

		/// <summary>
		///		Graba los datos del archivo
		/// </summary>
		private void Save()
		{ 
			// Graba el archivo
			LibCommonHelper.Files.HelperFiles.SaveTextFile(File.FullFileName, Content);
			// Indica que no hay modificaciones pendientes
			IsUpdated = false;
		}

		/// <summary>
		///		Borra el archivo
		/// </summary>
		private void Delete()
		{
			if (DocWriterViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea borrar este archivo?"))
			{ 
				// Borra el archivo
				new Application.Bussiness.Solutions.FileBussiness().Delete(File);
				// Cierra la ventana
				IsUpdated = false;
				Close(BauMvvm.ViewModels.Controllers.SystemControllerEnums.ResultType.Yes);
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
			get { return _title; }
			set { CheckProperty(ref _title, value); }
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
		protected FileModel File { get; }
	}
}
