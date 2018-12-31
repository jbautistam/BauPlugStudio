using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDataBaseStudio.Application.Repository
{
	/// <summary>
	///		Helper para los repository
	/// </summary>
	internal class HelperRepository
	{   
		// Constantes privadas
		private const string cnstStrCDataStart = "##StartCdata##";
		private const string cnstStrCDataEnd = "##EndCData##";
		private const string TagCDataStart = "<![CDATA[";
		private const string TagCDataEnd = "]]>";

		/// <summary>
		///		Normaliza el contenido de un informe en la lectura. Crea de nuevo los CData. Tener en cuenta que ReportDefinition
		/// es un XML dentro de otro XML y da error si se encuentra un "<![CDATA[]]>" interno
		/// </summary>
		internal string NormalizeContentLoad(string value)
		{
			string result = value;

				// Reemplaza los valores convertidos en la grabación del CData
				if (!result.IsEmpty())
				{
					result = result.ReplaceWithStringComparison(cnstStrCDataStart, TagCDataStart);
					result = result.ReplaceWithStringComparison(cnstStrCDataEnd, TagCDataEnd);
				}
				// Devuelve la cadena normalizada
				return result;
		}

		/// <summary>
		///		Normaliza el contenido de un informe en la grabación. Crea de nuevo los CData. Tener en cuenta que ReportDefinition
		/// es un XML dentro de otro XML y da error si se encuentra un "<![CDATA[]]>" interno
		/// </summary>
		internal string NormalizeContentSave(string value)
		{
			string result = value;

				// Reemplaza los valores convertidos en la grabación del CData
				if (!result.IsEmpty())
				{
					result = result.ReplaceWithStringComparison(TagCDataStart, cnstStrCDataStart, StringComparison.CurrentCultureIgnoreCase);
					result = result.ReplaceWithStringComparison(TagCDataEnd, cnstStrCDataEnd);
				}
				// Devuelve la cadena normalizada
				return result;
		}
	}
}
