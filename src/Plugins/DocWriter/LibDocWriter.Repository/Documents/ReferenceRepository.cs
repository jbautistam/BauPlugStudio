using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Repository.Documents
{
	/// <summary>
	///		Repository de <see cref="ReferenceModel"/>
	/// </summary>
	public class ReferenceRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Reference";
		private const string TagPathProject = "PathProject";
		private const string TagFileName = "FileName";

		/// <summary>
		///		Carga los datos de la referencia
		/// </summary>
		public ReferenceModel Load(Model.Solutions.FileModel file)
		{
			ReferenceModel reference = new ReferenceModel(file);

				// Carga el archivo si existe
				if (System.IO.File.Exists(file.DocumentFileName))
				{
					MLFile fileML = new XMLParser().Load(file.DocumentFileName);

						if (fileML != null)
							foreach (MLNode nodeML in fileML.Nodes)
								if (nodeML.Name == TagRoot)
									foreach (MLNode childML in nodeML.Nodes)
									{
										reference.ProjectName = nodeML.Nodes[TagPathProject].Value;
										reference.FileNameReference = nodeML.Nodes[TagFileName].Value;
									}
				}
				// Devuelve la referencia
				return reference;
		}

		/// <summary>
		///		Graba los datos de una referencia
		/// </summary>
		public void Save(ReferenceModel reference)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade el nombre de archivo
				nodeML.Nodes.Add(TagPathProject, reference.ProjectName);
				nodeML.Nodes.Add(TagFileName, reference.FileNameReference);
				// Graba el archivo
				new XMLWriter().Save(reference.File.FullFileName, fileML);
		}
	}
}
