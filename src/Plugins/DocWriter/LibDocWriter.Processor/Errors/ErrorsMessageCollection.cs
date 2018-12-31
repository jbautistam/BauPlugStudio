using System;
using System.Collections.Generic;

using Bau.Libraries.NhamlCompiler.Errors;

namespace Bau.Libraries.LibDocWriter.Processor.Errors
{
	/// <summary>
	///		Colección de <see cref="ErrorMessage"/>
	/// </summary>
	public class ErrorsMessageCollection : List<ErrorMessage>
	{
		/// <summary>
		///		Añade una serie de errores
		/// </summary>
		public void AddRange(string fileName, CompilerErrorsCollection errors)
		{
			foreach (CompilerError error in errors)
				Add(fileName, error);
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		public void Add(string fileName, CompilerError compilerError)
		{
			ErrorMessage error = new ErrorMessage();

				// Asigna los datos
				error.FileName = fileName;
				error.Token = compilerError.Token;
				error.Message = compilerError.Description;
				error.Row = compilerError.Row;
				error.Column = compilerError.Column;
				// Devuelve el mensaje
				Add(error);
		}

		/// <summary>
		///		Añade un error genérico
		/// </summary>
		public void Add(string fileName, string message)
		{
			Add(fileName, new CompilerError
								{
									Token = "General",
									Description = message,
									Row = 0,
									Column = 0
								}
				);
		}
	}
}
