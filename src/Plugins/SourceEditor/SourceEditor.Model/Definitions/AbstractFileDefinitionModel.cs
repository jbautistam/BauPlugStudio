using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Clase abstracta para la definición de archivos
	/// </summary>
	public class AbstractFileDefinitionModel : AbstractDefinitionModel
	{
		public AbstractFileDefinitionModel(ProjectDefinitionModel definition, string name, string extension,
										   string icon, bool showExtensionAtTree,
										   OpenMode mode = OpenMode.Owner)
					: base(name, icon, mode)
		{
			Definition = definition;
			Extension = extension;
			ShowExtensionAtTree = showExtensionAtTree;
		}

		/// <summary>
		///		Definición de proyecto
		/// </summary>
		public ProjectDefinitionModel Definition { get; }

		/// <summary>
		///		Extensión del archivo
		/// </summary>
		public string Extension { get; }

		/// <summary>
		///		Muestra la extensión del archivo en el árbol
		/// </summary>
		public bool ShowExtensionAtTree { get; }
	}
}
