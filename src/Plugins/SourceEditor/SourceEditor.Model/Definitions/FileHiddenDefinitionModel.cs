using System;

namespace Bau.Libraries.SourceEditor.Model.Definitions
{
	/// <summary>
	///		Definición de un archivo oculto
	/// </summary>
	public class FileHiddenDefinitionModel : AbstractFileDefinitionModel
	{
		public FileHiddenDefinitionModel(ProjectDefinitionModel definition, string extension,
										 OpenMode mode = OpenMode.Owner)
								: base(definition, null, extension, null, false, mode)
		{
		}
	}
}
