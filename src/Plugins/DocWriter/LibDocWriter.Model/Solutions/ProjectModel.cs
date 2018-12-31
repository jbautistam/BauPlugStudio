using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Solutions
{
	/// <summary>
	///		Modelo con los datos de un proyecto
	/// </summary>
	public class ProjectModel : Base.BaseDocWriterModel
	{ 
		// Constantes públicas
		public const string FileName = "WebDefinition.wdx";
		// Enumerados públicos
		public enum WebDefinitionType
		{
			Unknown,
			Web,
			EBook,
			Template
		}

		public ProjectModel(SolutionModel solution)
		{ 
			// Inicializa los objetos
			Solution = solution;
			File = new FileModel(this);
			// Inicializa los valores predeterminados
			AddWebTitle = true;
			WebType = WebDefinitionType.Web;
			ItemsPerCategory = 5;
			ItemsPerSiteMap = 50;
			MaxWidthImage = 400;
			ThumbsWidth = 200;
			//EBookPages = new eBooks.eBookPagesCollection();
		}

		/// <summary>
		///		Convierte las variables en un diccionario
		/// </summary>
		public Dictionary<string, string> ConvertVariables()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();

				// Convierte el texto de las variables en un diccionario
				if (!VariablesText.IsEmpty())
				{
					string [] lines = VariablesText.Split('\n');

						if (lines != null)
							foreach (string line in lines)
								if (!line.IsEmpty())
								{
									string [] lineParts = line.Split('=');

										if (lineParts != null && lineParts.Length == 2)
										{
											string key = lineParts[0];

												// Añade el identificador de variable si no existía
												if (!key.StartsWith("$"))
													key = "$" + key;
												// Añade el contenido de las variables
												variables.Add(key.TrimIgnoreNull(), lineParts[1].TrimIgnoreNull());
										}
								}
				}
				// Devuelve el diccionario
				return variables;
		}

		/// <summary>
		///		Nombre del proyecto (no es el nombre de archivo si no el nombre del directorio donde se crea el archivo de proyecto)
		/// </summary>
		public override string Name
		{
			get
			{
				if (File != null && !File.FullFileName.IsEmpty())
					return System.IO.Path.GetFileName(File.Path);
				else
					return base.Name;
			}
			set { base.Name = value; }
		}

		/// <summary>
		///		Solución
		/// </summary>
		public SolutionModel Solution { get;}

		/// <summary>
		///		Archivo del proyecto
		/// </summary>
		public FileModel File { get;}

		/// <summary>
		///		Título del proyecto
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Tipo de Web
		/// </summary>
		public WebDefinitionType WebType { get; set; }

		/// <summary>
		///		Palabras clave predeterminadas
		/// </summary>
		public string KeyWords { get; set; }

		/// <summary>
		///		URL base de la Web
		/// </summary>
		public string URLBase { get; set; }

		/// <summary>
		///		Página principal
		/// </summary>
		public string PageMain { get; set; }

		/// <summary>
		///		Elementos a mostrar por categoría
		/// </summary>
		public int ItemsPerCategory { get; set; }

		/// <summary>
		///		Elementos a mostrar en el sitemap
		/// </summary>
		public int ItemsPerSiteMap { get; set; }

		/// <summary>
		///		Tamaño máximo para una imagen
		/// </summary>
		public int MaxWidthImage { get; set; }

		/// <summary>
		///		Ancho de los thumbs de imágenes para las categorías y RSS
		/// </summary>
		public int ThumbsWidth { get; set; }

		/// <summary>
		///		Indica si se debe añadir el título de la Web al título de la página
		/// </summary>
		public bool AddWebTitle { get; set; }

		/// <summary>
		///		Número de párrafos para el resumen RSS
		/// </summary>
		public int ParagraphsSummaryNumber { get; set; }

		/// <summary>
		///		Nombre del WebMaster del sitio
		/// </summary>
		public string WebMaster { get; set; }

		/// <summary>
		///		Copyright del sitio
		/// </summary>
		public string Copyright { get; set; }

		/// <summary>
		///		Editor del sitio
		/// </summary>
		public string Editor { get; set; }

		/// <summary>
		///		Plantillas
		/// </summary>
		public Documents.TemplatesArrayModel Templates { get; set; } = new Documents.TemplatesArrayModel();

		/// <summary>
		///		Texto con las variables del proyecto
		/// </summary>
		public string VariablesText { get; set; }

		/// <summary>
		///		Comandos a ejecutar tras la compilación
		/// </summary>
		public string PostCompileCommands { get; set; }
	}
}
