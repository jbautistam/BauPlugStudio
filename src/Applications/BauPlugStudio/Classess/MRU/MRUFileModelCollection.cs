using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Applications.BauPlugStudio.Classess.MRU
{
	/// <summary>
	///		Colección de <see cref="MRUFileModel"/>
	/// </summary>
	internal class MRUFileModelCollection : Libraries.LibDataStructures.Base.BaseExtendedModelCollection<MRUFileModel>
	{
		/// <summary>
		///		Añade un MRU a la colección
		/// </summary>
		internal void Add(string source, string fileName, string text)
		{ 
			// Elimina el anterior
			RemoveLast(source, fileName);
			// Añade el archivo
			if (System.IO.File.Exists(fileName))
				Add(new MRUFileModel(source, fileName, text));
		}

		/// <summary>
		///		Obtiene las aplicaciones para las que se han añadido MRU
		/// </summary>
		internal System.Collections.Generic.List<string> GetApplications()
		{
			System.Collections.Generic.List<string> applications = new System.Collections.Generic.List<string>();

				// Recorre los elementos añadiendo las aplicaciones
				foreach (MRUFileModel objMRU in this)
					if (!applications.Exists(application => application.EqualsIgnoreCase(objMRU.Source)))
						applications.Add(objMRU.Source);
				// Devuelve la colección de elementos
				return applications;
		}

		/// <summary>
		///		Elimina los MRU anteriores con los mismos datos
		/// </summary>
		private void RemoveLast(string source, string fileName)
		{
			for (int index = Count - 1; index >= 0; index--)
				if (this [index].Source.EqualsIgnoreCase(source) &&
						this [index].FileName.EqualsIgnoreCase(fileName))
					RemoveAt(index);
		}
	}
}
