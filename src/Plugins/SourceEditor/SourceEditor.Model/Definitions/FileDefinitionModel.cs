using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Definición de un archivo
	/// </summary>
	public class FileDefinitionModel : AbstractFileDefinitionModel
	{
		public FileDefinitionModel(ProjectDefinitionModel definition, string name, string icon,
								   string extension, bool showExtensionAtTree, string template = null,
								   string extensionHighlight = null,
								   OpenMode mode = OpenMode.Owner)
						: base(definition, name, extension, icon, showExtensionAtTree, mode)
		{
			Template = template;
			ExtensionHighlight = extensionHighlight;
		}

		/// <summary>
		///		Plantilla
		/// </summary>
		public string Template { get; }

		/// <summary>
		///		Extensión de archivo para resalte del código fuente
		/// </summary>
		public string ExtensionHighlight { get; }
	}
}
