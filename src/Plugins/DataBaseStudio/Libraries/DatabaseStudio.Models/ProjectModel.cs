using System;

namespace Bau.Libraries.DatabaseStudio.Models
{
	/// <summary>
	///		Clase con los datos de un proyecto
	/// </summary>
    public class ProjectModel : LibDataStructures.Base.BaseExtendedModel
    {
		// Constantes públicas con las extensiones de los archivos de proyecto
		public const string ReportExtension = ".schrpt";
		public const string ImportScriptExtension = ".schscr";
		public const string QueryExtension = ".schsql";
		public const string StyleExtension = ".schstl";

		/// <summary>
		///		Conexiones
		/// </summary>
		public Connections.ConnectionModelCollection Connections { get; } = new Connections.ConnectionModelCollection();

		/// <summary>
		///		Modos de distribución
		/// </summary>
		public Deployment.DeploymentModelCollection Deployments { get; } = new Deployment.DeploymentModelCollection();
    }
}
