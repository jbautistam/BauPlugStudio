using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibBlogReader.Model;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibBlogReader.Repository
{
	/// <summary>
	///		Repository para la estructura de blogs
	/// </summary>
	internal class BlogsStructureRepository
	{   
		// Constantes privadas
		private const string TagRoot = "Blogs";
		private const string TagFolder = "Folder";
		private const string TagBlog = "Blog";
		private const string TagID = "ID";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagPath = "Path";
		private const string TagUrl = "Url";
		private const string TagDownloadPodcast = "DownloadPodcast";
		private const string TagEnabled = "Enabled";
		private const string TagNumberNotRead = "NumberNotRead";
		private const string TagDateLastDownload = "DateLastDownload";
		private const string TagDateLastEntry = "DateLastEntry";

		/// <summary>
		///		Carga una estructura de carpetas
		/// </summary>
		internal FolderModel Load(string path)
		{
			FolderModel folder = new FolderModel();
			string fileName = GetFileName(path);
			MLFile fileML = new XMLParser().Load(fileName);

				// Si existe el archivo, lo carga
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagFolder)
									folder.Folders.Add(LoadFolder(path, folder, childML));
								else if (childML.Name == TagBlog)
									folder.Blogs.Add(LoadBlog(path, folder, childML));
				// Devuelve la carpeta cargada
				return folder;
		}

		/// <summary>
		///		Carga los datos de una carpeta de un nodo
		/// </summary>
		private FolderModel LoadFolder(string path, FolderModel parent, MLNode nodeML)
		{
			FolderModel folder = new FolderModel();

				// Carga los datos de la carpeta
				folder.Name = nodeML.Nodes[TagName].Value;
				folder.NumberNotRead = nodeML.Attributes[TagNumberNotRead].Value.GetInt(0);
				folder.Parent = parent;
				// Carga los hijos de la carpetas
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagFolder)
						folder.Folders.Add(LoadFolder(path, folder, childML));
					else if (childML.Name == TagBlog)
						folder.Blogs.Add(LoadBlog(path, folder, childML));
				// Devuelve la carpeta
				return folder;
		}

		/// <summary>
		///		Carga los datos de un blog
		/// </summary>
		private BlogModel LoadBlog(string path, FolderModel parent, MLNode nodeML)
		{
			BlogModel blog = new BlogModel();

				// Carga los datos
				blog.Folder = parent;
				blog.GlobalId = nodeML.Attributes[TagID].Value;
				blog.Name = nodeML.Nodes[TagName].Value;
				blog.Description = nodeML.Nodes[TagDescription].Value;
				blog.Path = nodeML.Nodes[TagPath].Value;
				blog.URL = nodeML.Nodes[TagUrl].Value;
				blog.NumberNotRead = nodeML.Attributes[TagNumberNotRead].Value.GetInt(0);
				blog.DateLastDownload = nodeML.Attributes[TagDateLastDownload].Value.GetDateTime();
				blog.DateLastEntry = nodeML.Attributes[TagDateLastEntry].Value.GetDateTime();
				blog.DownloadPodcast = nodeML.Attributes[TagDownloadPodcast].Value.GetBool();
				blog.Enabled = nodeML.Attributes[TagEnabled].Value.GetBool();
				// Asigna el lazy de entradas
				blog.LazyEntries.LazyData = new Lazy<EntriesModelCollection>(() => new EntryXmlRepository().Load(blog, path));
				// Devuelve los datos del blog
				return blog;
		}

		/// <summary>
		///		Graba una estructura de carpetas
		/// </summary>
		internal void Save(FolderModel folder, string path)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los nodos de las carpetas
				foreach (FolderModel child in folder.Folders)
					nodeML.Nodes.Add(GetNode(child));
				// Añade los nodos de los blogs
				foreach (BlogModel blog in folder.Blogs)
					nodeML.Nodes.Add(GetNode(blog));
				// Graba el archivo
				new XMLWriter().Save(GetFileName(path), fileML);
		}

		/// <summary>
		///		Obtiene el nodo XML de una carpeta
		/// </summary>
		private MLNode GetNode(FolderModel folder)
		{
			MLNode nodeML = new MLNode(TagFolder);

				// Añade los datos de la carpeta
				nodeML.Attributes.Add(TagNumberNotRead, folder.NumberNotRead);
				nodeML.Nodes.Add(TagName, folder.Name);
				// Añade los nodos de las carpetas
				foreach (FolderModel child in folder.Folders)
					nodeML.Nodes.Add(GetNode(child));
				// Añade los nodos de los blogs
				foreach (BlogModel blog in folder.Blogs)
					nodeML.Nodes.Add(GetNode(blog));
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nodo XML de un blog
		/// </summary>
		private MLNode GetNode(BlogModel blog)
		{
			MLNode nodeML = new MLNode(TagBlog);

				// Añade los datos del nodo
				nodeML.Attributes.Add(TagID, blog.GlobalId);
				nodeML.Attributes.Add(TagNumberNotRead, blog.NumberNotRead);
				nodeML.Attributes.Add(TagDateLastDownload, blog.DateLastDownload ?? DateTime.Now);
				nodeML.Attributes.Add(TagDateLastEntry, blog.DateLastEntry ?? DateTime.Now);
				nodeML.Attributes.Add(TagDownloadPodcast, blog.DownloadPodcast);
				nodeML.Attributes.Add(TagEnabled, blog.Enabled);
				nodeML.Nodes.Add(TagName, blog.Name);
				nodeML.Nodes.Add(TagDescription, blog.Description);
				nodeML.Nodes.Add(TagPath, blog.Path);
				nodeML.Nodes.Add(TagUrl, blog.URL);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nombre de archivo
		/// </summary>
		private string GetFileName(string path)
		{
			return System.IO.Path.Combine(path, "BlogsStructure.xml");
		}
	}
}
