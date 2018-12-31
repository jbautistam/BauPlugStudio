using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibBlogReader.Model
{
	/// <summary>
	///		Colección de <see cref="EntryModel"/>
	/// </summary>
	public class EntriesModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<EntryModel>
	{   
		// Constantes privadas
		private const int MaxItems = 40;

		/// <summary>
		///		Ordena las entradas por nombre de blog y fecha
		/// </summary>
		public void SortByDate(bool ascending = true)
		{
			Sort(new Comparers.EntryComparerByDate(ascending));
		}

		/// <summary>
		///		Obtiene una entrada por su URL
		/// </summary>
		public EntryModel SearchByURL(string url)
		{
			return this.FirstOrDefault<EntryModel>(entry => entry.URL.EqualsIgnoreCase(url));
		}

		/// <summary>
		///		Comprueba si existe una URL
		/// </summary>
		public bool ExistsURL(string url)
		{
			return SearchByURL(url) != null;
		}

		/// <summary>
		///		Obtiene el número de elementos no leídos
		/// </summary>
		public int GetNumberNotRead()
		{
			int number = 0;

				// Obtiene el número de elementos no leídos
				foreach (EntryModel entry in this)
					if (entry.Status == EntryModel.StatusEntry.NotRead)
						number++;
				// Devuelve el número de elementos
				return number;
		}

		/// <summary>
		///		Borra los datos antiguos
		/// </summary>
		public void DeleteOldData()
		{
			for (int index = Count - 1; index >= 0; index--)
				if (Count > MaxItems && this[index].DatePublish < DateTime.Now.AddDays(-30) &&
						this[index].Status == EntryModel.StatusEntry.Deleted)
					RemoveAt(index);
		}

		/// <summary>
		///		Borra una entrada
		/// </summary>
		internal bool Delete(EntryModel entry)
		{
			if (Exists(entry.ID))
			{
				Remove(entry);
				entry.Blog.IsDirty = true;
				return true;
			}
			else
				return false;
		}
	}
}
