using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DevConference.Application.Repository
{
	/// <summary>
	///		Repository para intercambio de pistas
	/// </summary>
	internal class TrackExchangeRepository
	{
		// Constantes privadas
		private const string TagRoot = "Tracks";
		private const string TagTrack = "Track";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		private const string TagUrl = "Url";

		/// <summary>
		///		Carga un archivo de canales
		/// </summary>
		public TrackModelCollection LoadTracks(string xml)
		{
			TrackModelCollection tracks = new TrackModelCollection();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

				// Carga el archivo
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode trackML in rootML.Nodes)
								if (trackML.Name == TagTrack)
								{
									TrackModel track = new TrackModel();

										// Carga los datos
										track.Title = trackML.Nodes[TagName].Value;
										track.Description = trackML.Nodes[TagDescription].Value;
										track.Url = trackML.Nodes[TagUrl].Value;
										// Añade el canal al nodo
										if (!track.Url.IsEmpty())
											tracks.Add(track);
								}
				// Devuelve los canales
				return tracks;
		}

		/// <summary>
		///		Obtiene el XML de un archivo de intercambio de canales
		/// </summary>
		internal string GetXml(TrackModelCollection tracks)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Convierte los canales
				foreach (TrackModel track in tracks)
				{
					MLNode nodeML = rootML.Nodes.Add(TagTrack);

						// Añade los datos al nodo
						nodeML.Nodes.Add(TagName, track.Title);
						nodeML.Nodes.Add(TagDescription, track.Description);
						nodeML.Nodes.Add(TagUrl, track.Url);
				}
				// Devuelve el XML
				return new LibMarkupLanguage.Services.XML.XMLWriter().ConvertToString(fileML);
		}
	}
}
