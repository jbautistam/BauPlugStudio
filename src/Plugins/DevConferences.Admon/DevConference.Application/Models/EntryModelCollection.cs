using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Colección de <see cref="EntryModel"/>
	/// </summary>
    public class EntryModelCollection : BaseModelCollection<EntryModel>
    {
		/// <summary>
		///		Mezcla una colección de entradas
		/// </summary>
		public EntryModelCollection Merge(EntryModelCollection entriesNew)
		{
			EntryModelCollection entriesMerged = new EntryModelCollection();

				// Mezcla las entradas
				foreach (EntryModel entryNew in entriesNew)
					if (Search(entryNew.Id, entryNew.Title) == null)
					{
						// Añade la entrada
						Add(entryNew);
						// Añade la entrada a la colección de entradas añadidas
						entriesMerged.Add(entryNew);
					}
				// Devuelve la colección de entradas mezcladas
				return entriesMerged;
		}

		/// <summary>
		///		Clona una colección de entradas
		/// </summary>
		public EntryModelCollection Clone()
		{
			EntryModelCollection entries = new EntryModelCollection();

				// Clona la colección
				foreach (EntryModel entry in this)
					entries.Add(entry.Clone());
				// Devuelve la colección clonada
				return entries;
		}

		/// <summary>
		///		Cambia el estado de los elementos no leidos
		/// </summary>
		public bool SetStatusForUnRead(EntryModel.Status status)
		{
			bool updated = false;

				// Cambia los elementos no leidos
				for (int index = Count - 1; index >= 0; index--)
					if (this[index].State == EntryModel.Status.Unread)
					{
						// Modifica el estado
						this[index].State = status;
						// Indica que se ha modificado
						updated = true;
					}
				// Devuelve el valor que indica si se ha asignado algún valor
				return updated;
		}

		/// <summary>
		///		Busca una entrada por su URL de vídeo
		/// </summary>
		public EntryModel SearchByUrlVideo(string urlVideo)
		{
			return this.FirstOrDefault(item => item.UrlVideo.EqualsIgnoreCase(urlVideo));
		}
	}
}
