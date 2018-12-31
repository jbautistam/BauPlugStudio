using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Definición de un paquete
	/// </summary>
	public class PackageDefinitionModel : AbstractFileDefinitionModel
	{
		public PackageDefinitionModel(ProjectDefinitionModel definition, string name, string icon,
									  string fileName, OpenMode mode = OpenMode.Owner)
							: base(definition, name, null, icon, false, mode)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Nombre del archivo que identifica el paquete
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Indica si se ocultan los archivos con las extensiones del paquete
		/// </summary>
		public bool HideFilesByExtension { get; set; }

		/// <summary>
		///		Indica si se pueden añadir archivos a un paquete
		/// </summary>
		public bool CanAddFiles { get; set; }

		/// <summary>
		///		Extensiones de los archivos del paquete
		/// </summary>
		public System.Collections.Generic.List<string> Extensions { get; } = new System.Collections.Generic.List<string>();
	}
}
