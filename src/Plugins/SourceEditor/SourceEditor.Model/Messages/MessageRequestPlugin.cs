using System;

namespace Bau.Libraries.SourceEditor.Model.Messages
{
	/// <summary>
	///		Mensaje para añadir un plugin al editor de código
	/// </summary>
	public class MessageRequestPlugin
	{
		public MessageRequestPlugin()
		{
		}

		/// <summary>
		///		Método para crear un plugin
		/// </summary>
		public void CreatePlugin(string projectExtension, Plugins.IPluginSourceEditor plugin)
		{
			if (!Plugins.ContainsKey(projectExtension))
				Plugins.Add(projectExtension, plugin);
			else
				System.Diagnostics.Debug.WriteLine($"Ya existe un plugin con la extensión: {projectExtension}");
		}

		/// <summary>
		///		Plugin
		/// </summary>
		public System.Collections.Generic.Dictionary<string, Plugins.IPluginSourceEditor> Plugins { get; } = new System.Collections.Generic.Dictionary<string, Plugins.IPluginSourceEditor>();
	}
}
