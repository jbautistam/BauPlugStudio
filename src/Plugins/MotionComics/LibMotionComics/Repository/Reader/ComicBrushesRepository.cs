using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems.Brushes;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Clase para carga de brochas
	/// </summary>
	internal class ComicBrushesRepository
	{
		internal ComicBrushesRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		///		Carga la primera brocha de una serie de nodos
		/// </summary>
		internal AbstractBaseBrushModel LoadFirstBrush(MLNode nodeML)
		{
			AbstractBaseBrushModel brush = null;

				// Carga los datos de la brocha (sólo recoge la primera de todas las que puede haber)
				foreach (MLNode childML in nodeML.Nodes)
					if (brush == null)
						brush = LoadBrush(childML);
				// Devuelve la brocha
				return brush;
		}

		/// <summary>
		///		Carga las datos de una brocha
		/// </summary>
		internal AbstractBaseBrushModel LoadBrush(MLNode nodeML)
		{ // Carga los datos de la brocha
			switch (nodeML.Name)
			{
				case ComicRepositoryConstants.TagSolidBrush:
					return LoadSolidBrush(nodeML);
				case ComicRepositoryConstants.TagImageBrush:
					return LoadImageBrush(nodeML);
				case ComicRepositoryConstants.TagRadialBrush:
					return LoadRadialBrush(nodeML);
				case ComicRepositoryConstants.TagLinearBrush:
					return LoadLinearBrush(nodeML);
			}
			// Devuelve la brocha
			return null;
		}

		/// <summary>
		///		Carga los datos de una brocha sólida
		/// </summary>
		private SolidBrushModel LoadSolidBrush(MLNode nodeML)
		{
			return new SolidBrushModel(nodeML.Attributes[ComicRepositoryConstants.TagKey].Value,
									   nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value,
									   nodeML.Attributes[ComicRepositoryConstants.TagColor].Value);
		}

		/// <summary>
		///		Carga los datos de una brocha de imagen
		/// </summary>
		private ImageBrushModel LoadImageBrush(MLNode nodeML)
		{
			return new ImageBrushModel(nodeML.Attributes[ComicRepositoryConstants.TagKey].Value,
									   nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value,
									   nodeML.Attributes[ComicRepositoryConstants.TagFileName].Value,
									   Mediator.CommonRepository.GetRectangle(nodeML.Attributes[ComicRepositoryConstants.TagViewBox].Value),
									   Mediator.CommonRepository.GetRectangle(nodeML.Attributes[ComicRepositoryConstants.TagViewPort].Value),
									   Mediator.CommonRepository.ConvertTile(nodeML.Attributes[ComicRepositoryConstants.TagTileMode].Value),
									   Mediator.CommonRepository.GetStretchMode(nodeML.Attributes[ComicRepositoryConstants.TagStretch].Value));
		}

		/// <summary>
		///		Carga un gradiante circular
		/// </summary>
		private RadialGradientBrushModel LoadRadialBrush(MLNode nodeML)
		{
			RadialGradientBrushModel radial = new RadialGradientBrushModel(nodeML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		   nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value);

				// Carga los datos del gradiante
				radial.Center = Mediator.CommonRepository.GetPoint(nodeML.Attributes[ComicRepositoryConstants.TagCenter].Value);
				radial.RadiusX = nodeML.Attributes[ComicRepositoryConstants.TagRadiusX].Value.GetDouble(1);
				radial.RadiusY = nodeML.Attributes[ComicRepositoryConstants.TagRadiusY].Value.GetDouble(1);
				radial.Origin = Mediator.CommonRepository.GetPoint(nodeML.Attributes[ComicRepositoryConstants.TagOrigin].Value);
				radial.SpreadMethod = Mediator.CommonRepository.GetSpreadMethod(nodeML.Attributes[ComicRepositoryConstants.TagSpread].Value);
				// Asigna los puntos de parada
				radial.Stops.AddRange(LoadStops(nodeML));
				// Devuelve el gradiante
				return radial;
		}

		/// <summary>
		///		Carga un gradiante lineal
		/// </summary>
		private LinearGradientBrushModel LoadLinearBrush(MLNode nodeML)
		{
			LinearGradientBrushModel linear = new LinearGradientBrushModel(nodeML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		   nodeML.Attributes[ComicRepositoryConstants.TagResourceKey].Value);

				// Carga los datos del gradiante
				linear.Start = Mediator.CommonRepository.GetPoint(nodeML.Attributes[ComicRepositoryConstants.TagStart].Value);
				linear.End = Mediator.CommonRepository.GetPoint(nodeML.Attributes[ComicRepositoryConstants.TagEnd].Value);
				linear.SpreadMethod = Mediator.CommonRepository.GetSpreadMethod(nodeML.Attributes[ComicRepositoryConstants.TagSpread].Value);
				// Añade los puntos de parada
				linear.Stops.AddRange(LoadStops(nodeML));
				// Devuelve el gradiante
				return linear;
		}

		/// <summary>
		///		Carga los puntos de parada
		/// </summary>
		private System.Collections.Generic.List<GradientStopModel> LoadStops(MLNode nodeML)
		{
			System.Collections.Generic.List<GradientStopModel> stops = new System.Collections.Generic.List<GradientStopModel>();

				// Añade los puntos de parada
				foreach (MLNode childML in nodeML.Nodes)
					stops.Add(new GradientStopModel(Mediator.CommonRepository.GetColor(childML.Attributes[ComicRepositoryConstants.TagColor].Value),
																					   childML.Attributes[ComicRepositoryConstants.TagOffset].Value.GetDouble(0)));
				// Devuelve la colección de puntos
				return stops;
		}

		/// <summary>
		///		Mediator
		/// </summary>
		private ComicReaderMediator Mediator { get; }
	}
}
