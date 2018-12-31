using System;

namespace Bau.Libraries.DatabaseStudio.Application.EventArguments
{
	/// <summary>
	///		Argumentos del evento de progreso
	/// </summary>
    public class ProgressEventArgs : EventArgs
    {
		public ProgressEventArgs(Models.Deployment.DeploymentModel deployment, string fileName, string message, int actual, int total, bool isError)
		{
			Deployment = deployment;
			FileName = fileName;
			Message = message;
			IsError = isError;
			Actual = actual;
			Total = total;
		}

		/// <summary>
		///		Datos de la distribución
		/// </summary>
		public Models.Deployment.DeploymentModel Deployment { get; }

		/// <summary>
		///		Nombre de archivo que se está procesando
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; }

		/// <summary>
		///		Indica si es un mensaje de error
		/// </summary>
		public bool IsError { get; }

		/// <summary>
		///		Progreso actual
		/// </summary>
		public int Actual { get; }

		/// <summary>
		///		Total de elementos
		/// </summary>
		public int Total { get; }
	}
}
