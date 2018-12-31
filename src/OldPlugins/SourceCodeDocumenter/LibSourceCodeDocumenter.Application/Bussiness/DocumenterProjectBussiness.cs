using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="Model.DocumenterProjectModel"/>
	/// </summary>
	public class DocumenterProjectBussiness
	{
		/// <summary>
		///		Carga un archivo
		/// </summary>
		public Model.DocumenterProjectModel Load(string fileName)
		{
			return new Repository.DocumenterProjectRepository().Load(fileName);
		}

		/// <summary>
		///		Graba un archivo de proyecto
		/// </summary>
		public void Save(Model.DocumenterProjectModel project, string fileName)
		{
			new Repository.DocumenterProjectRepository().Save(project, fileName);
		}
	}
}
