using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="LibraryModel"/>
	/// </summary>
	public class LibraryBussiness
	{
		/// <summary>
		///		Carga las bibliotecas de un directorio
		/// </summary>
		public LibraryModelCollection Load(string path)
		{
			LibraryModelCollection libraries = new LibraryModelCollection();

				// Carga los directorios si existen
				if (Directory.Exists(path))
				{
					string[] paths = Directory.GetDirectories(path);

						// Añade los directorios
						foreach (string child in paths)
						{
							LibraryModel library = new LibraryModel();

								// Asigna los datos
								library.Path = child;
								// Añade la biblioteca
								libraries.Add(library);
						}
				}
				// Devuelve las librerías
				return libraries;
		}

		/// <summary>
		///		Graba una librería
		/// </summary>
		public void Save(string pathParent, LibraryModel oldLibrary, string strNewName)
		{
			if (oldLibrary.PathName.IsEmpty())
				LibCommonHelper.Files.HelperFiles.MakePath(Path.Combine(pathParent, strNewName));
			else if (!oldLibrary.PathName.EqualsIgnoreCase(strNewName))
				LibCommonHelper.Files.HelperFiles.Rename(oldLibrary.Path,
														 Path.Combine(Path.GetDirectoryName(oldLibrary.Path), strNewName));
		}

		/// <summary>
		///		Elimina una biblioteca
		/// </summary>
		public void Delete(LibraryModel library)
		{
			LibCommonHelper.Files.HelperFiles.KillPath(library.Path);
		}
	}
}
