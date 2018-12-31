using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.LibBlogReader.Model;

namespace Bau.Libraries.LibBlogReader.Repository
{
	/// <summary>
	///		Repository para las entradas de un blog
	/// </summary>
	internal class EntryXmlRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Entries";
		private const string TagEntry = "Entry";
		private const string TagDatePublish = "Publish";
		private const string TagStatus = "Status";
		private const string TagName = "Name";
		private const string TagUrl = "Url";
		private const string TagUrlEnclosure = "UrlEnclosure";
		private const string TagDownloadFileName = "DownloadFileName";
		private const string TagContent = "Content";
		private const string TagAuthor = "Author";

		/// <summary>
		///		Carga las entradas de un blog
		/// </summary>
		internal EntriesModelCollection Load(BlogModel blog, string path)
		{
			EntriesModelCollection entries = new EntriesModelCollection();
			MLFile fileML = new XMLParser().Load(GetFileName(blog, path));

				// Obtiene las entradas
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagEntry)
									entries.Add(LoadEntry(blog, childML));
				// Devuelve las entradas
				return entries;
		}

		/// <summary>
		///		Carga los datos de una entrada
		/// </summary>
		private EntryModel LoadEntry(BlogModel blog, MLNode nodeML)
		{
			EntryModel entry = new EntryModel();

				// Carga los datos de la entrada
				entry.Blog = blog;
				entry.DatePublish = nodeML.Attributes[TagDatePublish].Value.GetDateTime() ?? DateTime.Now;
				entry.Status = (EntryModel.StatusEntry) nodeML.Attributes[TagStatus].Value.GetInt(0);
				entry.Name = nodeML.Nodes[TagName].Value;
				entry.URL = nodeML.Nodes[TagUrl].Value;
				entry.UrlEnclosure = nodeML.Nodes[TagUrlEnclosure].Value;
				entry.DownloadFileName = nodeML.Nodes[TagDownloadFileName].Value;
				entry.Content = nodeML.Nodes[TagContent].Value;
				entry.Author = nodeML.Nodes[TagAuthor].Value;
				// Devuelve la entrada
				return entry;
		}

		/// <summary>
		///		Graba las entradas de un blog
		/// </summary>
		internal void Save(BlogModel blog, string path)
		{
			SaveToFile(blog, GetFileName(blog, path));
		}

		/// <summary>
		///		Graba las entradas de un blog
		/// </summary>
		internal void SaveToFile(BlogModel blog, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode objMLRoot = fileML.Nodes.Add(TagRoot);

				// Elimina los borrados antiguos
				blog.Entries.DeleteOldData();
				// Añade las entradas
				foreach (EntryModel entry in blog.Entries)
				{
					MLNode nodeML = objMLRoot.Nodes.Add(TagEntry);

						// Añade los datos
						nodeML.Attributes.Add(TagDatePublish, entry.DatePublish);
						nodeML.Attributes.Add(TagStatus, (int) entry.Status);
						nodeML.Nodes.Add(TagName, entry.Name);
						nodeML.Nodes.Add(TagUrl, entry.URL);
						nodeML.Nodes.Add(TagUrlEnclosure, entry.UrlEnclosure);
						nodeML.Nodes.Add(TagDownloadFileName, entry.DownloadFileName);
						if (entry.Status != EntryModel.StatusEntry.Deleted)
							nodeML.Nodes.Add(TagContent, entry.Content);
						nodeML.Nodes.Add(TagAuthor, entry.Author);
				}
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nombre de archivo de un blog
		/// </summary>
		private string GetFileName(BlogModel blog, string path)
		{
			return System.IO.Path.Combine(path, blog.Path, "Entries.xml");
		}
	}
}
