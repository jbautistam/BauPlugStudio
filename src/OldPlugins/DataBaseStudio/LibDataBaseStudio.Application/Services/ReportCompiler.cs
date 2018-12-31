using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibReports.Renderer;
using Bau.Libraries.LibDataBaseStudio.Model.Reports;
using Bau.Libraries.LibDataBaseStudio.Model.Connections;

namespace Bau.Libraries.LibDataBaseStudio.Application.Services
{
	/// <summary>
	///		Servicio de generación de informes
	/// </summary>
	public class ReportCompiler
	{ 
		// Enumerados públicos
		/// <summary>
		///		Tipo de informe
		/// </summary>
		public enum ReportType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Html</summary>
			Html,
			/// <summary>Pdf</summary>
			Pdf
		}

		/// <summary>
		///		Genera un informe
		/// </summary>
		public bool Generate(string pathReport, ReportModel report, SchemaConnectionModelCollection connections,
							 ReportExecutionModel parameter, ReportType type, string fileName, out string error)
		{ 
			// Inicializa los argumentos de salida
			error = "";
			// Procesa la generación del informe
			try
			{ 
				//ReportManager reportGenerator = new ReportManager(GetProviders(connections), GetRenderer(type));

				//	// Genera el archivo
				//	if (!reportGenerator.GenerateByXml(pathReport, report.ReportDefinition, fileName, GetFilter(parameter, pathReport)))
				//		foreach (string generationError in reportGenerator.Errors)
				//			error = error.AddWithSeparator(generationError, Environment.NewLine);

			}
			catch (Exception exception)
			{	
				error = exception.Message;
			}
			// Devuelve el valor que indica si es correcto
			return error.IsEmpty();
		}

		/// <summary>
		///		Obtiene el renderer adecuado
		/// </summary>
		private IReportRenderer GetRenderer(ReportType type)
		{
			switch (type)
			{
				case ReportType.Html:
					return new LibReports.Renderer.Html.RendererHTML();
				case ReportType.Pdf:
					return new LibReports.Renderer.PDF.RendererPdf();
				default:
					throw new NotImplementedException("Tipo de generador desconocido");
			}
		}

		///// <summary>
		/////		Obtiene los proveedores adecuados
		///// </summary>
		//private ReportDataProviderCollection GetProviders(SchemaConnectionModelCollection connections)
		//{
		//	ReportDataProviderCollection providers = new ReportDataProviderCollection();

		//		// Asigna los proveedores de datos
		//		foreach (SchemaConnectionModel connection in connections)
		//			providers.Add(new ReportSqlServerProvider(connection.Name, "DataBase", connection.GetConnectionString()));
		//		// Devuelve la colección de proveedores de datos
		//		return providers;
		//}

		///// <summary>
		/////		Obtiene los parámetros de un filtro
		///// </summary>
		//private Filter GetFilter(ReportExecutionModel parameter, string pathReport)
		//{
		//	Filter filter = new Filter();

		//		// Asigna los parámetros
		//		filter.Parameters.AddRange(ConvertParameters(parameter.Parameters));
		//		// Asigna los parámetros externos
		//		filter.ExternalData.AddRange(ConvertParameters(parameter.FixedParameters));
		//		// Asigna los archivos
		//		foreach (ReportExecutionFileModel file in parameter.Files)
		//			filter.AdditionalFiles.Add(new AdditionalFile(file.GlobalId, ConvertType(file.IDType),
		//														  ConvertFileName(file.FileName, pathReport), true));
		//		// Devuelve el filtro
		//		return filter;
		//}

		///// <summary>
		/////		Convierte una serie de parámetros
		///// </summary>
		//private FilterParametersCollection ConvertParameters(ParameterModelCollection parameters)
		//{
		//	FilterParametersCollection filterParameters = new FilterParametersCollection();

		//		// Convierte los parámetros
		//		foreach (ParameterModel parameter in parameters)
		//		{
		//			string name = parameter.ID.TrimIgnoreNull();

		//				// Crea el parámetro
		//				if (!name.IsEmpty())
		//				{ 
		//					// Añade el carácter @ inicial si es necesario
		//					if (!name.StartsWith("@"))
		//						name = "@" + name;
		//					// Añade el parámetro a la colección
		//					filterParameters.Add(name, parameter.Value);
		//				}
		//		}
		//		// Devuelve la colección convertida
		//		return filterParameters;
		//}

		/// <summary>
		///		Convierte un nombre de archivo. Si no existe, lo considera un directorio relativo al origen del informe
		/// </summary>
		private string ConvertFileName(string fileName, string pathReport)
		{
			string fileTarget = fileName;

				// Si no existe el nombre de archivo, puede que sea un directorio relativo
				if (!fileName.IsEmpty() && !pathReport.IsEmpty() && !System.IO.File.Exists(fileName))
				{ 
					// Obtiene el nombre de archivo combinado con el directorio del informe
					fileTarget = System.IO.Path.Combine(pathReport, fileName);
					// Si tampoco existe el nombre de archivo combinado, recupera el original
					if (!System.IO.File.Exists(fileTarget))
						fileTarget = fileName;
				}
				// Devuelve el nombre de archivo
				return fileTarget;
		}

		///// <summary>
		/////		Convierte el tipo de archivo
		///// </summary>
		//private AdditionalFile.FileType ConvertType(ReportExecutionFileModel.FileType type)
		//{
		//	switch (type)
		//	{
		//		case ReportExecutionFileModel.FileType.Image:
		//			return AdditionalFile.FileType.Image;
		//		case ReportExecutionFileModel.FileType.Font:
		//			return AdditionalFile.FileType.Font;
		//		default:
		//			return AdditionalFile.FileType.Style;
		//	}
		//}
	}
}
