using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.DevConference.Admon.Application.ModelsManager;

namespace Bau.Libraries.DevConference.Admon.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="TrackManagerModel"/>
	/// </summary>
	internal class ExchangeEntriesRepository
	{
		// Constantes privadas
		private const string TagRoot = "Entries";
		private const string TagEntry = "Entry";
		private const string TagPublish = "Publish";
		private const string TagTitle = "Name";
		private const string TagSummary = "Content";
		private const string TagAuthor = "Author";
		private const string TagUrl = "Url";

		/// <summary>
		///		Carga las entradas de un archivo
		/// </summary>
		internal EntryModelCollection Load(string fileName)
		{
			EntryModelCollection entries = new EntryModelCollection();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga las entradas
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								if (nodeML.Name == TagEntry)
								{
									EntryModel entry = new EntryModel();

										// Carga los datos
										entry.PublishedAt = nodeML.Attributes[TagPublish].Value.GetDateTime(DateTime.Now);
										entry.Title = nodeML.Nodes[TagTitle].Value;
										entry.UrlVideo = nodeML.Nodes[TagUrl].Value;
										entry.Summary = nodeML.Nodes[TagSummary].Value;
										entry.Authors = nodeML.Nodes[TagAuthor].Value;
										// Añade la entrada a la colección
										if (!string.IsNullOrWhiteSpace(entry.UrlVideo))
											entries.Add(entry);
								}
				// Devuelve los objetos
				return entries;
		}
	}
}
