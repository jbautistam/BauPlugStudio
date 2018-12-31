using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para la carga de datos de páginas
	/// </summary>
	internal class ComicPageRepository
	{
		internal ComicPageRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		///		Carga los datos de una página
		/// </summary>
		internal void LoadPage(MLNode nodeML, ComicModel comic)
		{
			PageModel page = new PageModel(comic);

				// Asigna las propiedades
				page.Brush = Mediator.BrushesRepository.LoadFirstBrush(nodeML);
				// Carga los componentes de la página
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case ComicRepositoryConstants.TagImage:
								page.Content.Add(LoadContentImage(page, comic, childML));
							break;
						case ComicRepositoryConstants.TagFrame:
						case ComicRepositoryConstants.TagBalloon:
								page.Content.Add(LoadFrame(page, childML));
							break;
						case ComicRepositoryConstants.TagText:
								page.Content.Add(LoadText(page, null, childML));
							break;
						case ComicRepositoryConstants.TagTimeLine:
								page.TimeLines.Add(Mediator.TimeLineRepository.LoadTimeLine(page, childML));
							break;
					}
				// Añade la página al cómic
				comic.Pages.Add(page);
		}

		/// <summary>
		///		Obtiene el contenido de una página
		/// </summary>
		internal ImageModel LoadContentImage(PageModel page, ComicModel comic, MLNode nodeML)
		{
			ImageModel image = new ImageModel(page, nodeML.Attributes[ComicRepositoryConstants.TagKey].Value);
			string fileName = nodeML.Attributes[ComicRepositoryConstants.TagFileName].Value;

				// Asigna las propiedades de la imagen
				image.ResourceKey = nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value;
				// Asigna el nombre de archivo
				if (!fileName.IsEmpty())
					image.FileName = System.IO.Path.Combine(comic.Path, fileName);
				// Asigna los atributos
				Mediator.CommonRepository.AssignAttributesPageItem(nodeML, image);
				// Devuelve el contenido
				return image;
		}

		/// <summary>
		///		Carga un frame
		/// </summary>
		private FrameModel LoadFrame(PageModel page, MLNode nodeML)
		{
			FrameModel frame = new FrameModel(page, nodeML.Attributes[ComicRepositoryConstants.TagKey].Value);

				// Asigna las propiedades
				frame.Stretch = Mediator.CommonRepository.GetStretchMode(nodeML.Attributes[ComicRepositoryConstants.TagStretch].Value);
				frame.Brush = Mediator.BrushesRepository.LoadFirstBrush(nodeML);
				// Asigna los radios
				frame.RadiusX = nodeML.Attributes[ComicRepositoryConstants.TagRadiusX].Value.GetDouble();
				frame.RadiusY = nodeML.Attributes[ComicRepositoryConstants.TagRadiusY].Value.GetDouble();
				// Asigna los atributos
				Mediator.CommonRepository.AssignAttributesPageItem(nodeML, frame);
				// Obtiene los datos
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case ComicRepositoryConstants.TagPen:
								frame.Pen = LoadPen(childML);
							break;
						case ComicRepositoryConstants.TagShape:
								frame.Shape = Mediator.ShapesRepository.LoadShape(page, childML);
							break;
						case ComicRepositoryConstants.TagContent:
								LoadBalloonContent(frame, childML);
							break;
						case ComicRepositoryConstants.TagText:
								frame.Texts.Add(LoadText(frame.Page, frame, childML));
							break;
					}
				// Devuelve el frame
				return frame;
		}

		/// <summary>
		///		Carga el contenido de un bocadillo 
		/// </summary>
		private void LoadBalloonContent(FrameModel objBalloon, MLNode nodeML)
		{
			foreach (MLNode childML in nodeML.Nodes)
				switch (childML.Name)
				{
					case ComicRepositoryConstants.TagText:
							objBalloon.Texts.Add(LoadText(objBalloon.Page, objBalloon, childML));
						break;
				}
		}

		/// <summary>
		///		Carga el contenido de un texto
		/// </summary>
		private TextModel LoadText(PageModel page, FrameModel parent, MLNode nodeML)
		{
			TextModel text = new TextModel(page, nodeML.Attributes[ComicRepositoryConstants.TagKey].Value);

				// Asigna los parámetros básicos
				Mediator.CommonRepository.AssignAttributesPageItem(nodeML, text);
				// Obtiene los valores del nodo
				text.IsBold = nodeML.Attributes[ComicRepositoryConstants.TagBold].Value.GetBool();
				text.IsItalic = nodeML.Attributes[ComicRepositoryConstants.TagItalic].Value.GetBool();
				text.FontName = nodeML.Attributes[ComicRepositoryConstants.TagFont].Value;
				text.Size = nodeML.Attributes[ComicRepositoryConstants.TagSize].Value.GetDouble(10);
				text.Color = new ColorModel(nodeML.Attributes[ComicRepositoryConstants.TagColor].Value);
				// Asigna el contenido
				if (nodeML.Nodes.Count != 0)
				{ 
					// Asigna el texto
					foreach (MLNode childML in nodeML.Nodes)
						switch (childML.Name)
						{
							case ComicRepositoryConstants.TagContent:
									text.Texts.Add(LoadText(childML.Attributes[ComicRepositoryConstants.TagLanguage].Value,
															childML.Value));
								break;
							case ComicRepositoryConstants.TagTransform:
									text.Transforms.AddRange(Mediator.TransformRepository.LoadTransforms(childML));
								break;
						}
				}
				else
					text.Texts.Add(LoadText("", nodeML.Value));
				// Asigna las dimensiones
				if (parent != null)
				{ 
					// Asigna el ancho
					if (text.Dimensions.Width == null)
						text.Dimensions.Width = parent.Dimensions.WidthDefault - text.Dimensions.LeftDefault;
					// Asigna el alto
					if (text.Dimensions.Height == null)
						text.Dimensions.Height = parent.Dimensions.HeightDefault - text.Dimensions.TopDefault;
				}
				// Devuelve el texto
				return text;
		}

		/// <summary>
		///		Carga un texto de un idioma
		/// </summary>
		private TextContentModel LoadText(string languageKey, string text)
		{
			return new TextContentModel(languageKey, text);
		}

		/// <summary>
		///		Carga los datos de un lápiz
		/// </summary>
		private PenModel LoadPen(MLNode nodeML)
		{
			PenModel pen = new PenModel();

				// Asigna los valores
				pen.Color = new ColorModel(nodeML.Attributes[ComicRepositoryConstants.TagColor].Value);
				pen.Thickness = nodeML.Attributes[ComicRepositoryConstants.TagWidth].Value.GetDouble(1);
				pen.Dots.AddRange(GetDots(nodeML.Attributes[ComicRepositoryConstants.TagDots].Value));
				pen.CapDots = GetLineCap(nodeML.Attributes[ComicRepositoryConstants.TagCapDots].Value);
				pen.StartLineCap = GetLineCap(nodeML.Attributes[ComicRepositoryConstants.TagStartLineCap].Value);
				pen.EndLineCap = GetLineCap(nodeML.Attributes[ComicRepositoryConstants.TagEndLineCap].Value);
				pen.JoinMode = GetLineJoin(nodeML.Attributes[ComicRepositoryConstants.TagJoinMode].Value);
				pen.DashOffset = nodeML.Attributes[ComicRepositoryConstants.TagDashOffset].Value.GetDouble();
				pen.MiterLimit = nodeML.Attributes[ComicRepositoryConstants.TagMiterLimit].Value.GetDouble();
				// Devuelve el lápiz
				return pen;
		}

		/// <summary>
		///		Obtiene el modo de unión de las líneas
		/// </summary>
		private PenModel.LineJoin GetLineJoin(string value)
		{
			return value.GetEnum(PenModel.LineJoin.Miter);
		}

		/// <summary>
		///		Obtiene el modo de inicio / fin de las líneas
		/// </summary>
		private PenModel.LineCap GetLineCap(string value)
		{
			return value.GetEnum(PenModel.LineCap.Flat);
		}

		/// <summary>
		///		Obtiene los puntos de la línea
		/// </summary>
		private List<double> GetDots(string value)
		{
			List<double> dots = new List<double>();

			// Carga los valores
			if (!value.IsEmpty())
			{
				string[] valueParts = value.Split(',');

					foreach (string dot in valueParts)
					{
						double? width = dot.GetDouble();

						if (width != null)
							dots.Add(width ?? 0);
					}
			}
			// Devuelve la colección
			return dots;
		}

		/// <summary>
		///		Agregrador de repository
		/// </summary>
		private ComicReaderMediator Mediator { get; }
	}
}
