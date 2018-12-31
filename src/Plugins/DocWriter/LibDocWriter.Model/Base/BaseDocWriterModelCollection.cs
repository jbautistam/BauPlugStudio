using System;

namespace Bau.Libraries.LibDocWriter.Model.Base
{
	/// <summary>
	///		Clase base para las colecciones de <see cref="BaseDocWriterModel"/>
	/// </summary>
	public class BaseDocWriterModelCollection<TypeData> : LibDataStructures.Base.BaseExtendedModelCollection<TypeData> where TypeData : BaseDocWriterModel
	{ 
	}
}