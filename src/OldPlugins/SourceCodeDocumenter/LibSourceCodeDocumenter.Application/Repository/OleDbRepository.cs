using System;

using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="Model.OleDbModel"/>
	/// </summary>
	internal class OleDbRepository
	{   
		// Constantes privadas
		private const string TagRoot = "OleDb";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagConnectionString = "ConnectionString";

		/// <summary>
		///		Carga los datos de documentación
		/// </summary>
		internal Model.OleDbModel Load(string fileName)
		{
			Model.OleDbModel oleDb = new Model.OleDbModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos de documentación
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name.Equals(TagRoot))
						{
							oleDb.Name = nodeML.Nodes[TagName].Value;
							oleDb.Description = nodeML.Nodes[TagDescription].Value;
							oleDb.ConnectionString = nodeML.Nodes[TagConnectionString].Value;
						}
				// Devuelve los datos de documentación
				return oleDb;
		}

		/// <summary>
		///		Graba los datos de una conexión
		/// </summary>
		internal void Save(Model.OleDbModel oleDb, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los datos de generación
				nodeML.Nodes.Add(TagName, oleDb.Name);
				nodeML.Nodes.Add(TagDescription, oleDb.Description);
				nodeML.Nodes.Add(TagConnectionString, oleDb.ConnectionString);
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}
	}
}