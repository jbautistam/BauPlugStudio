using System;
using System.Collections.Generic;

namespace Bau.Libraries.PlugStudioProjects.Models
{
	/// <summary>
	///		Colección de <see cref="MenuModel"/>
	/// </summary>
	public class MenuModelCollection : List<MenuModel>
	{
		/// <summary>
		///		Añade un elemento a la colección
		/// </summary>
		public void Add(MenuModel.MenuType type, string key, string text, string icon)
		{
			Add(new MenuModel(type, key, text, icon));
		}
	}
}
