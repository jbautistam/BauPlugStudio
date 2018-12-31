using System;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Colección de <see cref="FileSentencesModel"/>
	/// </summary>
	public class FileSentencesModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<FileSentencesModel>
	{
		/// <summary>
		///		Compacta una serie de archivos
		/// </summary>
		public FileSentencesModel Compact()
		{
			FileSentencesModel file = new FileSentencesModel();

				// Compacta los archivos
				foreach (FileSentencesModel source in this)
				{ 
					// Compacta los datos básicos
					file.Synonymous.AddRange(source.Synonymous);
					// Compacta la página y categoría
					file.CategoryDefinition.Compact(source.CategoryDefinition);
					file.PageDefinition.Compact(source.PageDefinition);
				}
				// Devuelve el archivo
				return file;
		}
	}
}
