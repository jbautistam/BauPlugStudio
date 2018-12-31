using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		View para mostrar un frame 
	/// </summary>
	internal class ComicFrameView : Grid
	{
		/// <summary>
		///		Inicializa la figura
		/// </summary>
		public void InitShape(FrameModel frame, string languageSelected, string languageDefault)
		{ 
			// Asigna el borde del control
			if (frame.Shape != null)
				Border = new ComicShapeView(frame.Shape);
			else
			{
				Border = new Rectangle();
				if (frame.RadiusX != null)
					(Border as Rectangle).RadiusX = frame.RadiusX ?? 0;
				if (frame.RadiusY != null)
					(Border as Rectangle).RadiusY = frame.RadiusY ?? 0;
			}
			Border.Stretch = Stretch.Fill;
			// Asigna el fondo
			Border.Fill = ViewTools.GetBrush(frame, frame.Brush);
			// Asigna el borde
			ViewTools.AssignPen(Border, frame.Pen);
			// Añade la figura al canvas
			Children.Add(Border);
			// Asigna la posición a la figura
			Grid.SetRow(Border, 0);
			Grid.SetColumn(Border, 0);
			// Añade los textos
			InitTexts(frame, languageSelected, languageDefault);
		}

		/// <summary>
		///		Inicializa los textos
		/// </summary>
		private void InitTexts(FrameModel frame, string languageSelected, string languageDefault)
		{
			foreach (TextModel text in frame.Texts)
			{
				TextFormattedGeometry textGeometry = new TextFormattedGeometry();

					// Asigna las propiedades
					textGeometry.InitView(text, text.Texts.GetText(languageSelected, languageDefault));
					// Añade el texto al control
					Children.Add(textGeometry);
			}
		}

		/// <summary>
		///		Mueve el fondo
		/// </summary>
		internal void MoveBrush(double top, double left)
		{
			ImageBrush brush = Border.Fill as ImageBrush;

				if (brush != null)
					brush.Transform = new TranslateTransform(left, top);
		}

		/// <summary>
		///		Figura del borde
		/// </summary>
		public Shape Border { get; set; }
	}
}
