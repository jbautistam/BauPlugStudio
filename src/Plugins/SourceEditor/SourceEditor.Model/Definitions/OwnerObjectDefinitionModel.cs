using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Definición de los objetos propietarios
	/// </summary>
	public class OwnerObjectDefinitionModel : AbstractDefinitionModel
	{
		public OwnerObjectDefinitionModel(string id, string text, string icon, bool isRootNode) : base(id, icon)
		{
			GlobalId = id;
			Name = text;
			IsRootNode = isRootNode;
		}

		/// <summary>
		///		Indica si es un nodo raíz
		/// </summary>
		public bool IsRootNode { get; }
	}
}
