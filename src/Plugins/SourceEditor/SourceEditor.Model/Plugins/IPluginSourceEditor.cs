using System;

namespace Bau.Libraries.SourceEditor.Model.Plugins
{
	/// <summary>
	///		Interface para los plugins de SourceEditor
	/// </summary>
	public interface IPluginSourceEditor
	{
		/// <summary>
		///		Abre un archivo
		/// </summary>
		bool OpenFile(Solutions.FileModel file, bool isNew);

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		bool Rename(Solutions.FileModel file, string newFileName, string title);

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		bool ExecuteAction(Solutions.FileModel file, Definitions.MenuModel menu);

		/// <summary>
		///		Obtiene los nodos hijo de un nodo propietario
		/// </summary>
		Solutions.OwnerChildModelCollection LoadOwnerChilds(Solutions.FileModel file, Solutions.OwnerChildModel parent);

		/// <summary>
		///		Definiciones de proyectos, archivos y menús
		/// </summary>
		Definitions.ProjectDefinitionModel Definition { get; }
	}
}
