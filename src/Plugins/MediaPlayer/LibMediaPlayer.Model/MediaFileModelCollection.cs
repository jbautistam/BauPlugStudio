using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Colección de <see cref="MediaFileModel"/>
	/// </summary>
	public class MediaFileModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<MediaFileModel>
	{   
		/// <summary>
		///		Comprueba si existe un archivo
		/// </summary>
		public bool ExistsByFile(MediaFileModel file)
		{
			return this.FirstOrDefault(item => (!file.Url.IsEmpty() && item.Url.EqualsIgnoreCase(file.Url)) || 
											   (!file.FileName.IsEmpty() &&	item.FileName.EqualsIgnoreCase(file.FileName))) != null;
		}

		/// <summary>
		///		Obtiene el número de elementos no escuchados
		/// </summary>
		public int GetNumberNotPlayed()
		{
			int number = 0;

				// Obtiene el número de elementos no leídos
				foreach (MediaFileModel file in this)
					if (file.Status == MediaFileModel.StatusFile.NotPlayed)
						number++;
				// Devuelve el número de elementos
				return number;
		}

		/// <summary>
		///		Borra un archivo
		/// </summary>
		internal bool Delete(MediaFileModel file)
		{
			if (Exists(file.ID))
			{
				Remove(file);
				return true;
			}
			else
				return false;
		}
	}
}
