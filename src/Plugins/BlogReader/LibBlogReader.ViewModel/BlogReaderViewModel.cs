using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers.Common;
using Bau.Libraries.Plugins.ViewModels.Controllers.Settings;

namespace Bau.Libraries.LibBlogReader.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class BlogReaderViewModel : BaseControllerViewModel
	{
		// Enumerados públicos
		/// <summary>
		///		Indice de iconos
		/// </summary>
		public enum IconIndex
		{
			/// <summary>Icono para la nueva carpeta</summary>
			NewFolder,
			/// <summary>Icono para el nuevo blog</summary>
			NewBlog
		}

		public BlogReaderViewModel(string moduleName, IHostViewModelController hostController, 
								   IHostSystemController hostSystemController, IHostDialogsController hostDialogsController, Controllers.IViewsController viewsController,
								   System.Collections.Generic.Dictionary<IconIndex, string> dctImagesRoutes)
										: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{
			// Crea la instancia estática
			Instance = this;
			// Asigna el manager de blogs y el controlador de vistas
			BlogManager = new Application.BlogReaderManager();
			ViewsController = viewsController;
			// Asigna las propiedades de configuración
			BlogManager.Configuration.PathBlogs = PathBlogs;
			// Asigna las rutas a los iconos
			ImageRoutes = dctImagesRoutes;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
			Controllers.Process.BlogDownloadProcess downloader = new Controllers.Process.BlogDownloadProcess(MinutesBetweenDownload, DownloadEnabled);

				// Inicializa el evento de descarga
				downloader.DownloadProcess += (sender, evntArgs) =>
														{
															HostController.Messenger.SendLog(ModuleName, MessageLog.LogType.Information,
																							 ModuleName, evntArgs.Description, null);
															SendMesageChangeStatus(null);
														};
				downloader.BlogEntryDownload += (sender, evntArgs) =>
														{
															HostController.Messenger.SendParameters(ModuleName, "DOWNLOAD_BLOG_ENTRY",
																									"DOWNLOAD", GetParametersBlog(evntArgs.BlogEntry), null);
														};
				// Carga los datos
				BlogManager.Load();
				// Inicializa el proceso de descarga planificada de blogs
				HostController.Scheduler.AddProcess(downloader);
		}

		/// <summary>
		///		Crea los parámetros de un mensaje a partir de una entrada
		/// </summary>
		private ParametersModelCollection GetParametersBlog(Model.EntryModel entry)
		{
			var parameters = new ParametersModelCollection();

				// Añade los datos del blog
				parameters.Add(ModuleName, "BlogName", entry.Blog.Name);
				parameters.Add(ModuleName, "BlogDescription", entry.Blog.Description);
				parameters.Add(ModuleName, "BlogUrl", entry.Blog.URL);
				// Añade los datos de la entrada
				parameters.Add(ModuleName, "EntryName", entry.Name);
				parameters.Add(ModuleName, "EntryDescription", entry.Description);
				parameters.Add(ModuleName, "EntryContent", entry.Content);
				parameters.Add(ModuleName, "EntryUrl", entry.URL);
				// Devuelve la colección de parámetros
				return parameters;
		}

		/// <summary>
		///		Envía un mensaje de cambio de estado
		/// </summary>
		internal void SendMesageChangeStatus(object content = null)
		{
			HostController.Messenger.Send(new Controllers.Messengers.MessageChangeStatusNews(content));
		}

		/// <summary>
		///		Obtiene la ruta a un icono
		/// </summary>
		internal string GetIconRoute(IconIndex iconIndex)
		{
			if (ImageRoutes != null && ImageRoutes.TryGetValue(iconIndex, out string icon))
				return icon;
			else
				return null;
		}

		/// <summary>
		///		Manager de blogs
		/// </summary>
		public Application.BlogReaderManager BlogManager { get; private set; }

		/// <summary>
		///		Directorio a partir del que se encuentran los datos de los blogs
		/// </summary>
		public string PathBlogs
		{
			get
			{
				string pathBlogs = GetParameter(nameof(PathBlogs));

					// Si no se ha encontrado ningún parámetro crea el nombre del directorio
					if (pathBlogs.IsEmpty())
						pathBlogs = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "BlogsData");
					// Devuelve el directorio
					return pathBlogs;
			}
			set
			{
				SetParameter(nameof(PathBlogs), value);
				BlogManager.Configuration.PathBlogs = value;
			}
		}

		/// <summary>
		///		Minutos entre descargas
		/// </summary>
		public int MinutesBetweenDownload
		{
			get { return GetParameter(nameof(MinutesBetweenDownload)).GetInt(60); }
			set { SetParameter(nameof(MinutesBetweenDownload), value); }
		}

		/// <summary>
		///		Indica si las descargas están activas
		/// </summary>
		public bool DownloadEnabled
		{
			get { return GetParameter(nameof(DownloadEnabled)).GetBool(true); }
			set { SetParameter(nameof(DownloadEnabled), value); }
		}

		/// <summary>
		///		Configuración de registros por páginas
		/// </summary>
		public int RecordsPerPage
		{
			get { return GetParameter(nameof(RecordsPerPage)).GetInt(25); }
			set { SetParameter(nameof(RecordsPerPage), value); }
		}

		/// <summary>
		///		Indica si se deben ver las entradas leídas
		/// </summary>
		public bool SeeEntriesRead
		{
			get { return GetParameter(nameof(SeeEntriesRead)).GetBool(false); }
			set { SetParameter(nameof(SeeEntriesRead), value); }
		}

		/// <summary>
		///		Indica si se deben ver las entradas no leídas
		/// </summary>
		public bool SeeEntriesNotRead
		{
			get { return GetParameter(nameof(SeeEntriesNotRead)).GetBool(true); }
			set { SetParameter(nameof(SeeEntriesNotRead), value); }
		}

		/// <summary>
		///		Indica si se deben ver las entradas interesantes
		/// </summary>
		public bool SeeEntriesInteresting
		{
			get { return GetParameter(nameof(SeeEntriesInteresting)).GetBool(false); }
			set { SetParameter(nameof(SeeEntriesInteresting), value); }
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Rutas para las imágenes
		/// </summary>
		public System.Collections.Generic.Dictionary<IconIndex, string> ImageRoutes { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static BlogReaderViewModel Instance { get; private set; }
	}
}
