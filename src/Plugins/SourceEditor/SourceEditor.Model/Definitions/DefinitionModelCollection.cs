using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Colección de <see cref="AbstractDefinitionModel"/>
	/// </summary>
	public class DefinitionModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<AbstractDefinitionModel>
	{
		/// <summary>
		///		Añade una definición de archivo
		/// </summary>
		public FileDefinitionModel Add(ProjectDefinitionModel definition, string name, string icon, string extension,
									   bool showExtensionAtTree, string template = null, string extensionHighlight = null,
									   AbstractDefinitionModel.OpenMode mode = AbstractDefinitionModel.OpenMode.Owner)
		{
			FileDefinitionModel file = new FileDefinitionModel(definition, name, icon, extension,
															   showExtensionAtTree, template, extensionHighlight, mode);

				// Añade el archivo
				Add(file);
				// Devuelve el objeto añadido
				return file;
		}

		/// <summary>
		///		Añade una definición de archivo oculto
		/// </summary>
		public FileHiddenDefinitionModel AddHidden(ProjectDefinitionModel definition, string extension)
		{
			FileHiddenDefinitionModel file = new FileHiddenDefinitionModel(definition, extension);

				// Añade el archivo
				Add(file);
				// Devuelve el archivo añadido
				return file;
		}

		/// <summary>
		///		Comprueba si se puede mostrar un archivo
		/// </summary>
		public bool CanShow(Solutions.FileModel file)
		{ 
			// Recorre los archivos
			foreach (AbstractDefinitionModel definition in this)
				if ((definition is FileHiddenDefinitionModel &&
					 (definition as FileHiddenDefinitionModel).Extension.EqualsIgnoreCase(file.Extension)) ||
						((definition is FileDefinitionModel) &&
								(definition as FileDefinitionModel).Definition.Extension.EqualsIgnoreCase(file.Extension)))
					return false;
			// Si ha llegado hasta aquí es porque debe mostrarse
			return true;
		}

		/// <summary>
		///		Obtiene una definición de archivo por su extensión
		/// </summary>
		public AbstractDefinitionModel SearchByExtension(string extension)
		{ 
			// Obtiene la definición de archivo
			foreach (AbstractDefinitionModel definition in this)
				if (definition is FileDefinitionModel &&
						(definition as FileDefinitionModel).Extension.EqualsIgnoreCase(extension))
					return definition;
				else if (definition is PackageDefinitionModel)
					foreach (string strPackExtension in (definition as PackageDefinitionModel).Extensions)
						if (strPackExtension.EqualsIgnoreCase(extension))
							return definition;
			// Si ha llegado hasta aquí es que no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Añade los archivos de imagen típicos
		/// </summary>
		public void AddFilesImage(ProjectDefinitionModel definition, string icon)
		{
			definition.FilesDefinition.Add(definition, "Imagen PNG", icon,
										   "png", true, null, null, AbstractDefinitionModel.OpenMode.Image);
			definition.FilesDefinition.Add(definition, "Imagen JPG", icon,
										   "jpg", true, null, null, AbstractDefinitionModel.OpenMode.Image);
			definition.FilesDefinition.Add(definition, "Imagen GIF", icon,
										   "gif", true, null, null, AbstractDefinitionModel.OpenMode.Image);
			definition.FilesDefinition.Add(definition, "Imagen BMP", icon,
										   "bmp", true, null, null, AbstractDefinitionModel.OpenMode.Image);
		}

		/// <summary>
		///		Comprueba si un fichero debe mostrar la extensión en el árbol
		/// </summary>
		public bool MustShowExtension(Solutions.FileModel file)
		{ 
			// Recorre la colección comprobando se se debe mostrar la extensión
			foreach (AbstractFileDefinitionModel definition in this)
				if (file.Extension.EqualsIgnoreCase(definition.Extension))
					return definition.ShowExtensionAtTree;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return false;
		}
	}
}
