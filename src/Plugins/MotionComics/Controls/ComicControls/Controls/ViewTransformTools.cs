using System;
using System.ComponentModel;
using System.Windows.Media;

using Bau.Libraries.LibMotionComic.Model.Components.Transforms;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Herramientas para transformaciones de vistas / controles
	/// </summary>
	internal static class ViewTransformTools
	{
		/// <summary>
		///		Obtiene las transformaciones de la figura
		/// </summary>
		internal static Transform GetTransforms(TransformModelCollection transforms)
		{
			Transform transform = new TranslateTransform(0, 0);

				// Obtiene las transformaciones de la figura
				if (transforms != null && transforms.Count > 0)
				{ 
					// Crea un grupo de transformaciones
					transform = new TransformGroup();
					// Añade las transformaciones de la figura
					foreach (AbstractTransformModel figureTransform in transforms)
						if (figureTransform is TranslateTransformModel)
							(transform as TransformGroup).Children.Add(ConvertTransform(figureTransform as TranslateTransformModel));
						else if (figureTransform is MatrixTransformModel)
							(transform as TransformGroup).Children.Add(ConvertTransform(figureTransform as MatrixTransformModel));
						else if (figureTransform is RotateTransformModel)
							(transform as TransformGroup).Children.Add(ConvertTransform(figureTransform as RotateTransformModel));
						else if (figureTransform is ScaleTransformModel)
							(transform as TransformGroup).Children.Add(ConvertTransform(figureTransform as ScaleTransformModel));
				}
				// Devuelve la transformación
				return transform;
		}

		/// <summary>
		///		Convierte una transformación de escala
		/// </summary>
		private static Transform ConvertTransform(ScaleTransformModel transform)
		{
			return new ScaleTransform(transform.ScaleX, transform.ScaleY, transform.Center.X, transform.Center.Y);
		}

		/// <summary>
		///		Convierte una rotación
		/// </summary>
		private static Transform ConvertTransform(RotateTransformModel transform)
		{
			return new RotateTransform(transform.Angle, transform.Origin.X, transform.Origin.Y);
		}

		/// <summary>
		///		Convierte una transformación de traslación
		/// </summary>
		private static Transform ConvertTransform(TranslateTransformModel transform)
		{
			return new TranslateTransform(transform.Left, transform.Top);
		}

		/// <summary>
		///		Convierte una transformación de matriz
		/// </summary>
		private static Transform ConvertTransform(MatrixTransformModel transform)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(Matrix));

				// Obtiene una transformación de matriz
				return new MatrixTransform((Matrix) converter.ConvertFrom(transform.Data));
		}
	}
}
