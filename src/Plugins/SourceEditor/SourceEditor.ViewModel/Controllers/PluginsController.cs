using System;
using System.Collections.Generic;

using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Plugins;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de plugins de <see cref="SourceEditorViewModel"/>
	/// </summary>
	internal class PluginsController
	{
		// Variables privadas
		private Dictionary<string, IPluginSourceEditor> _dctPlugins = new Dictionary<string, IPluginSourceEditor>();
		private ProjectDefinitionModelCollection _definitions = null;
		private bool _isDirty = false;

		/// <summary>
		///		Añade un plugin al diccionario
		/// </summary>
		internal void Add(string project, IPluginSourceEditor plugin)
		{
			_dctPlugins.Add(ComputeKey(project), plugin);
			_isDirty = true;
		}

		/// <summary>
		///		Obtiene las definiciones de proyectos
		/// </summary>
		private ProjectDefinitionModelCollection GetProjectDefinitions()
		{
			ProjectDefinitionModelCollection definitions = new ProjectDefinitionModelCollection();

				// Recorre los plugins
				foreach (KeyValuePair<string, IPluginSourceEditor> objKey in _dctPlugins)
					definitions.Add(objKey.Value.Definition);
				// Devuelve las definiciones de proyectos
				return definitions;
		}

		/// <summary>
		///		Llama a un plugin para abrir un archivo
		/// </summary>
		internal bool OpenFile(ProjectDefinitionModel definition, FileModel file, bool isNew)
		{
			IPluginSourceEditor plugin = Search(definition.Extension);

				// Abre el archivo con el plugin localizado
				if (plugin != null)
					return plugin.OpenFile(file, isNew);
				else
					return false;
		}

		/// <summary>
		///		Ejecuta una acción sobre un plugin
		/// </summary>
		internal bool ExecuteAction(ProjectDefinitionModel definition, FileModel file, MenuModel menu)
		{
			IPluginSourceEditor plugin = Search(definition.Extension);

				// Ejecuta la acción con el plugin localizado
				if (plugin != null)
					return plugin.ExecuteAction(file, menu);
				else
					return false;
		}

		/// <summary>
		///		Cambia el nombre de un archivo
		/// </summary>
		internal bool Rename(FileModel file, string newFileName, string title)
		{
			IPluginSourceEditor plugin = Search(file);

				// Ejecuta la acción con el plugin localizado
				if (plugin != null)
					return plugin.Rename(file, newFileName, title);
				else
					return false;
		}

		/// <summary>
		///		Carga los hijos de un nodo
		/// </summary>
		internal OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			IPluginSourceEditor plugin = Search(file);

				// Ejecuta la acción con el plugin localizado
				if (plugin != null)
					return plugin.LoadOwnerChilds(file, parent);
				else
					return new OwnerChildModelCollection();
		}

		/// <summary>
		///		Busca un plugin
		/// </summary>
		private IPluginSourceEditor Search(FileModel file)
		{
			return Search(file.SearchProject()?.Definition.Extension);
		}

		/// <summary>
		///		Obtiene el plugin asociado a un proyecto
		/// </summary>
		internal IPluginSourceEditor Search(string project)
		{
			// Obtiene el plugin
			if (!string.IsNullOrEmpty(project))
				if (_dctPlugins.TryGetValue(ComputeKey(project), out IPluginSourceEditor plugin))
					return plugin;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Calcula la clave
		/// </summary>
		private string ComputeKey(string key)
		{
			return key?.ToUpper();
		}

		/// <summary>
		///		Definiciones de proyectos
		/// </summary>
		internal ProjectDefinitionModelCollection ProjectDefinitions
		{
			get
			{
				// Carga las definiciones
				if (_definitions == null || _isDirty)
					_definitions = GetProjectDefinitions();
				// Indica que se han cargado las definiciones
				_isDirty = false;
				// Devuelve las definiciones
				return _definitions;
			}
		}
	}
}
