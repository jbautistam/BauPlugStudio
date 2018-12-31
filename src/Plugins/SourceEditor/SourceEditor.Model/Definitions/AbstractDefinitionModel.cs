using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Clase base para las definiciones
	/// </summary>
	public abstract class AbstractDefinitionModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Modo de apertura
		/// </summary>
		public enum OpenMode
		{
			/// <summary>Se abre en una ventana propietaria del proyecto</summary>
			Owner,
			/// <summary>Ventana de código fuente</summary>
			SourceCode,
			/// <summary>Ventana de imagen</summary>
			Image
		}

		public AbstractDefinitionModel(string name, string icon, OpenMode mode = OpenMode.Owner)
		{
			Name = name;
			Icon = icon;
			IDOpenMode = mode;
		}

		/// <summary>
		///		Icono
		/// </summary>
		public string Icon { get; set; }

		/// <summary>
		///		Elementos hijo manejados por la aplicación propietaria
		/// </summary>
		public OwnerObjectDefinitionModelCollection OwnerChilds { get; } = new OwnerObjectDefinitionModelCollection();

		/// <summary>
		///		Modo de apertura
		/// </summary>
		public OpenMode IDOpenMode { get; set; }

		/// <summary>
		///		Menús asociados al elemento
		/// </summary>
		public MenuModelCollection Menus { get; } = new MenuModelCollection();
	}
}
