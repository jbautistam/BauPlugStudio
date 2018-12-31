using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibBlogReader.Model
{
	/// <summary>
	///		Clase con los datos de una carpeta
	/// </summary>
	public class FolderModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Obtiene la carpeta principal
		/// </summary>
		public FolderModel GetRoot()
		{
			if (Parent == null)
				return this;
			else
				return Parent.GetRoot();
		}

		/// <summary>
		///		Obtiene recursivamente los blogs
		/// </summary>
		public BlogsModelCollection GetBlogsRecursive()
		{
			BlogsModelCollection blogs = new BlogsModelCollection();

				// Añade los blogs de la carpeta
				foreach (BlogModel blog in Blogs)
					blogs.Add(blog);
				// Añade recursivamente los blogs de las carpetas
				foreach (FolderModel folder in Folders)
					blogs.AddRange(folder.GetBlogsRecursive());
				// Devuelve la colección de blogs
				return blogs;
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public int GetNumberNotRead()
		{ 
			// Calcula el número de elementos no leídos
			NumberNotRead = Blogs.GetNumberNotRead() + Folders.GetNumberNotRead();
			// Y devuelve el valor
			return NumberNotRead;
		}

		/// <summary>
		///		Borra una carpeta
		/// </summary>
		public void Delete(FolderModel folder)
		{
			if (Folders.Search(folder.GlobalId) != null)
				Folders.RemoveByID(folder.GlobalId);
			else
				foreach (FolderModel childFolder in Folders)
					childFolder.Delete(folder);
		}

		/// <summary>
		///		Borra un blog
		/// </summary>
		public void Delete(BlogModel blog)
		{
			if (Blogs.Search(blog.GlobalId) != null)
				Blogs.RemoveByID(blog.GlobalId);
			else
				foreach (FolderModel childFolder in Folders)
					childFolder.Delete(blog);
		}

		/// <summary>
		///		Borra una entrada
		/// </summary>
		public bool Delete(EntryModel entry)
		{
			bool deleted = Blogs.Delete(entry);

				// Borra la entrada de las carpetas
				if (!deleted)
					deleted = Folders.Delete(entry);
				// Devuelve el valor que indica si se ha borrado
				return deleted;
		}

		/// <summary>
		///		Número de elementos no leídos
		/// </summary>
		public int NumberNotRead { get; set; }

		/// <summary>
		///		Carpeta padre
		/// </summary>
		public FolderModel Parent { get; set; }

		/// <summary>
		///		Carpetas contenidas en esta carpeta
		/// </summary>
		public FoldersModelCollection Folders { get; } = new FoldersModelCollection();

		/// <summary>
		///		Blogs de esta carpeta
		/// </summary>
		public BlogsModelCollection Blogs { get; } = new BlogsModelCollection();

		/// <summary>
		///		Nombre completo de la carpeta
		/// </summary>
		public string FullName
		{
			get
			{
				string fullName = Name;

					// Recoge el nombre de la carpeta padre
					if (Parent != null && !Parent.Name.IsEmpty())
						fullName = Parent.FullName + "\\" + fullName;
					// Devuelve el nombre completo
					return fullName;
			}
		}
	}
}
