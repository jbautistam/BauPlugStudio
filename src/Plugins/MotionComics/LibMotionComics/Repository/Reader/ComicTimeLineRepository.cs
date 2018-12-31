using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model.Components;
using Bau.Libraries.LibMotionComic.Model.Components.Actions;
using Bau.Libraries.LibMotionComic.Model.Components.Actions.Eases;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para carga de timeline y acciones
	/// </summary>
	internal class ComicTimeLineRepository
	{
		internal ComicTimeLineRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		///		Carga los datos de un timeline
		/// </summary>
		internal TimeLineModel LoadTimeLine(PageModel page, MLNode nodeML)
		{
			TimeLineModel timeLine = new TimeLineModel(page, nodeML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));

				// Asigna los atributos
				AssignAttributesAction(nodeML, timeLine.Parameters, 0, 3);
				// Carga el contenido del timeLine
				foreach (MLNode childML in nodeML.Nodes)
				{
					ActionBaseModel action = null;

						// Obtiene la acción
						switch (childML.Name)
						{
							case ComicRepositoryConstants.TagActionShowImage:
									action = new SetVisibilityActionModel(timeLine,
																		  childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		  childML.Attributes[ComicRepositoryConstants.TagVisible].Value.GetBool(true),
																		  childML.Attributes[ComicRepositoryConstants.TagOpacity].Value.GetDouble(),
																		  childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagActionResize:
									action = new ResizeActionModel(timeLine,
																   childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																   childML.Attributes[ComicRepositoryConstants.TagWidth].Value.GetDouble(),
																   childML.Attributes[ComicRepositoryConstants.TagHeight].Value.GetDouble(),
																   childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagActionRotate:
									action = new RotateActionModel(timeLine,
																   childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																   childML.Attributes[ComicRepositoryConstants.TagOriginX].Value.GetDouble(0.5),
																   childML.Attributes[ComicRepositoryConstants.TagOriginY].Value.GetDouble(0.5),
																   childML.Attributes[ComicRepositoryConstants.TagAngle].Value.GetDouble(0),
																   childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagActionZoom:
									action = new ZoomActionModel(timeLine,
																 childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																 childML.Attributes[ComicRepositoryConstants.TagOriginX].Value.GetDouble(0.5),
																 childML.Attributes[ComicRepositoryConstants.TagOriginY].Value.GetDouble(0.5),
																 childML.Attributes[ComicRepositoryConstants.TagScaleX].Value.GetDouble(1),
																 childML.Attributes[ComicRepositoryConstants.TagScaleY].Value.GetDouble(1),
																 childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagActionTranslate:
									action = new TranslateActionModel(timeLine,
																	  childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																	  childML.Attributes[ComicRepositoryConstants.TagTop].Value.GetDouble(),
																	  childML.Attributes[ComicRepositoryConstants.TagLeft].Value.GetDouble(),
																	  childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagActionSetZIndex:
									action = new SetZIndexModel(timeLine,
																childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true),
																childML.Attributes[ComicRepositoryConstants.TagZIndex].Value.GetInt(1));
								break;
							case ComicRepositoryConstants.TagActionPath:
									action = new PathActionModel(timeLine,
																 childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																 Mediator.CommonRepository.Normalize(childML.Value),
																 childML.Attributes[ComicRepositoryConstants.TagRotateWithTangent].Value.GetBool(true),
																 childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagSetActionViewBox:
									action = new BrushViewBoxActionModel(timeLine,
																		 childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		 Mediator.CommonRepository.GetRectangle(childML.Attributes[ComicRepositoryConstants.TagViewBox].Value),
																		 Mediator.CommonRepository.GetRectangle(childML.Attributes[ComicRepositoryConstants.TagViewPort].Value),
																		 Mediator.CommonRepository.ConvertTile(childML.Attributes[ComicRepositoryConstants.TagTileMode].Value),
																		 childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagSetActionRadialBrush:
									action = new BrushRadialActionModel(timeLine,
																		childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagCenter].Value),
																		Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagOrigin].Value),
																		childML.Attributes[ComicRepositoryConstants.TagRadiusX].Value.GetDouble(),
																		childML.Attributes[ComicRepositoryConstants.TagRadiusY].Value.GetDouble(),
																		childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
							case ComicRepositoryConstants.TagSetActionLinearBrush:
									action = new BrushLinearActionModel(timeLine,
																		childML.Attributes[ComicRepositoryConstants.TagKey].Value,
																		Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagStart].Value),
																		Mediator.CommonRepository.GetPoint(childML.Attributes[ComicRepositoryConstants.TagEnd].Value),
																		Mediator.CommonRepository.GetSpreadMethod(childML.Attributes[ComicRepositoryConstants.TagSpread].Value),
																		childML.Attributes[ComicRepositoryConstants.TagMustAnimate].Value.GetBool(true));
								break;
						}
						// Si realmente se ha leído alguna acción
						if (action != null)
						{ 
							// Asigna los atributos
							AssignAttributesAction(childML, action.Parameters, timeLine.Parameters.Start, timeLine.Parameters.Duration);
							// Asigna las funciones
							LoadEases(action, childML);
							// Añade la acción al timeline
							timeLine.Actions.Add(action);
						}
				}
				// Devuelve el timeline
				return timeLine;
		}

		/// <summary>
		///		Asigna los atributos básicos a una acción
		/// </summary>
		private void AssignAttributesAction(MLNode nodeML, AnimationParameters parameters, int startDefault, int durationDefault)
		{
			parameters.Start = nodeML.Attributes[ComicRepositoryConstants.TagStart].Value.GetInt(startDefault);
			parameters.Duration = nodeML.Attributes[ComicRepositoryConstants.TagDuration].Value.GetInt(durationDefault);
			parameters.AccelerationRatio = nodeML.Attributes[ComicRepositoryConstants.TagAcceleration].Value.GetDouble();
			parameters.DecelerationRatio = nodeML.Attributes[ComicRepositoryConstants.TagDeceleration].Value.GetDouble();
			parameters.SpeedRatio = nodeML.Attributes[ComicRepositoryConstants.TagSpeed].Value.GetDouble();
		}

		/// <summary>
		///		Carga las funciones asociadas a una acción
		/// </summary>
		private void LoadEases(ActionBaseModel action, MLNode nodeML)
		{
			foreach (MLNode childML in nodeML.Nodes)
				switch (childML.Name)
				{
					case ComicRepositoryConstants.TagBounceEase:
							action.Eases.Add(new BounceEaseModel(action,
																 GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value),
																 childML.Attributes[ComicRepositoryConstants.TagBounces].Value.GetInt(2),
																 childML.Attributes[ComicRepositoryConstants.TagBounciness].Value.GetDouble(0.5)));
						break;
					case ComicRepositoryConstants.TagBackEase:
							action.Eases.Add(new BackEaseModel(action,
															   GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value),
															   childML.Attributes[ComicRepositoryConstants.TagAmplitude].Value.GetDouble(0.5)));
						break;
					case ComicRepositoryConstants.TagCircleEase:
							action.Eases.Add(new CircleEaseModel(action,
																 GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value)));
						break;
					case ComicRepositoryConstants.TagCubicEase:
							action.Eases.Add(new CubicEaseModel(action,
																GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value)));
						break;
					case ComicRepositoryConstants.TagElasticEase:
							action.Eases.Add(new ElasticEaseModel(action,
																  GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value),
																  childML.Attributes[ComicRepositoryConstants.TagOscillations].Value.GetInt(2),
																  childML.Attributes[ComicRepositoryConstants.TagSpringiness].Value.GetInt(1)));
						break;
					case ComicRepositoryConstants.TagQuadraticEase:
							action.Eases.Add(new QuadraticEaseModel(action,
																	GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value)));
						break;
					case ComicRepositoryConstants.TagQuarticEase:
							action.Eases.Add(new QuarticEaseModel(action,
																  GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value)));
						break;
					case ComicRepositoryConstants.TagSineEase:
							action.Eases.Add(new SineEaseModel(action,
															   GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value)));
						break;
					case ComicRepositoryConstants.TagExponentialEase:
							action.Eases.Add(new ExponentialEaseModel(action,
																	  GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value),
																	  childML.Attributes[ComicRepositoryConstants.TagExponent].Value.GetDouble(1)));
						break;
					case ComicRepositoryConstants.TagPowerEase:
							action.Eases.Add(new PowerEaseModel(action,
																GetEaseMode(childML.Attributes[ComicRepositoryConstants.TagEasingMode].Value),
																childML.Attributes[ComicRepositoryConstants.TagPower].Value.GetDouble(1)));
						break;
				}
		}

		/// <summary>
		///		Obtiene el modo de ejecución de las funciones
		/// </summary>
		private EaseBaseModel.Mode GetEaseMode(string value)
		{
			if (value == EaseBaseModel.Mode.EaseIn.ToString())
				return EaseBaseModel.Mode.EaseIn;
			else if (value == EaseBaseModel.Mode.EaseOut.ToString())
				return EaseBaseModel.Mode.EaseOut;
			else
				return EaseBaseModel.Mode.EaseInOut;
		}

		/// <summary>
		///		Agregrador de repository
		/// </summary>
		private ComicReaderMediator Mediator { get; }
	}
}
