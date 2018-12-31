using System;
using System.Text;
using System.Text.RegularExpressions;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Processor.Pages;

namespace Bau.Libraries.LibDocWriter.Processor.Steps.PagesSteps
{
	/// <summary>
	///		Procesador del paso de postproceso: cambia las URL y elimina los temporales
	/// </summary>
	internal class StepPagesPostCompileProcessor : AbstractBaseSteps
	{   
		// Variables privadas
		private System.Collections.Generic.Dictionary<string, FileTargetModel> _files;

		internal StepPagesPostCompileProcessor(Generator processor, CompilerData compilerData) : base(processor, compilerData) { }

		/// <summary>
		///		Compila los documentos y lo guarda en temporal
		/// </summary>
		internal override void Process()
		{   
			// Convierte los archivos en un diccionario
			_files = ConvertFilesToDictionary();
			// Convierte las URL de los archivos
			foreach (FileTargetModel file in Data.Files)
				if (file.File.FileType == Model.Solutions.FileModel.DocumentType.Document ||
					file.File.FileType == Model.Solutions.FileModel.DocumentType.Tag ||
					file.File.FileType == Model.Solutions.FileModel.DocumentType.SiteMap)
				{
					string fileName = file.GetFullFileNameCompiledTemporal(Processor);

						// Elimina el archivo corto
						LibCommonHelper.Files.HelperFiles.KillFile(file.GetFullFileNameCompiledShort(Processor));
						// Cambia las URL
						if (System.IO.File.Exists(fileName))
						{ 
							// Cambia las URL	
							PostProcess(file, fileName);
							// Elimina el archivo
							LibCommonHelper.Files.HelperFiles.KillFile(fileName);
						}
				}
		}

		/// <summary>
		///		Convierte los archivos en un diccionario
		/// </summary>
		private System.Collections.Generic.Dictionary<string, FileTargetModel> ConvertFilesToDictionary()
		{
			System.Collections.Generic.Dictionary<string, FileTargetModel> files = new System.Collections.Generic.Dictionary<string, FileTargetModel>();

			// Añade los archivos al diccionario
			foreach (FileTargetModel file in Data.Files)
			{ 
				// Añade el archivo al diccionario
				if (!files.ContainsKey(file.DocumentFileName))
					files.Add(file.DocumentFileName.ToUpper(), file);
				// Si es un css también añade el scss
				if (file.DocumentFileName.EndsWith(".css", StringComparison.CurrentCultureIgnoreCase))
				{
					string fileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(file.DocumentFileName),
															 System.IO.Path.GetFileNameWithoutExtension(file.DocumentFileName) + ".scss");

						if (!files.ContainsKey(fileName))
							files.Add(fileName.ToUpper(), file);
				}
			}
			// Devuelve el diccionario
			return files;
		}

		/// <summary>
		///		Postprocesa un archivo temporal
		/// </summary>
		private void PostProcess(FileTargetModel file, string fileName)
		{
			string content;
			string fileNameTarget = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName),
														   System.IO.Path.GetFileNameWithoutExtension(fileName));

				// Si es una plantilla, cambia la extensión
				if (fileNameTarget.EndsWith(Model.Solutions.FileModel.SiteMapExtension, StringComparison.CurrentCultureIgnoreCase))
					fileNameTarget = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileNameTarget),
															System.IO.Path.GetFileNameWithoutExtension(fileNameTarget) + ".htm");
				// Cambia las URL
				content = ChangeUrl(file, LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName));
				// Graba el archivo
				LibCommonHelper.Files.HelperFiles.SaveTextFile(fileNameTarget, content);
				// Cambia la fecha de creación del archivo
				System.IO.File.SetCreationTime(fileNameTarget, file.DateUpdate);
				System.IO.File.SetLastWriteTime(fileNameTarget, file.DateUpdate);
		}

		/// <summary>
		///		Cambia las URL del texto
		/// </summary>
		private string ChangeUrl(FileTargetModel file, string content)
		{ 
			// Convierte las Url
			if (!content.IsEmpty())
			{ 
				// Cambia las variables globales de URL principal, URL de imagen y URL de thumb
				content = content.ReplaceWithStringComparison(SectionSourceModel.VariableMainUrlPage, file.GetAbsoluteUrlPage(Processor));
				content = content.ReplaceWithStringComparison(SectionSourceModel.VariableMainUrlImage, file.GetAbsoluteUrlImage(Processor));
				// Reemplaza las URL
				content = ReplaceUrls(file, content).ToString();
			}
			// Devuelve el contenido
			return content;
		}

		/// <summary>
		///		Reemplaza las URLs de un archivo por las URL relativas
		/// </summary>
		private StringBuilder ReplaceUrls(FileTargetModel file, string content)
		{
			StringBuilder builder = new StringBuilder();
			Match match = Regex.Match(content, @"\s*(href|src)\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))");
			int lastPosition = 0;

				// Mientras se encuentre una cadena
				while (match.Success)
				{   
					// Añade a la cadena de salida lo anterior
					builder.Append(content.Substring(lastPosition, match.Index - lastPosition));
					// Añade el contenido
					builder.Append(ReplaceInner(file, match.Value));
					// Guarda la posición actual
					lastPosition = match.Index + match.Length;
					// Obtiene la siguiente coincidencia
					match = match.NextMatch();
				}
				// Añade el resto de la cadena
				if (lastPosition < content.Length)
					builder.Append(content.Substring(lastPosition));
				// Devuelve el contenido
				return builder;
		}

		/// <summary>
		///		Reemplaza la cadena interna
		/// </summary>
		private string ReplaceInner(FileTargetModel file, string content)
		{
			Match match = Regex.Match(content, @"[""'](?<1>[^""']*)[""']");
			string result = null;

				// Cambia la cadena entre comillas
				if (match.Success)
				{
					string inQuotes = match.Value.Trim().Substring(1, match.Value.Trim().Length - 2).ToUpper();

						// Busca el nombre de archivo
						if (_files.ContainsKey(inQuotes))
						{
							FileTargetModel objRelative = _files [inQuotes];

								// Transforma la URL
								result = content.Substring(0, match.Index);
								result += "\"" + GetUrlRelative(file.RelativeFullFileNameTarget, objRelative.RelativeFullFileNameTarget) + "\"";
								result += content.Substring(match.Index + match.Length);
						}
				}
				// Devuelve el resultado
				if (!result.IsEmpty())
					return result;
				else
					return content;
		}

		/// <summary>
		///		Obtiene la Url relativa
		/// </summary>
		private string GetUrlRelative(string urlPage, string urlTarget)
		{
			string [] urlPageParts = Split(urlPage);
			string [] urlTargetParts = Split(urlTarget);
			string url = "";
			int index = 0, indexTarget;

				// Quita los directorios iniciales que sean iguales
				while (index < urlTargetParts.Length - 1 &&
					   urlTargetParts[index].Equals(urlPageParts[index], StringComparison.CurrentCultureIgnoreCase))
					index++;
				// Añade todos los saltos hacia atrás (..) que sean necesarios
				indexTarget = index;
				while (indexTarget < urlPageParts.Length - 1)
				{ 
					// Añade el salto
					url += "../";
					// Incrementa el índice
					indexTarget++;
				}
				// Añade los archivos finales
				while (index < urlTargetParts.Length)
				{ 
					// Añade el directorio
					url += urlTargetParts [index];
					// Añade el separador
					if (index < urlTargetParts.Length - 1)
						url += "/";
					// Incrementa el índice
					index++;
				}
				// Devuelve la URL
				return url;
		}

		/// <summary>
		///		Parte una cadena por \ o por /
		/// </summary>
		private string [] Split(string url)
		{
			if (url.IsEmpty())
				return new string[] { "" };
			else if (url.IndexOf('/') >= 0)
				return url.Split('/');
			else
				return url.Split('\\');
		}
	}
}
