using System;

using Bau.Libraries.Plugins.ViewModels.Controllers.Messengers;
using Bau.Libraries.Plugins.ViewModels.Controllers.Processes;
using Bau.Libraries.Plugins.ViewModels.Controllers.Settings;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Applications.BauPlugStudio.Controllers
{
	/// <summary>
	///		Controlador para las comunicación entre elementos de plugins
	/// </summary>
	public class HostViewModelController : IHostViewModelController
	{
		public HostViewModelController(string applicationName)
		{
			ApplicationName = applicationName;
			Messenger = new MessengerController();
			Configuration = new Configuration(ApplicationName, null);
			TasksProcessor = new TasksQueue();
			Scheduler = new SchedulerController();
		}

		/// <summary>
		///		Nombre de la aplicación
		/// </summary>
		public string ApplicationName { get; }

		/// <summary>
		///		Controlador de configuración
		/// </summary>
		public Configuration Configuration { get; }

		/// <summary>
		///		Controlador de mensajes
		/// </summary>
		public MessengerController Messenger { get; }

		/// <summary>
		///		Controlador de colas de tareas
		/// </summary>
		public TasksQueue TasksProcessor { get; }

		/// <summary>
		///		Planificador de procesos
		/// </summary>
		public SchedulerController Scheduler { get; }
	}
}
