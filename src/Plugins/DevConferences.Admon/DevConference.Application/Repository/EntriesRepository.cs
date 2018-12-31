using System;
using System.Collections.Generic;

using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DevConference.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="Models.EntryModel"/>
	/// </summary>
	internal class EntriesRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "DevConference";
		internal const string TagConference = "Conference";
		private const string TagId = "Id";
		private const string TagCreatedAt = "CreatedAt";
		private const string TagPublishedAt = "PublishedAt";
		private const string TagStatus = "Status";
		private const string TagName = "Name";
		private const string TagSummary = "Summary";
		private const string TagThumb = "Thumb";
		private const string TagWebSite = "WebSite";
		private const string TagVideo = "Video";
		private const string TagSlides = "Slides";
		private const string TagAuthors = "Authors";

		/// <summary>
		///		Carga las entradas de una categoría
		/// </summary>
		internal EntryModelCollection LoadCategoryEntries(string xml)
		{
			EntryModelCollection entries = new EntryModelCollection();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

				// Carga el archivo
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								if (nodeML.Name == TagConference)
								{
									EntryModel entry = LoadEntry(nodeML);

										// Añade el artículo a la colección
										if (!entry.Title.IsEmpty())
											entries.Add(entry);
								}
				// Devuelve la colección de artículos
				return entries;
		}

		/// <summary>
		///		Carga los datos de una entrada
		/// </summary>
		internal EntryModel LoadEntry(MLNode nodeML)
		{
			EntryModel entry = new EntryModel();

				// Carga los datos
				entry.Id = nodeML.Attributes[TagId].Value;
				entry.Title = nodeML.Nodes[TagName].Value;
				entry.Summary = nodeML.Nodes[TagSummary].Value;
				entry.Authors = nodeML.Nodes[TagAuthors].Value;
				entry.UrlImage = nodeML.Nodes[TagThumb].Value;
				entry.UrlVideo = nodeML.Nodes[TagVideo].Value;
				entry.UrlWebSite = nodeML.Nodes[TagWebSite].Value;
				entry.UrlSlides = nodeML.Nodes[TagSlides].Value;
				entry.PublishedAt = nodeML.Attributes[TagCreatedAt].Value.GetDateTime(DateTime.Now);
				entry.CreatedAt = nodeML.Attributes[TagPublishedAt].Value.GetDateTime(DateTime.Now);
				entry.State = nodeML.Attributes[TagStatus].Value.GetEnum(EntryModel.Status.Unread);
				// Devuelve la entrada
				return entry;
		}

		/// <summary>
		///		Obtiene el XML de las entradas de una categoría
		/// </summary>
		internal string GetXml(CategoryModel category)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Añade las entradas
				foreach (EntryModel entry in category.Entries)
					rootML.Nodes.Add(GetNode(entry));
				// Devuelve la cadena XML
				return new LibMarkupLanguage.Services.XML.XMLWriter().ConvertToString(fileML);
		}

		/// <summary>
		///		Obtiene el nodo de una entrada
		/// </summary>
		internal MLNode GetNode(EntryModel entry)
		{
			MLNode nodeML = new MLNode(TagConference);

				// Añade los datos
				nodeML.Attributes.Add(TagId, entry.Id);
				nodeML.Nodes.Add(TagName, entry.Title);
				nodeML.Nodes.Add(TagSummary, entry.Summary);
				nodeML.Nodes.Add(TagAuthors, entry.Authors);
				nodeML.Nodes.Add(TagThumb, entry.UrlImage?.ToString());
				nodeML.Nodes.Add(TagWebSite, entry.UrlWebSite?.ToString());
				nodeML.Nodes.Add(TagSlides, entry.UrlSlides?.ToString());
				nodeML.Nodes.Add(TagVideo, entry.UrlVideo?.ToString());
				nodeML.Attributes.Add(TagCreatedAt, entry.CreatedAt);
				nodeML.Attributes.Add(TagPublishedAt, entry.PublishedAt);
				nodeML.Attributes.Add(TagStatus, entry.State.ToString());
				// Devuelve el nodo
				return nodeML;
		}
	}
}
