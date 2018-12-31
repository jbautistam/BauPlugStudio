using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs
{
	/// <summary>
	///		ViewModel de <see cref="BlogModel"/>
	/// </summary>
	public class BlogViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private FolderModel _parent;
		private BlogModel _blog;
		private string _name, _description, _url;
		private bool _downloadPodcast, _enabled;
		private DateTime? _lastDownload, _lastEntry;

		public BlogViewModel(FolderModel parent, BlogModel blog)
		{
			if (blog == null)
			{
				_blog = new BlogModel();
				_blog.Folder = parent;
			}
			else
				_blog = blog;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Parent = _blog.Folder;
			Name = _blog.Name;
			Description = _blog.Description;
			URL = _blog.URL;
			DownloadPodcast = _blog.DownloadPodcast;
			Enabled = _blog.Enabled;
			LastDownload = _blog.DateLastDownload;
			LastEntry = _blog.DateLastEntry;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					BlogReaderViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del blog");
				else if (URL.IsEmpty())
					BlogReaderViewModel.Instance.ControllerWindow.ShowMessage("Introduzca la URL del blog");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Asigna la carpeta
				if (_parent != null && !_parent.Blogs.Exists(_blog.GlobalId))
					_parent.Blogs.Add(_blog);
				// Asigna los datos del formulario al objeto
				_blog.Folder = Parent;
				_blog.Name = Name;
				_blog.Description = Description;
				_blog.URL = URL;
				_blog.DownloadPodcast = DownloadPodcast;
				_blog.Enabled = Enabled;
				// Graba el objeto
				BlogReaderViewModel.Instance.BlogManager.Save();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del blog
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Carpeta padre
		/// </summary>
		public FolderModel Parent
		{
			get { return _parent; }
			set { CheckObject(ref _parent, value); }
		}

		/// <summary>
		///		Descripción del blog
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		URL del blog
		/// </summary>
		public string URL
		{
			get { return _url; }
			set { CheckProperty(ref _url, value); }
		}

		/// <summary>
		///		Indica si se debe descargar el archivo de podcast automáticamente
		/// </summary>
		public bool DownloadPodcast
		{
			get { return _downloadPodcast; }
			set { CheckProperty(ref _downloadPodcast, value); }
		}

		/// <summary>
		///		Indica si el blog está activo
		/// </summary>
		public bool Enabled
		{
			get { return _enabled; }
			set { CheckProperty(ref _enabled, value); }
		}

		/// <summary>
		///		Fecha de última descarga del blog
		/// </summary>
		public DateTime? LastDownload
		{
			get { return _lastDownload; }
			set { CheckProperty(ref _lastDownload, value); }
		}

		/// <summary>
		///		Fecha de la última entrada del blog
		/// </summary>
		public DateTime? LastEntry
		{
			get { return _lastEntry; }
			set { CheckProperty(ref _lastEntry, value); }
		}
	}
}
