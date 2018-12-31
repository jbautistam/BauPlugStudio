using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDataBaseStudio.Model.Base
{
	/// <summary>
	///		Clase base para los elementos con conexiones asociadas
	/// </summary>
	public abstract class ConnectionItemBase : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Comprueba si una conexión está asociada
		/// </summary>
		public bool ExistsConnection(Connections.SchemaConnectionModel connection)
		{
			// Comprueba si la conexión está asociada en el documentador
			foreach (string strGlobalId in ConnectionsGuid)
				if (strGlobalId.EqualsIgnoreCase(connection.GlobalId))
					return true;
			// Si ha llegado hasta aquí es porque no existe
			return false;
		}

		/// <summary>
		///		Archivos de conexión
		/// </summary>
		public System.Collections.Generic.List<string> ConnectionsGuid { get; } = new System.Collections.Generic.List<string>();
	}
}
