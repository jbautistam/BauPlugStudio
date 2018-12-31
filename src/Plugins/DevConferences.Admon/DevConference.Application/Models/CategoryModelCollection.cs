using System;

namespace Bau.Libraries.DevConference.Application.Models
{
	/// <summary>
	///		Colección de <see cref="CategoryModel"/>
	/// </summary>
    public class CategoryModelCollection : BaseModelCollection<CategoryModel>
    {
		/// <summary>
		///		Clona una colección
		/// </summary>
		internal CategoryModelCollection Clone(TrackModel target)
		{
			CategoryModelCollection categories = new CategoryModelCollection();

				// Clona las categorías
				foreach (CategoryModel category in this)
					categories.Add(category.Clone(target));
				// Devuelve la colección clonada
				return categories;
		}

		/// <summary>
		///		Mezcla una colección de categorías
		/// </summary>
		public CategoryModelCollection Merge(TrackModel track, CategoryModelCollection categoriesNew)
		{
			CategoryModelCollection categoriesMerged = new CategoryModelCollection();

				// Mezcla las categorías
				foreach (CategoryModel categoryNew in categoriesNew)
				{
					CategoryModel categoryFound = Search(categoryNew.Id, categoryNew.Title);

						if (categoryFound == null)
						{
							// Añade la categoría a la colección (indicando que está cargada)
							categoryNew.IsLoaded = true;
							Add(categoryNew.Clone(track));
							// Añade la categoría a las categorías mezcladas
							categoriesMerged.Add(categoryNew.Clone(track));
						}
						else
						{
							EntryModelCollection entriesMerged = categoryFound.Entries.Merge(categoryNew.Entries);

								// Si se han encontrado entradas nuevas, se añade a la lista de categorías mezcladas
								if (entriesMerged.Count > 0)
								{
									CategoryModel categoryMerged = categoryFound.Clone(track);

										// Limpia las entradas
										categoryMerged.Entries.Clear();
										// Añade sólo las entradas mezcladas
										categoryMerged.Entries.AddRange(entriesMerged);
										// y añade la categoría a la lista
										categoriesMerged.Add(categoryMerged);
								}
						}
				}
				// Devuelve la categorías en las que ha habido algún cambio
				return categoriesMerged;
		}

		/// <summary>
		///		Elimina una entrada
		/// </summary>
		internal void RemoveEntry(EntryModel entry)
		{
			foreach (CategoryModel category in this)
				category.Entries.Remove(entry);
		}

		/// <summary>
		///		Ordena las categorías
		/// </summary>
		public override void SortByTitle(bool ascending = true)
		{
			// Ordena las categorías
			base.SortByTitle(ascending);
			// Ordena las entradas
			foreach (CategoryModel category in this)
				category.Entries.SortByTitle(ascending);
		}
	}
}
