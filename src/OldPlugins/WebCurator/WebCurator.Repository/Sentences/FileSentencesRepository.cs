using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;
using Bau.Libraries.WebCurator.Model.Sentences;

namespace Bau.Libraries.WebCurator.Repository.Sentences
{
	/*
	<?xml version='1.0' encoding='utf-8'?>
	<Sentences>
		<!-- Sinónimos: separe cada cadena de sinónimos por | 
			Por ejemplo fondo de pantalla | wallpaper | fondo de escritorio
			Los sinónimos que aparecen con un carácter ~ inicial, nunca se utilizan como sinónimo real, son
			simplemente nombres especiales, por ejemplo: ~fondoshort
		-->
		<Synonymous></Synonymous>
		<!-- Frases: introduzca entre {} separadas por | las cadenas que se puedan intercambiar
			Por ejemplo: Puede descargar este { fondo de pantalla | papel tapiz }
			También se pueden utilizar sinónimos, para ello introduzca entre corchetes
			el sinónimo.
			Por ejemplo: Puede descargar este [fondo de pantalla]
			Las secciones de frase que comienzan por -{ son opcionales, por ejemplo:
			Instale este [Fondo] en su [tablet] pulsando sobre la imagen superior y -{descargándosela en su { [tablet] | dispositivo }}.
			El atributo Level identifica las frases que pueden intercambiarse
			El atributo Maximum identifica el número de frases que pueden aparecer del mismo nivel
		-->
		<!-- Dentro de las frases se pueden utilizar los siguientes marcadores: 
			@File: nombre del archivo (sin extensión)
		-->
		<!-- Frases para el título -->
		<Title></Title>
		<!-- Frases para la descripción -->
		<Description></Description>
		<!-- Palabras clave -->
		<KeyWords></KeyWords>
		<!-- Frases para el cuerpo -->
		<Group Level = '1' Order = '1' Maximum = '1' Probability = '1'>
			<Sentence></Sentence>
		</Group>
	</Sentences>
	*/

	/// <summary>
	///		Repository de <see cref="FileSentencesModel"/>
	/// </summary>
	public class FileSentencesRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Sentences";
		private const string TagSynonymous = "Synonymous";
		private const string TagSynonymousName = "Name";
		private const string TagCategory = "Category";
		private const string TagPage = "Page";
		private const string TagTitle = "Title";
		private const string TagDescription = "Description";
		private const string TagKeyWords = "KeyWords";
		private const string TagGroup = "Group";
		private const string TagLevel = "Level";
		private const string TagOrder = "Order";
		private const string TagMaximum = "Maximum";
		private const string TagProbability = "Probability";
		private const string TagSentences = "Sentence";

		/// <summary>
		///		Carga un archivo de frases
		/// </summary>
		public FileSentencesModel Load(string fileName)
		{
			FileSentencesModel file = new FileSentencesModel();
			MLFile fileML = new XMLParser().Load(fileName);

				// Asigna las propiedades
				file.FileName = fileName;
				// Carga el XML del archivo
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagSynonymous:
											file.Synonymous.Add(LoadSynonymous(childML));
										break;
									case TagCategory:
											LoadPage(childML, file.CategoryDefinition);
										break;
									case TagPage:
											LoadPage(childML, file.PageDefinition);
										break;
								}
				// Devuelve el archivo de frases
				return file;
		}

		/// <summary>
		///		Agrega los datos de un sinónimo
		/// </summary>
		private SynonymousModel LoadSynonymous(MLNode nodeML)
		{
			SynonymousModel synonymous = new SynonymousModel(nodeML.Attributes[TagSynonymousName].Value);

				// Asigna los valores
				synonymous.Values = nodeML.Value.SplitToList("|", false);
				// Devuelve el sinónimo
				return synonymous;
		}


		/// <summary>
		///		Carga los datos de una página
		/// </summary>
		private void LoadPage(MLNode nodeML, PageDefinitionModel page)
		{
			foreach (MLNode childML in nodeML.Nodes)
				switch (childML.Name)
				{
					case TagTitle:
							page.Titles.Add(childML.Value);
						break;
					case TagDescription:
							page.Descriptions.Add(childML.Value);
						break;
					case TagKeyWords:
							page.KeyWords.Add(childML.Value);
						break;
					case TagGroup:
							page.Groups.Add(LoadGroup(childML));
						break;
				}
		}

		/// <summary>
		///		Carga los datos de un grupo
		/// </summary>
		private GroupModel LoadGroup(MLNode nodeML)
		{
			GroupModel group = new GroupModel();

				// Carga los datos del grupo
				group.Level = nodeML.Attributes[TagLevel].Value;
				group.Order = nodeML.Attributes[TagOrder].Value.GetInt(0);
				group.Maximum = nodeML.Attributes[TagMaximum].Value.GetInt(1);
				group.Probability = nodeML.Attributes[TagProbability].Value.GetDouble(1);
				// Carga las frases
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == TagSentences)
						group.Sentences.Add(childML.Value);
				// Devuelve los datos del grupo
				return group;
		}
	}
}
