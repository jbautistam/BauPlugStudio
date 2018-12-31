using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMotionComic.Model.Components.Transforms;

namespace Bau.Libraries.LibMotionComic.Repository.Reader
{
	/// <summary>
	///		Repository para las transformaciones
	/// </summary>
	internal class ComicTransformsRepository
	{
		internal ComicTransformsRepository(ComicReaderMediator mediator)
		{
			Mediator = mediator;
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
