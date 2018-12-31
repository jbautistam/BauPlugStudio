using System;

namespace Bau.Libraries.SourceEditor.Application
{
	/// <summary>
	///		Manager para SourceEditor
	/// </summary>
	public class SourceEditorManager
	{
		public SourceEditorManager(string pathData)
		{
			PathData = pathData;
		}

		/// <summary>
		///		Directorio de datos
		/// </summary>
		public string PathData { get; set; }
	}
}
