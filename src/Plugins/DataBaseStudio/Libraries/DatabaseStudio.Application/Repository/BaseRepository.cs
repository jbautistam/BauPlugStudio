using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDataStructures.Base;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.DatabaseStudio.Application.Repository
{
	/// <summary>
	///		Base para los repository
	/// </summary>
    internal abstract class BaseRepository
    {
		// Constantes privadas
		private const string TagId = "Id";
		private const string TagName = "Name";
		private const string TagDescription = "Description";
		// Constantes privadas para la normalización de datos
		private const string CDataStart = "##StartCdata##";
		private const string CDataEnd = "##EndCData##";
		private const string TagCDataStart = "<![CDATA[";
		private const string TagCDataEnd = "]]>";

		/// <summary>
		///		Carga un archivo
		/// </summary>
		protected MLFile LoadFile(string fileName)
		{
			MLFile fileML = null;

				// Carga el archivo
				if (System.IO.File.Exists(fileName))
					try
					{
						fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);
					}
					catch (Exception exception)
					{
						System.Diagnostics.Trace.TraceError($"Error al cargar el archivo {fileName}. {exception.Message}");
					}
				// Devuelve el archivo cargado
				return fileML;
		}

		/// <summary>
		///		Graba un archivo
		/// </summary>
		protected void SaveFile(string fileName, MLFile fileML)
		{
			// Crea el directorio
			LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileName));
			// Graba el archivo
			try
			{
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Trace.TraceError($"Error al grabar el archivo {fileName}. {exception.Message}");
			}
		}

		/// <summary>
		///		Carga los datos básicos de un nodo
		/// </summary>
		protected void LoadBase(MLNode nodeML, BaseExtendedModel item)
		{
			item.GlobalId = nodeML.Attributes[TagId].Value;
			item.Name = nodeML.Nodes[TagName].Value;
			item.Description = nodeML.Nodes[TagDescription].Value;
		}

		/// <summary>
		///		Obtiene los datos básicos de un nodo
		/// </summary>
		protected MLNode GetMLNodeBase(string tag, BaseExtendedModel item)
		{
			MLNode nodeML = new MLNode(tag);

				// Asigna los datos al nodo
				FillNodeBase(nodeML, item);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene los datos básicos de un nodo
		/// </summary>
		protected void FillNodeBase(MLNode nodeML, BaseExtendedModel item)
		{
			nodeML.Attributes.Add(TagId, item.GlobalId);
			nodeML.Nodes.Add(TagName, item.Name);
			nodeML.Nodes.Add(TagDescription, item.Description);
		}

		/// <summary>
		///		Normaliza el contenido de un informe en la lectura. Crea de nuevo los CData. Tener en cuenta que ReportDefinition
		/// es un XML dentro de otro XML y da error si se encuentra un "<![CDATA[]]>" interno
		/// </summary>
		protected string NormalizeContentLoad(string value)
		{
			string result = value;

				// Reemplaza los valores convertidos en la grabación del CData
				if (!string.IsNullOrEmpty(result))
				{
					result = result.ReplaceWithStringComparison(CDataStart, TagCDataStart);
					result = result.ReplaceWithStringComparison(CDataEnd, TagCDataEnd);
				}
				// Devuelve la cadena normalizada
				return result;
		}

		/// <summary>
		///		Normaliza el contenido de un informe en la grabación. Crea de nuevo los CData. Tener en cuenta que ReportDefinition
		/// es un XML dentro de otro XML y da error si se encuentra un "<![CDATA[]]>" interno
		/// </summary>
		protected string NormalizeContentSave(string value)
		{
			string result = value;

				// Reemplaza los valores convertidos en la grabación del CData
				if (!string.IsNullOrEmpty(result))
				{
					result = result.ReplaceWithStringComparison(TagCDataStart, CDataStart, StringComparison.CurrentCultureIgnoreCase);
					result = result.ReplaceWithStringComparison(TagCDataEnd, CDataEnd);
				}
				// Devuelve la cadena normalizada
				return result;
		}
    }
}
