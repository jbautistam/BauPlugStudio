using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Clase abstracta para los elementos de un proyecto
	/// </summary>
	public abstract class AbstractSolutionItemModel<TypeDefinition> : LibDataStructures.Base.BaseExtendedModel
														 where TypeDefinition : Definitions.AbstractDefinitionModel
	{
		public AbstractSolutionItemModel(TypeDefinition definition, string name, bool hasChild)
		{
			Definition = definition;
			Name = name;
			HasChilds = hasChild;
		}

		/// <summary>
		///		Indica si el elemento tiene hijos
		/// </summary>
		public bool HasChilds { get; protected set; }

		/// <summary>
		///		Definición que se aplica al elemento
		/// </summary>
		public TypeDefinition Definition { get; }
	}
}
