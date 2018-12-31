using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Composición con los archivos índice (anterior, posterior y superior) de una página
	/// </summary>
	internal class FilesIndexComposition
	{
		/// <summary>
		///		Normaliza los nombres de los documentos
		/// </summary>
		internal void Normalize()
		{ 
			// Normaliza la página superior
			PageTop = NormalizeFile(PageTop);
			// Normaliza el breadcrumb
			if (FilesBreadCrumb != null)
				for (int index = FilesBreadCrumb.Count - 1; index >= 0; index--)
					FilesBreadCrumb [index] = NormalizeFile(FilesBreadCrumb [index]);
		}

		/// <summary>
		///		Crea un archivo nuevo clonando con el título real
		/// </summary>
		private FileTargetModel NormalizeFile(FileTargetModel file)
		{
			FileTargetModel newFile = null;

				// Si realmente hay un archivo origen
				if (file != null)
				{ 
					// Crea el nuevo objeto
					newFile = file.Clone();
					// Cambia el título
					if (!file.PreviousTitle.IsEmpty())
						newFile.Title = file.PreviousTitle;
				}
				// Devuelve el nuevo archivo
				return newFile;
		}

		/// <summary>
		///		Página superior
		/// </summary>
		internal FileTargetModel PageTop { get; set; }

		/// <summary>
		///		Página anterior
		/// </summary>
		internal FileTargetModel PagePrevious { get; set; }

		/// <summary>
		///		Página siguiente
		/// </summary>
		internal FileTargetModel PageNext { get; set; }

		/// <summary>
		///		Páginas relacionadas
		/// </summary>
		internal System.Collections.Generic.List<DocumentTargetModel> PagesRelated { get; set; }

		/// <summary>
		///		Archivos padre
		/// </summary>
		internal FileTargetModelCollection FilesBreadCrumb { get; set; }
	}
}
