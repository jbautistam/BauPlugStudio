using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Applications.BauPlugStudio.Controllers.Plugins
{
	/// <summary>
	///		Clase con los datos de un directorio de plugins
	/// </summary>
	public class PluginPathModel
	{
		public PluginPathModel(string path, bool enabled)
		{
			Path = path;
			Enabled = enabled;
		}

		/// <summary>
		///		Nombre del directorio de plugins
		/// </summary>
		public string Name
		{
			get
			{
				if (Path.IsEmpty())
					return "-";
				else
					return System.IO.Path.GetFileName(Path);
			}
		}

		/// <summary>
		///		Directorio de plugins
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		///		Indica si el plugin está activo
		/// </summary>
		public bool Enabled { get; set; }
	}
}
