using System;
using System.IO;

using Bau.Libraries.LibFeeds.Syndication.RSS.Data;
using Bau.Libraries.LibFeeds.Syndication.RSS.Transforms;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.RssSteps
{
	/// <summary>
	///		Generador del archivo principal de RSS
	/// </summary>
	internal class StepRssProcessor : AbstractBaseSteps
	{
		// Constantes privadas
		private const int ItemsRSS = 20;

		internal StepRssProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Crea el sitemap
		/// </summary>
		internal override void Process()
		{ 
			LibCommonHelper.Files.HelperFiles.MakePath(Processor.PathTarget);
			LibCommonHelper.Files.HelperFiles.SaveTextFile(Path.Combine(Processor.PathTarget, "MainFeed.rss"),
														   new RSSWriter().GetXML(GetFeedData()));
		}

		/// <summary>
		///		Obtiene los datos de un feed
		/// </summary>
		private RSSChannel GetFeedData()
		{
			RSSChannel channel = new RSSChannel();
			int itemsWrite = 0;

				// Ordena por fecha
				Data.Files.SortByDate(false);
				// Añade las propiedades del canal
				channel.Link = CombineUrl(Processor.Project.URLBase, Processor.Project.PageMain);
				channel.Logo = new RSSImage(Processor.Project.URLBase, "Imagen", channel.Link);
				channel.Author = new RSSAuthor(Processor.Project.WebMaster);
				channel.Copyright = Processor.Project.Copyright;
				channel.Title = Processor.Project.Name;
				channel.Description = Processor.Project.Description;
				channel.Editor = Processor.Project.Editor;
				channel.LastBuildDate = DateTime.Now;
				// Añade las entradas del canal
				foreach (Pages.FileTargetModel file in Data.Files)
					if (file.File.FileType == Model.Solutions.FileModel.DocumentType.Document &&
						file.ShowAtRss && file.ShowMode == Model.Documents.DocumentModel.ShowChildsMode.None &&
						itemsWrite < ItemsRSS)
					{
						RSSEntry rssEntry = new RSSEntry();
						Model.Documents.DocumentModel document;

							// Carga el documento compilado en corto
							document = new Application.Bussiness.Documents.DocumentBussiness().Load(Processor.Project, 
																									file.GetFullFileNameCompiledShort(Processor));
							// Asigna los valores a la entrada
							rssEntry.Authors.Add(new RSSAuthor(Processor.Project.WebMaster));
							// entry.Categories.Add();
							rssEntry.Content = document.Content;
							rssEntry.Link = CombineUrl(Processor.Project.URLBase, file.RelativeFullFileNameTarget);
							rssEntry.Title = file.Title;
							rssEntry.DateCreated = file.DateUpdate;
							// Añade la entrada al canal
							channel.Entries.Add(rssEntry);
							// Incrementa el número de entradas escritas
							itemsWrite++;
					}
				// Devuelve los datos del canal
				return channel;
		}

		/// <summary>
		///		Combina dos URL
		/// </summary>
		private string CombineUrl(string path, string url)
		{
			return Path.Combine(path, url).Replace("\\", "/");
		}
	}
}
