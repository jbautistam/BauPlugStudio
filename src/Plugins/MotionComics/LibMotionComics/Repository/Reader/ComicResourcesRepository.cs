using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para la carga de recursos de Cómic
	/// </summary>
	internal class ComicResourcesRepository
	{
		internal ComicResourcesRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		///		Carga los recursos
		/// </summary>
		internal void LoadResources(string path, MLNode childML, ComicModel comic)
		{
			string fileName = childML.Attributes[ComicRepositoryConstants.TagFileName].Value;

				// Carga los recursos del nodo o un nuevo archivo
				if (!fileName.IsEmpty())
					LoadFileResource(System.IO.Path.Combine(path, fileName), comic);
				else
					LoadResources(childML, comic);
		}

		/// <summary>
		///		Carga un archivo de recursos
		/// </summary>
		private void LoadFileResource(string fileName, ComicModel comic)
		{ 
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == ComicRepositoryConstants.TagResources)
							LoadResources(nodeML, comic);
		}

		/// <summary>
		///		Carga los recursos de un cómic
		/// </summary>
		private void LoadResources(MLNode nodeML, ComicModel comic)
		{
			foreach (MLNode childML in nodeML.Nodes)
				switch (childML.Name)
				{
					case ComicRepositoryConstants.TagImage:
							LoadImageResource(childML, comic);
						break;
					case ComicRepositoryConstants.TagShape:
							LoadShapeResource(childML, comic);
						break;
					default:
							LoadBrushResource(childML, comic);
						break;
				}
		}

		/// <summary>
		///		Carga un recurso de imagen
		/// </summary>
		private void LoadImageResource(MLNode nodeML, ComicModel comic)
		{
			ImageModel image = Mediator.PagesRepository.LoadContentImage(null, comic, nodeML);

				// Añade la imagen a la lista de recursos
				comic.Resources.Add(image.Key, image);
		}

		/// <summary>
		///		Carga un recurso de figura
		/// </summary>
		private void LoadShapeResource(MLNode nodeML, ComicModel comic)
		{
			ShapeModel shape = Mediator.ShapesRepository.LoadShape(null, nodeML);

				// Añade la figura
				comic.Resources.Add(shape.Key, shape);
		}

		/// <summary>
		///		Carga los recursos de brocha
		/// </summary>
		private void LoadBrushResource(MLNode nodeML, ComicModel comic)
		{
			AbstractBaseBrushModel brush = Mediator.BrushesRepository.LoadBrush(nodeML);

				// Si realmente se ha cargado un dato de brocha, se añade a la colección
				if (brush != null)
					comic.Resources.Add(brush.Key, brush);
		}

		/// <summary>
		///		Agregrador de repository
		/// </summary>
		private ComicReaderMediator Mediator { get; }
	}
}
