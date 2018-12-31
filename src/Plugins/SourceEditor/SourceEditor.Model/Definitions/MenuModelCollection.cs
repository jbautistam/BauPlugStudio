using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Colección de <see cref="MenuModel"/>
	/// </summary>
	public class MenuModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<MenuModel>
	{
		/// <summary>
		///		Añade un elemento a la colección
		/// </summary>
		public void Add(MenuModel.MenuType type, string key, string name, string icon)
		{
			Add(new MenuModel(type, key, name, icon));
		}
	}
}
