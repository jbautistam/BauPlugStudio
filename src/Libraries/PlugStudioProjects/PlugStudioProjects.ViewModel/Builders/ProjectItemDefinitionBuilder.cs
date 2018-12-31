using System;

using Bau.Libraries.PlugStudioProjects.Models;

namespace Bau.Libraries.PlugStudioProjects.Builders
{
	/// <summary>
	///		Generador para la definición de un elemento
	/// </summary>
	public class ProjectItemDefinitionBuilder
	{
		public ProjectItemDefinitionBuilder(string id, string name, ProjectItemDefinitionModel.ItemType type) : this(null, id, name, type) {}

		public ProjectItemDefinitionBuilder(ProjectItemDefinitionBuilder parent, 
											string id, string name, ProjectItemDefinitionModel.ItemType type)
		{
			// Inicializa los generadores
			ParentBuilder = parent;
			// Crea el elemento
			Item = new ProjectItemDefinitionModel(id, name, type);
			// Añade el elemento padre
			if (parent != null)
				ParentBuilder.Item.Items.Add(Item);
		}

		/// <summary>
		///		Añade una definición
		/// </summary>
		public ProjectItemDefinitionBuilder WithItem(string id, string name, ProjectItemDefinitionModel.ItemType type)
		{
			return new ProjectItemDefinitionBuilder(this, id, name, type);
		}

		/// <summary>
		///		Asigna la descripción
		/// </summary>
		public ProjectItemDefinitionBuilder WithDescription(string description)
		{
			// Asigna la propiedad
			Item.Description = description;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna el icono
		/// </summary>
		public ProjectItemDefinitionBuilder WithIcon(string icon)
		{
			// Asigna la propiedad
			Item.Icon = icon;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna la negrita
		/// </summary>
		public ProjectItemDefinitionBuilder WithBold()
		{
			// Asigna la propiedad
			Item.IsBold = true;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna el color
		/// </summary>
		public ProjectItemDefinitionBuilder WithForeground(BauMvvm.ViewModels.Media.MvvmColor foreground)
		{
			// Asigna la propiedad
			Item.Foreground = foreground;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna la extensión de archivo
		/// </summary>
		public ProjectItemDefinitionBuilder WithExtension(string extension)
		{
			// Asigna la propiedad
			if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
				extension = "." + extension;
			Item.Extension = extension;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna la ventana de edición
		/// </summary>
		public ProjectItemDefinitionBuilder WithEditor(ProjectItemDefinitionModel.WindowEditorType editorType)
		{
			// Asigna la propiedad
			Item.EditorType = editorType;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna la plantilla de archivo
		/// </summary>
		public ProjectItemDefinitionBuilder WithTemplate(string template)
		{
			// Asigna la propiedad
			Item.Template = template;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Asigna el archivo de ayuda
		/// </summary>
		public ProjectItemDefinitionBuilder WithHelpFile(string helpFile)
		{
			// Asigna la propiedad
			Item.HelpFile = helpFile;
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Añade un menú
		/// </summary>
		public ProjectItemDefinitionBuilder WithMenu(MenuModel.MenuType type, string key, string text, string icon)
		{
			// Añade el menú
			Item.Menus.Add(type, key, text, icon);
			// Devuelve el generador
			return this;
		}

		/// <summary>
		///		Genera el elemento
		/// </summary>
		public ProjectItemDefinitionModel Build()
		{
			if (ParentBuilder != null)
				return ParentBuilder.Build();
			else
				return Item;
		}

		/// <summary>
		///		Pasa al generador del elemento padre
		/// </summary>
		public ProjectItemDefinitionBuilder Back()
		{
			return ParentBuilder;
		}

		/// <summary>
		///		Generador de elementos
		/// </summary>
		private ProjectItemDefinitionBuilder ParentBuilder { get; }

		/// <summary>
		///		Elemento definido
		/// </summary>
		public ProjectItemDefinitionModel Item { get; }
	}
}
