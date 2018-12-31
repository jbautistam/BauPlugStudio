using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.SourceEditor.Model.Definitions;
using Bau.Libraries.SourceEditor.Model.Messages;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.SourceEditor.ViewModel.Controllers
{
	/// <summary>
	///		Controlador de mensajes
	/// </summary>
	public class MessagesController : BasePluginMessengerController
	{
		public MessagesController() : base(SourceEditorViewModel.Instance, SourceEditorViewModel.Instance.HostController, true)
		{
		}

		/// <summary>
		///		Trata los mensajes
		/// </summary>
		public override void TreatMessage(object sender, Message message)
		{
			if (message.Content != null && message.Content is MessageGetProjects)
				TreatGetProjects(message.Source, message.Content as MessageGetProjects);
		}

		/// <summary>
		///		Solicita las definiciones de proyectos
		/// </summary>
		public void RequestProjectDefinitions()
		{
			MessageRequestPlugin messageRequest = new MessageRequestPlugin();

				// Envía el mensaje
				HostMessenger.Send(new Message(SourceEditorViewModel.Instance.ModuleName,
											   typeof(MessageRequestPlugin).ToString(), "Request",
											   messageRequest));
				// Añade los plugins devueltos en el mensaje a la colección de plugins
				foreach (System.Collections.Generic.KeyValuePair<string, Model.Plugins.IPluginSourceEditor> objKey in messageRequest.Plugins)
					if (!string.IsNullOrEmpty(objKey.Key))
						PluginsManager.Add(objKey.Key, objKey.Value);
		}

		/// <summary>
		///		Envía el mensaje para abrir un archivo
		/// </summary>
		public bool OpenFile(ProjectDefinitionModel definition, FileModel file, bool isNew)
		{
			return PluginsManager.OpenFile(definition, file, isNew);
		}

		/// <summary>
		///		Envía el mensaje de ejecución de una opción de menú sobre un archivo
		/// </summary>
		public void ExecuteAction(ProjectDefinitionModel definition, FileModel file, MenuModel menu)
		{
			PluginsManager.ExecuteAction(definition, file, menu);
		}

		/// <summary>
		///		Envía el mensaje de cambio del nombre de un archivo
		/// </summary>
		internal void RenameFile(FileModel file, string newFileName, string title)
		{
			PluginsManager.Rename(file, newFileName, title);
		}

		/// <summary>
		///		Envía el mensaje de obtener hijos de un nodo
		/// </summary>
		internal OwnerChildModelCollection LoadOwnerChilds(FileModel file, OwnerChildModel parent)
		{
			return PluginsManager.LoadOwnerChilds(file, parent);
		}

		/// <summary>
		///		Trata el mensaje de obtener proyectos de un tipo
		/// </summary>
		private void TreatGetProjects(string moduleName, MessageGetProjects message)
		{
			if (!SourceEditorViewModel.Instance.LastFileSolution.IsEmpty())
			{
				SolutionModel solution = new Application.Bussiness.Solutions.SolutionBussiness().Load(message.ProjectsDefinition,
																									  SourceEditorViewModel.Instance.LastFileSolution);

					// Obtiene los nombres de archivo
					foreach (ProjectModel project in solution.GetAllProjects())
						if (message.ProjectsDefinition.SearchByExtension(project.Definition.Extension) != null)
							message.ProjectFiles.Add(project.FullFileName);
			}
		}

		/// <summary>
		///		Manager de plugins (para no escribir tanto)
		/// </summary>
		internal PluginsController PluginsManager
		{
			get { return SourceEditorViewModel.Instance.PluginsController; }
		}
	}
}
