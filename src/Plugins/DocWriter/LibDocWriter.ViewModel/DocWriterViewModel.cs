using System;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.LibDocWriter.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class DocWriterViewModel : BaseControllerViewModel
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
			NewDocument,
			/// <summary>Icono para añadir referencia</summary>
			AddReference,
			/// <summary>Icono para compilar</summary>
			Compile
		}

		/// <summary>
		///		Inicializa los parámetros de ViewModel
		/// </summary>
		public DocWriterViewModel(string moduleName, IHostViewModelController hostController, 
								  IHostSystemController hostSystemController, IHostDialogsController hostDialogsController,
								  Controllers.IViewsController viewsController,
								  System.Collections.Generic.Dictionary<IconIndex, string> imagesRoutes)
									: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los controladores
			ViewsController = viewsController;
			// Configura los parámetros adicionales
			ImageRoutes = imagesRoutes;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
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
		internal string GetIconRoute(IconIndex intIconIndex)
		{
			if (ImageRoutes != null && ImageRoutes.TryGetValue(intIconIndex, out string icon))
				return icon;
			else
				return null;
		}

		/// <summary>
		///		Controlador de formularios de la aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Nombre del archivo de instrucciones del editor de código
		/// </summary>
		internal string FileNameEditorInstructions
		{
			get
			{
				return System.IO.Path.Combine(Instance.HostController.Configuration.PathBaseData,
											  "EditorInstructions.xml");
			}
		}

		/// <summary>
		///		Directorio de generación
		/// </summary>
		internal string PathGeneration
		{
			get { return GetPath("PathGeneration", "Generation"); }
			set { SetParameter("PathGeneration", value); }
		}

		/// <summary>
		///		Directorio de datos
		/// </summary>
		internal string PathData
		{
			get { return GetPath("PathData", "DocWriter"); }
			set { SetParameter("PathData", value); }
		}

		/// <summary>
		///		Indica si se debe minimizar la salida de la compilación
		/// </summary>
		public bool Minimize
		{
			get { return GetParameter("Minimize").GetBool(); }
			set { SetParameter("Minimize", value); }
		}

		/// <summary>
		///		Indica si se debe grabar antes de compilar
		/// </summary>
		public bool SaveBeforeCompile
		{
			get { return GetParameter("SaveBeforeCompile").GetBool(); }
			set { SetParameter("SaveBeforeCompile", value); }
		}

		/// <summary>
		///		Indica si se debe abrir un navegador externo al finalizar la compilación
		/// </summary>
		public bool OpenExternalWebBrowser
		{
			get { return GetParameter("OpenExternalWebBrowser").GetBool(); }
			set { SetParameter("OpenExternalWebBrowser", value); }
		}

		/// <summary>
		///		Ultima solución cargada
		/// </summary>
		public string LastFileSolution
		{
			get { return GetParameter("LastFileSolution"); }
			set { SetParameter("LastFileSolution", value); }
		}

		/// <summary>
		///		Rutas para las imágenes
		/// </summary>
		public System.Collections.Generic.Dictionary<IconIndex, string> ImageRoutes { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static DocWriterViewModel Instance { get; private set; }
	}
}
