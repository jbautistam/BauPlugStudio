using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.Model.WebSites
{
	/// <summary>
	///		Clase con los datos de un proyecto destino
	/// </summary>
	public class ProjectTargetModel : LibDataStructures.Base.BaseModel
	{
		public ProjectTargetModel()
		{
			SectionWithPages = new System.Collections.Generic.List<string>();
			SectionMenus = new System.Collections.Generic.List<string>();
		}

		/// <summary>
		///		Nombre del proyecto
		/// </summary>
		public string ProjectName
		{
			get
			{
				if (ProjectFileName.IsEmpty())
					return "Sin nombre de proyecto";
				else
					return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(ProjectFileName));
			}
		}

		/// <summary>
		///		Proyecto de DocWriter destino
		/// </summary>
		public string ProjectFileName { get; set; }

		/// <summary>
		///		Nombre de las secciones con páginas asociadas (index, news...)
		/// </summary>
		public System.Collections.Generic.List<string> SectionWithPages { get; set; }

		/// <summary>
		///		Nombre de las secciones con menús
		/// </summary>
		public System.Collections.Generic.List<string> SectionMenus { get; set; }

		/// <summary>
		///		Nombre del archivo del menú de tags
		/// </summary>
		public string SectionTagMenuFileName { get; set; }
	}
}
