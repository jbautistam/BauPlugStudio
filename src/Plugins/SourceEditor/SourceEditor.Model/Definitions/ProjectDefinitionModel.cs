using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Definición de un proyecto
	/// </summary>
	public class ProjectDefinitionModel : AbstractDefinitionModel
	{
		public ProjectDefinitionModel(string name, string icon, string module, string type,
									  string extension, OpenMode mode = OpenMode.Owner)
										: base(name, icon, mode)
		{
			Module = module;
			Type = type;
			Extension = extension;
		}

		/// <summary>
		///		Obtiene los menús del elemento especificado
		/// </summary>
		public MenuModelCollection GetMenus(Solutions.FileModel file)
		{
			MenuModelCollection menus = new MenuModelCollection();

				// Obtiene los menús
				if (file is Solutions.ProjectModel)
					menus.AddRange(Menus);
				else
					foreach (AbstractDefinitionModel abstractDefinition in FilesDefinition)
					{
						AbstractFileDefinitionModel fileConcrete = abstractDefinition as AbstractFileDefinitionModel;

							if (fileConcrete != null && file.Extension.EqualsIgnoreCase(fileConcrete.Extension))
								menus.AddRange(abstractDefinition.Menus);
					}
				// Devuelve la colección de menús
				return menus;
		}

		/// <summary>
		///		Módulo al que pertenece la definición
		/// </summary>
		public string Module { get; }

		/// <summary>
		///		Tipo de la definición
		/// </summary>
		public string Type { get; }

		/// <summary>
		///		Extensión de archivo
		/// </summary>
		public string Extension { get; }

		/// <summary>
		///		Definiciones de archivos
		/// </summary>
		public DefinitionModelCollection FilesDefinition { get; } = new DefinitionModelCollection();
	}
}
