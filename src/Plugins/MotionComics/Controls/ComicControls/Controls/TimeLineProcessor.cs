using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

using Bau.Libraries.LibMotionComic.Model.Components.Actions;
using Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases;

namespace Bau.Controls.ComicControls.Controls
{
	/// <summary>
	///		Procesador para un TimeLine
	/// </summary>
	internal class TimeLineProcessor
	{   
		// Eventos públicos
		public event EventHandler AnimationStart;
		public event EventHandler AnimationEnd;
		// Variables privadas
		private Storyboard sbStoryBoard;

		internal TimeLineProcessor(ComicPageView comicPageView, bool useAnimation)
		{
			PageView = comicPageView;
			UseAnimation = useAnimation;
		}

		/// <summary>
		///		Ejecuta una serie de acciones
		/// </summary>
		internal void Execute(TimeLineModel timeLine)
		{   
			// Crea el storyboard de las animaciones
			if (sbStoryBoard == null)
			{ 
				// Crea el storyBoard
				sbStoryBoard = new Storyboard();
				// Asigna el evento de fin de animación
				sbStoryBoard.Completed += (sender, evntArgs) => AnimationEnd?.Invoke(this, EventArgs.Empty);
			}
			// Limpia el storyBoard
			sbStoryBoard.Children.Clear();
			// Asigna las propiedades de duración
			sbStoryBoard.BeginTime = TimeSpan.FromSeconds(timeLine.Parameters.Start);
			sbStoryBoard.Duration = new Duration(TimeSpan.FromSeconds(timeLine.Parameters.Duration));
			// Recorre las acciones añadiéndolas al storyboard
			foreach (ActionBaseModel action in timeLine.Actions)
				if (action != null)
				{
					FrameworkElement control = PageView.GetPageControl(action.TargetKey);

						// Ejecuta la acción
						if (control != null) // && control.RenderTransform is TransformGroup)
						{
							if (action is SetVisibilityActionModel)
								ExecuteVisibility(control, action as SetVisibilityActionModel);
							else if (action is ResizeActionModel)
								ExecuteResize(timeLine, control, action as ResizeActionModel);
							else if (action is RotateActionModel)
								ExecuteRotation(control, action as RotateActionModel);
							else if (action is ZoomActionModel)
								ExecuteZoom(control, action as ZoomActionModel);
							else if (action is TranslateActionModel)
								ExecuteTranslate(control, action as TranslateActionModel);
							else if (action is PathActionModel)
								ExecutePathAnimation(control, action as PathActionModel);
							else if (action is SetZIndexModel)
								ExecuteZIndexAnimation(control, action as SetZIndexModel);
							else if (action is BrushViewBoxActionModel)
								ExecuteViewBoxAnimation(control, action as BrushViewBoxActionModel);
							else if (action is BrushRadialActionModel)
								ExecuteBrushRadial(control, action as BrushRadialActionModel);
							else if (action is BrushLinearActionModel)
								ExecuteBrushLinear(control, action as BrushLinearActionModel);
						}
				}
			// Inicia la animación
			if (sbStoryBoard.Children.Count > 0)
			{ 
				// Lanza el evento de inicio de animación
				AnimationStart?.Invoke(this, EventArgs.Empty);
				// Arranca la animación
				sbStoryBoard.Begin();
			}
		}

		/// <summary>
		///		Comprueba si se deben añadir animaciones
		/// </summary>
		private bool CheckMustAnimate(ActionBaseModel action)
		{
			return sbStoryBoard != null && UseAnimation && action.MustAnimate && action.TimeLine.MustAnimate;
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		private void ExecuteVisibility(FrameworkElement control, SetVisibilityActionModel action)
		{
			if (CheckMustAnimate(action))
				CreateDoubleAnimation(control, null, action.Opacity,
									  new PropertyPath(UIElement.OpacityProperty),
									  action);
			else
				control.Opacity = action.Opacity;
		}

		/// <summary>
		///		Cambia el tamaño de un control
		/// </summary>
		private void ExecuteResize(TimeLineModel timeLine, FrameworkElement control, ResizeActionModel action)
		{
			if (CheckMustAnimate(action))
			{
				if (action.Width != null)
					CreateDoubleAnimation(control, null, action.Width ?? 100,
										  new PropertyPath(ComicPageView.PageWidthProperty), action);
				if (action.Height != null)
					CreateDoubleAnimation(control, null, action.Height ?? 100,
										  new PropertyPath(ComicPageView.PageHeightProperty), action);
			}
			else
			{
				if (action.Width != null)
					ComicPageView.SetPageWidth(control, action.Width ?? 100);
				if (action.Height != null)
					ComicPageView.SetPageHeight(control, action.Height ?? 100);
			}
		}

		/// <summary>
		///		Zoom de una imagen
		/// </summary>
		private void ExecuteZoom(FrameworkElement control, ZoomActionModel action)
		{
			TransformGroup group = (TransformGroup) control.RenderTransform;
			int indexScale = -1;

				// Obtiene la transformación de rotación
				for (int index = 0; index < group.Children.Count; index++)
					if (group.Children[index] is ScaleTransform)
						indexScale = index;
				// Crea la transformación si no estaba en el grupo
				if (indexScale < 0)
				{
					group.Children.Add(new ScaleTransform(1, 1, 0, 0));
					indexScale = group.Children.Count - 1;
				}
				// Añade la animación
				if (CheckMustAnimate(action))
				{
					CreateDoubleAnimation(control, null, action.ScaleX,
										  new PropertyPath($"(UIElement.RenderTransform).(TransformGroup.Children)[{indexScale}].(ScaleTransform.ScaleX)"),
										  action);
					CreateDoubleAnimation(control, null, action.ScaleY,
										  new PropertyPath($"(UIElement.RenderTransform).(TransformGroup.Children)[{indexScale}].(ScaleTransform.ScaleY)"),
										  action);
				}
				else
				{ 
					// Elimina la escala
					group.Children.RemoveAt(indexScale);
					// Añade la escala
					group.Children.Add(new ScaleTransform(action.ScaleX, action.ScaleY, action.OriginX, action.OriginY));
				}
		}

		/// <summary>
		///		Ejecuta la traslación de un elemento
		/// </summary>
		private void ExecuteTranslate(FrameworkElement control, TranslateActionModel action)
		{
			TransformGroup group = (TransformGroup) control.RenderTransform;
			int indexTranslate = -1;

				// Crea la transformación de rotación si no existe
				foreach (Transform transform in group.Children)
					if (transform is TranslateTransform)
						indexTranslate = group.Children.IndexOf(transform);
				// Crea la animación
				if (indexTranslate < 0)
				{
					group.Children.Add(new TranslateTransform(0, 0));
					indexTranslate = group.Children.Count - 1;
				}
				// Añade la traslación del elemento
				if (CheckMustAnimate(action))
				{
					if (action.Top != null)
						CreateDoubleAnimation(control, null, action.Top,
											  new PropertyPath(ComicPageView.PageTopProperty),
											  action);
					if (action.Left != null)
						CreateDoubleAnimation(control, null, action.Left,
											  new PropertyPath(ComicPageView.PageLeftProperty),
											  action);
				}
				else
				{ 
					// Elimina la traslación
					group.Children.RemoveAt(indexTranslate);
					// Añade la traslación
					if (action.Left != null)
						ComicPageView.SetPageLeft(control, action.Left ?? 0);
					if (action.Top != null)
						ComicPageView.SetPageTop(control, action.Top ?? 0);
				}
		}

		/// <summary>
		///		Ejecuta el cambio de ZIndex de un control
		/// </summary>
		private void ExecuteZIndexAnimation(FrameworkElement control, SetZIndexModel action)
		{
			if (CheckMustAnimate(action))
				CreateIntAnimation(control, null, action.ZIndex,
													 new PropertyPath(System.Windows.Controls.Grid.ZIndexProperty),
													 action);
			else
				System.Windows.Controls.Grid.SetZIndex(control, action.ZIndex);
		}

		/// <summary>
		///		Ejecuta el cambio de ViewBox de una animación
		/// </summary>
		private void ExecuteViewBoxAnimation(FrameworkElement control, BrushViewBoxActionModel action)
		{
			if (action.ViewBox != null || action.ViewPort != null)
			{
				Brush brushControl = GetBrushFromControl(control);

					if (brushControl != null && brushControl is TileBrush)
					{
						TileBrush brush = brushControl as TileBrush;

							if (brush != null)
							{ 
								// Anima el viewbox
								if (action.ViewBox != null)
								{ 
									// Inicializa el viewBox
									if (brush.Viewbox == null)
									{
										brush.Viewbox = new Rect(0, 0, 1, 1);
										brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
									}
									brush.Viewbox = ViewTools.ConvertToRect(action.ViewBox);
								}
								// Anima el viewport
								if (action.ViewPort != null)
								{ 
									// Inicializa el ViewPort
									if (brush.Viewport == null)
									{
										brush.Viewport = new Rect(0, 0, 1, 1);
										brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
									}
									// Asigna el modo de relleno
									brush.TileMode = ViewTools.Convert(action.TileMode);
									brush.Viewport = ViewTools.ConvertToRect(action.ViewPort);
								}
							}
					}
			}
		}

		/// <summary>
		///		Ejecuta una animación sobre un fondo radial
		/// </summary>
		private void ExecuteBrushRadial(FrameworkElement control, BrushRadialActionModel action)
		{
			if (action.Center != null || action.Origin != null || action.RadiusX != null || action.RadiusY != null)
			{
				Brush brushControl = GetBrushFromControl(control);

					if (brushControl != null && brushControl is RadialGradientBrush)
					{
						RadialGradientBrush brush = brushControl as RadialGradientBrush;

							if (brush != null)
							{ 
								// Anima el centro
								if (action.Center != null)
									brush.Center = ViewTools.Convert(action.Center);
								// Anima el origen
								if (action.Origin != null)
									brush.GradientOrigin = ViewTools.Convert(action.Origin);
								// Anima el radio X
								if (action.RadiusX != null && action.RadiusX != brush.RadiusX)
									brush.RadiusX = action.RadiusX ?? brush.RadiusX;
								// Anima el radio Y
								if (action.RadiusY != null && action.RadiusY != brush.RadiusY)
									brush.RadiusY = action.RadiusY ?? brush.RadiusY;
							}
					}
			}
		}

		/// <summary>
		///		Ejecuta una animación sobre un fondo lineal
		/// </summary>
		private void ExecuteBrushLinear(FrameworkElement control, BrushLinearActionModel action)
		{
			if (action.Start != null || action.End != null)
			{
				Brush brushControl = GetBrushFromControl(control);

					if (brushControl != null && brushControl is LinearGradientBrush)
					{
						LinearGradientBrush brush = brushControl as LinearGradientBrush;

						if (brush != null)
						{ 
							// Cambia el modo de Spread
							brush.SpreadMethod = ViewTools.Convert(action.SpreadMethod);
							// Anima el centro
							if (action.Start != null)
								brush.StartPoint = ViewTools.Convert(action.Start);
							// Anima el origen
							if (action.End != null)
								brush.EndPoint = ViewTools.Convert(action.End);
						}
					}
			}
		}

		/// <summary>
		///		Obtiene la brocha del control
		/// </summary>
		private Brush GetBrushFromControl(FrameworkElement control)
		{
			if (control is ComicFrameView)
				return (control as ComicFrameView)?.Border.Fill;
			else if (control is ComicPageView)
				return (control as ComicPageView)?.Background;
			else
				return null;
		}

		/// <summary>
		///		Rota una imagen
		/// </summary>
		private void ExecuteRotation(FrameworkElement control, RotateActionModel action)
		{
			TransformGroup group = (TransformGroup) control.RenderTransform;
			int indexRotation = -1;

			// Cambia el origen de la transformación
			control.RenderTransformOrigin = new Point(action.OriginX, action.OriginY);
			// Crea la transformación de rotación si no existe
			for (int index = 0; index < group.Children.Count; index++)
				if (group.Children[index] is RotateTransform)
					indexRotation = index;
			// Crea la animación
			if (indexRotation < 0)
			{
				group.Children.Add(new RotateTransform(0, 0.5, 0.5));
				indexRotation = group.Children.Count - 1;
			}
			// Añade la rotación
			if (CheckMustAnimate(action))
				CreateDoubleAnimation(control, null, action.Angle,
									  new PropertyPath($"(UIElement.RenderTransform).(TransformGroup.Children)[{indexRotation}].(RotateTransform.Angle)"),
									  action);
			else
			{ 
				// Elimina la rotación
				group.Children.RemoveAt(indexRotation);
				// Añade una nueva rotación
				group.Children.Add(new RotateTransform(action.Angle, action.OriginX, action.OriginY));
			}
		}

		/// <summary>
		///		Ejecuta una animación sobre una ruta
		/// </summary>
		private void ExecutePathAnimation(FrameworkElement control, PathActionModel action)
		{
			if (CheckMustAnimate(action))
			{
				PathGeometry pathGeometry = new PathGeometry();

					// Añade la ruta a la geometría
					pathGeometry.AddGeometry(Geometry.Combine(Geometry.Empty, Geometry.Parse(action.Path),
											 GeometryCombineMode.Union, null));
					// Asigna las animaciones
					CreateDoubleAnimationUsingPath(control, action, pathGeometry, PathAnimationSource.X,
												   new PropertyPath(ComicPageView.PageTopProperty));
					CreateDoubleAnimationUsingPath(control, action, pathGeometry, PathAnimationSource.Y,
												   new PropertyPath(ComicPageView.PageLeftProperty));
			}
		}

		/// <summary>
		///		Obtiene una animación que utiliza una ruta
		/// </summary>
		private DoubleAnimationUsingPath CreateDoubleAnimationUsingPath(UIElement control, ActionBaseModel action, PathGeometry pathGeometry,
																		PathAnimationSource source, PropertyPath propertyPath)
		{
			DoubleAnimationUsingPath animation = new DoubleAnimationUsingPath();

				// Asigna las propiedades de la animación
				animation.PathGeometry = pathGeometry;
				animation.Source = source;
				// Añade la animación al storyboard
				AddAnimationToStoryBoard(control, animation, action, propertyPath);
				// Devuelve la animación
				return animation;
		}

		/// <summary>
		///		Crea una animación Double sobre un control
		/// </summary>
		private void CreateDoubleAnimation(DependencyObject control, double? from, double? to, PropertyPath propertyPath, ActionBaseModel action)
		{
			DoubleAnimation animation = new DoubleAnimation();

				// Asigna las propiedades
				if (from != null)
					animation.From = from;
				if (to != null)
					animation.To = to;
				// Asigna las funciones
				animation.EasingFunction = AssignEasingFuntion(action);
				// Añade la animación al storyBoard
				AddAnimationToStoryBoard(control, animation, action, propertyPath);
		}

		/// <summary>
		///		Crea una animación Int sobre un control
		/// </summary>
		private void CreateIntAnimation(UIElement control, int? from, int? to, PropertyPath propertyPath, ActionBaseModel action)
		{
			Int32Animation animation = new Int32Animation();

				// Asigna las propiedades
				if (from != null)
					animation.From = from;
				if (to != null)
					animation.To = to;
				// Asigna las funciones
				animation.EasingFunction = AssignEasingFuntion(action);
				// Añade la animación al storyBoard
				AddAnimationToStoryBoard(control, animation, action, propertyPath);
		}

		/// <summary>
		///		Crea la animación de punto sobre un control
		/// </summary>
		private void CreatePointAnimation(UIElement control, Point? from, Point? to, PropertyPath propertyPath, ActionBaseModel action)
		{
			PointAnimation animation = new PointAnimation();

				// Asigna las propiedades From y To de la animación
				if (from != null)
					animation.From = from;
				if (to != null)
					animation.To = to;
				// Asigna las funciones
				animation.EasingFunction = AssignEasingFuntion(action);
				// Asigna la animación al storyBoard
				AddAnimationToStoryBoard(control, animation, action, propertyPath);
		}

		/// <summary>
		///		Crea la animación de rectángulor sobre un control
		/// </summary>
		private void CreateRectangleAnimation(DependencyObject control, Rect? from, Rect to, PropertyPath propertyPath, BrushViewBoxActionModel action)
		{
			RectAnimation animation = new RectAnimation();

				// Asigna las propiedades From y To de la animación
				if (from != null)
					animation.From = from;
				if (to != null)
					animation.To = to;
				// Asigna las funciones
				animation.EasingFunction = AssignEasingFuntion(action);
				// Asigna la animación al storyboard
				AddAnimationToStoryBoard(control, animation, action, propertyPath);
		}

		/// <summary>
		///		Asigna la función asociada a la animación
		/// </summary>
		private IEasingFunction AssignEasingFuntion(ActionBaseModel action)
		{
			if (action != null && action.Eases.Count > 0)
				return GetEasingFunction(action.Eases[0]);
			else
				return null;
		}

		/// <summary>
		///		Obtiene la función asociada con la animación
		/// </summary>
		private IEasingFunction GetEasingFunction(EaseBaseModel ease)
		{
			if (ease is BounceEaseModel)
				return GetBounceEase(ease as BounceEaseModel);
			else if (ease is BackEaseModel)
				return GetBackEase(ease as BackEaseModel);
			else if (ease is CircleEaseModel)
				return GetCircleEase(ease as CircleEaseModel);
			else if (ease is CubicEaseModel)
				return GetCubicEase(ease as CubicEaseModel);
			else if (ease is ElasticEaseModel)
				return GetElasticEase(ease as ElasticEaseModel);
			else if (ease is QuadraticEaseModel)
				return GetQuadraticEase(ease as QuadraticEaseModel);
			else if (ease is QuarticEaseModel)
				return GetQuarticeEase(ease as QuarticEaseModel);
			else if (ease is SineEaseModel)
				return GetSineEase(ease as SineEaseModel);
			else if (ease is ExponentialEaseModel)
				return GetExponentialEase(ease as ExponentialEaseModel);
			else if (ease is PowerEaseModel)
				return GetPowerEase(ease as PowerEaseModel);
			else
				return null;
		}

		/// <summary>
		///		Obtiene una función Bounce
		/// </summary>
		private BounceEase GetBounceEase(BounceEaseModel bounceEase)
		{
			BounceEase ease = new BounceEase();

				// Asigna las propiedades
				ease.Bounces = bounceEase.Bounces;
				ease.Bounciness = bounceEase.Bounciness;
				ease.EasingMode = ConvertEaseMode(bounceEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función Back
		/// </summary>
		private BackEase GetBackEase(BackEaseModel backEase)
		{
			BackEase ease = new BackEase();

				// Asigna las propiedades
				ease.Amplitude = backEase.Amplitude;
				ease.EasingMode = ConvertEaseMode(backEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función Circle
		/// </summary>
		private CircleEase GetCircleEase(CircleEaseModel circleEase)
		{
			CircleEase ease = new CircleEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(circleEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función cúbica
		/// </summary>
		private CubicEase GetCubicEase(CubicEaseModel cubicEase)
		{
			CubicEase ease = new CubicEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(cubicEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función de tipo muelle para la animación
		/// </summary>
		private ElasticEase GetElasticEase(ElasticEaseModel elasticEase)
		{
			ElasticEase ease = new ElasticEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(elasticEase.EaseMode);
				ease.Oscillations = elasticEase.Oscillations;
				ease.Springiness = elasticEase.Springiness;
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función cuadrática
		/// </summary>
		private QuadraticEase GetQuadraticEase(QuadraticEaseModel quadraticEase)
		{
			QuadraticEase ease = new QuadraticEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(quadraticEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función quartic
		/// </summary>
		private QuarticEase GetQuarticeEase(QuarticEaseModel quarticEase)
		{
			QuarticEase ease = new QuarticEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(quarticEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función seno
		/// </summary>
		private SineEase GetSineEase(SineEaseModel sineEase)
		{
			SineEase ease = new SineEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(sineEase.EaseMode);
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una función exponencial
		/// </summary>
		private ExponentialEase GetExponentialEase(ExponentialEaseModel exponentialEase)
		{
			ExponentialEase ease = new ExponentialEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(exponentialEase.EaseMode);
				ease.Exponent = exponentialEase.Exponent;
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene una potencia
		/// </summary>
		private PowerEase GetPowerEase(PowerEaseModel powerEase)
		{
			PowerEase ease = new PowerEase();

				// Asigna las propiedades
				ease.EasingMode = ConvertEaseMode(powerEase.EaseMode);
				ease.Power = powerEase.Power;
				// Devuelve la función
				return ease;
		}

		/// <summary>
		///		Obtiene el modo de aplicación de una función ease
		/// </summary>
		private EasingMode ConvertEaseMode(EaseBaseModel.Mode mode)
		{
			switch (mode)
			{
				case EaseBaseModel.Mode.EaseIn:
					return EasingMode.EaseIn;
				case EaseBaseModel.Mode.EaseOut:
					return EasingMode.EaseOut;
				default:
					return EasingMode.EaseInOut;
			}
		}

		/// <summary>
		///		Añade una animación al storyBoard
		/// </summary>
		private void AddAnimationToStoryBoard(DependencyObject control, AnimationTimeline animation, ActionBaseModel action, PropertyPath propertyPath)
		{   
			// Asigna las propiedades de inicio y duración
			animation.BeginTime = TimeSpan.FromSeconds(action.Parameters.Start);
			animation.Duration = TimeSpan.FromSeconds(action.Parameters.Duration);
			// Asigna las propiedades de velocidad
			if (action.Parameters.AccelerationRatio != null)
				animation.AccelerationRatio = action.Parameters.AccelerationRatio ?? 0;
			else if (action.TimeLine.Parameters.AccelerationRatio != null)
				animation.AccelerationRatio = action.TimeLine.Parameters.AccelerationRatio ?? 0;
			if (action.Parameters.DecelerationRatio != null)
				animation.DecelerationRatio = action.Parameters.DecelerationRatio ?? 0;
			else if (action.TimeLine.Parameters.DecelerationRatio != null)
				animation.DecelerationRatio = action.TimeLine.Parameters.DecelerationRatio ?? 0;
			if (action.Parameters.SpeedRatio != null)
				animation.SpeedRatio = action.Parameters.SpeedRatio ?? 0;
			else if (action.TimeLine.Parameters.SpeedRatio != null)
				animation.SpeedRatio = action.TimeLine.Parameters.SpeedRatio ?? 0;
			// Añade los datos a la animación
			Storyboard.SetTarget(animation, control);
			Storyboard.SetTargetProperty(animation, propertyPath);
			// Añade la animación al storyboard
			sbStoryBoard.Children.Add(animation);
		}

		/// <summary>
		///		Página de la vista
		/// </summary>
		private ComicPageView PageView { get; }

		/// <summary>
		///		Indica si se debe utilizar una animación
		/// </summary>
		private bool UseAnimation { get; }
	}
}