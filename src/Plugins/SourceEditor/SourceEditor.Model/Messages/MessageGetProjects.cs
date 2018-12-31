using System;

namespace Bau.Libraries.SourceEditor.Model.Messages
{
	/// <summary>
	///		Mensaje para obtener los proyectos abiertos en la solución
	/// </summary>
	public class MessageGetProjects
	{
		public MessageGetProjects(Definitions.ProjectDefinitionModel definition)
		{
			ProjectsDefinition.Add(definition);
		}

		public MessageGetProjects(Definitions.ProjectDefinitionModelCollection definitions)
		{
			ProjectsDefinition = definitions;
		}

		/// <summary>
		///		Definiciones de proyectos buscadas
		/// </summary>
		public Definitions.ProjectDefinitionModelCollection ProjectsDefinition { get; } = new Definitions.ProjectDefinitionModelCollection();

		/// <summary>
		///		Nombres de archivos de proyecto
		/// </summary>
		public System.Collections.Generic.List<string> ProjectFiles { get; } = new System.Collections.Generic.List<string>();
	}
}
