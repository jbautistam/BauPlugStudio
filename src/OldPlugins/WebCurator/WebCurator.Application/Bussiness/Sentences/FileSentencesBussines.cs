using System;

using Bau.Libraries.WebCurator.Model.Sentences;

namespace Bau.Libraries.WebCurator.Application.Bussiness.Sentences
{
	/// <summary>
	///		Clase de negocio de <see cref="FileSentecesModel"/>
	/// </summary>
	public class FileSentencesBussines
	{
		/// <summary>
		///		Carga los archivos de sentencias de un directorio
		/// </summary>
		public FileSentencesModelCollection LoadAll(string path)
		{
			FileSentencesModelCollection files = new FileSentencesModelCollection();

				// Carga los archivos
				if (System.IO.Directory.Exists(path))
				{
					string[] pathFiles = System.IO.Directory.GetFiles(path, "*" + FileSentencesModel.Extension);

						// Añade los archivos
						foreach (string pathFile in pathFiles)
						{
							FileSentencesModel file = new FileSentencesModel();

								// Asigna las propiedades
								file.FileName = pathFile;
								// Añade el archivo a la colección
								files.Add(file);
						}
				}
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Carga los datos de un archivo de frases
		/// </summary>
		public FileSentencesModel Load(string fileName)
		{
			return new Repository.Sentences.FileSentencesRepository().Load(fileName);
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		public void Delete(FileSentencesModel file)
		{
			LibCommonHelper.Files.HelperFiles.KillFile(file.FileName);
		}
	}
}
