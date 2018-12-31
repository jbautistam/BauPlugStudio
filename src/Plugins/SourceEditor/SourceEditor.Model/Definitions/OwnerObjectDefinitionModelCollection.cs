using System;
using System.Linq;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Colección de <see cref="OwnerObjectDefinitionModel"/>
	/// </summary>
	public class OwnerObjectDefinitionModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<OwnerObjectDefinitionModel>
	{
		/// <summary>
		///		Añade un objeto propietario
		/// </summary>
		public OwnerObjectDefinitionModel Add(string id, string text, string icon, bool isRootNode)
		{
			OwnerObjectDefinitionModel owner = new OwnerObjectDefinitionModel(id, text, icon, isRootNode);

				// Añade el objeto
				Add(owner);
				// Devuelve el objeto añadido
				return owner;
		}

		/// <summary>
		///		Obtiene recursivamente la definición del elemento
		/// </summary>
		public OwnerObjectDefinitionModel SearchRecursive(string id)
		{
			OwnerObjectDefinitionModel definition = Search(id);

				// Si no se ha encontrado la definición, la busca recursivamente
				if (definition == null)
					foreach (OwnerObjectDefinitionModel child in this)
						if (definition == null)
							definition = child.OwnerChilds.SearchRecursive(id);
				// Devuelve la definición
				return definition;
		}
	}
}
