using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Colección de <see cref="OwnerChildModel"/>
	/// </summary>
	public class OwnerChildModelCollection : LibDataStructures.Base.BaseModelCollection<OwnerChildModel>
	{
		/// <summary>
		///		Añade un elemento a la colección
		/// </summary>
		public OwnerChildModel Add(string id, FileModel parent, string text,
								   Definitions.OwnerObjectDefinitionModel ownerObjectDefinition,
								   bool hasChilds)
		{
			OwnerChildModel child = new OwnerChildModel(id, parent, text, ownerObjectDefinition, hasChilds);

				// Añade el objeto
				Add(child);
				// ... y lo devuelve
				return child;
		}
	}
}
