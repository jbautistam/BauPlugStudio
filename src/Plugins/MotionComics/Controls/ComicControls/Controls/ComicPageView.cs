using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Actions;
using Bau.Libraries.LibMotionComic.Model.Components.PageItems;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Control para mostrar una página de un cómic
	/// </summary>
	internal class ComicPageView : Grid
	{ 
		// Propiedades
		public static readonly DependencyProperty UseAnimationProperty =
							DependencyProperty.Register("UseAnimation", typeof(bool), typeof(ComicPageView),
														new FrameworkPropertyMetadata(true,
																					  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static readonly DependencyProperty ShowAdornersProperty =
							DependencyProperty.Register("ShowAdorners", typeof(bool), typeof(ComicPageView),
													    new FrameworkPropertyMetadata(false,
																					  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		public static readonly DependencyProperty AbsoluteWidthProperty =
							DependencyProperty.Register("AbsoluteWidth", typeof(double), typeof(ComicPageView),
														new FrameworkPropertyMetadata(1500.0,
																					  FrameworkPropertyMetadataOptions.AffectsArrange));
		public static readonly DependencyProperty AbsoluteHeightProperty =
							DependencyProperty.Register("AbsoluteHeight", typeof(double), typeof(ComicPageView),
														new FrameworkPropertyMetadata(3000.0,
																					  FrameworkPropertyMetadataOptions.AffectsArrange));
		// Propiedades de idioma
		public static readonly DependencyProperty LanguageSelectedProperty =
							DependencyProperty.Register("LanguageSelected", typeof(string), typeof(ComicPageView));
		public static readonly DependencyProperty LanguageDefaultProperty =
							DependencyProperty.Register("LanguageDefault", typeof(string), typeof(ComicPageView));
		// Propiedades adjuntas
		public static readonly DependencyProperty PageTopProperty =
							DependencyProperty.RegisterAttached("PageTop", typeof(double), typeof(ComicPageView),
																new FrameworkPropertyMetadata((double) double.NaN,
																							  FrameworkPropertyMetadataOptions.AffectsMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsArrange |
																							  FrameworkPropertyMetadataOptions.AffectsParentMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty PageLeftProperty =
							DependencyProperty.RegisterAttached("PageLeft", typeof(double), typeof(ComicPageView),
																new FrameworkPropertyMetadata((double) double.NaN,
																							  FrameworkPropertyMetadataOptions.AffectsMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsArrange |
																							  FrameworkPropertyMetadataOptions.AffectsParentMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty PageWidthProperty =
							DependencyProperty.RegisterAttached("PageWidth", typeof(double), typeof(ComicPageView),
																new FrameworkPropertyMetadata((double) double.NaN,
																							  FrameworkPropertyMetadataOptions.AffectsMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsArrange |
																							  FrameworkPropertyMetadataOptions.AffectsParentMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsParentArrange));
		public static readonly DependencyProperty PageHeightProperty =
							DependencyProperty.RegisterAttached("PageHeight", typeof(double), typeof(ComicPageView),
																new FrameworkPropertyMetadata((double) double.NaN,
																							  FrameworkPropertyMetadataOptions.AffectsMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsArrange |
																							  FrameworkPropertyMetadataOptions.AffectsParentMeasure |
																							  FrameworkPropertyMetadataOptions.AffectsParentArrange));
		// Eventos públicos
		public event EventHandler StartAnimation;
		public event EventHandler EndAnimation;
		// Variables privadas
		private TimeLineProcessor _animationProcessor = null;

		/// <summary>
		///		Carga los datos de una página
		/// </summary>
		public void LoadPage(PageModel page)
		{   
			// Guarda la página
			AbsoluteWidth = page.Comic.Width;
			AbsoluteHeight = page.Comic.Height;
			// Comienza el proceso
			BeginInit();
			// Limpia las imágenes
			Clear();
			// Asigna el fondo
			if (page.Brush != null)
				Background = ViewTools.GetBrush(page, page.Brush);
			else
				Background = null;
			// Añade las imágenes de la página
			foreach (AbstractPageItemModel pageContent in page.Content)
				if (pageContent is ImageModel)
					AddImage(pageContent as ImageModel);
				else if (pageContent is FrameModel)
					AddFrame(pageContent as FrameModel);
				else if (pageContent is TextModel)
					AddText(pageContent as TextModel);
			// Finaliza el proceso
			EndInit();
		}

		/// <summary>
		///		Limpia el control
		/// </summary>
		public void Clear()
		{
			Children.Clear();
		}

		/// <summary>
		///		Ejecuta las acciones de una línea de tiempo
		/// </summary>
		internal void Execute(TimeLineModel timeLine)
		{ 
			// Si no existía ninguna animación se crea
			if (_animationProcessor == null)
			{ 
				// Crea el objeto
				_animationProcessor = new TimeLineProcessor(this, UseAnimation);
				// Asigna los manejadores de eventos
				_animationProcessor.AnimationStart += (sender, evntArgs) =>
							{
								StartAnimation?.Invoke(this, EventArgs.Empty);
								IsPlayingAnimation = true;
							};
				_animationProcessor.AnimationEnd += (sender, evntArgs) =>
							{
								EndAnimation?.Invoke(this, EventArgs.Empty);
								IsPlayingAnimation = false;
							};
			}
			// Ejecuta la animación
			_animationProcessor.Execute(timeLine);
		}

		/// <summary>
		///		Añade un control de imagen
		/// </summary>
		private void AddImage(ImageModel image)
		{
			Image view = new Image();

				// Carga la imagen
				view.Source = ViewTools.LoadImage(image);
				view.Stretch = Stretch.Fill;
				// Asigna las dimensiones a la imagen
				if (image.Dimensions.Width == null)
					image.Dimensions.Width = view.Source.Width;
				if (image.Dimensions.Height == null)
					image.Dimensions.Height = view.Source.Height;
				// Añade el control
				AddControl(view, image);
		}

		/// <summary>
		///		Añade un frame a la página
		/// </summary>
		private void AddFrame(FrameModel shape)
		{
			ComicFrameView view = new ComicFrameView();

				// Carga la figura
				view.InitShape(shape, LanguageSelected, LanguageDefault);
				// Añade el control
				AddControl(view, shape);
		}

		/// <summary>
		///		Añade un control con un texto
		/// </summary>
		private void AddText(TextModel text)
		{
			TextFormattedGeometry view = new TextFormattedGeometry();

				// Carga el texto
				view.InitView(text, text.Texts.GetText(LanguageSelected, LanguageDefault));
				// Añade el control
				AddControl(view, text);
		}

		/// <summary>
		///		Añade un control
		/// </summary>
		private void AddControl(UIElement control, AbstractPageItemModel pageItem)
		{   
			// Le asigna la clave al control
			(control as FrameworkElement).Tag = pageItem.Key;
			// Cambia la visibilidad y opacidad del control
			if (pageItem.Visible)
				control.Opacity = 1;
			else
				control.Opacity = 0;
			if (pageItem.Opacity != 0)
				control.Opacity = pageItem.Opacity;
			// Inicializa un grupo de transformaciones
			control.RenderTransform = new TransformGroup();
			// Asigna las propiedades
			ComicPageView.SetPageTop(control, pageItem.Dimensions.TopDefault);
			ComicPageView.SetPageLeft(control, pageItem.Dimensions.LeftDefault);
			ComicPageView.SetPageWidth(control, pageItem.Dimensions.WidthDefault);
			ComicPageView.SetPageHeight(control, pageItem.Dimensions.HeightDefault);
			Grid.SetZIndex(control, pageItem.ZIndex);
			// Añade el control a la vista
			Children.Add(control);
			// Añade el adorno si es necesario
			if (ShowAdorners)
				AdornerLayer.GetAdornerLayer(control)?.Add(new FourCornersAdorner(control));
		}

		/// <summary>
		///		Obtiene un control
		/// </summary>
		internal FrameworkElement GetPageControl(string key)
		{ 
			// Busca la imagen
			if (key.EqualsIgnoreCase("Page"))
				return this;
			else
				foreach (UIElement control in Children)
					if (((control as FrameworkElement)?.Tag as string) == key)
						return control as FrameworkElement;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return null;
		}

		/// <summary>
		///		Sobrescribe el método que mide el tamaño deseado
		/// </summary>
		protected override Size MeasureOverride(Size constraint)
		{
			Size desiredSize = new Size(0, 0);

				// Recorre los elementos hijo calculando el tamaño máximo deseado
				foreach (UIElement child in Children)
				{
					Rect scaled = GetScaledRectangle(child, constraint.Width, constraint.Height);

						// Mide el tamaño
						child.Measure(new Size(scaled.Width, scaled.Height));
						// Obtiene el ancho máximo hasta ahora
						desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
						desiredSize.Height = Math.Max(desiredSize.Height, child.DesiredSize.Height);
				}
				// Normaliza el ancho máximo 
				if (double.IsPositiveInfinity(constraint.Width))
					desiredSize.Width = desiredSize.Width;
				else
					desiredSize.Width = constraint.Width;
				// Normaliza el alto máximo
				if (double.IsPositiveInfinity(constraint.Height))
					desiredSize.Height = desiredSize.Height;
				else
					desiredSize.Height = constraint.Height;
				// Devuelve el tamaño obtenido
				return desiredSize;
		}

		/// <summary>
		///		Sobrescribe el método que coloca los controles
		/// </summary>
		protected override Size ArrangeOverride(Size arrangeSize)
		{ 
			// Coloca los controles
			foreach (UIElement child in Children)
			{
				Rect newRectangle = GetScaledRectangle(child, arrangeSize.Width, arrangeSize.Height);

					// Coloca el control
					child.Arrange(newRectangle);
			}
			// Devuelve el tamaño
			return arrangeSize;
		}

		/// <summary>
		///		Obtiene un rectángulo escalado
		/// </summary>
		private Rect GetScaledRectangle(UIElement control, double viewPortWidth, double viewPortHeight)
		{
			double controlTop = GetPageTop(control);
			double controlLeft = GetPageLeft(control);
			double controlWidth = GetPageWidth(control);
			double controlHeight = GetPageHeight(control);

				// Obtiene el rectángulo
				if (!double.IsNaN(controlTop) && !double.IsNaN(controlLeft) &&
						!double.IsNaN(controlWidth) && !double.IsNaN(controlHeight))
					return new Rect(GetScaledX(controlLeft, viewPortWidth),
									GetScaledY(controlTop, viewPortHeight),
									GetScaledX(controlWidth, viewPortWidth),
									GetScaledY(controlHeight, viewPortHeight));
				else
					return new Rect(0, 0, viewPortWidth, viewPortHeight);
		}

		/// <summary>
		///		Obtiene una X escalada
		/// </summary>
		private double GetScaledX(double x, double xViewPort)
		{
			return (x / AbsoluteWidth) * xViewPort;
		}

		/// <summary>
		///		Obtiene una Y escalada
		/// </summary>
		private double GetScaledY(double y, double yViewPort)
		{
			return (y / AbsoluteHeight) * yViewPort;
		}

		/// <summary>
		///		Obtiene el valor de la propiedad adjunta "PageTop"
		/// </summary>
		public static double GetPageTop(DependencyObject dependency)
		{
			return (double) dependency.GetValue(PageTopProperty);
		}

		/// <summary>
		///		Asigna el valor de la propiedad adjunta "PageTop"
		/// </summary>
		public static void SetPageTop(DependencyObject dependency, double value)
		{
			dependency.SetValue(PageTopProperty, value);
		}

		/// <summary>
		///		Obtiene el valor de la propiedad adjunta "PageLeft"
		/// </summary>
		public static double GetPageLeft(DependencyObject dependency)
		{
			return (double) dependency.GetValue(PageLeftProperty);
		}

		/// <summary>
		///		Asigna el valor de la propiedad adjunta "PageLeft"
		/// </summary>
		public static void SetPageLeft(DependencyObject dependency, double value)
		{
			dependency.SetValue(PageLeftProperty, value);
		}

		/// <summary>
		///		Obtiene el valor de la propiedad adjunta "PageWidth"
		/// </summary>
		public static double GetPageWidth(DependencyObject dependency)
		{
			return (double) dependency.GetValue(PageWidthProperty);
		}

		/// <summary>
		///		Asigna el valor de la propiedad adjunta "PageWidth"
		/// </summary>
		public static void SetPageWidth(DependencyObject dependency, double value)
		{
			dependency.SetValue(PageWidthProperty, value);
		}

		/// <summary>
		///		Obtiene el valor de la propiedad adjunta "PageHeight"
		/// </summary>
		public static double GetPageHeight(DependencyObject dependency)
		{
			return (double) dependency.GetValue(PageHeightProperty);
		}

		/// <summary>
		///		Asigna el valor de la propiedad adjunta "PageHeight"
		/// </summary>
		public static void SetPageHeight(DependencyObject dependency, double value)
		{
			dependency.SetValue(PageHeightProperty, value);
		}

		/// <summary>
		///		Ancho absoluto total
		/// </summary>
		public double AbsoluteWidth
		{
			get { return (double) GetValue(AbsoluteWidthProperty); }
			set { SetValue(AbsoluteWidthProperty, value); }
		}

		/// <summary>
		///		Alto absoluto total
		/// </summary>
		public double AbsoluteHeight
		{
			get { return (double) GetValue(AbsoluteHeightProperty); }
			set { SetValue(AbsoluteHeightProperty, value); }
		}

		/// <summary>
		///		Idioma seleccionado
		/// </summary>
		public string LanguageSelected
		{
			get { return (string) GetValue(LanguageSelectedProperty); }
			set { SetValue(LanguageSelectedProperty, value); }
		}

		/// <summary>
		///		Idioma predeterminado
		/// </summary>
		public string LanguageDefault
		{
			get { return (string) GetValue(LanguageDefaultProperty); }
			set { SetValue(LanguageDefaultProperty, value); }
		}

		/// <summary>
		///		Indica si se deben mostrar o no los ardorners sobre los controles
		/// </summary>
		public bool ShowAdorners
		{
			get { return (bool) GetValue(ShowAdornersProperty); }
			set { SetValue(ShowAdornersProperty, value); }
		}

		/// <summary>
		///		Indica si se deben animar las páginas
		/// </summary>
		public bool UseAnimation
		{
			get { return (bool) GetValue(UseAnimationProperty); }
			set { SetValue(UseAnimationProperty, value); }
		}

		/// <summary>
		///		Indica si está ejecutando la animación
		/// </summary>
		public bool IsPlayingAnimation { get; private set; } = false;
	}
}
