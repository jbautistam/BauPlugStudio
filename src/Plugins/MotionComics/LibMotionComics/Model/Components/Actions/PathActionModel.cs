using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Actions
{
	/// <summary>
	///		Acción para animar un elemento por una ruta
	/// </summary>
	public class PathActionModel : ActionBaseModel
	{
		public PathActionModel(TimeLineModel timeLine, string targetKey, string path, bool rotateWithTangent, bool mustAnimate)
							: base(timeLine, targetKey, mustAnimate)
		{
			Path = path;
			RotateWithTangent = rotateWithTangent;
		}

		/// <summary>
		///		Ruta de movimiento
		/// </summary>
		public string Path { get; }

		/// <summary>
		///		Indica si se debe rotar siguiendo la tangente de la ruta
		/// </summary>
		public bool RotateWithTangent { get; }
	}
}
