using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Colección de <see cref="FileTargetModel"/>
	/// </summary>
	internal class FileTargetModelCollection : System.Collections.Generic.List<FileTargetModel>
	{
		internal FileTargetModelCollection(Generator processor)
		{
			Processor = processor;
			FileBussinessManager = new FileBussiness();
		}

		/// <summary>
		///		Añade un archivo a la colección
		/// </summary>
		internal FileTargetModel Add(string fileSource, string pathTarget, string fileTarget)
		{
			FileTargetModel target = new FileTargetModel(new FileModel(Processor.Project, fileSource), pathTarget, fileTarget);

				// Asigna los datos
				target.DateUpdate = GetDateCreation(fileSource);
				// Añade el archivo a la colección
				Add(target);
				// Devuelve el archivo creado
				return target;
		}

		/// <summary>
		///		Obtiene la fecha de creación de un archivo
		/// </summary>
		private DateTime GetDateCreation(string fileSource)
		{
			if (File.Exists(fileSource))
			{
				FileInfo fileInfo = new FileInfo(fileSource);

				return fileInfo.CreationTime;
			}
			else
				return DateTime.Now.AddMinutes(5);
		}

		#if DEBUG
		/// <summary>
		///		Imprime los datos de depuración
		/// </summary>
		internal void Debug()
		{
			foreach (FileTargetModel file in this)
			{
				System.Diagnostics.Debug.WriteLine($"{file.File.DocumentFileName} \r\n\t\t{file.GetFullFileName(Processor)}");
				if (file.File.FileType == FileModel.DocumentType.Document)
				{
					System.Diagnostics.Debug.WriteLine("  --> Final: " + file.GetFullFileNameCompiledTemporal(Processor));
					System.Diagnostics.Debug.WriteLine("  --> DocumentFileName: " + file.DocumentFileName);
					System.Diagnostics.Debug.WriteLine("  --> RelativeFullFileNameTarget: " + file.RelativeFullFileNameTarget);
				}
			}
		}
		#endif

		/// <summary>
		///		Procesador al que se asocia la colección de archivos
		/// </summary>
		internal Generator Processor { get; }

		/// <summary>
		///		Objeto de negocio de archivos
		/// </summary>
		private FileBussiness FileBussinessManager { get; set; }

		/// <summary>
		///		Obtiene los archivos hijos de un directorio
		/// </summary>
		internal FileTargetModelCollection GetChildsCategory(FileTargetModel category, bool isRecursive)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);

				// Recorre los archivos buscando los hijos de un directorio
				foreach (FileTargetModel file in this)
					if (file.File.FileType == FileModel.DocumentType.Document &&
							IsChild(category.File, file.File, isRecursive) &&
							file.Page == 0)
						files.Add(file);
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene los archivos hijo de una colección
		/// </summary>
		internal FileTargetModelCollection GetChilds(FilesModelCollection files, bool isRecursive, bool onlyRss)
		{
			FileTargetModelCollection childs = new FileTargetModelCollection(Processor);

				// Recorre los archivos añadiendo los hijos
				foreach (FileModel file in files)
					childs.AddRange(GetChilds(file, isRecursive, onlyRss));
				// Elimina los duplicados
				childs.RemoveDuplicates();
				// Devuelve la colección
				return childs;
		}

		/// <summary>
		///		Obtiene los hijos de un archivo
		/// </summary>
		internal FileTargetModelCollection GetChilds(FileModel fileParent, bool isRecursive, bool onlyRss)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);

				// Recorre los archivos buscando los hijos de un directorio
				foreach (FileTargetModel file in this)
					if (file.File.FileType == FileModel.DocumentType.Document &&
							IsChild(fileParent, file.File, isRecursive) &&
							(!onlyRss || file.ShowAtRss))
						files.Add(file);
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Comprueba si un archivo es hijo de otro
		/// </summary>
		private bool IsChild(FileModel fileParent, FileModel file, bool isRecursive)
		{
			return !fileParent.FullFileName.EqualsIgnoreCase(file.FullFileName) &&
						   (fileParent.Path.EqualsIgnoreCase(Path.GetDirectoryName(file.Path)) ||
							  (isRecursive && file.Path.StartsWith(fileParent.Path, StringComparison.CurrentCultureIgnoreCase)));
		}

		/// <summary>
		///		Obtiene los hijos de un documento de mapa de sitio
		/// </summary>
		internal FileTargetModelCollection GetChildsSitemap(FileTargetModel file, DocumentModel document)
		{
			FileTargetModelCollection files = GetChilds(document.ChildPages, true, false);

				// Elimina las páginas internas de categoría
				for (int index = files.Count - 1; index >= 0; index--)
					if (files [index].Page > 0)
						files.RemoveAt(index);
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene los archivos hijo de una etiqueta
		/// </summary>
		internal FileTargetModelCollection GetChildsTag(FileTargetModel file, DocumentModel document)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);
			Application.Bussiness.Documents.DocumentBussiness bussiness = new Application.Bussiness.Documents.DocumentBussiness();

				// Obtiene los documentos que tengan la etiqueta asociada
				foreach (FileTargetModel child in this)
					if (child.File.FileType == FileModel.DocumentType.Document)
					{
						DocumentModel documentChild = bussiness.Load(Processor.Project,
																	 child.GetFullFileNameCompiledShort(Processor));
						bool found = false;

							// Recorre las etiquetas comprobando si se asocian a esta etiqueta
							foreach (FileModel tagDocument in documentChild.Tags)
								if (tagDocument.Path.EqualsIgnoreCase(file.File.Path))
									found = true;
							// Si pertenece a esta etiqueta, se añade
							if (found)
								files.Add(child);
					}
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección
				return files;
		}

		/// <summary>
		///		Obtiene los hermanos de un archivo
		/// </summary>
		internal FileTargetModelCollection GetSiblings(FileTargetModel file, bool onlyRss)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);

				// Recorre los archivos buscando los que se encuentren en el mismo directorio
				foreach (FileTargetModel sibling in this)
					if (sibling.File.FileType == FileModel.DocumentType.Document)
						if (IsSibling(sibling, file))
							files.Add(sibling);
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Comprueba si dos elementos son hermanos
		/// </summary>
		private bool IsSibling(FileTargetModel first, FileTargetModel second)
		{
			return !first.File.FullFileName.EqualsIgnoreCase(second.File.FullFileName) &&
							  GetPathCheck(first.PathTarget).EqualsIgnoreCase(GetPathCheck(second.PathTarget));
		}

		/// <summary>
		///		Obtiene un directorio comprobando antes si está vacío el inicial
		/// </summary>
		private string GetPathCheck(string path)
		{
			if (path.IsEmpty())
				return "###";
			else
				return Path.GetDirectoryName(path);
		}

		/// <summary>
		///		Obtiene una colección con una "rodaja" de elementos
		/// </summary>
		internal FileTargetModelCollection Slice(int start, int intNumber)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);

				// Añade los archivos
				for (int index = start; index < start + intNumber; index++)
					if (index < Count)
						files.Add(this [index]);
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene la página anterior a un archivo
		/// </summary>
		internal FileTargetModel GetPreviousPage(FileTargetModel file)
		{
			FileTargetModel objPrevious = null;

			// Busca el elemento
			foreach (FileTargetModel target in this)
				if (target.File.FileType == FileModel.DocumentType.Document &&
						target.ShowMode == DocumentModel.ShowChildsMode.None)
				{
					if (target.DateUpdate < file.DateUpdate &&
							  (objPrevious == null || target.DateUpdate > objPrevious.DateUpdate))
						objPrevious = target;
				}
			// Devuelve el objeto
			return objPrevious;
		}

		/// <summary>
		///		Obtiene la página siguiente a un archivo
		/// </summary>
		internal FileTargetModel GetNextPage(FileTargetModel file)
		{
			FileTargetModel nextPage = null;

				// Busca el elemento
				foreach (FileTargetModel target in this)
					if (target.File.FileType == FileModel.DocumentType.Document &&
							target.ShowMode == DocumentModel.ShowChildsMode.None)
					{
						if (target.DateUpdate > file.DateUpdate &&
								  (nextPage == null || target.DateUpdate < nextPage.DateUpdate))
							nextPage = target;
					}
				// Devuelve el objeto
				return nextPage;
		}

		/// <summary>
		///		Obtiene la colección de páginas relacionadas
		/// </summary>
		internal FileTargetModelCollection GetPagesRelated(FileTargetModel file, DocumentModel document)
		{
			FileTargetModelCollection files = new FileTargetModelCollection(Processor);

				// Si el archivo tiene páginas relacionadas, las añade, si no, busca las páginas más cercanas
				// de la categoría
				if (document.ChildPages != null && document.ChildPages.Count > 0)
				{
					foreach (FileModel child in document.ChildPages)
						foreach (FileTargetModel target in this)
							if (child.FullFileName == target.File.FullFileName)
								files.Add(target);
				}
				else
				{
					// Añade los documentos de la categoría
					files = GetSiblings(file, true);
					// Quita archivos hasta quedarse con los más cercanos en fecha
					files = files.SliceByDate(file.DateUpdate, 10);
				}
				// Elimina los duplicados
				files.RemoveDuplicates();
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Obtiene los archivos más cercanos a una fecha
		/// </summary>
		private FileTargetModelCollection SliceByDate(DateTime pivot, int numberFiles)
		{
			int indexMedium = 0;

				// Ordena la colección de forma ascendente
				SortByDate(true);
				// Obtiene el índice del archivo más cercano a la fecha
				for (int index = 1; index < Count; index++)
					if (Math.Abs((this [index].DateUpdate - pivot).TotalSeconds) < Math.Abs((this[indexMedium].DateUpdate - pivot).TotalSeconds))
						indexMedium = index;
				// Obtiene el índice inferior a partir del índice medio
				indexMedium -= numberFiles / 2;
				if (indexMedium < 0)
					indexMedium = 0;
				// Normaliza el índice para coger el número máximo de archivos posibles
				while (indexMedium > 0 && indexMedium + numberFiles > Count)
					indexMedium--;
				// Devuelve la colección cortada
				return Slice(indexMedium, numberFiles);
		}

		/// <summary>
		///		Obtiene el archivo padre de un archivo
		/// </summary>
		internal FileTargetModel GetParent(FileTargetModel file)
		{
			// Recorre la colección
			foreach (FileTargetModel parent in this)
				if (parent.File.FileType == FileModel.DocumentType.Document &&
						IsChild(parent.File, file.File, false))
					return parent;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Obtiene la colección de padres de un archivo
		/// </summary>
		internal FileTargetModelCollection GetParents(FileTargetModel file)
		{
			FileTargetModelCollection parents = new FileTargetModelCollection(Processor);

				// Añade los padres
				foreach (FileTargetModel parent in this)
					if (parent.File.FileType == FileModel.DocumentType.Document &&
							IsChild(parent.File, file.File, true) &&
							parent.Page == 0)
						parents.Add(parent);
				// Ordena por nombres
				parents.SortByPath(true);
				// Elimina los duplicados
				parents.RemoveDuplicates();
				// Devuelve la colección de padres
				return parents;
		}

		/// <summary>
		///		Elimina los duplicados de una colección
		/// </summary>
		private void RemoveDuplicates()
		{
			for (int bottom = Count - 2; bottom >= 0; bottom--)
				for (int top = Count - 1; top > bottom; top--)
					if (this [top].RelativeFullFileNameTarget.EqualsIgnoreCase(this [bottom].RelativeFullFileNameTarget))
						RemoveAt(top);
		}

		/// <summary>
		///		Ordena los elementos por fecha
		/// </summary>
		internal void SortByDate(bool ascending)
		{
			Sort((first, second) => (ascending ? 1 : -1) * first.DateUpdate.CompareTo(second.DateUpdate));
		}

		/// <summary>
		///		Ordena los elementos por directorio
		/// </summary>
		internal void SortByPath(bool ascending)
		{
			Sort((first, second) => (ascending ? 1 : -1) * first.PathTarget.CompareTo(second.PathTarget));
		}

		/// <summary>
		///		Ordena los elementos por directorio
		/// </summary>
		internal void SortByFullFileName(bool ascending)
		{
			Sort((first, second) => (ascending ? 1 : -1) * first.File.FullFileName.CompareTo(second.File.FullFileName));
		}

		/// <summary>
		///		Ordena los elementos por nombre
		/// </summary>
		internal void SortByName(bool ascending)
		{
			Sort((first, second) => (ascending ? 1 : -1) * first.FileNameTarget.CompareTo(second.FileNameTarget));
		}
	}
}
