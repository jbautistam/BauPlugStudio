using System;
using System.Collections.Generic;

using Bau.Libraries.DevConference.Admon.Application.ModelsManager;
using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConference.Admon.Application
{
	/// <summary>
	///		Manager de canales
	/// </summary>
	public class AppTrackManager
	{
		public AppTrackManager(string pathBase)
		{
			PathBase = pathBase;
		}

		/// <summary>
		///		Carga los canales
		/// </summary>
		public void Load()
		{
			TrackManagers.Clear();
			TrackManagers.AddRange(new Repository.TrackManagerRepository().Load(GetFileName()));
		}

		/// <summary>
		///		Graba los datos de canales
		/// </summary>
		public void Save()
		{
			new Repository.TrackManagerRepository().Save(TrackManagers, GetFileName());
		}

		/// <summary>
		///		Elimina un administrador de canales
		/// </summary>
		public void RemoveTrackManager(TrackManagerModel trackManager)
		{
			TrackManagers.Remove(trackManager);
		}

		/// <summary>
		///		Elimina un canal
		/// </summary>
		public void RemoveTrack(TrackModel track)
		{
			foreach (TrackManagerModel trackManager in TrackManagers)
				trackManager.Tracks.Remove(track);
		}

		/// <summary>
		///		Elimina una entrada
		/// </summary>
		public void RemoveEntry(EntryModel entry)
		{
			foreach (TrackManagerModel manager in TrackManagers)
				manager.RemoveEntry(entry);
		}

		/// <summary>
		///		Carga las entradas de un archivo de intercambio
		/// </summary>
		public EntryModelCollection LoadExchangeFileEntries(string fileName)
		{
			return new Repository.ExchangeEntriesRepository().Load(fileName);
		}
		
		/// <summary>
		///		Obtiene el nombre de archivo XML
		/// </summary>
		private string GetFileName()
		{
			return System.IO.Path.Combine(PathBase, "Tracks.xml");
		}

		/// <summary>
		///		Directorio base
		/// </summary>
		public string PathBase { get; set; }

		/// <summary>
		///		Canales
		/// </summary>
		public List<TrackManagerModel> TrackManagers { get; } = new List<TrackManagerModel>();
	}
}
