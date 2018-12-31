using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Modelo con los datos de una biblioteca
	/// </summary>
	public class LibraryModel : LibDataStructures.Base.BaseModel
	{
		/// <summary>
		///		Nombre del directorio
		/// </summary>
		public string PathName
		{
			get { return System.IO.Path.GetFileName(Path); }
		}

		/// <summary>
		///		Directorio
		/// </summary>
		public string Path { get; set; }
	}
}
