using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConference.Application.Repository
{
	/// <summary>
	///		Repository para los canales
	/// </summary>
	public class TracksRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Channels";
		private const string TagTrack = "Track";
		private const string TagCategory = "Category";
		private const string TagDeleted = "Deleted";
		private const string TagId = "Id";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagUrl = "Url";
		private const string TagEnabled = "Enabled";
		private const string TagDateLastDownload = "DateLastDownload";
		private const string TagDateLastError = "DateLastError";

		/// <summary>
		///		Carga una serie de canales de una cadena XML
		/// </summary>
		public TrackModelCollection LoadTracks(string xml)
		{
			TrackModelCollection tracks = new TrackModelCollection();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode trackML in nodeML.Nodes)
								if (trackML.Name == TagTrack)
								{
									TrackModel track = new TrackModel();

										// Carga los datos de la carpeta
										track.Title = trackML.Nodes[TagName].Value;
										track.Description = trackML.Nodes[TagDescription].Value;
										track.Url = trackML.Nodes[TagUrl].Value;
										track.DateLastDownload = trackML.Attributes[TagDateLastDownload].Value.GetDateTime(DateTime.Now.AddDays(-2));
										track.DateLastError = trackML.Attributes[TagDateLastError].Value.GetDateTime();
										track.Enabled = trackML.Attributes[TagEnabled].Value.GetBool(true);
										track.IsLoaded = false;
										// Carga las categorías del canal
										foreach (MLNode channelML in trackML.Nodes)
											if (channelML.Name == TagCategory)
											{
												CategoryModel category = new CategoryModel(track);

													// Carga los datos
													category.Id = channelML.Attributes[TagId].Value;
													category.IsDeleted = channelML.Attributes[TagDeleted].Value.GetBool();
													category.Title = channelML.Nodes[TagName].Value;
													// Añade el canal a la categoría
													track.Categories.Add(category);
											}
										// Añade las categorías a la colección
										if (!track.Url.IsEmpty())
											tracks.Add(track);
								}
				// Devuelve las categorías
				return tracks;
		}

		/// <summary>
		///		Obtiene el XML de una colección de canales
		/// </summary>
		internal string GetXml(TrackModelCollection tracks)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Obtiene los nodos del archivo
				foreach (TrackModel track in tracks)
				{
					MLNode trackML = rootML.Nodes.Add(TagTrack);

						// Añade las propiedades
						trackML.Nodes.Add(TagName, track.Title);
						trackML.Nodes.Add(TagDescription, track.Description);
						trackML.Nodes.Add(TagUrl, track.Url);
						trackML.Attributes.Add(TagEnabled, track.Enabled);
						trackML.Attributes.Add(TagDateLastDownload, track.DateLastDownload);
						// trackML.Attributes.Add(TagDateLastError, track.DateLastError);
						// Añade los canales
						foreach (CategoryModel category in track.Categories)
						{
							MLNode categoryML = trackML.Nodes.Add(TagCategory);

								// Añade las propiedades
								categoryML.Attributes.Add(TagId, category.Id);
								categoryML.Attributes.Add(TagDeleted, category.IsDeleted);
								categoryML.Nodes.Add(TagName, category.Title);
						}
				}
				// Obtiene la cadena XML
				return new LibMarkupLanguage.Services.XML.XMLWriter().ConvertToString(fileML);
		}
	}
}
