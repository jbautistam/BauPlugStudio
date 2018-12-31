using System;
using System.Collections.Generic;

using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibDataBaseStudio.Application.Repository
{
	/// <summary>
	///		Base para los elementos de una conexión
	/// </summary>
	internal abstract class ConnectionItemRepositoryBase
	{   
		// Constantes privadas
		private const string TagConnectionGuid = "GuidConnection";

		/// <summary>
		///		Carga los GUID de las conexiones del archivo
		/// </summary>
		internal List<string> LoadConnections(MLNode nodeML)
		{
			List<string> guids = new List<string>();

				// Añade los Guid encontrados en el archivo a la lista
				foreach (MLNode connectionML in nodeML.Nodes)
					if (connectionML.Name == TagConnectionGuid)
						guids.Add(connectionML.Value);
				// Devuelve la conexión
				return guids;
		}

		/// <summary>
		///		Obtiene los nodos de una conexión
		/// </summary>
		internal MLNodesCollection GetConnectionsNodes(Model.Base.ConnectionItemBase connectionItem)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los nodos
				foreach (string connection in connectionItem.ConnectionsGuid)
					nodesML.Add(TagConnectionGuid, connection);
				// Devuelve la colección de nodos
				return nodesML;
		}
	}
}
