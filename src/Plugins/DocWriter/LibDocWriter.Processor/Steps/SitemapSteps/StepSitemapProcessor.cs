using System;
using System.IO;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.SitemapSteps
{
	/// <summary>
	///		Generador de SiteMap
	/// </summary>
	internal class StepSitemapProcessor : AbstractBaseSteps
	{
		internal StepSitemapProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Crea el sitemap
		/// </summary>
		internal override void Process()
		{
			Save(Processor.Project.URLBase, Path.Combine(Processor.PathTarget, "sitemap.xml"), Load(Data.Files));
		}

		/// <summary>
		///		Carga los archivos de un directorio
		/// </summary>
		private SitemapEntriesCollection Load(Pages.FileTargetModelCollection files)
		{
			SitemapEntriesCollection entries = new SitemapEntriesCollection();

				// Añade los archivos
				foreach (Pages.FileTargetModel file in files)
				{
					string extension = Path.GetExtension(file.FileNameTarget);

						if (extension.EqualsIgnoreCase(".htm") || extension.EqualsIgnoreCase(".html"))
							entries.Add(file.RelativeFullFileNameTarget, file.DateUpdate);
				}
				// Devuelve la colección de entradas
				return entries;
		}

		/// <summary>
		///		Obtiene el XML de una colección de entradas
		/// </summary>
		private void Save(string urlBase, string fileName, SitemapEntriesCollection entries)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add("urlset");

				// Añade el espacio de nombres
				nodeML.Attributes.Add("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
				// Añade las entradas
				foreach (SitemapEntry entry in entries)
				{
					MLNode entryML = nodeML.Nodes.Add("url");

						// Añade los datos del nodo
						entryML.Nodes.Add("loc", GetURL(entry.FileName, urlBase));
						entryML.Nodes.Add("lastmod", string.Format("{0:yyyy-MM-dd}", entry.DateLastUpdate));
						entryML.Nodes.Add("changefreq", entry.ChangeFrequence.ToString().ToLower());
						entryML.Nodes.Add("priority", entry.Priority.ToString().Replace(',', '.'));
				}
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene la URL de un archivo
		/// </summary>
		private string GetURL(string fileName, string urlBase)
		{ 
			// Normaliza las barras de inicio y fin
			if (fileName.StartsWith("\\"))
				fileName = fileName.Substring(1);
			if (urlBase.IsEmpty())
				urlBase = "";
			if (!urlBase.EndsWith("/"))
				urlBase += "/";
			// Devuelve la cadena
			return (urlBase + fileName).Replace("\\", "/");
		}
	}
}
