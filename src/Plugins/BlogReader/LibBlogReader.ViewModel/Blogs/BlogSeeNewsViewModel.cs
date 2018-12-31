using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Forms;
using Bau.Libraries.LibBlogReader.Application.Bussiness.Blogs;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel para ver las noticias de <see cref="BlogModel"/>
	/// </summary>
	public class BlogSeeNewsViewModel : BaseFormViewModel
	{   
		// Enumerados privadas
		/// <summary>
		///		Acciones que se pueden realizar con una entrada
		/// </summary>
		private enum EntryAction
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Marcar la entrada como leida / no leida</summary>
			MarkRead,
			/// <summary>Marcar la entrada como interesante</summary>
			MarkInteresting,
			/// <summary>Borrar la entrada</summary>
			Delete,
			/// <summary>Reproducir el adjunto</summary>
			Play
		}
		// Variables privadas
		private BlogEntriesCollectionViewModel _entries, _entriesForView;
		private BlogEntryViewModel _selectedEntry;
		private bool _mustSeeInteresting, _mustSeeNotRead, _mustSeeRead, _isDirty;
		private int _entriesNumber, _recordsPerPage, _actualPage, _pageIndex;
		private string _htmlNews;

		public BlogSeeNewsViewModel(BlogsModelCollection blogs)
		{ 
			// Inicializa la lista de blogs
			Blogs = blogs;
			// Inicializa las propiedades
			PropertyChanged += (sender, args) =>
									{
										if (args != null && args.PropertyName.EqualsIgnoreCase(nameof(SelectedEntry)))
											DeleteCommand.OnCanExecuteChanged();
										IsUpdated = false;
									};
			// Inicializa el ViewModel
			InitViewModel();
		}

		/// <summary>
		///		Inicializa el ViewModel: propiedades, menús y comandos
		/// </summary>
		private void InitViewModel()
		{ 
			// Inicializa las propiedades
			_mustSeeRead = BlogReaderViewModel.Instance.SeeEntriesRead;
			_mustSeeNotRead = BlogReaderViewModel.Instance.SeeEntriesNotRead;
			_mustSeeInteresting = BlogReaderViewModel.Instance.SeeEntriesInteresting;
			RecordsPerPage = BlogReaderViewModel.Instance.RecordsPerPage;
			// Inicializa los comandos de página
			FirstPageCommand = CreateCommandForPage(nameof(FirstPageCommand));
			NextPageCommand = CreateCommandForPage(nameof(NextPageCommand));
			PreviousPageCommand = CreateCommandForPage(nameof(PreviousPageCommand));
			LastPageCommand = CreateCommandForPage(nameof(LastPageCommand));
			// Inicializa los comandos del menú
			MarkAsReadCommand = new BaseCommand(parameter => ExecuteAction(nameof(MarkAsReadCommand), parameter),
												parameter => CanExecuteAction(nameof(MarkAsReadCommand), parameter));
			MarkAsNotReadCommand = new BaseCommand(parameter => ExecuteAction(nameof(MarkAsNotReadCommand), parameter),
												   parameter => CanExecuteAction(nameof(MarkAsNotReadCommand), parameter));
			MarkAsInterestingCommand = new BaseCommand(parameter => ExecuteAction(nameof(MarkAsInterestingCommand), parameter),
													   parameter => CanExecuteAction(nameof(MarkAsInterestingCommand), parameter));
			ExportEntriesCommand = new BaseCommand(parameter => ExecuteAction(nameof(ExportEntriesCommand), parameter),
												   parameter => CanExecuteAction(nameof(ExportEntriesCommand), parameter));
			PlayCommand = new BaseCommand(parameter => ExecuteAction(nameof(PlayCommand), parameter),
										  parameter => CanExecuteAction(nameof(PlayCommand), parameter))
									.AddListener(this, nameof(SelectedEntry));
			// Carga las noticias
			TreatSelectedEntriesChanged();
		}

		/// <summary>
		///		Crea un comando asociado a la página HTML
		/// </summary>
		private BaseCommand CreateCommandForPage(string action)
		{
			return new BaseCommand(parameter => ExecuteAction(action, parameter),
								   parameter => CanExecuteAction(action, parameter))
							.AddListener(this, nameof(Pages)).AddListener(this, nameof(ActualPage)).AddListener(this, nameof(RecordsPerPage));
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(MarkAsReadCommand):
						SetStatusEntriesSelected(EntryModel.StatusEntry.Read);
					break;
				case nameof(MarkAsNotReadCommand):
						SetStatusEntriesSelected(EntryModel.StatusEntry.NotRead);
					break;
				case nameof(MarkAsInterestingCommand):
						SetStatusEntriesSelected(EntryModel.StatusEntry.Interesting);
					break;
				case nameof(FirstPageCommand):
						ActualPage = 1;
						TreatSelectedEntriesChanged();
					break;
				case nameof(PreviousPageCommand):
						ActualPage--;
						TreatSelectedEntriesChanged();
					break;
				case nameof(LastPageCommand):
						ActualPage = Pages;
						TreatSelectedEntriesChanged();
					break;
				case nameof(NextPageCommand):
						ActualPage++;
						TreatSelectedEntriesChanged();
					break;
				case nameof(RefreshCommand):
						Refresh();
					break;
				case nameof(DeleteCommand):
						DeleteSelectedEntries(parameter);
					break;
				case nameof(ExportEntriesCommand):
						ExportEntries();
					break;
				case nameof(PlayCommand):
						PlayEntry();
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
				case nameof(FirstPageCommand):
				case nameof(PreviousPageCommand):
					return ActualPage > 1;
				case nameof(LastPageCommand):
				case nameof(NextPageCommand):
					return ActualPage < Pages;
				case nameof(DeleteCommand):
					return SelectedEntry != null || (SelectedEntry == null && Blogs.GetNumberNotRead() == 0 && Entries.Count > 0);
				case nameof(MarkAsReadCommand):
				case nameof(MarkAsNotReadCommand):
				case nameof(MarkAsInterestingCommand):
					return SelectedEntry != null;
				case nameof(ExportEntriesCommand):
				case nameof(PlayCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Actualiza la vista
		/// </summary>
		private void Refresh()
		{
			SelectedEntry = null;
			MarkAsDirty();
		}

		/// <summary>
		///		Obtiene las entradas de los blogs
		/// </summary>
		private BlogEntriesCollectionViewModel GetEntries()
		{
			BlogEntriesCollectionViewModel entriesViewModel = new BlogEntriesCollectionViewModel();

				// Ordena los blogs
				Blogs.SortByName();
				// Asigna las entradas
				foreach (BlogModel blog in Blogs)
				{ 
					// Ordena las entradas por fecha
					blog.Entries.SortByDate(false);
					// Añade las entradas a la vista (no hace un foreach porque puede que se cambie la colección en las descargas)
					for (int index = 0; index < blog.Entries.Count; index++)
						if (blog.Entries[index].Status != EntryModel.StatusEntry.Deleted)
							entriesViewModel.Add(new BlogEntryViewModel(blog.Name, blog.Entries[index]));
				}
				// Devuelve las entradas
				return entriesViewModel;
		}

		/// <summary>
		///		Obtiene las entradas seleccionadas
		/// </summary>
		private BlogEntriesCollectionViewModel GetEntriesSelected()
		{
			if (Entries.CountSelected > 0)
				return Entries.GetSelected();
			else
				return Entries.GetByParameters(SeeNotRead, SeeRead, SeeInteresting);
		}

		/// <summary>
		///		Borra las entradas seleccionadas
		/// </summary>
		private void DeleteSelectedEntries(object parameter)
		{
			BlogEntriesCollectionViewModel entriesForDelete = Entries.GetSelected();

				// Si no se ha seleccionado ninguna, obtiene las no leídas para borrarlas
				if (entriesForDelete.Count == 0)
					entriesForDelete = Entries.GetRead();
				// Si hay algo que se pueda borrar ...
				if (entriesForDelete.Count > 0 &&
						BlogReaderViewModel.Instance.ControllerWindow.ShowQuestion("¿Realmente desea eliminar las entradas seleccionadas?"))
					DeleteEntries(entriesForDelete);
		}

		/// <summary>
		///		Borra una serie de entradas
		/// </summary>
		private void DeleteEntries(BlogEntriesCollectionViewModel entriesForDelete)
		{
			if (entriesForDelete.Count > 0)
			{
				BlogsModelCollection blogsUpdated = new BlogsModelCollection();
				bool deleted = false;

					// Borra las entradas
					foreach (BlogEntryViewModel entryViewModel in entriesForDelete)
					{ 
						// Añade el blog de la entrada
						if (!blogsUpdated.Exists(entryViewModel.Entry.Blog.GlobalId))
							blogsUpdated.Add(entryViewModel.Entry.Blog);
						// Marca como borrado y añade el ID para borrarlo en la base de datos
						entryViewModel.Entry.Status = EntryModel.StatusEntry.Deleted;
						// Quita la entrada de la colección
						Entries.Remove(entryViewModel);
						// Elimina los archivos asociados
						if (!string.IsNullOrEmpty(entryViewModel.Entry.DownloadFileName) &&
								System.IO.File.Exists(entryViewModel.Entry.DownloadFileName))
							LibCommonHelper.Files.HelperFiles.KillFile(System.IO.Path.Combine(BlogReaderViewModel.Instance.PathBlogs, entryViewModel.Entry.DownloadFileName));
						// Indica que algo se ha borrado
						deleted = true;
					}
					// Si realmente hay algo que borrar
					if (deleted)
						TreatUpdateEntries(blogsUpdated);
			}
		}

		/// <summary>
		///		Obtiene el HTML de las entradas seleccionadas para mostrarlas en el explorador
		/// </summary>
		private string GetHtmlNews()
		{
			BlogEntriesCollectionViewModel entries = EntriesForView;
			System.Text.StringBuilder sbHtml = new System.Text.StringBuilder();
			string lastBlog = null;
			int startIndex, endIndex;

				// Actualiza el número de entradas y de páginas
				EntriesNumber = entries.Count;
				if (RecordsPerPage == 0)
					RecordsPerPage = 25;
				Pages = EntriesNumber / RecordsPerPage;
				if (EntriesNumber % RecordsPerPage != 0)
					Pages++;
				// Normaliza la página actual
				if (ActualPage > Pages)
					ActualPage = Pages;
				else if (ActualPage < 1)
					ActualPage = 1;
				// Asigna el índice de inicio y fin
				startIndex = (ActualPage - 1) * RecordsPerPage;
				endIndex = startIndex + RecordsPerPage;
				// Cabecera HTML
				sbHtml.AppendLine("<html><head>");
				sbHtml.AppendLine("<meta content='text/html; charset=utf-8' http-equiv='Content-Type'>");
				sbHtml.AppendLine("<meta name = 'Content-Type' content = 'Content-Type: text/html; charset=utf-8'>");
				sbHtml.AppendLine("<meta name='lang' content='es'>");
				sbHtml.AppendLine("</head><body>");
				// Obtiene el HTML
				for (int index = startIndex; index < endIndex; index++)
					if (index >= 0 && index < entries.Count)
					{
						BlogEntryViewModel entry = entries[index];

							// Título del blog
							if (!lastBlog.EqualsIgnoreCase(entry.BlogName))
							{ 
								// Cabecera de blog
								sbHtml.AppendLine($"<h1 style='clear:both;'>{entry.BlogName}</h1>");
								// Guarda la última cabecera de blog
								lastBlog = entry.BlogName;
							}
							// Cabecera de entrada
							sbHtml.AppendLine($"<h2 style='clear:both;'><a href='{entry.URL}'>{entry.Title}</a></h2>");
							// Comandos sobre la entrada
							sbHtml.AppendLine("<p>");
							if (entry.Entry.Status == EntryModel.StatusEntry.NotRead)
								sbHtml.AppendLine(GetJavaScriptExternalFuction(entry, "Marcar leído", EntryAction.MarkRead));
							if (entry.Entry.Status == EntryModel.StatusEntry.Read)
								sbHtml.AppendLine(GetJavaScriptExternalFuction(entry, "Marcar no leído", EntryAction.MarkRead));
							if (entry.Entry.Status == EntryModel.StatusEntry.Read || entry.Entry.Status == EntryModel.StatusEntry.NotRead)
								sbHtml.AppendLine(GetJavaScriptExternalFuction(entry, "Marcar interesante", EntryAction.MarkInteresting));
							sbHtml.AppendLine(GetJavaScriptExternalFuction(entry, "Borrar", EntryAction.Delete));
							sbHtml.AppendLine("</p>");
							// Cuerpo
							sbHtml.AppendLine(RemoveIframe(entry.Content) + "<br>");
							// Adjunto
							if (!string.IsNullOrEmpty(entry.UrlEnclosure))
								sbHtml.AppendLine($"<p><strong>Adjunto: </strong>{GetJavaScriptExternalFuction(entry, System.IO.Path.GetFileName(entry.UrlEnclosure), EntryAction.Play)}</p><br>");
							// Autor - Fecha
							sbHtml.AppendLine($@"<p style='text-align:right;'>
													<strong>Autor: </strong>{entry.Author} - 
													<strong>Fecha: </strong>{entry.DatePublish:dd-MM-yyyy HH:mm}
												</p>");
							// Separador
							sbHtml.AppendLine("<hr>");
					}
				// Cierre HTML
				sbHtml.AppendLine("</body></html>");
				// Devuelve el HTML
				return sbHtml.ToString();
		}

		/// <summary>
		///		Elimina el contenido de las etiquetas iFrame que pueden bloquear el explorador
		/// </summary>
		private string RemoveIframe(string text)
		{
			string result = text;

				// Quita la etiqueta "iframe"
				while (!result.IsEmpty() && result.IndexOf("<iframe") >= 0)
					result = System.Text.RegularExpressions.Regex.Replace(result, "<iframe(.|\n)*?</iframe>", string.Empty);
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Obtiene un vínculo a una llamada de función externa en JavaScript
		/// </summary>
		private string GetJavaScriptExternalFuction(BlogEntryViewModel entry, string title, EntryAction action)
		{
			return $"<a href='' onclick=\"window.external.ExternalFunction('{action.ToString()}|{entry.Entry.URL}');return false;\">{title}</a>";
		}

		/// <summary>
		///		Ejecuta las funciones llamadas desde el explorador
		/// </summary>
		public void ExecuteFromExplorer(string arguments)
		{
			if (!arguments.IsEmpty())
			{
				string[] parameters = arguments.Split('|');

					if (parameters.Length == 2)
					{
						BlogEntryViewModel entry = Entries.Search(parameters[1]);

							if (entry != null)
							{
								BlogEntriesCollectionViewModel entries = new BlogEntriesCollectionViewModel { entry };
								EntryAction action = parameters[0].GetEnum(EntryAction.Unknown);

									switch (action)
									{
										case EntryAction.MarkRead:
												if (entry.Entry.Status == EntryModel.StatusEntry.Read)
													SetStatusEntries(entries, EntryModel.StatusEntry.NotRead);
												else
													SetStatusEntries(entries, EntryModel.StatusEntry.Read);
											break;
										case EntryAction.MarkInteresting:
												if (entry.Entry.Status == EntryModel.StatusEntry.Interesting)
													SetStatusEntries(entries, EntryModel.StatusEntry.Read);
												else
													SetStatusEntries(entries, EntryModel.StatusEntry.Interesting);
											break;
										case EntryAction.Delete:
												DeleteEntries(entries);
											break;
										case EntryAction.Play:
												PlayEntry();
											break;
									}
							}
					}
			}
		}

		/// <summary>
		///		Marca los elementos seleccionados como leídos
		/// </summary>
		public void MarkRead()
		{
			SetStatusEntriesSelected(EntryModel.StatusEntry.Read);
		}

		/// <summary>
		///		Modifica el estado de las entradas seleccionadas
		/// </summary>
		private void SetStatusEntriesSelected(EntryModel.StatusEntry idNewStatus)
		{
			SetStatusEntries(GetEntriesSelected(), idNewStatus);
		}

		/// <summary>
		///		Modifica el estado de las entradas seleccionadas
		/// </summary>
		private void SetStatusEntries(BlogEntriesCollectionViewModel entries, EntryModel.StatusEntry idNewStatus)
		{
			BlogsModelCollection blogsUpdated = new BlogsModelCollection();
			bool updated = false;

				// Marca los elementos seleccionados como leídos
				foreach (BlogEntryViewModel entry in entries)
					if (CanChangeStatus(entry, idNewStatus))
					{ 
						// Cambia el estado
						entry.Status = idNewStatus;
						// Añade el blog
						if (!blogsUpdated.Exists(entry.Entry.Blog.GlobalId))
							blogsUpdated.Add(entry.Entry.Blog);
						// Indica que se ha modificado
						updated = true;
					}
				// Si realmente se ha marcado alguno ... 
				if (updated)
					TreatUpdateEntries(blogsUpdated);
		}

		/// <summary>
		///		Trata la modificación de las entradas las entradas
		/// </summary>
		private void TreatUpdateEntries(BlogsModelCollection blogsUpdated)
		{   
			// Graba las entradas de los blogs modificados
			foreach (BlogModel blog in blogsUpdated)
				new EntryBussiness().Save(blog, BlogReaderViewModel.Instance.BlogManager.Configuration.PathBlogs);
			// Modifica el número de elementos no leídos
			new BlogBussiness().UpdateNumberNotRead(blogsUpdated);
			BlogReaderViewModel.Instance.BlogManager.Save();
			// Cambia el estado del comando borrar
			DeleteCommand.OnCanExecuteChanged();
			// Actualiza el árbol
			BlogReaderViewModel.Instance.SendMesageChangeStatus();
		}

		/// <summary>
		///		Comprueba si se puede cambiar el estado de una entrada
		/// </summary>
		private bool CanChangeStatus(BlogEntryViewModel entry, EntryModel.StatusEntry idNewStatus)
		{
			bool canChange = false;

				// Comprueba si se puede cambiar el estado de una entrada
				if (entry.Status != idNewStatus)
				{ 
					// Indica que por ahora se puede cambiar
					canChange = true;
					// Comprueba el nuevo valor
					if (idNewStatus == EntryModel.StatusEntry.Read && entry.Status == EntryModel.StatusEntry.Interesting)
						canChange = false;
				}
				// Devuelve el valor que indica si se puede cambiar el estado
				return canChange;
		}

		/// <summary>
		///		Marca el modelo como sucio para que se recargue el explorador
		/// </summary>
		public void MarkAsDirty()
		{
			_isDirty = true;
			TreatSelectedEntriesChanged();
		}

		/// <summary>
		///		Trata la modificación en los parámetros de lectura de la lista
		/// </summary>
		private void TreatSelectedEntriesChanged()
		{ 
			// Asigna los datos a la configuración
			BlogReaderViewModel.Instance.SeeEntriesRead = SeeRead;
			BlogReaderViewModel.Instance.SeeEntriesNotRead = SeeNotRead;
			BlogReaderViewModel.Instance.SeeEntriesInteresting = SeeInteresting;
			// Graba la configuración
			BlogReaderViewModel.Instance.HostController.Configuration.Save();
			// Lanza el evento de modificación de entradas seleccionadas
			HtmlNews = GetHtmlNews();
		}

		/// <summary>
		///		Exporta las entradas
		/// </summary>
		private void ExportEntries()
		{
			string fileName = BlogReaderViewModel.Instance.DialogsController.OpenDialogSave(null, "Archivos XML (*.xml)|*.xml|Todos los archivos (*.*)|*.*",
																						   "ExportEntries.xml", ".xml");

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					BlogEntriesCollectionViewModel entriesViewModel = EntriesForView;
					BlogModel blog = new BlogModel();

						// Crea los datos del blog
						blog.Name = "Export";
						// Añade las entradas
						foreach (BlogEntryViewModel entryViewModel in entriesViewModel)
							blog.Entries.Add(new EntryModel
													{
														Name = entryViewModel.Title,
														Content = entryViewModel.Content,
														URL = entryViewModel.URL
													}
											);
						// Graba los datos del blog
						BlogReaderViewModel.Instance.BlogManager.SaveBlog(blog, fileName);
				}
		}

		/// <summary>
		///		Obtiene el nombre de adjunto de la entrada seleccionada
		/// </summary>
		private KeyValuePair<string, string> GetAttachment(EntryModel entry)
		{
			KeyValuePair<string, string> attachment = new KeyValuePair<string, string>();

				// Obtiene el nombre de archivo o la URL del adjunto
				if (entry != null && !string.IsNullOrWhiteSpace(entry.UrlEnclosure))
				{
					// Obtiene el nombre de archivo
					attachment = new KeyValuePair<string, string>(entry.Name, System.IO.Path.Combine(BlogReaderViewModel.Instance.PathBlogs, entry.DownloadFileName));
					// Si no existe el archivo, obtiene la Url
					if (!System.IO.File.Exists(attachment.Value))
						attachment = new KeyValuePair<string, string>(entry.Name, entry.UrlEnclosure);
				}
				// Devuelve el nombre del adjunto
				return attachment;
		}

		/// <summary>
		///		Reproduce el adjunto de una entrada
		/// </summary>
		private void PlayEntry()
		{
			BlogEntriesCollectionViewModel entries = GetEntriesSelected();
			var attachments = new Dictionary<string, List<KeyValuePair<string, string>>>();

				// Obtiene los nombres de los archivos adjuntos
				foreach (BlogEntryViewModel entry in entries)
				{
					KeyValuePair<string, string> attachment = GetAttachment(entry?.Entry);

						if (!string.IsNullOrEmpty(attachment.Value))
						{
							if (attachments.TryGetValue(entry.BlogName, out var dictionaryEntry))
								dictionaryEntry.Add(attachment);
							else
								attachments.Add(entry.BlogName, new List<KeyValuePair<string, string>> { attachment });
						}
				}
				// Ejecuta los archivos
				if (attachments.Count > 0)
				{
					foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> attachment in attachments)
						BlogReaderViewModel.Instance.HostController.Messenger.SendFilesToMediaPlayer(BlogReaderViewModel.Instance.ModuleName, attachment.Key, attachment.Value);
				}					
				else
					BlogReaderViewModel.Instance.ControllerWindow.ShowMessage("No se ha seleccionado ningún elemento con adjuntos");
		}

		/// <summary>
		///		Blogs
		/// </summary>
		private BlogsModelCollection Blogs { get; }

		/// <summary>
		///		Entradas
		/// </summary>
		public BlogEntriesCollectionViewModel Entries
		{
			get
			{ 
				// Si no está cargado, obtiene las entradas
				if (_entries == null)
					_entries = GetEntries();
				// Devuelve las entradas
				return _entries;
			}
			set { CheckObject(ref _entries, value); }
		}

		/// <summary>
		///		Entradas que se ven en el explorador
		/// </summary>
		private BlogEntriesCollectionViewModel EntriesForView
		{
			get
			{ 
				// Obtiene las entradas que se deben mostrar en el explorador
				if (_entriesForView == null || _isDirty)
				{ 
					// Recarga la lista
					_entriesForView = GetEntriesSelected();
					// Indica que se han leído los últimos datos
					_isDirty = false;
				}
				// Devuelve las entradas de la vista
				return _entriesForView;
			}
		}

		/// <summary>
		///		Entrada seleccionada
		/// </summary>
		public BlogEntryViewModel SelectedEntry
		{
			get { return _selectedEntry; }
			set { CheckObject(ref _selectedEntry, value); }
		}

		/// <summary>
		///		Indica si se deben ver o no las entradas interesantes
		/// </summary>
		public bool SeeInteresting
		{
			get { return _mustSeeInteresting; }
			set
			{
				if (CheckProperty(ref _mustSeeInteresting, value))
					MarkAsDirty();
			}
		}

		/// <summary>
		///		Indica si se deben ver o no las entradas no leídas
		/// </summary>
		public bool SeeNotRead
		{
			get { return _mustSeeNotRead; }
			set
			{
				if (CheckProperty(ref _mustSeeNotRead, value))
					MarkAsDirty();
			}
		}

		/// <summary>
		///		Indica si se deben ver o no las entradas leídas
		/// </summary>
		public bool SeeRead
		{
			get { return _mustSeeRead; }
			set
			{
				if (CheckProperty(ref _mustSeeRead, value))
					MarkAsDirty();
			}
		}

		/// <summary>
		///		Número de entradas seleccionadas
		/// </summary>
		public int EntriesNumber
		{
			get { return _entriesNumber; }
			set { CheckProperty(ref _entriesNumber, value); }
		}

		/// <summary>
		///		Registros por página
		/// </summary>
		public int RecordsPerPage
		{
			get { return _recordsPerPage; }
			set { CheckProperty(ref _recordsPerPage, value); }
		}

		/// <summary>
		///		Página actual
		/// </summary>
		public int ActualPage
		{
			get { return _actualPage; }
			set { CheckProperty(ref _actualPage, value); }
		}

		/// <summary>
		///		Número de páginas
		/// </summary>
		public int Pages
		{
			get { return _pageIndex; }
			set { CheckProperty(ref _pageIndex, value); }
		}

		/// <summary>
		///		Texto Html de las noticias
		/// </summary>
		public string HtmlNews
		{
			get { return _htmlNews; }
			set { CheckProperty(ref _htmlNews, value); }
		}

		/// <summary>
		///		Comando para mostrar la primera página
		/// </summary>
		public BaseCommand FirstPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la siguiente página
		/// </summary>
		public BaseCommand NextPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la página anterior
		/// </summary>
		public BaseCommand PreviousPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la última página
		/// </summary>
		public BaseCommand LastPageCommand { get; private set; }

		/// <summary>
		///		Comando para marcar un elemento como leído
		/// </summary>
		public BaseCommand MarkAsReadCommand { get; private set; }

		/// <summary>
		///		Comando para marcar un elemento como no leído
		/// </summary>
		public BaseCommand MarkAsNotReadCommand { get; private set; }

		/// <summary>
		///		Comando para marcar un elemento como interesante
		/// </summary>
		public BaseCommand MarkAsInterestingCommand { get; private set; }

		/// <summary>
		///		Comando para exportar entradas
		/// </summary>
		public BaseCommand ExportEntriesCommand { get; private set; }

		/// <summary>
		///		Comando para reproducir archivos
		/// </summary>
		public BaseCommand PlayCommand { get; private set; }
	}
}