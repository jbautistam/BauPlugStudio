using System;

using Bau.Applications.BauPlugStudio.Controllers.Plugins;

namespace Bau.Applications.BauPlugStudio.ViewModels.Tools.Configuration
{
	/// <summary>
	///		ViewModel para un directorio de plugins
	/// </summary>
	public class PluginPathItemViewModel : Bau.Libraries.MVVM.ViewModels.ListItems.BaseListItemViewModel
	{
		public PluginPathItemViewModel(PluginPathModel plugin)
		{
			PathPlugin = plugin;
			IsChecked = PathPlugin.Enabled;
			Text = PathPlugin.Name;
			Path = PathPlugin.Path;
			Tag = PathPlugin;
		}

		/// <summary>
		///		Directorio de plugins
		/// </summary>
		public PluginPathModel PathPlugin { get; set; }

		/// <summary>
		///		Directorio 
		/// </summary>
		public string Path { get; set; }
	}
}
