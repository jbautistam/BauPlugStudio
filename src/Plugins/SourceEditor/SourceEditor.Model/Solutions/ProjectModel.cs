using System;

namespace Bau.Libraries.SourceEditor.Model.Solutions
{
	/// <summary>
	///		Modelo con los datos de un proyecto
	/// </summary>
	public class ProjectModel : FileModel
	{
		public ProjectModel(SolutionModel solution, Definitions.ProjectDefinitionModel definition, string fileName) : base(solution, fileName)
		{
			Solution = solution;
			Definition = definition;
		}

		/// <summary>
		///		Indica que en este caso, aunque tenga nombre de archivo completo, se comporta como una carpeta
		/// </summary>
		public override bool IsFolder
		{
			get { return true; }
		}

		/// <summary>
		///		Directorio base --> Sobrescribe porque aunque sea marcado como directorio, en realidad tiene nombre de archivo
		/// </summary>
		public override string PathBase
		{
			get { return System.IO.Path.GetDirectoryName(FullFileName); }
		}

		/// <summary>
		///		Solución a la que se asocia el proyecto
		/// </summary>
		public SolutionModel Solution { get; }

		/// <summary>
		///		Definición del proyecto
		/// </summary>
		public Definitions.ProjectDefinitionModel Definition { get; }
	}
}
