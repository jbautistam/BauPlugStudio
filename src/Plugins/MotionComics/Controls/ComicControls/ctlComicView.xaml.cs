using System;
using System.Windows;
using System.Windows.Controls;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMotionComic;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Actions;
using Bau.Libraries.LibMotionComic.Model.Components.Entities;

namespace Bau.Controls.ComicControls
{
	/// <summary>
	///		Control de usuario para la visualización de un cómic
	/// </summary>
	public partial class ctlComicView : UserControl
	{ 
		// Variables privadas
		private bool _isLoading = false;
		private System.Collections.Generic.List<string> _languages = new System.Collections.Generic.List<string>();

		public ctlComicView()
		{
			InitializeComponent();
			cboLanguages.SelectionChanged += (sender, evntArgs) => ChangeSelectedLanguage();
			udtComic.StartAnimation += (sender, evntArgs) => DisableControls();
			udtComic.EndAnimation += (sender, evntArgs) =>
										{ 
											// Activa los controles
											EnableControls();
											// Si se está ejecutando continuamente el cómic, se pasa a la siguiente acción
											if (IsPlayingComic)
												PlayComic();
										};
			EnableControls();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		public bool LoadComic(string fileName, out string error)
		{   // Inicializa los argumentos de salida
			error = "";
			// Guarda las propiedades
			FileName = fileName;
			// Activa los botones
			cmdNextAction.IsEnabled = true;
			cmdNextPage.IsEnabled = true;
			cmdPreviousPage.IsEnabled = true;
			// Carga el cómic
			try
			{ 
				// Indica que se está cargando el cómic
				_isLoading = true;
				// Crea el manager de cómics
				Manager = new ComicManager(fileName);
				// Carga el archivo de cómics
				Manager.Load();
				// Carga el combo de idiomas
				if (cboLanguages.Items.Count == 0 && Manager.Comic.Languages.Count != 0)
				{ 
					// Carga el combo de idiomas
					LoadComboLanguages(Manager.Comic.Languages);
					// Asigna el lenguage predeterminado
					DefaultLanguage = GetDefaultLanguage(Manager.Comic.Languages);
				}
				// Asigna las propiedades del cómic al formulario
				Title = Manager.Comic.Title;
				// Indica que se ha terminado de cargar el cómic
				_isLoading = false;
				// Muestra la página actual
				ShowPage();
			}
			catch (Exception exception)
			{
				error = $"Error en la carga del cómic.{Environment.NewLine}{exception.Message}";
				cmdNextAction.IsEnabled = false;
				cmdNextPage.IsEnabled = false;
				cmdPreviousPage.IsEnabled = false;
			}
			// Devuelve el valor que indica si la carga ha sido correcta
			return error.IsEmpty();
		}

		/// <summary>
		///		Carga el combo de idiomas
		/// </summary>
		private void LoadComboLanguages(ResourcesDictionary languages)
		{
			int indexDefault = 0;

				// Limpia el combo
				cboLanguages.Items.Clear();
				// Añade los idiomas
				foreach (System.Collections.Generic.KeyValuePair<string, AbstractComponentModel> key in languages)
					if (key.Value is LanguageModel)
					{
						LanguageModel language = key.Value as LanguageModel;

							if (language != null)
							{ 
								// Añade el idioma al combo
								cboLanguages.Items.Add(language.Name);
								// Añade el idioma a la colección
								_languages.Add(language.Key);
								// Asigna el índice predeterminado
								if (language.IsDefault)
									indexDefault = _languages.Count - 1;
							}
					}
				// Selecciona el idioma predeterminado
				if (cboLanguages.Items.Count > 0 && cboLanguages.Items.Count > indexDefault)
					cboLanguages.SelectedIndex = indexDefault;
		}

		/// <summary>
		///		Obtiene el idioma predeterminado
		/// </summary>
		private string GetDefaultLanguage(ResourcesDictionary languages)
		{ 
			// Obtiene el idioma predeterminado
			foreach (System.Collections.Generic.KeyValuePair<string, AbstractComponentModel> key in languages)
				if (key.Value is LanguageModel)
				{
					LanguageModel language = key.Value as LanguageModel;

						if (language.IsDefault)
							return language.Key;
				}
			// Devuelve la clave del idioma
			return "sp";
		}

		/// <summary>
		///		Cambia el idioma seleccionado
		/// </summary>
		private void ChangeSelectedLanguage()
		{
			if (_languages != null && _languages.Count > 0 &&
				cboLanguages.SelectedIndex >= 0 && cboLanguages.SelectedIndex < _languages.Count)
			{ 
				// Asigna el idioma predeterminado
				SelectedLanguage = _languages[cboLanguages.SelectedIndex];
				// Muestra la página
				ShowPage();
			}
		}

		/// <summary>
		///		Siguiente página
		/// </summary>
		private void NextPage()
		{
			Manager.MoveNexPage();
			ShowPage();
		}

		/// <summary>
		///		Página anterior
		/// </summary>
		private void PreviousPage()
		{
			Manager.MovePagePrevious();
			ShowPage();
		}

		/// <summary>
		///		Pasa a la siguiente acción
		/// </summary>
		private void NextAction()
		{
			TimeLineModel timeLine = Manager.MoveNextAction();

				// Ejecuta la acción
				if (timeLine != null)
					udtComic.Execute(timeLine);
				else
					NextPage();
		}

		/// <summary>
		///		Muestra la página
		/// </summary>
		private void ShowPage()
		{
			if (!_isLoading)
			{   
				// Carga la página
				udtComic.LanguageSelected = SelectedLanguage;
				udtComic.LanguageDefault = DefaultLanguage;
				udtComic.LoadPage(Manager.ActualPage);
				// Muestra la página
				lblPage.Content = $"{Manager.IndexActualPage + 1} / {Manager.Comic.Pages.Count}";
				// Habilita los controles
				EnableControls();
			}
		}

		/// <summary>
		///		Ejecuta el cómic sin paradas
		/// </summary>
		private void PlayComic()
		{
			if (!_isLoading)
			{   
				// Indica que se está ejecutando el cómic
				IsPlayingComic = true;
				// Ejecuta la siguiente acción o finaliza la visualización del cómic
				if (CanGoNextAction() || CanGoNextPage())
				{
					int actualPage = Manager.IndexActualPage;

						// Ejecuta la siguiente acción
						NextAction();
						// Si la siguiente acción ha saltado de página, la ejecuta
						if (actualPage != Manager.IndexActualPage)
						{ 
							// Espera un par de segundos
							System.Threading.Thread.Sleep(2000);
							// Pasa a la siguiente acción
							NextAction();
						}
				}
				else
					IsPlayingComic = false;
			}
		}

		/// <summary>
		///		Habilita / inhabilita los controles
		/// </summary>
		private void EnableControls()
		{
			cmdPreviousPage.IsEnabled = Manager != null && CanGoPreviousPage() && !IsPlayingComic;
			cmdNextAction.IsEnabled = Manager != null && CanGoNextAction() && !IsPlayingComic;
			cmdNextPage.IsEnabled = Manager != null && CanGoNextPage() && !IsPlayingComic;
			cmdRefresh.IsEnabled = true;
		}

		/// <summary>
		///		Inhabilita los controles (temporalmente, al iniciarse una animcación)
		/// </summary>
		private void DisableControls()
		{
			cmdPreviousPage.IsEnabled = false;
			cmdNextAction.IsEnabled = false;
			cmdNextPage.IsEnabled = false;
			cmdRefresh.IsEnabled = false;
		}

		/// <summary>
		///		Indica si se puede pasar a la siguiente acción
		/// </summary>
		private bool CanGoNextAction()
		{
			return CanGoNextPage() || Manager.IndexActualAction < Manager.ActualPage.TimeLines.Count;
		}

		/// <summary>
		///		Indica si se puede pasar a la página anterior
		/// </summary>
		private bool CanGoPreviousPage()
		{
			return Manager.IndexActualPage > 0;
		}

		/// <summary>
		///		Indica si se puede pasar a la siguiente página
		/// </summary>
		private bool CanGoNextPage()
		{
			return Manager.IndexActualPage < Manager.Comic.Pages.Count - 1;
		}

		/// <summary>
		///		Nombre del archivo que se está cargando
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		///		Cómic que se está visualizando
		/// </summary>
		private ComicManager Manager { get; set; }

		/// <summary>
		///		Título del cómic
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Ancho del cómic
		/// </summary>
		public double ComicWidth
		{
			get { return Manager.Comic.Width; }
		}

		/// <summary>
		///		Alto del cómic
		/// </summary>
		public double ComicHeight
		{
			get { return Manager.Comic.Height; }
		}

		/// <summary>
		///		Muestra los adorners
		/// </summary>
		public bool ShowAdorners
		{
			get { return udtComic.ShowAdorners; }
			set { udtComic.ShowAdorners = value; }
		}

		/// <summary>
		///		Indica que se está ejecutando el cómic
		/// </summary>
		public bool IsPlayingComic { get; private set; }

		/// <summary>
		///		Idioma predeterminado
		/// </summary>
		private string DefaultLanguage { get; set; }

		/// <summary>
		///		Idioma seleccionado
		/// </summary>
		private string SelectedLanguage { get; set; }

		private void cmdPreviousPage_Click(object sender, RoutedEventArgs e)
		{
			PreviousPage();
		}

		private void cmdNextPage_Click(object sender, RoutedEventArgs e)
		{
			NextPage();
		}

		private void cmdNextAction_Click(object sender, RoutedEventArgs e)
		{
			NextAction();
		}

		private void cmdRefresh_Click(object sender, RoutedEventArgs e)
		{
			LoadComic(FileName, out string error);
		}

		private void cmdPlay_Click(object sender, RoutedEventArgs e)
		{
			if (IsPlayingComic)
				IsPlayingComic = false;
			else
				PlayComic();
		}
	}
}
