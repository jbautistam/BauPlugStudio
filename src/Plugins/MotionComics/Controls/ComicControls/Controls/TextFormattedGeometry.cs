using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Control con una figura compuesta por texto formateado
	/// </summary>
	internal class TextFormattedGeometry : Shape
	{ 
		// Propiedades
		public static readonly DependencyProperty TextProperty =
							DependencyProperty.Register("Text", typeof(string), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(null,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty FontProperty =
							DependencyProperty.Register("Font", typeof(string), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata("Verdana",
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty BoldProperty =
							DependencyProperty.Register("Bold", typeof(bool), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(false,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty ItalicProperty =
							DependencyProperty.Register("Italic", typeof(bool), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(false,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty HighlightProperty =
							DependencyProperty.Register("Highlight", typeof(bool), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(false,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty FontSizeProperty =
							DependencyProperty.Register("FontSize", typeof(double), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(10.0,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty TextTopProperty =
							DependencyProperty.Register("TextTop", typeof(double), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(0.0,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty TextLeftProperty =
							DependencyProperty.Register("TextLeft", typeof(double), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(0.0,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty MaxTextWidthProperty =
							DependencyProperty.Register("MaxTextWidth", typeof(double), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(10.0,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		public static readonly DependencyProperty MaxTextHeightProperty =
							DependencyProperty.Register("MaxTextHeight", typeof(double), typeof(TextFormattedGeometry),
														new FrameworkPropertyMetadata(10.0,
																					  FrameworkPropertyMetadataOptions.AffectsRender,
																					  new PropertyChangedCallback(OnPropertyChanged)));
		// Variables privadas
		private Geometry _geometry = null;
		private bool _isDirty = false;

		public TextFormattedGeometry()
		{
			Highlight = false;
			Fill = Brushes.Black;
			FontSize = 20;
		}

		/// <summary>
		///		Inicializa los textos
		/// </summary>
		internal void InitView(TextModel text, string content)
		{ 
			// Guarda el modelo
			PageItem = text;
			// Asigna las propiedades
			Text = content;
			if (!string.IsNullOrEmpty(text.FontName))
				Font = text.FontName;
			FontSize = text.Size;
			Bold = text.IsBold;
			Italic = text.IsItalic;
			if (!string.IsNullOrEmpty(text.Color.Color))
				Fill = new SolidColorBrush(Color.FromArgb(text.Color.Alpha, text.Color.Red,
														  text.Color.Green, text.Color.Blue));
			// Coloca el texto
			TextTop = text.Dimensions.TopDefault;
			TextLeft = text.Dimensions.LeftDefault;
			MaxTextWidth = text.Dimensions.WidthDefault;
			MaxTextHeight = text.Dimensions.HeightDefault;
		}

		/// <summary>
		///		Crea la geometría basándose en el texto formateado
		/// </summary>
		private Geometry CreateGeometry()
		{
			FormattedText formattedText;
			FontStyle fontStyle = FontStyles.Normal;
			FontWeight fontWeight = FontWeights.Medium;
			Geometry textGeometry;

			// Obtiene el estilo del texto
			if (Bold)
				fontWeight = FontWeights.Bold;
			if (Italic)
				fontStyle = FontStyles.Italic;
			// Asigna el ancho de la fuente
			if (FontSize == 0)
				FontSize = 10;
			// Crea el texto formateado basándose en el conjunto de propiedades
			//! En este caso la brocha no importa porque simplemente se crea la geometría
			formattedText = new FormattedText(Text ?? string.Empty, System.Globalization.CultureInfo.CurrentCulture, 
											  FlowDirection.LeftToRight, 
											  new Typeface(new FontFamily(Font), fontStyle, fontWeight, FontStretches.Normal),
											  FontSize, Brushes.Black, 
											  VisualTreeHelper.GetDpi(this).PixelsPerDip);
			// Asigna el ancho máximo del texto
			if (MaxTextWidth == 0)
				formattedText.MaxTextWidth = 80;
			else
				formattedText.MaxTextWidth = MaxTextWidth;
			if (MaxTextHeight == 0)
				formattedText.MaxTextHeight = 80;
			else
				formattedText.MaxTextHeight = MaxTextHeight;
			// Obtiene la geometría del texto
			if (Highlight)
				textGeometry = formattedText.BuildHighlightGeometry(new Point(TextLeft, TextTop));
			else
				textGeometry = formattedText.BuildGeometry(new Point(TextLeft, TextTop));
			// Asigna las transformaciones
			textGeometry.Transform = ViewTransformTools.GetTransforms(PageItem.Transforms);
			// Devuelve la geometría
			return textGeometry;

			/*
					string testString = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor";

					// Create the initial formatted text string.
					FormattedText formattedText = new FormattedText(
							testString,
							CultureInfo.GetCultureInfo("en-us"),
							FlowDirection.LeftToRight,
							new Typeface("Verdana"),
							32,
							Brushes.Black);

					// Set a maximum width and height. If the text overflows these values, an ellipsis "..." appears.
					formattedText.MaxTextWidth = 300;
					formattedText.MaxTextHeight = 240;

					// Use a larger font size beginning at the first (zero-based) character and continuing for 5 characters.
					// The font size is calculated in terms of points -- not as device-independent pixels.
					formattedText.SetFontSize(36 * (96.0 / 72.0), 0, 5);

					// Use a Bold font weight beginning at the 6th character and continuing for 11 characters.
					formattedText.SetFontWeight(FontWeights.Bold, 6, 11);

					// Use a linear gradient brush beginning at the 6th character and continuing for 11 characters.
					formattedText.SetForegroundBrush(
																	new LinearGradientBrush(
																	Colors.Orange,
																	Colors.Teal,
																	90.0),
																	6, 11);

					// Use an Italic font style beginning at the 28th character and continuing for 28 characters.
					formattedText.SetFontStyle(FontStyles.Italic, 28, 28);

					// Draw the formatted text string to the DrawingContext of the control.
					drawingContext.DrawText(formattedText, new Point(10, 0));
			*/
		}

		/// <summary>
		///		Callback para los cambios en propiedades
		/// </summary>
		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as TextFormattedGeometry)._isDirty = true;
		}

		/// <summary>
		///		Elemento de la página
		/// </summary>
		private TextModel PageItem { get; set; }

		/// <summary>
		///		Texto
		/// </summary>
		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		///		Fuente
		/// </summary>
		public string Font
		{
			get { return (string) GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

		/// <summary>
		///		Tamaño de la fuente
		/// </summary>
		public double FontSize
		{
			get { return (double) GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		/// <summary>
		///		Negrita
		/// </summary>
		public bool Bold
		{
			get { return (bool) GetValue(BoldProperty); }
			set { SetValue(BoldProperty, value); }
		}

		/// <summary>
		///		Cursiva
		/// </summary>
		public bool Italic
		{
			get { return (bool) GetValue(ItalicProperty); }
			set { SetValue(ItalicProperty, value); }
		}

		/// <summary>
		///		Indica si se recoge el borde externo en la geometría
		/// </summary>
		public bool Highlight
		{
			get { return (bool) GetValue(HighlightProperty); }
			set { SetValue(HighlightProperty, value); }
		}

		/// <summary>
		///		Posición superior del texto
		/// </summary>
		public double TextTop
		{
			get { return (double) GetValue(TextTopProperty); }
			set { SetValue(TextTopProperty, value); }
		}

		/// <summary>
		///		Posición superior izquierda del texto
		/// </summary>
		public double TextLeft
		{
			get { return (double) GetValue(TextLeftProperty); }
			set { SetValue(TextLeftProperty, value); }
		}

		/// <summary>
		///		Ancho máximo del texto
		/// </summary>
		public double MaxTextWidth
		{
			get { return (double) GetValue(MaxTextWidthProperty); }
			set { SetValue(MaxTextWidthProperty, value); }
		}

		/// <summary>
		///		Alto máximo del texto
		/// </summary>
		public double MaxTextHeight
		{
			get { return (double) GetValue(MaxTextHeightProperty); }
			set { SetValue(MaxTextHeightProperty, value); }
		}

		/// <summary>
		///		Obtiene la geometría del texto / figura
		/// </summary>
		protected override Geometry DefiningGeometry
		{
			get
			{ 
				// Si no hay geometría o se ha cambiado, se calcula de nuevo
				if (_geometry == null || _isDirty)
					_geometry = CreateGeometry();
				// Devuelve la geometría creada
				return _geometry;
			}
		}
	}
}
