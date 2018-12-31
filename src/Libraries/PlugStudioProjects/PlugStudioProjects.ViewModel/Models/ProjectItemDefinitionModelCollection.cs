using System;
using System.Collections.Generic;

namespace Bau.Libraries.PlugStudioProjects.Models
{
	/// <summary>
	///		Colección de definiciones de proyecto
	/// </summary>
	public class ProjectItemDefinitionModelCollection : List<ProjectItemDefinitionModel>
	{
		/// <summary>
		///		Busca una definición de elemento por su Id
		/// </summary>
		public ProjectItemDefinitionModel Search(string id)
		{
			// Busca los elementos en esta colección
			foreach (ProjectItemDefinitionModel item in this)
			{
				ProjectItemDefinitionModel child = item.Search(id);

					if (child != null)
						return child;
			}
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Busca una definición de elemento por su extensión
		/// </summary>
		public ProjectItemDefinitionModel SearchByExtension(string extension)
		{
			// Busca los elementos en esta colección
			foreach (ProjectItemDefinitionModel item in this)
			{
				ProjectItemDefinitionModel child = item.SearchByExtension(extension);

					if (child != null)
						return child;
			}
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}
	}
}
