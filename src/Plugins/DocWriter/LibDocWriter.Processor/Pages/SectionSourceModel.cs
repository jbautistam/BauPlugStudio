using System;

using Bau.Libraries.LibDocWriter.Model.Documents;

namespace Bau.Libraries.LibDocWriter.Processor.Pages
{
	/// <summary>
	///		Datos de un documento origen compilado
	/// </summary>
	internal class SectionSourceModel
	{ 
		// Constantes privadas
		internal const string VariableMainUrlPage = "¬¬~~MainUrlPage~~¬¬";
		internal const string VariableMainUrlImage = "¬¬~~MainUrlImage~~¬¬";
		internal const string VariableContent = "¬¬~~Content~~¬¬";
		internal const string VariableAdditionalContent = "¬¬~~AdditionalContent~~¬¬";
		internal const string VariableTitle = "¬¬~~Title~~¬¬";
		internal const string VariableFullTitle = "¬¬~~FullTitle~~¬¬";
		internal const string VariableDescription = "¬¬~~Description~~¬¬";
		internal const string VariableKeywords = "¬¬~~KeyWords~~¬¬";

		internal SectionSourceModel(FileTargetModel fileTarget, DocumentModel document)
		{
			FileTarget = fileTarget;
			Source = document;
		}

		/// <summary>
		///		Obtiene el nombre de la sección  para una variable
		/// </summary>
		internal string GetNameSection()
		{
			return System.IO.Path.GetFileNameWithoutExtension(FileTarget.File.FileName);
		}

		/// <summary>
		///		Reemplaza las variables de página en el contenido compilado
		/// </summary>
		internal string ReplaceVariablesPageTemplate(string content, string strAdditionalContent,
													 string title, string strFullTitle, string description,
													 string keyWords)
		{		
			// Reemplaza el contenido
			content = ContentCompiled.Replace(SectionSourceModel.VariableContent, content);
			content = content.Replace(VariableAdditionalContent, strAdditionalContent);
			content = content.Replace(VariableTitle, title);
			content = content.Replace(VariableFullTitle, strFullTitle);
			content = content.Replace(VariableDescription, description);
			content = content.Replace(VariableKeywords, keyWords);
			// Devuelve la cadena
			return content;
		}

		/// <summary>
		///		Archivo destino
		/// </summary>
		internal FileTargetModel FileTarget { get; }

		/// <summary>
		///		Documento origen
		/// </summary>
		internal DocumentModel Source { get; }

		/// <summary>
		///		Contenido compilado
		/// </summary>
		internal string ContentCompiled { get; set; }
	}
}
