using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Projects.Models;

namespace Bau.Libraries.LibNSharpDoc.Projects.Repository
{
	/// <summary>
	///		Repository para <see cref="ProjectDocumentationModel"/>
	/// </summary>
	internal class ProjectRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Project";
		private const string TagDocumentationType = "DocumentationType";
		private const string TagShowPublic = "ShowPublic";
		private const string TagShowInternal = "ShowInternal";
		private const string TagShowProtected = "ShowProtected";
		private const string TagShowPrivate = "ShowPrivate";
		private const string TagTemplatePath = "TemplatePath";
		private const string TagProvider = "Provider";
		private const string TagProviderPath = "Path";
		private const string TagProviderTemplatePath = "TemplatePath";
		private const string TagProviderAdditionalPath = "AdditionalPath";
		private const string TagProviderType = "Type";
		private const string TagProviderParameter = "Parameter";
		private const string TagProviderParameterKey = "Key";
		private const string TagProviderParameterValue = "Value";

		/// <summary>
		///		Carga los datos de un archivo
		/// </summary>
		internal ProjectDocumentationModel Load(string fileName)
		{
			ProjectDocumentationModel project = new ProjectDocumentationModel();
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName));

				// Asigna el nombre de archivo
				project.FileName = fileName;
				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
						{ 
							// Añade los parámetros básicos
							project.IDType = (ProjectDocumentationModel.DocumentationType) nodeML.Nodes[TagDocumentationType].Value.GetInt(0);
							project.OutputPath = nodeML.Nodes[TagProviderPath].Value;
							project.TemplatePath = nodeML.Nodes[TagProviderTemplatePath].Value;
							project.GenerationParameters.ShowPublic = nodeML.Nodes[TagShowPublic].Value.GetBool();
							project.GenerationParameters.ShowProtected = nodeML.Nodes[TagShowProtected].Value.GetBool();
							project.GenerationParameters.ShowInternal = nodeML.Nodes[TagShowInternal].Value.GetBool();
							project.GenerationParameters.ShowPrivate = nodeML.Nodes[TagShowPrivate].Value.GetBool();
							// Añade los proveedores
							foreach (MLNode childML in nodeML.Nodes)
								if (childML.Name == TagProvider)
									project.Providers.Add(LoadProvider(childML));
						}
				// Devuelve el proyecto
				return project;
		}

		/// <summary>
		///		Carga los datos de un proveedor
		/// </summary>
		private ProviderModel LoadProvider(MLNode nodeML)
		{
			ProviderModel provider = new ProviderModel();

				// Carga los datos
				provider.Type = nodeML.Nodes[TagProviderType].Value;
				// Carga los parámetros
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagProviderParameter)
						provider.Parameters.Add(childML.Attributes[TagProviderParameterKey].Value,
												childML.Nodes[TagProviderParameterValue].Value);
				// Devuelve los datos del proveedor
				return provider;
		}

		/// <summary>
		///		Graba los datos de un mensaje
		/// </summary>
		internal void Save(string fileName, ProjectDocumentationModel project)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Añade los parámetros básicos
				nodeML.Nodes.Add(TagDocumentationType, (int) project.IDType);
				nodeML.Nodes.Add(TagProviderPath, project.OutputPath);
				nodeML.Nodes.Add(TagProviderTemplatePath, project.TemplatePath);
				// Añade los parámetros de documentación
				nodeML.Nodes.Add(TagShowPublic, project.GenerationParameters.ShowPublic);
				nodeML.Nodes.Add(TagShowProtected, project.GenerationParameters.ShowProtected);
				nodeML.Nodes.Add(TagShowInternal, project.GenerationParameters.ShowInternal);
				nodeML.Nodes.Add(TagShowPrivate, project.GenerationParameters.ShowPrivate);
				// Añade los nodos de proveedor
				foreach (ProviderModel provider in project.Providers)
					nodeML.Nodes.Add(GetNodeProvider(provider));
				// Graba el archivo
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo de un provedor
		/// </summary>
		private MLNode GetNodeProvider(ProviderModel provider)
		{
			MLNode nodeML = new MLNode(TagProvider);

				// Añade los nodos de proveedor
				nodeML.Nodes.Add(TagProviderType, provider.Type);
				// Añade los parámetros
				foreach (System.Collections.Generic.KeyValuePair<string, StructParameterModel> parameter in provider.Parameters.Parameters)
				{
					MLNode childML = nodeML.Nodes.Add(TagProviderParameter);

						// Añade los valores del parámetro
						childML.Attributes.Add(TagProviderParameterKey, parameter.Value.Key);
						childML.Nodes.Add(TagProviderParameterValue, parameter.Value.Value?.ToString());
				}
				// Devuelve el nodo
				return nodeML;
		}
	}
}