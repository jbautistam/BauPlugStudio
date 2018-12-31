using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Applications.BauPlugStudio.Controllers.Plugins
{
	/// <summary>
	///		Directorios de plugins
	/// </summary>
	public class PluginPathModelCollection : List<PluginPathModel>
	{   
		// Constantes privadas
		private const string SeparatorPaths = "#¬¬#";
		private const string SeparatorData = "@~|@";

		/// <summary>
		///		Carga los directorios de plugins
		/// </summary>
		public void Load()
		{
			string path = Properties.Settings.Default.PathPlugins;

				// Limpia la colección de directorios
				Clear();
				// Primero, añade los directorios que se encuentran bajo el directorio "Plugins" de la aplicación
				AddPathsDefault(System.IO.Path.Combine(Globals.HostController.HostViewModelController.Configuration.PathBaseApplication, "Plugins"));
				// Después añade los directorios configurados
				if (!path.IsEmpty())
				{
					List<string> paths = path.SplitByString(SeparatorPaths);

						// Separa los directorios
						if (paths != null && paths.Count > 0)
							foreach (string pathPlugin in paths)
							{
								List<string> pathParts = pathPlugin.TrimIgnoreNull().SplitByString(SeparatorData);

									if (pathParts != null && pathParts.Count == 2)
										Add(pathParts[0], pathParts[1].GetInt(0) == 1);
							}
				}
				// En depuración, añade todos los directorios que se encuentren bajo ../Plugins/xxx.Plugin
				#if DEBUG
				if (Count == 0)
				{
					Clear();
					LoadPluginsDevelopment();
				}
				#endif
		}

		/// <summary>
		///		Añade los directorios hijo de un directorio a la colección de directorios de plugin
		/// </summary>
		private void AddPathsDefault(string path)
		{
			string[] paths;

				// Crea el directorio si no existe
				if (!System.IO.Directory.Exists(path))
					Libraries.LibCommonHelper.Files.HelperFiles.MakePath(path);
				// Añade los directorios
				paths = System.IO.Directory.GetDirectories(path);
				// Añade los plugins del directorio predeterminado
				foreach (string pathChild in paths)
					Add(pathChild, true);
		}

		/// <summary>
		///		Añade un directorio
		/// </summary>
		private void Add(string path, bool enabled)
		{
			// Normaliza el directorio
			path = path.TrimIgnoreNull();
			while (!path.IsEmpty() && path.EndsWith("\\"))
				path = path.Substring(path.Length - 1);
			// Añade el directorio
			if (!path.IsEmpty() && System.IO.Directory.Exists(path))
			{
				PluginPathModel pluginPath = Search(path);

					if (pluginPath == null)
						Add(new PluginPathModel(path, enabled));
					else
						pluginPath.Enabled = enabled;
			}
		}

		/// <summary>
		///		Busca un directorio de plugins
		/// </summary>
		private PluginPathModel Search(string path)
		{
			return this.FirstOrDefault<PluginPathModel>(pluginPath => pluginPath.Path.EqualsIgnoreCase(path));
		}

		/// <summary>
		///		Comprueba si existe un directorio en la colección
		/// </summary>
		private bool Exists(string path)
		{
			// Comprueba si existe
			foreach (PluginPathModel pluginPath in this)
				if (path.EqualsIgnoreCase(pluginPath.Path))
					return true;
			// Si ha llegado hasta aquí es porque no existe
			return false;
		}

		/// <summary>
		///		Graba los directorios de plugins
		/// </summary>
		public void Save()
		{
			string fullPaths = "";

				// Añade los directorios a la cadena de salida
				foreach (PluginPathModel pluginPath in this)
				{
					string pathTrim = pluginPath.Path.TrimIgnoreNull();

						if (!pathTrim.IsEmpty() && System.IO.Directory.Exists(pathTrim))
							fullPaths = fullPaths.AddWithSeparator(pathTrim + SeparatorData + (pluginPath.Enabled ? "1" : "0"),
																   SeparatorPaths, false);
				}
				// Añade el parámetro
				Properties.Settings.Default.PathPlugins = fullPaths;
				// Graba la configuración
				Properties.Settings.Default.Save();
		}

		/// <summary>
		///		Convierte la colección en una lista con los nombres de directorios
		/// </summary>
		internal List<string> ConvertToList()
		{
			var paths = new	List<string>();

				// Convierte la colección
				foreach (PluginPathModel pluginPath in this)
					if (pluginPath.Enabled && !string.IsNullOrWhiteSpace(pluginPath.Path))
						paths.Add(pluginPath.Path);
				// Devuelve la lista de directorios
				return paths;
		}

		/// <summary>
		///		Carga los plugins que se encuentren en la carpeta "Plugins" del directorio de la aplicación en desarrollo
		/// </summary>
		private void LoadPluginsDevelopment()
		{
			string path = Globals.HostController.HostViewModelController.Configuration.PathBaseApplication;

				// Pasa al directorio donde se encuentran los fuentes
				// En teoría está en un directorio como este: BauPlugStudio\Applications\BauPlugStudio\bin\Debug\
				path = System.IO.Path.GetDirectoryName(path); // debug
				path = System.IO.Path.GetDirectoryName(path); // bin
				path = System.IO.Path.GetDirectoryName(path); // BauPlugStudio
				path = System.IO.Path.GetDirectoryName(path); // Applications
				path = System.IO.Path.GetDirectoryName(path); // .
				// Pasa al directorio de plugins
				path = System.IO.Path.Combine(path, "Plugins");
				// Carga los plugins
				if (System.IO.Directory.Exists(path))
					foreach (string child in System.IO.Directory.GetDirectories(path))
						foreach (string pathApplication in System.IO.Directory.GetDirectories(child))
							if (pathApplication.EndsWith(".plugin", StringComparison.CurrentCultureIgnoreCase))
							{
								string endPath = System.IO.Path.Combine(pathApplication, "bin\\debug");

									if (System.IO.Directory.Exists(endPath))
										Add(new PluginPathModel(endPath, true));
							}
				// Graba los directorios de plugins
				Save();
		}
	}
}
