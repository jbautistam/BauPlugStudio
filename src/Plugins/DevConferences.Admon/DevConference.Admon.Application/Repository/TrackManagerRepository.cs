using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.DevConference.Admon.Application.ModelsManager;

namespace Bau.Libraries.DevConference.Admon.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="TrackManagerModel"/>
	/// </summary>
	internal class TrackManagerRepository
	{
		// Constantes privadas
		private const string TagRoot = "DevConferenceAdmon";
		private const string TagTrackManager = "TrackManager";
		private const string TagPath= "Path";
		private const string TagTrack = "Track";
		private const string TagCategory = "Category";
		private const string TagId = "Id";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagUrl = "Url";
		private const string TagConference = "Conference";
		private const string TagCreatedAt = "CreatedAt";
		private const string TagPublishedAt = "PublishedAt";
		private const string TagSummary = "Summary";
		private const string TagThumb = "Thumb";
		private const string TagVideoCode = "VideoCode";
		private const string TagWebSite = "WebSite";
		private const string TagVideo = "Video";
		private const string TagSlides = "Slides";
		private const string TagAuthors = "Authors";

		/// <summary>
		///		Carga las entradas de un archivo
		/// </summary>
		internal List<TrackManagerModel> Load(string fileName)
		{
			List<TrackManagerModel> trackManagers = new List<TrackManagerModel>();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga las entradas
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode nodeML in rootML.Nodes)
								if (nodeML.Name == TagTrackManager)
								{
									TrackManagerModel manager = new TrackManagerModel();

										// Carga los datos
										manager.Id = nodeML.Attributes[TagId].Value;
										manager.Title = nodeML.Nodes[TagName].Value;
										manager.Path = nodeML.Nodes[TagPath].Value;
										// Carga los canales
										manager.Tracks.AddRange(LoadTracks(nodeML));
										// Añade el manager a la colección
										trackManagers.Add(manager);
								}
				// Devuelve los objetos
				return trackManagers;
		}

		/// <summary>
		///		Carga los canales de un nodo
		/// </summary>
		private TrackModelCollection LoadTracks(MLNode rootML)
		{
			TrackModelCollection tracks = new TrackModelCollection();

				// Carga los canales
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagTrack)
					{
						TrackModel track = new TrackModel();

							// Carga los datos
							track.Id = nodeML.Attributes[TagId].Value;
							track.Title = nodeML.Nodes[TagName].Value;
							track.Url = nodeML.Nodes[TagUrl].Value;
							// Carga las categorías
							track.Categories.AddRange(LoadCategories(track, nodeML));
							// Añade el canal
							tracks.Add(track);
					}
				// Devuelve la colección de canales
				return tracks;
		}

		/// <summary>
		///		Carga las categorías de un nodo
		/// </summary>
		private CategoryModelCollection LoadCategories(TrackModel track, MLNode rootML)
		{
			CategoryModelCollection categories = new CategoryModelCollection();
				
				// Carga las categorías
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagCategory)
					{
						CategoryModel category = new CategoryModel(track);

							// Carga las propiedades
							category.Id = nodeML.Attributes[TagId].Value;
							category.Title = nodeML.Nodes[TagName].Value;
							// Carga las entradas
							category.Entries.AddRange(LoadEntries(nodeML));
							// Añade la categoría
							categories.Add(category);
					}
				// Devuelve la colección
				return categories;
		}

		/// <summary>
		///		Carga las entradas de un nodo
		/// </summary>
		private EntryModelCollection LoadEntries(MLNode rootML)
		{
			EntryModelCollection entries = new EntryModelCollection();

				// Carga las entradas
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagConference)
					{
						EntryModel entry = new EntryModel();

							// Asigna las propiedades
							entry.Id = nodeML.Attributes[TagId].Value;
							entry.Title = nodeML.Nodes[TagName].Value;
							entry.Summary = nodeML.Nodes[TagDescription].Value;
							entry.Authors = nodeML.Nodes[TagAuthors].Value;
							entry.UrlImage = nodeML.Nodes[TagThumb].Value;
							entry.UrlVideo = nodeML.Nodes[TagVideo].Value;
							entry.UrlWebSite = nodeML.Nodes[TagWebSite].Value;
							entry.UrlSlides = nodeML.Nodes[TagSlides].Value;
							entry.PublishedAt = nodeML.Nodes[TagPublishedAt].Value.GetDateTime(DateTime.Now);
							entry.CreatedAt = nodeML.Nodes[TagCreatedAt].Value.GetDateTime(DateTime.Now);
							// Añade la entrada
							entries.Add(entry);
					}
				// Devuelve la colección
				return entries;
		}

		/// <summary>
		///		Graba las entradas a un archivo
		/// </summary>
		internal void Save(List<TrackManagerModel> trackManagers, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Añade las entradas
				foreach (TrackManagerModel trackManager in trackManagers)
				{
					MLNode nodeML = rootML.Nodes.Add(TagTrackManager);

						// Añade los datos
						nodeML.Attributes.Add(TagId, trackManager.Id);
						nodeML.Nodes.Add(TagName, trackManager.Title);
						nodeML.Nodes.Add(TagPath, trackManager.Path);
						// Añade los canales
						nodeML.Nodes.AddRange(GetTracksNodes(trackManager.Tracks));
				}
				// Graba el archivo
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene los nodos de canales
		/// </summary>
		private MLNodesCollection GetTracksNodes(TrackModelCollection tracks)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los canales
				foreach (TrackModel track in tracks)
				{
					MLNode nodeML = nodesML.Add(TagTrack);

						// Carga los datos
						nodeML.Attributes.Add(TagId, track.Id);
						nodeML.Nodes.Add(TagName, track.Title);
						nodeML.Nodes.Add(TagUrl, track.Url);
						// Añade las categorías
						nodeML.Nodes.AddRange(GetCategoriesNodes(track.Categories));
				}
				// Devuelve la colección de nodos
				return nodesML;
		}

		/// <summary>
		///		Obtiene los nodos de las categorías
		/// </summary>
		private MLNodesCollection GetCategoriesNodes(CategoryModelCollection categories)
		{
			MLNodesCollection nodesML = new MLNodesCollection();
				
				// Crea los nodos
				foreach (CategoryModel category in categories)
				{
					MLNode nodeML = nodesML.Add(TagCategory);

						// Asigna las propiedades
						nodeML.Attributes.Add(TagId, category.Id);
						nodeML.Nodes.Add(TagName, category.Title);
						// Añade las entradas
						nodeML.Nodes.AddRange(GetEntriesNodes(category.Entries));
				}
				// Devuelve la colección
				return nodesML;
		}

		/// <summary>
		///		Obtiene los nodos de las entradas
		/// </summary>
		private MLNodesCollection GetEntriesNodes(EntryModelCollection entries)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Crea los nodos
				foreach (EntryModel entry in entries)
				{
					MLNode nodeML = nodesML.Add(TagConference);

						// Asigna las propiedades
						nodeML.Attributes.Add(TagId, entry.Id);
						nodeML.Nodes.Add(TagName, entry.Title);
						nodeML.Nodes.Add(TagDescription, entry.Summary);
						nodeML.Nodes.Add(TagAuthors, entry.Authors);
						nodeML.Nodes.Add(TagThumb, entry.UrlImage);
						nodeML.Nodes.Add(TagVideo, entry.UrlVideo);
						nodeML.Nodes.Add(TagWebSite, entry.UrlWebSite);
						nodeML.Nodes.Add(TagSlides, entry.UrlSlides);
						nodeML.Nodes.Add(TagPublishedAt, entry.PublishedAt);
						nodeML.Nodes.Add(TagCreatedAt, entry.CreatedAt);
				}
				// Devuelve la colección
				return nodesML;
		}
	}
}
