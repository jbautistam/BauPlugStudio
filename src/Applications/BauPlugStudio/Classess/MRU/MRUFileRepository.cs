using System;

using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Applications.BauPlugStudio.Classess.MRU
{
	/// <summary>
	///		Repository de <see cref="MRUFileModel"/>
	/// </summary>
	internal class MRUFileRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Files";
		private const string TagMRU = "MRU";
		private const string TagSource = "Source";
		private const string TagFileName = "FileName";
		private const string TagText = "Text";

		/// <summary>
		///		Carga una colección de archivos
		/// </summary>
		internal MRUFileModelCollection Load()
		{
			MRUFileModelCollection recentFilesUsed = new MRUFileModelCollection();
			MLFile fileML = new XMLParser().Load(GetFileName());

				// Carga los archivos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagMRU)
									recentFilesUsed.Add(childML.Nodes [TagSource].Value,
														childML.Nodes [TagFileName].Value,
														childML.Nodes [TagText].Value);
				// Devuelve los archivos
				return recentFilesUsed;
		}

		/// <summary>
		///		Graba una colección de archivos
		/// </summary>
		internal void Save(MRUFileModelCollection objColRecentFilesUsed)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Crea los nodos
				foreach (MRUFileModel file in objColRecentFilesUsed)
				{
					MLNode childML = nodeML.Nodes.Add(TagMRU);

						// Añade las propiedades del archivo
						childML.Nodes.Add(TagSource, file.Source);
						childML.Nodes.Add(TagFileName, file.FileName);
						childML.Nodes.Add(TagText, file.Name);
				}
				// Crea el directorio y graba el archivo
				new XMLWriter().Save(GetFileName(), fileML);
		}

		/// <summary>
		///		Obtiene el nombre de archivo
		/// </summary>
		private string GetFileName()
		{
			return System.IO.Path.Combine(Globals.HostController.HostViewModelController.Configuration.PathBaseData, "MRUFiles.xml");
		}
	}
}
