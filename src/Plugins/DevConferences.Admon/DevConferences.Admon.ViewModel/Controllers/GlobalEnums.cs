using System;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Controllers
{
	/// <summary>
	///		Enumerados globales (comunican el ViewModel con las aplicaciones de presentación [WPF, UWP, Xamarin...]
	/// </summary>
	public static class GlobalEnums
	{	
		/// <summary>Tipo de nodos en el árbol del explorador</summary>
		public enum NodeTypes
		{ 
			/// <summary>Manager de canales</summary>
			TrackManager,
			/// <summary>Canal</summary>
			Track,
			/// <summary>Categoría</summary>
			Category,
			/// <summary>Entrada</summary>
			Entry,
		}
	}
}
