using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.BauMvvm.ViewModels.Media;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel para <see cref="EntryModel"/>
	/// </summary>
	public class BlogEntryViewModel : BauMvvm.ViewModels.Forms.ControlItems.ControlItemViewModel
	{ 
		// Variables privadas
		private string _blogName, _name, _content, _author, _url, _urlEnclosure;
		private DateTime _publish;
		private EntryModel.StatusEntry _status;
		private bool _hasAttachment;

		public BlogEntryViewModel(string blogName, EntryModel entry) : base(blogName, entry)
		{ 
			// Inicializa la entrada
			Entry = entry;
			// Inicializa las propiedades
			InitProperties(blogName);
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties(string blogName)
		{
			BlogName = blogName;
			if (!Entry.Name.IsEmpty())
				Title = Entry.Name;
			else
				Title = Entry.URL;
			Author = Entry.Author;
			URL = Entry.URL;
			UrlEnclosure = Entry.UrlEnclosure;
			DatePublish = Entry.DatePublish;
			HasAttachment = !string.IsNullOrEmpty(UrlEnclosure);
			Status = Entry.Status;
		}

		/// <summary>
		///		Cambia los estados
		/// </summary>
		private void ChangeStatus()
		{ 
			// Inicializa las propiedades
			Foreground = MvvmColor.Black; // = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
			IsBold = false;
			IsItalic = false;
			// Cambia las propiedades
			switch (Status)
			{
				case EntryModel.StatusEntry.Interesting:
						Foreground = MvvmColor.Navy;
						IsItalic = true;
					break;
				case EntryModel.StatusEntry.NotRead:
						Foreground = MvvmColor.Red;
						IsBold = true;
					break;
			}
		}

		/// <summary>
		///		Entrada
		/// </summary>
		internal EntryModel Entry { get; }

		/// <summary>
		///		Nombre del blog al que pertenece la entrada
		/// </summary>
		public string BlogName
		{
			get { return _blogName; }
			set { CheckProperty(ref _blogName, value); }
		}

		/// <summary>
		///		Título de la entrada
		/// </summary>
		public string Title
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Autor de la entrada
		/// </summary>
		public string Author
		{
			get { return _author; }
			set { CheckProperty(ref _author, value); }
		}

		/// <summary>
		///		Contenido de la entrada
		/// </summary>
		public string Content
		{
			get
			{ 
				// Obtiene el contenido. Lo carga de la base de datos si aún no estaba allí
				if (_content.IsEmpty())
					_content = Entry.Content;
				// Devuelve el contenido
				return _content;
			}
			set { CheckProperty(ref _content, value); }
		}

		/// <summary>
		///		URL de la entrada
		/// </summary>
		public string URL
		{
			get { return _url; }
			set { CheckProperty(ref _url, value); }
		}

		/// <summary>
		///		Url del adjunto
		/// </summary>
		public string UrlEnclosure 
		{ 
			get { return _urlEnclosure; }
			set { CheckProperty(ref _urlEnclosure, value); }
		}

		/// <summary>
		///		Indica si la entrada tiene un adjunto
		/// </summary>
		public bool HasAttachment
		{
			get { return _hasAttachment; }
			set { CheckProperty(ref _hasAttachment, value); }
		}

		/// <summary>
		///		Fecha de publicación
		/// </summary>
		public DateTime DatePublish
		{
			get { return _publish; }
			set { CheckProperty(ref _publish, value); }
		}

		/// <summary>
		///		Estado de la entrada
		/// </summary>
		public EntryModel.StatusEntry Status
		{
			get { return _status; }
			set
			{
				if (CheckProperty(ref _status, value))
				{
					Entry.Status = value;
					Entry.Blog.IsDirty = true;
					ChangeStatus();
				}
			}
		}
	}
}
