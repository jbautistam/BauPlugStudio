using System;

using Bau.Libraries.DevConference.Application.Models;

namespace Bau.Libraries.DevConference.Admon.Application.ModelsManager
{
	/// <summary>
	///		Manager de una lista de <see cref="TrackModel"/>
	/// </summary>
	public class TrackManagerModel : BaseModel
	{
		/// <summary>
		///		Elimina una entrada
		/// </summary>
		internal void RemoveEntry(EntryModel entry)
		{
			Tracks.RemoveEntry(entry);
		}

		/// <summary>
		///		Directorio de generación
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		///		Canales
		/// </summary>
		public TrackModelCollection Tracks { get; } = new TrackModelCollection();
	}
}
