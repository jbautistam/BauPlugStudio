using System;
using System.Collections.Generic;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.LibMediaPlayer.ViewModel.Tools.Media;

namespace Bau.Plugins.MediaPlayer.Views.Media
{
	/// <summary>
	///		Ventana de log
	/// </summary>
	public partial class MediaPlayerView : UserControl, IFormView
	{   
		// Variables privadas
		private System.Windows.Media.MediaPlayer _mediaPlayer = new System.Windows.Media.MediaPlayer ();
		private List<string> _filesToPlay = new List<string>();
		private System.Timers.Timer _tmrTimer;

		public MediaPlayerView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el DataContext
			grdView.DataContext = ViewModel = new MediaListViewModel();
			FormView = new BaseFormView(ViewModel);
			// Crea el temporizador
			_tmrTimer = new System.Timers.Timer(TimeSpan.FromSeconds(2).TotalMilliseconds);
			_tmrTimer.Elapsed += (sender, args) => Dispatcher.Invoke(new Action(() => UpdateProgressBar()), null);
			// Asigna los manejadores de eventos
			//Unloaded += (sender, args) => StopPlay();
			ViewModel.Play += (sender, args) => EnqueFiles(args.Files);
			ViewModel.Stop += (sender, args) => StopPlay();
			// Asocia los manejadores de eventos del mediaplayer
			_mediaPlayer.MediaEnded += (sender, args) => PlayNext();
			_mediaPlayer.MediaOpened += (sender, args) => ShowProgressBar();
		}

		/// <summary>
		///		Muestra la barra de progreso
		/// </summary>
		private void ShowProgressBar()
		{
			// Muestra la barra de progreso
			prgMedia.Minimum = 0;
			prgMedia.Maximum = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
			prgMedia.Value = 0;
			prgMedia.Visibility = System.Windows.Visibility.Visible;
			// Arranca el temporizador
			_tmrTimer.Start();
		}

		/// <summary>
		///		Actualiza la barra de progreso
		/// </summary>
		private void UpdateProgressBar()
		{
			if (prgMedia.IsVisible && prgMedia.Maximum > _mediaPlayer.Position.TotalSeconds)
				prgMedia.Value = _mediaPlayer.Position.TotalSeconds;
		}

		/// <summary>
		///		Encola una serie de archivos
		/// </summary>
		private void EnqueFiles(List<string> files)
		{
			// Encola los archivos
			_filesToPlay.Clear();
			_filesToPlay.AddRange(files);
			// Reproduce el prime archivo
			Play();
		}

		/// <summary>
		///		Reproduce un archivo
		/// </summary>
		private void Play()
		{
			if (_filesToPlay.Count > 0 && !string.IsNullOrWhiteSpace(_filesToPlay[0]))
			{
				// Reproduce el archivo
				_mediaPlayer.Open(new Uri(_filesToPlay[0]));
				_mediaPlayer.Play();
				// Indica que se está reproduciendo un archivo
				ViewModel.IsPlaying = true;
			}
		}

		/// <summary>
		///		Reproduce el siguiente archivo
		/// </summary>
		private void PlayNext()
		{
			if (_filesToPlay.Count > 0)
			{
				_filesToPlay.RemoveAt(0);
				Play();
			}
		}

		/// <summary>
		///		Detiene la reproducción de un archivo
		/// </summary>
		private void StopPlay()
		{
			// Detiene la reproducción
			_mediaPlayer.Stop();
			// Indica que se ha detenido
			ViewModel.IsPlaying = false;
			// y oculta la barra de progreso y detiene el temporizador
			prgMedia.Visibility = System.Windows.Visibility.Collapsed;
			_tmrTimer.Stop();
		}

		/// <summary>
		///		ViewModel de lista de archivos multimedia
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel de lista de archivos multimedia
		/// </summary>
		public MediaListViewModel ViewModel { get; }

		private void prgMedia_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			double x = e.GetPosition(prgMedia).X;

				if (prgMedia.ActualWidth > 0 && x > 0)
				{
					double percent = Math.Min((x * 100) / prgMedia.ActualWidth, 100.0);
					TimeSpan newPosition = TimeSpan.FromSeconds(prgMedia.Maximum * percent / 100);

						if (newPosition.TotalSeconds < _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds)
							_mediaPlayer.Position = newPosition;
				}
		}
	}
}
