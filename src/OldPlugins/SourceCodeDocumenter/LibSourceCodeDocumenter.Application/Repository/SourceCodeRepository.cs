using System;

using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="SourceCodeModel"/>
	/// </summary>
	internal class SourceCodeRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Source";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagSourceFileName = "FileName";

		/// <summary>
		///		Carga los datos de documentación
		/// </summary>
		internal Model.SourceCodeModel Load(string fileName)
		{
			Model.SourceCodeModel sourceCode = new Model.SourceCodeModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos de documentación
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot))
						{
							sourceCode.Name = nodeML.Nodes[TagName].Value;
							sourceCode.Description = nodeML.Nodes[TagDescription].Value;
							sourceCode.SourceFileName = nodeML.Nodes[TagSourceFileName].Value;
						}
				// Devuelve los datos de documentación
				return sourceCode;
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		internal void Save(Model.SourceCodeModel sourceCode, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los datos de generación
				nodeML.Nodes.Add(TagName, sourceCode.Name);
				nodeML.Nodes.Add(TagDescription, sourceCode.Description);
				nodeML.Nodes.Add(TagSourceFileName, sourceCode.SourceFileName);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}