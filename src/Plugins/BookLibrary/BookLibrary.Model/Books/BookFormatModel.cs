using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Formato reconocido de un libro
	/// </summary>
	public class BookFormatModel : LibDataStructures.Base.BaseExtendedModel
	{   
		// Variables privadas
		private string _extension;

		/// <summary>
		///		Extensión del libro
		/// </summary>
		public string Extension
		{
			get { return _extension; }
			set
			{
				_extension = value;
				if (!string.IsNullOrEmpty(_extension))
				{
					_extension = _extension.Trim();
					if (!_extension.StartsWith("."))
						_extension = "." + _extension;
				}
			}
		}

		/// <summary>
		///		Formato del libro
		/// </summary>
		public BookModel.BookType BookType { get; set; }
	}
}
