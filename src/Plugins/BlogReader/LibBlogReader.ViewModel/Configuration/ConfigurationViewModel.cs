using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibBlogReader.ViewModel.Configuration
{
	/// <summary>
	///		ViewModel para la configuración
	/// </summary>
	public class ConfigurationViewModel : BauMvvm.ViewModels.BaseObservableObject
	{   
		// Variables privadas
		private string _pathBlogs;
		private int _minutesBetweenDownload, _recordsPerPage;
		private bool _downloadEnabled;

		public ConfigurationViewModel()
		{
			PathBlogs = BlogReaderViewModel.Instance.PathBlogs;
			MinutesBetweenDownload = BlogReaderViewModel.Instance.MinutesBetweenDownload;
			RecordsPerPage = BlogReaderViewModel.Instance.RecordsPerPage;
			DownloadEnabled = BlogReaderViewModel.Instance.DownloadEnabled;
		}

		/// <summary>
		///		Comprueba si los datos son correctos
		/// </summary>
		public bool ValidateData(out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Devuelve el valor que indica si los datos son correctos
			return error.IsEmpty();
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		public void Save()
		{
			BlogReaderViewModel.Instance.PathBlogs = PathBlogs;
			BlogReaderViewModel.Instance.MinutesBetweenDownload = MinutesBetweenDownload;
			BlogReaderViewModel.Instance.DownloadEnabled = DownloadEnabled;
			BlogReaderViewModel.Instance.RecordsPerPage = RecordsPerPage;
		}

		/// <summary>
		///		Directorio a partir del que se encuentran los datos de los blogs
		/// </summary>
		public string PathBlogs
		{
			get { return _pathBlogs; }
			set { CheckProperty(ref _pathBlogs, value); }
		}

		/// <summary>
		///		Minutos entre descargas
		/// </summary>
		public int MinutesBetweenDownload
		{
			get { return _minutesBetweenDownload; }
			set { CheckProperty(ref _minutesBetweenDownload, value); }
		}

		/// <summary>
		///		Indica si la descarga automática está activa
		/// </summary>
		public bool DownloadEnabled
		{
			get { return _downloadEnabled; }
			set { CheckProperty(ref _downloadEnabled, value); }
		}

		/// <summary>
		///		Registros que se muestran al visualizar los blogs
		/// </summary>
		public int RecordsPerPage
		{
			get { return _recordsPerPage; }
			set { CheckProperty(ref _recordsPerPage, value); }
		}
	}
}
