using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.SourceEditor.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class SourceEditorViewModel : BaseControllerViewModel
	{   
		// Enumerados públicos
		/// <summary>
		///		Indice de iconos
		/// </summary>
		public enum IconIndex
		{
			/// <summary>Icono para abrir una solución</summary>
			OpenSolution,
			/// <summary>Icono para la nueva carpeta</summary>
			NewFolder,
			/// <summary>Icono para el nuevo proyecto</summary>
			NewProject,
			/// <summary>Icono para nuevo documento</summary>
			NewDocument
		}

		/// <summary>
		///		Inicializa los parámetros de ViewModel
		/// </summary>
		public SourceEditorViewModel(string moduleName, IHostViewModelController hostController, 
								     IHostSystemController hostSystemController,
									 IHostDialogsController hostDialogsController,
									 Controllers.IViewsController viewsController,
									 System.Collections.Generic.Dictionary<IconIndex, string> dctImagesRoutes)
								: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los controladores
			MessagesController = new Controllers.MessagesController();
			ViewsController = viewsController;
			// Configura el manager
			Manager = new Application.SourceEditorManager(PathData);
			// Configura los parámetros adicionales
			ImageRoutes = dctImagesRoutes;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
		}

		/// <summary>
		///		Inicializa la comunicación con otros plugins
		/// </summary>
		public override void InitComunicationBetweenPlugins()
		{   
			// Inicializa los proyectos
			MessagesController.RequestProjectDefinitions();
			// Llama al método base
			base.InitComunicationBetweenPlugins();
		}

		/// <summary>
		///		Obtiene el directorio asociado a un parámetro
		/// </summary>
		private string GetPath(string parameterName, string pathAdditional)
		{
			string path = "";

				// Obtiene la configuración
				if (HostController != null)
				{ 
					// Obtiene el directorio
					path = GetParameter(parameterName);
					// Si no hay nada, crea el directorio por defecto
					if (path.IsEmpty())
						path = System.IO.Path.Combine(HostController.Configuration.PathBaseData, pathAdditional);
				}
				// Devuelve el directorio
				return path;
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
		///		Manager de SourceEditor
		/// </summary>
		public Application.SourceEditorManager Manager { get; }

		/// <summary>
		///		Controlador de formularios de la aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Controlador de mensajes del proyecto
		/// </summary>
		public Controllers.MessagesController MessagesController { get; }

		/// <summary>
		///		Controlador de plugins
		/// </summary>
		internal Controllers.PluginsController PluginsController { get; } = new Controllers.PluginsController();

		/// <summary>
		///		Directorio de datos
		/// </summary>
		internal string PathData
		{
			get { return GetPath("PathData", "Projects"); }
			set
			{
				SetParameter("PathData", value);
				Manager.PathData = value;
			}
		}

		/// <summary>
		///		Ultima solución cargada
		/// </summary>
		public string LastFileSolution
		{
			get
			{
				string file = GetParameter("LastFileSolution");

					// Si no se encuentra el archivo de solución, crea uno nuevo en el directorio predeterminado
					if (file.IsEmpty() || !System.IO.File.Exists(file))
						file = System.IO.Path.Combine(PathData, "Solutions.sesln");
					// Devuelve el archivo de solución
					return file;
			}
			set { SetParameter("LastFileSolution", value); }
		}

		/// <summary>
		///		Rutas para las imágenes
		/// </summary>
		public System.Collections.Generic.Dictionary<IconIndex, string> ImageRoutes { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static SourceEditorViewModel Instance { get; private set; }
	}
}
