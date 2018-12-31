using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Clase con los datos de un menú
	/// </summary>
	public class MenuModel : LibDataStructures.Base.BaseExtendedModel
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

		public MenuModel(MenuType type, string key, string name, string icon)
		{
			Type = type;
			Key = key;
			Name = name;
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
		///		Icono
		/// </summary>
		public string Icon { get; }
	}
}
