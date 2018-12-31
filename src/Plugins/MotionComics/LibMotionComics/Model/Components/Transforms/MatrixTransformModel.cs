using System;

namespace Bau.Libraries.LibMotionComic.Model.Components.Transforms
{
	/// <summary>
	///		Transformación de tipo matriz
	/// </summary>
	public class MatrixTransformModel : AbstractTransformModel
	{
		public MatrixTransformModel(string data = null)
		{
			Data = data;
		}

		/// <summary>
		///		Datos de la transformación
		/// </summary>
		public string Data { get; set; }
	}
}
