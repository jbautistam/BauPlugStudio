using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="SchemaConnectionModel"/>
	/// </summary>
	public class SourceCodeBussiness
	{
		/// <summary>
		///		Carga una conexión
		/// </summary>
		public Model.SourceCodeModel Load(string fileName)
		{
			return new Repository.SourceCodeRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		public void Save(Model.SourceCodeModel sourceCode, string fileName)
		{
			new Repository.SourceCodeRepository().Save(sourceCode, fileName);
		}
	}
}
