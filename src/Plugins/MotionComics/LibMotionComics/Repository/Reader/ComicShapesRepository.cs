using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;
using Bau.Libraries.LibMotionComic.Model.Components.Transforms;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para las figuras
	/// </summary>
	internal class ComicShapesRepository
	{
		internal ComicShapesRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		///		Carga una figura
		/// </summary>
		internal ShapeModel LoadShape(PageModel page, MLNode nodeML)
		{
			ShapeModel shape = new ShapeModel(page, nodeML.Attributes[ComicRepositoryConstants.TagKey].Value);

				// Asigna los atributos básicos
				Mediator.CommonRepository.AssignAttributesPageItem(nodeML, shape);
				// Asigna el resto de propiedades
				shape.ComponentKey = nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value;
				shape.FillMode = GetFillMode(nodeML.Attributes[ComicRepositoryConstants.TagFillRule].Value);
				// Carga las figuras
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case ComicRepositoryConstants.TagFigure:
								shape.Figures.Add(Mediator.ShapesRepository.LoadFigure(shape, childML));
							break;
						case ComicRepositoryConstants.TagTransform:
								shape.Transforms.AddRange(Mediator.ShapesRepository.LoadTransforms(childML));
							break;
					}
				// Devuelve la figura
				return shape;
		}

		/// <summary>
		///		Obtiene el modo de relleno
		/// </summary>
		private ShapeModel.FillRule GetFillMode(string value)
		{
			if (value.EqualsIgnoreCase("None"))
				return ShapeModel.FillRule.None;
			else if (value.EqualsIgnoreCase("NonZero"))
				return ShapeModel.FillRule.NonZero;
			else
				return ShapeModel.FillRule.EvenOdd;
		}

		/// <summary>
		///		Carga los datos de una figura
		/// </summary>
		internal FigureModel LoadFigure(AbstractComponentModel pageItem, MLNode nodeML)
		{
			FigureModel figure = new FigureModel();

				// Asigna los datos
				figure.FillMode = GetFillMode(nodeML.Attributes[ComicRepositoryConstants.TagFillRule].Value);
				// Asigna las propiedades
				figure.Brush = Mediator.BrushesRepository.LoadFirstBrush(nodeML);
				// Asigna el contenido de la figura
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case ComicRepositoryConstants.TagData:
								// Asigna los datos de la figura
								figure.Data = childML.Value;
								// Normaliza la cadena de datos
								figure.Data = Mediator.CommonRepository.Normalize(figure.Data);
							break;
						case ComicRepositoryConstants.TagTransform:
								figure.Transforms.AddRange(Mediator.ShapesRepository.LoadTransforms(childML));
							break;
					}
				// Devuelve la figura cargada
				return figure;
		}

		/// <summary>
		///		Carga una serie de transformaciones
		/// </summary>
		internal TransformModelCollection LoadTransforms(MLNode nodeML)
		{
			TransformModelCollection transforms = new TransformModelCollection();

				// Carga las transformaciones
				foreach (MLNode childML in nodeML.Nodes)
					switch (childML.Name)
					{
						case ComicRepositoryConstants.TagTranslate:
								transforms.Add(new TranslateTransformModel(childML.Attributes[ComicRepositoryConstants.TagTop].Value.GetDouble(0),
																		   childML.Attributes[ComicRepositoryConstants.TagLeft].Value.GetDouble(0)));
							break;
						case ComicRepositoryConstants.TagMatrix:
								transforms.Add(new MatrixTransformModel(childML.Attributes[ComicRepositoryConstants.TagData].Value));
							break;
						case ComicRepositoryConstants.TagRotate:
								transforms.Add(new RotateTransformModel(childML.Attributes[ComicRepositoryConstants.TagAngle].Value.GetDouble(0),
																		Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagOrigin].Value,
																										   0.5, 0.5)));
							break;
						case ComicRepositoryConstants.TagScale:
								transforms.Add(new ScaleTransformModel(childML.Attributes[ComicRepositoryConstants.TagScaleX].Value.GetDouble(1),
																	   childML.Attributes[ComicRepositoryConstants.TagScaleY].Value.GetDouble(1),
																	   Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagCenter].Value,
																										  0.5, 0.5)));
							break;
					}
				// Devuelve la colección de transformaciones
				return transforms;
		}

		/// <summary>
		///		Agregador de repository
		/// </summary>
		private ComicReaderMediator Mediator { get; }
	}
}
