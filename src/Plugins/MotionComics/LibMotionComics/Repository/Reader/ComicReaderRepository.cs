using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Entities;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para la carga de cómics
	/// </summary>
	internal class ComicReaderRepository
	{
		/// <summary>
		///		Carga los datos de un cómic
		/// </summary>
		internal ComicModel Load(string path, string fileName, bool loadFull)
		{
			ComicModel comic = new ComicModel(path);
			string strFullFileName = System.IO.Path.Combine(path, fileName);
			ComicReaderMediator mediator = new ComicReaderMediator();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(strFullFileName);

				// Recorre los nodos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == ComicRepositoryConstants.TagRoot)
						{ 
							// Obtiene el ancho y el alto
							comic.Width = nodeML.Attributes[ComicRepositoryConstants.TagWidth].Value.GetInt(1000);
							comic.Height = nodeML.Attributes[ComicRepositoryConstants.TagHeight].Value.GetInt(1000);
							// Asigna las propiedades básicas del cómic
							comic.Title = nodeML.Nodes[ComicRepositoryConstants.TagTitle].Value.TrimIgnoreNull();
							comic.Summary = nodeML.Nodes[ComicRepositoryConstants.TagSummary].Value.TrimIgnoreNull();
							comic.ThumbFileName = nodeML.Nodes[ComicRepositoryConstants.TagThumbFileName].Value.TrimIgnoreNull();
							// Obtiene los componentes del cómic
							if (loadFull)
								foreach (MLNode childML in nodeML.Nodes)
									switch (childML.Name)
									{
										case ComicRepositoryConstants.TagInclude:
										case ComicRepositoryConstants.TagResources:
												mediator.ResourcesRepository.LoadResources(path, childML, comic);
											break;
										case ComicRepositoryConstants.TagPage:
												mediator.PagesRepository.LoadPage(childML, comic);
											break;
									}
							// Carga los idiomas (aunque no sea una carga completa)
							LoadLanguage(comic, nodeML);
						}
				// Devuelve el cómic cargado
				return comic;
		}

		/// <summary>
		///		Carga el idioma
		/// </summary>
		private void LoadLanguage(ComicModel comic, MLNode nodeML)
		{ 
			// Carga los idiomas
			foreach (MLNode childML in nodeML.Nodes)
				if (childML.Name == ComicRepositoryConstants.TagLanguage)
				{
					string key = childML.Attributes[ComicRepositoryConstants.TagKey].Value;

						if (!childML.Value.IsEmpty() && !key.IsEmpty())
							comic.Languages.Add(key,
												new LanguageModel(key, childML.Value,
																  childML.Attributes[ComicRepositoryConstants.TagDefault].Value.GetBool()));
				}
			// Asigna los predeterminados
			if (comic.Languages.Count > 0)
			{
				bool existsDefault = false;

					// Comprueba si existe un elemento predeterminado
					foreach (System.Collections.Generic.KeyValuePair<string, AbstractComponentModel> item in comic.Languages)
						if (item.Value is LanguageModel)
						{
							LanguageModel language = item.Value as LanguageModel;

								if (language.IsDefault)
									existsDefault = true;
						}
					// Si no existe ningún elemento predeterminado
					if (!existsDefault)
						foreach (System.Collections.Generic.KeyValuePair<string, AbstractComponentModel> item in comic.Languages)
							if (!existsDefault && item.Value is LanguageModel)
							{
								LanguageModel language = item.Value as LanguageModel;

									// Asigna el valor que indica si es predeterminado
									language.IsDefault = true;
									// Indica que ya se ha asignado un valor predeterminado
									existsDefault = true;
							}
			}
		}
	}
}
