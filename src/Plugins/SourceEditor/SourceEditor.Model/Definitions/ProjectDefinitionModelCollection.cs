using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Colección de <see cref="ProjectDefinitionModel"/>
	/// </summary>
	public class ProjectDefinitionModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<ProjectDefinitionModel>
	{
		/// <summary>
		///		Añade un proyecto a la colección
		/// </summary>
		public ProjectDefinitionModel Add(string name, string icon, string module, string type, string extension)
		{
			ProjectDefinitionModel project = new ProjectDefinitionModel(name, icon, module, type, extension);

				// Añade la definición
				Add(project);
				// Devuelve el objeto
				return project;
		}

		/// <summary>
		///		Obtiene una definición de proyecto
		/// </summary>
		public ProjectDefinitionModel Search(string module, string type)
		{
			ProjectDefinitionModel definition;

				// Busca la definición de proyecto
				definition = this.FirstOrDefault(project => project.Module.EqualsIgnoreCase(module) && project.Type.EqualsIgnoreCase(type));
				// Si no se ha encontrado ninguna definición la crea
				if (definition == null)
					definition = new ProjectDefinitionModel("Desconocido", null, module, type, null);
				// Devuelve la definición
				return definition;
		}

		/// <summary>
		///		Busca la definición de un proyecto por su extensión
		/// </summary>
		public ProjectDefinitionModel SearchByExtension(string extension)
		{
			// Quita el punto a la extensión
			if (extension.TrimIgnoreNull().StartsWith("."))
				extension = extension.Substring(1);
			// Busca la extensión del proyecto
			return this.FirstOrDefault(project => !project.Extension.IsEmpty() &&
												  (project.Extension.EqualsIgnoreCase(extension) || project.Extension.EqualsIgnoreCase("." + extension)));
		}
	}
}
