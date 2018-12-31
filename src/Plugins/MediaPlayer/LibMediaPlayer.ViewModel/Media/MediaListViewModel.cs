using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Bau.Libraries.LibMediaPlayer.Model;
using Bau.Libraries.LibMediaPlayer.Application.Bussiness;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Tools.Media
{
	/// <summary>
	///		ViewModel para la lista de archivos del MediaPlayer
	/// </summary>
	public class MediaListViewModel : BaseFormViewModel
	{ 
		// Eventos públicos
		public event EventHandler<Controllers.EventArguments.FileEventArguments> Play;
		public event EventHandler Stop;
		// Variables privadas
		private MediaItemListViewModel _selectedFile;
		private ObservableCollection<MediaItemListViewModel> _mediaFiles = new ObservableCollection<MediaItemListViewModel>();
		private List<string> _playingFiles = new List<string>();
		private bool _isPlaying, _canDownload;

		public MediaListViewModel()
		{
			// Inicializa los comandos y los manejadores de eventos
			PlayCommand = new BaseCommand(parameter => ExecuteAction(nameof(PlayCommand), parameter),
										  parameter => CanExecuteAction(nameof(PlayCommand), parameter))
								.AddListener(this, nameof(MediaFiles));
			StopCommand = new BaseCommand(parameter => ExecuteAction(nameof(StopCommand), parameter),
										  parameter => CanExecuteAction(nameof(StopCommand), parameter))
								.AddListener(this, nameof(IsPlaying));
			DownloadCommand = new BaseCommand(parameter => ExecuteAction(nameof(DownloadCommand), parameter),
											  parameter => CanExecuteAction(nameof(DownloadCommand), parameter))
								.AddListener(this, nameof(CanDownload));
			MediaPlayerViewModel.Instance.MessengerController.Play += (sender, args) => AddAlbum(args.Album);
			// Carga la lista de reproducción
			LoadReproductionList();
		}

		/// <summary>
		///		Carga la lista de reproducción
		/// </summary>
		private void LoadReproductionList()
		{
			ReproductionList = MediaPlayerViewModel.Instance.MediaManager.LoadReproductionList();
			ShowReproductionList();
		}

		/// <summary>
		///		Añade un álbum a la lista de reproducción
		/// </summary>
		private void AddAlbum(MediaAlbumModel album)
		{
			MediaAlbumModel newAlbum = ReproductionList.Albums.SearchByName(album.Name);

				// Si no se ha encontrado el archivo en la lista de reproducción, se añade
				if (newAlbum == null)
					newAlbum = ReproductionList.Albums.Add(album.Name, album.Description);
				// Añade los archivos
				foreach (MediaFileModel file in album.Files)
					if (!newAlbum.Files.ExistsByFile(file))
						newAlbum.Files.Add(file);
				// Graba la lista de reproducción
				SaveReproductionList();
				// Muestra la lista de reproducción
				ShowReproductionList();
		}

		/// <summary>
		///		Muestra la lista de reproducción
		/// </summary>
		private void ShowReproductionList()
		{
			// Limpia la lista
			MediaFiles.Clear();
			CanDownload = false;
			// Añade los elementos
			foreach (MediaAlbumModel album in ReproductionList.Albums)
				foreach (MediaFileModel file in album.Files)
				{
					var fileViewModel = new MediaItemListViewModel(file);

						// Añade el archivo
						MediaFiles.Add(fileViewModel);
						// Indica si hay algún archivo para descargar
						if (fileViewModel.CanDownload)
							CanDownload = true;
				}
		}

		/// <summary>
		///		Graba la lista de reproducción
		/// </summary>
		private void SaveReproductionList()
		{
			MediaPlayerViewModel.Instance.MediaManager.SaveReproductionList(ReproductionList);
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(PlayCommand):
						PlayFiles();
					break;
				case nameof(StopCommand):
						StopPlaying();
					break;
				case nameof(DownloadCommand):
						DownloadFiles();
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
				case nameof(PlayCommand):
					return true;
				case nameof(DownloadCommand):
					return CanDownload;
				case nameof(StopCommand):
					return IsPlaying;
				default:
					return false;
			}
		}

		/// <summary>
		///		Reproduce los archivos seleccionados
		/// </summary>
		private void PlayFiles()
		{
			// Elimina los archivos de reproducción
			_playingFiles.Clear();
			// Obtiene los elementos seleccionados
			foreach (MediaItemListViewModel fileViewModel in GetSelectedFiles())
				_playingFiles.Add(fileViewModel.FullFileName);
			// Llama al evento de reproducción de archivos
			Play?.Invoke(this, new Controllers.EventArguments.FileEventArguments(_playingFiles));
		}

		/// <summary>
		///		Obtiene los archivos seleccionados
		/// </summary>
		private List<MediaItemListViewModel> GetSelectedFiles()
		{
			List<MediaItemListViewModel> files = new List<MediaItemListViewModel>();

				// Obtiene los elementos seleccionados
				foreach (MediaItemListViewModel file in MediaFiles)
					if (file.IsSelected)
						files.Add(file);
				// Si no hay nada, añade todos los elementos
				if (files.Count == 0)
					foreach (MediaItemListViewModel file in MediaFiles)
						files.Add(file);
				// Devuelve los elementos
				return files;
		}

		/// <summary>
		///		Detiene la reproducción del adjunto
		/// </summary>
		private void StopPlaying()
		{
			Stop?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		///		Descarga los archivos
		/// </summary>
		private void DownloadFiles()
		{
			MediaFileModelCollection files = GetDownloadFiles();

				if (files.Count == 0)
					MediaPlayerViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún archivo para descargar");
				else
				{
					MediaFilesDownloader downloader = new MediaFilesDownloader(MediaPlayerViewModel.Instance.MediaManager.PathFiles, files);

						// Asigna los manejadores de eventos
						downloader.EndDownload += (sender, args) => LoadReproductionList();
						downloader.MediaFileDownload += (sender, args) => MediaPlayerViewModel.Instance.HostController.Messenger.SendLog(MediaPlayerViewModel.Instance.ModuleName,
																																		 MessageLog.LogType.Information,
																																		 $"Descargando {args.File.Url}", args.File.FileName, null);
						// Descarga los archivos
						downloader.DownloadFiles();
				}
		}

		/// <summary>
		///		Obtiene los archivos para descargar
		/// </summary>
		private MediaFileModelCollection GetDownloadFiles()
		{
			var files = new	MediaFileModelCollection();

				// Obtiene los archivos para descargar
				foreach (MediaItemListViewModel fileViewModel in GetSelectedFiles())
					if (fileViewModel.CanDownload)
						files.Add(fileViewModel.File);
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Borra los archivos
		/// </summary>
		private void Delete()
		{
			List<MediaItemListViewModel> files = GetSelectedFiles();

				if (files.Count > 0 &&
					MediaPlayerViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea quitar de la lista los elementos seleccionados?"))
				{
					// Borra físicamente los archivos
					if (MediaPlayerViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea eliminar del ordenador los archivos multimedia?"))
					{
						foreach (MediaItemListViewModel file in files)
							if (!file.FullFileName.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) &&
									!file.FullFileName.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
								LibCommonHelper.Files.HelperFiles.KillFile(file.FullFileName);
					}
					// Borra los archivos de la lista de reproducción
					foreach (MediaItemListViewModel file in files)
					{
						ReproductionList.Delete(file.File);
						MediaFiles.Remove(file);
					}
					// Graba la lista de reproducción
					SaveReproductionList();
				}
		}

		/// <summary>
		///		Lista de reproducción
		/// </summary>
		private MediaFolderModel ReproductionList { get; set; } = new MediaFolderModel();

		/// <summary>
		///		Elemento seleccionado
		/// </summary>
		public MediaItemListViewModel SelectedFile
		{
			get { return _selectedFile; }
			set { CheckObject(ref _selectedFile, value); }
		}

		/// <summary>
		///		Archivos
		/// </summary>
		public ObservableCollection<MediaItemListViewModel> MediaFiles
		{
			get { return _mediaFiles; }
			set { CheckObject(ref _mediaFiles, value); }
		}

		/// <summary>
		///		Indica si se puede descargar
		/// </summary>
		public bool CanDownload
		{
			get { return _canDownload; }
			set { CheckProperty(ref _canDownload, value); }
		}

		/// <summary>
		///		Indica si se está reproduciendo algún archivo
		/// </summary>
		public bool IsPlaying
		{
			get { return _isPlaying; }
			set { CheckProperty(ref _isPlaying, value); }
		}

		/// <summary>
		///		Comando para reproducir un archivo
		/// </summary>
		public BaseCommand PlayCommand { get; }

		/// <summary>
		///		Comando para detener la reproducción de un archivo
		/// </summary>
		public BaseCommand StopCommand { get; }

		/// <summary>
		///		Comando para descargar archivos
		/// </summary>
		public BaseCommand DownloadCommand { get; }
	}
}
