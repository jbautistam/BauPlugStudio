using System;

using Bau.Libraries.LibFtpClient;

namespace Bau.Libraries.FtpManager.ViewModel.FtpExplorer.FtpConnectionItems
{
	/// <summary>
	///		ViewModel para un elemento de una lista de archivos
	/// </summary>
	public class FtpFileListItemViewModel : BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel
	{   
		// Variables privadas
		private string _fileName;
		private DateTime _createdAt;
		private long _size;

		public FtpFileListItemViewModel(FtpEntry ftpEntry) : base(ftpEntry.Name, ftpEntry)
		{
			bool initialized = false;

				// Guarda el archivo
				File = ftpEntry;
				// Inicializa los datos del archivo
				if (File != null)
					try
					{ 
						// Asigna los datos
						FileName = File.Name;
						DateCreate = File.Date;
						Size = File.Size;
						// Indica que se ha inicializado correctamente
						initialized = true;
					}
					catch { }
				// Si no se ha inicializado, se muestran los datos de error
				if (!initialized)
				{
					FileName = "Error";
					DateCreate = DateTime.Now;
					Size = 0;
				}
		}

		/// <summary>
		///		Datos del archivo Ftp
		/// </summary>
		public FtpEntry File { get; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return _fileName; }
			set { CheckProperty(ref _fileName, value); }
		}

		/// <summary>
		///		Fecha de creación
		/// </summary>
		public DateTime DateCreate
		{
			get { return _createdAt; }
			set { CheckProperty(ref _createdAt, value); }
		}

		/// <summary>
		///		Tamaño del archivo
		/// </summary>
		public long Size
		{
			get { return _size; }
			set { CheckProperty(ref _size, value); }
		}
	}
}