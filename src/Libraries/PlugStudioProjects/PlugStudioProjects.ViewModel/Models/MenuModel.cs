using System;

namespace Bau.Libraries.PlugStudioProjects.Models
{
	/// <summary>
	///		Clase con los datos de un menú
	/// </summary>
	public class MenuModel
	{
		/// <summary>
		///		Tipo de menú
		/// </summary>
		public enum MenuType
		{
			/// <summary>Menú que se coloca sobre la opción "Nuevo archivo"</summary>
			NewFile,
			/// <summary>Menú que se coloca sobre la sección de "Comandos" del menú principal</summary>
			Command
		}

		public MenuModel(MenuType type, string key, string text, string icon)
		{
			Type = type;
			Key = key;
			Text = text;
			Icon = icon;
		}

		/// <summary>
		///		Tipo del menú
		/// </summary>
		public MenuType Type { get; }

		/// <summary>
		///		Clave de la opción de menú
		/// </summary>
		public string Key { get; }

		/// <summary>
		///		Texto del menú
		/// </summary>
		public string Text { get; }

		/// <summary>
		///		Icono
		/// </summary>
		public string Icon { get; }
	}
}
