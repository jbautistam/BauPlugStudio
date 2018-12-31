using System;
using System.Collections.Generic;

using Bau.Libraries.DevConference.Application.Models;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DevConference.Application.Repository
{
	/// <summary>
	///		Repository de <see cref="Models.CategoryModel"/>
	/// </summary>
	internal class CategoriesRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "DevConference";
		private const string TagCategory = "Category";
		private const string TagId = "Id";
		private const string TagName = "Name";

		/// <summary>
		///		Carga las categorías de un canal
		/// </summary>
		internal CategoryModelCollection LoadCategories(TrackModel track, string xml)
		{
			CategoryModelCollection categories = new CategoryModelCollection();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml);

				// Carga el archivo
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							foreach (MLNode categoryML in rootML.Nodes)
								if (categoryML.Name == TagCategory)
								{
									CategoryModel category = new CategoryModel(track);

										// Asigna las propiedades
										category.Id = categoryML.Attributes[TagId].Value;
										category.Title = categoryML.Nodes[TagName].Value;
										// Carga las entradas
										foreach (MLNode nodeML in categoryML.Nodes)
											if (nodeML.Name == EntriesRepository.TagConference)
												category.Entries.Add(new EntriesRepository().LoadEntry(nodeML));
										// Añade la categoría a la colección
										categories.Add(category);
								}
				// Devuelve la colección de categorías
				return categories;
		}

		/// <summary>
		///		Obtiene el XML de las categorías / entradas
		/// </summary>
		internal string GetXml(TrackModel track)
		{
			MLFile fileML = new MLFile();
			MLNode rootML = fileML.Nodes.Add(TagRoot);

				// Añade las categorías
				foreach (CategoryModel category in track.Categories)
				{
					MLNode categoryML = rootML.Nodes.Add(TagCategory);

						// Añade las propiedades
						categoryML.Attributes.Add(TagId, category.Id);
						categoryML.Nodes.Add(TagName, category.Title);
						// Añade las entradas
						foreach (EntryModel entry in category.Entries)
							categoryML.Nodes.Add(new EntriesRepository().GetNode(entry));
				}
				// Devuelve la cadena XML
				return new LibMarkupLanguage.Services.XML.XMLWriter().ConvertToString(fileML);
		}
	}
}
