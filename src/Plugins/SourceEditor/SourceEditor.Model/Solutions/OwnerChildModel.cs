using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Definición de objeto hijo de una definición de archivo
	/// </summary>
	public class OwnerChildModel : AbstractSolutionItemModel<Definitions.OwnerObjectDefinitionModel>
	{
		public OwnerChildModel(string id, FileModel parent, string text, Definitions.OwnerObjectDefinitionModel ownerObjectDefinition,
													 bool hasChilds)
								: base(ownerObjectDefinition, id, hasChilds)
		{
			Parent = parent;
			GlobalId = id;
			Text = text;
			OwnerObjectDefinition = ownerObjectDefinition;
		}

		/// <summary>
		///		Definición de objeto propietario
		/// </summary>
		public Definitions.OwnerObjectDefinitionModel OwnerObjectDefinition { get; }

		/// <summary>
		///		Archivo padre
		/// </summary>
		public FileModel Parent { get; }

		/// <summary>
		///		Texto
		/// </summary>
		public string Text { get; }

		/// <summary>
		///		Objetos hijo
		/// </summary>
		public OwnerChildModelCollection ObjectChilds { get; } = new OwnerChildModelCollection();
	}
}
