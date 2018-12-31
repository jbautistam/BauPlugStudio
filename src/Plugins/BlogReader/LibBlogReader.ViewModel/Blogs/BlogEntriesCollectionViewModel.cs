using System;
using System.Linq;
using System.Collections.ObjectModel;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibBlogReader.ViewModel.Blogs
{
	/// <summary>
	///		Colección de <see cref="BlogEntryViewModel"/>
	/// </summary>
	public class BlogEntriesCollectionViewModel : ObservableCollection<BlogEntryViewModel>
	{
		/// <summary>
		///		Obtiene las entradas seleccionadas
		/// </summary>
		internal BlogEntriesCollectionViewModel GetSelected()
		{
			BlogEntriesCollectionViewModel entries = new BlogEntriesCollectionViewModel();

				// Obtiene las seleccionadas
				foreach (BlogEntryViewModel entry in this)
					if (entry.IsSelected)
						entries.Add(entry);
				// Devuelve las entradas seleccionadas
				return entries;
		}

		/// <summary>
		///		Obtiene las entradas no leídas
		/// </summary>
		internal BlogEntriesCollectionViewModel GetByParameters(bool blnSeeNotRead, bool blnSeeRead, bool blnSeeInteresting)
		{
			BlogEntriesCollectionViewModel entries = new BlogEntriesCollectionViewModel();

				// Obtiene las entradas no leídas
				foreach (BlogEntryViewModel entry in this)
					if ((entry.Status == Model.EntryModel.StatusEntry.NotRead && blnSeeNotRead) ||
							(entry.Status == Model.EntryModel.StatusEntry.Read && blnSeeRead) ||
							(entry.Status == Model.EntryModel.StatusEntry.Interesting && blnSeeInteresting))
						entries.Add(entry);
				// Devuelve la colección de entradas
				return entries;
		}

		/// <summary>
		///		Obtiene las entradas leídas
		/// </summary>
		internal BlogEntriesCollectionViewModel GetRead()
		{
			BlogEntriesCollectionViewModel entries = new BlogEntriesCollectionViewModel();

				// Obtiene las entradas no leídas
				foreach (BlogEntryViewModel entry in this)
					if (entry.Status == Model.EntryModel.StatusEntry.Read)
						entries.Add(entry);
				// Devuelve la colección de entradas
				return entries;
		}

		/// <summary>
		///		Busca una entrada
		/// </summary>
		internal BlogEntryViewModel Search(string url)
		{
			return this.FirstOrDefault(entry => entry.Entry.URL.EqualsIgnoreCase(url));
		}

		/// <summary>
		///		Cuenta el número de entradas seleccionadas
		/// </summary>
		public int CountSelected
		{
			get { return this.Count(entry => entry.IsSelected); }
		}
	}
}
