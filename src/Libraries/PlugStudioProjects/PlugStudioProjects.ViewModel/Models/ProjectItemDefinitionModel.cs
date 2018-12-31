using System;

namespace Bau.Libraries.PlugStudioProjects.Models
{
	/// <summary>
	///		Elemento abstracto para definición de un proyecto
	/// </summary>
	public class ProjectItemDefinitionModel
	{
		/// <summary>
		///		Tipo de definición
		/// </summary>
		public enum ItemType
		{
			/// <summary>Proyecto</summary>
			Project,
			/// <summary>Archivo</summary>
			File,
			/// <summary>Carpeta</summary>
			Folder,
			/// <summary>Elemento fijo</summary>
			Fixed
		}
		/// <summary>
		///		Tipo de ventana de edición que se abre con el elemento
		/// </summary>
		public enum WindowEditorType
		{
			/// <summary>Desconocida. Manejada por el elemento padre</summary>
			Unknown,
			/// <summary>Editor de código</summary>
			Script,
			/// <summary>Editor de imágenes</summary>
			Image,
			/// <summary>Navegador web</summary>
			Browser
		}

		public ProjectItemDefinitionModel(string id, string name, ProjectItemDefinitionModel.ItemType type)
		{
			Id = id;
			Name = name;
			Type = type;
		}

		/// <summary>
		///		Selecciona las definiciones de un tipo
		/// </summary>
		internal ProjectItemDefinitionModelCollection Select(ItemType type)
		{
			ProjectItemDefinitionModelCollection definitions = new ProjectItemDefinitionModelCollection();

				// Si es del tipo seleccionado la añade
				if (Type == type)
					definitions.Add(this);
				// Añade los elementos hijo
				foreach (ProjectItemDefinitionModel item in Items)
					definitions.AddRange(item.Select(type));
				// Devuelve la colección de definiciones
				return definitions;
		}

		/// <summary>
		///		Busca una definición de elemento
		/// </summary>
		public ProjectItemDefinitionModel Search(string id)
		{
			if ((string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(id)) ||
					(!string.IsNullOrWhiteSpace(Id) && Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)))
				return this;
			else
				return Items.Search(id);
		}

		/// <summary>
		///		Busca una definición de elemento
		/// </summary>
		public ProjectItemDefinitionModel SearchByExtension(string extension)
		{
			if ((string.IsNullOrEmpty(Extension) && string.IsNullOrEmpty(extension)) ||
					(!string.IsNullOrWhiteSpace(Extension) && IsEqualExtension(extension)))
				return this;
			else
				return Items.SearchByExtension(extension);
		}

		/// <summary>
		///		Comprueba si las extensiones son iguales
		/// </summary>
		public bool IsEqualExtension(string extension)
		{
			// Compara las partes
			foreach (string part in Extension.Split(';'))
				if (part.Equals(extension, StringComparison.CurrentCultureIgnoreCase) ||
						("." + part).Equals(extension, StringComparison.CurrentCultureIgnoreCase) ||
						part.Equals("." + extension, StringComparison.CurrentCultureIgnoreCase))
					return true;
			// Si ha llegado hasta aquí es porque no son iguales
			return false;
		}

		/// <summary>
		///		Clave
		/// </summary>
		public string Id { get; }

		/// <summary>
		///		Nombre de la definición
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		Descripción de la definición
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		///		Tipo
		/// </summary>
		public ItemType Type { get; }

		/// <summary>
		///		Recurso del icono
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		///		Extensión
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		///		Tipo de editor
		/// </summary>
		public WindowEditorType EditorType { get; set; } = WindowEditorType.Unknown;

		/// <summary>
		///		Plantilla
		/// </summary>
		public string Template { get; set; }

		/// <summary>
		///		Archivo de ayuda
		/// </summary>
		public string HelpFile { get; set; }

		/// <summary>
		///		Color de texto
		/// </summary>
		public BauMvvm.ViewModels.Media.MvvmColor Foreground { get; set; }

		/// <summary>
		///		Indica si el texto se debe mostrar en negrita
		/// </summary>
		public bool IsBold { get; set; }

		/// <summary>
		///		Elementos del menú
		/// </summary>
		public MenuModelCollection Menus { get; } = new MenuModelCollection();

		/// <summary>
		///		Elementos hijo
		/// </summary>
		public ProjectItemDefinitionModelCollection Items { get; } = new ProjectItemDefinitionModelCollection();
	}
}
