using System;
using System.Windows.Media;
using System.Windows.Shapes;

using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Control para mostrar una figura de un cómic
	/// </summary>
	internal class ComicShapeView : Shape
	{ 
		// Variables privadas
		private ShapeModel _shape = null;
		private Geometry _geometry = null;
		private bool _isDirty = false;

		public ComicShapeView(ShapeModel shape)
		{
			_shape = shape;
			_geometry = CreateGeometry();
		}

		/// <summary>
		///		Crea la geometría
		/// </summary>
		private Geometry CreateGeometry()
		{
			PathGeometry pathGeometry = new PathGeometry();

				// Obtiene la figura de los recursos si es necesario
				_shape = GetShape(_shape);
				// Añade las figuras a la geometría
				foreach (FigureModel figure in _shape.Figures)
					pathGeometry.AddGeometry(Geometry.Combine(Geometry.Empty, Geometry.Parse(figure.Data),
															  GeometryCombineMode.Union,
															  ViewTransformTools.GetTransforms(figure.Transforms)));
				// Asigna el método de relleno
				pathGeometry.FillRule = ViewTools.Convert(_shape.FillMode);
				// Añade las geometrías a la geometría principal
				//! Esto no parece añadir nada en especial
				pathGeometry.Transform = ViewTransformTools.GetTransforms(_shape.Transforms);
				// Devuelve la geometría creada
				return pathGeometry;
		}

		/// <summary>
		///		Obtiene la definición de la figura
		/// </summary>
		private ShapeModel GetShape(ShapeModel shape)
		{ 
			// Obtiene las figuras buscando entre los recursos
			if (shape != null && !string.IsNullOrEmpty(shape.ComponentKey))
			{
				AbstractComponentModel resource = shape.Page.Comic.Resources.Search(shape.ComponentKey);

					// Obtiene los datos del recurso
					if (resource != null && resource is ShapeModel)
					{
						ShapeModel resourceShape = resource as ShapeModel;

							// Obtiene el modo de relleno
							shape.FillMode = resourceShape.FillMode;
							// Obtiene las figuras
							shape.Figures.Clear();
							shape.Figures.AddRange(resourceShape.Figures);
							// Añade las transformaciones
							shape.Transforms.InsertRange(0, resourceShape.Transforms);
					}
			}
			// Devuelve la figura
			return shape;
		}

		/// <summary>
		///		Geometría de la figura
		/// </summary>
		protected override Geometry DefiningGeometry
		{
			get
			{ 
				// Crea la geometría si no existía
				if (_geometry == null || _isDirty)
					_geometry = CreateGeometry();
				// Devuelve la geometría
				return _geometry;
			}
		}
	}
}