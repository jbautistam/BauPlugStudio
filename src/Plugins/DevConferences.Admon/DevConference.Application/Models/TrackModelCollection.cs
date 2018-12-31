using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Colección de <see cref="TrackModel"/>
	/// </summary>
    public class TrackModelCollection : BaseModelCollection<TrackModel>
    {
		/// <summary>
		///		Mezcla la colección de canales
		/// </summary>
		public TrackModelCollection Merge(TrackModelCollection tracksNew)
		{
			TrackModelCollection tracksMerged = new TrackModelCollection();

				// Mezcla la colección
				foreach (TrackModel trackNew in tracksNew)
				{
					TrackModel trackFound = Search(trackNew.Id);

						// Si el canal no estaba, se añade y se indica que se ha modificado
						if (trackFound == null)
						{
							// Añade el canal
							Add(trackNew);
							// Añade el canal a la lista de canales mezclados
							tracksMerged.Add(trackNew);
						}
						else 
						{
							CategoryModelCollection categoriesMerged = trackFound.Categories.Merge(trackFound, trackNew.Categories);

								// Si se ha mezclado alguna categoría, se añade la pista
								if (categoriesMerged.Count > 0)
								{
									TrackModel trackMerged = trackFound.Clone();

										// Quita las categorías que tuviera
										trackMerged.Categories.Clear();
										// Le añade las mezcladas
										trackMerged.Categories.AddRange(categoriesMerged.Clone(trackMerged));
										// y añade el canal a la colección de canales mezclados
										tracksMerged.Add(trackMerged);
								}
						}
				}
				// Devuelve el valor que indica si ha habido algo nuevo
				return tracksMerged;
		}

		/// <summary>
		///		Elimina una entrada
		/// </summary>
		public void RemoveEntry(EntryModel entry)
		{
			foreach (TrackModel track in this)
				track.Categories.RemoveEntry(entry);
		}

		/// <summary>
		///		Ordena los canales
		/// </summary>
		public override void SortByTitle(bool ascending = true)
		{
			// Ordena los canales
			base.SortByTitle(ascending);
			// Ordena las categorías
			foreach (TrackModel track in this)
				track.Categories.SortByTitle(ascending);
		}

		/// <summary>
		///		Busca un canal por su URL
		/// </summary>
		public TrackModel SearchByUrl(TrackModel track)
		{
			return this.FirstOrDefault(item => item.Url.EqualsIgnoreCase(track.Url));
		}
	}
}
