using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.WebCurator.Model.Sentences;

namespace Bau.Libraries.WebCurator.Application.Services.Generator
{
	/// <summary>
	///		Generador de frases para el compilador
	/// </summary>
	internal class FilesSentencesGenerator
	{   
		// Enumerados públicos
		/// <summary>
		///		Tipo de frase
		/// </summary>
		public enum SentenceType
		{
			/// <summary>Título de la página</summary>
			Title,
			/// <summary>Descripción de la página</summary>
			Description,
			/// <summary>Palabras clave de la página</summary>
			KeyWords,
			/// <summary>Cuerpo de la página</summary>
			Body
		}

		// Variables privadas	
		private FileSentencesModel _fileSentences;
		private Random _rnd = new Random();

		internal FilesSentencesGenerator(ProjectCompiler compiler)
		{
			Compiler = compiler;
		}

		/// <summary>
		///		Compacta un archivo de sentencias
		/// </summary>
		private FileSentencesModel CompactFileSentences()
		{
			return LoadFilesSentences().Compact();
		}

		/// <summary>
		///		Carga los archivos de sentencias
		/// </summary>
		private FileSentencesModelCollection LoadFilesSentences()
		{
			Bussiness.Sentences.FileSentencesBussines bussiness = new Bussiness.Sentences.FileSentencesBussines();
			FileSentencesModelCollection files = new FileSentencesModelCollection();

				// Carga las colecciones
				foreach (string fileName in Compiler.Project.FilesXMLSentences)
					files.Add(bussiness.Load(fileName));
				// Devuelve la colección de archivos
				return files;
		}

		/// <summary>
		///		Limpia las variables
		/// </summary>
		internal void ClearVariables()
		{
			Variables.Clear();
		}

		/// <summary>
		///		Añade una variable
		/// </summary>
		internal void AddVariable(string variable, string value)
		{ 
			// Normaliza el nombre de la variable
			if (variable.StartsWith("~"))
				variable = variable.Substring(1);
			if (!variable.StartsWith("@"))
				variable = "@" + variable;
			// Añade la variable a la colección
			if (!Variables.ContainsKey(variable))
				Variables.Add(variable, value);
		}

		/// <summary>
		///		Calcula una sentencia
		/// </summary>
		internal string Compute(FileSentencesModel.PageType pageIndex, SentenceType type)
		{
			switch (type)
			{
				case SentenceType.Title:
					return Compute(FilesSentences.SelectPage(pageIndex).Titles);
				case SentenceType.Description:
					return Compute(FilesSentences.SelectPage(pageIndex).Descriptions);
				case SentenceType.KeyWords:
					return Compute(FilesSentences.SelectPage(pageIndex).KeyWords);
				case SentenceType.Body:
					return Compute(FilesSentences.SelectPage(pageIndex).Groups);
				default:
					return "";
			}
		}

		/// <summary>
		///		Calcula la cadena a partir de un grupo
		/// </summary>
		private string Compute(GroupModelCollection groups)
		{
			string target = "";

				// Ordena los grupos
				groups.SortByOrder();
				// Crea las sentencias
				foreach (GroupModel group in groups)
					if (100 * group.Probability >= _rnd.Next(100))
					{
						SentenceModelCollection sentencesUsed = new SentenceModelCollection();

							for (int index = 0; index < group.Maximum && sentencesUsed.Count < group.Sentences.Count; index++)
							{
								int intSelected = 0;
								SentenceModel sentence;

									// Obtiene una sentencia aleatoriamente que no esté ya en la colección de sentencias utilizadas
									do
									{ 
										// Obtiene una sentencia aleatoriamente
										sentence = group.Sentences.GetRandom(_rnd);
										// Incrementa el número de cadenas buscadas
										intSelected++;
									}
									while (sentencesUsed.Exists(sentence.GlobalId) && intSelected < group.Sentences.Count);
									// Si no es una de las sentencias seleccionadas anteriormente, la procesa
									if (!sentencesUsed.Exists(sentence.GlobalId))
									{ 
										// Añade la cadena calculada
										target = target.AddWithSeparator(Compute(sentence), Environment.NewLine, false);
										// Añade la sentencia a la colección de sentencias utilizadas
										sentencesUsed.Add(sentence);
									}
							}
					}
				// Devuelve la cadena final
				return target;
		}

		/// <summary>
		///		Calcula la cadena de una colección de sentencias
		/// </summary>
		private string Compute(SentenceModelCollection sentences)
		{
			return Compute(sentences.GetRandom(_rnd));
		}

		/// <summary>
		///		Calcula una cadena a partir de una sentencia
		/// </summary>
		private string Compute(SentenceModel sentence)
		{
			return Parse(sentence.Sentence).TrimIgnoreNull().Replace("\t", " ").Replace("\n", " ");
		}

		/// <summary>
		///		Compila una cadena
		/// </summary>
		private string Parse(string sentence)
		{
			string result = "";
			int position = 0;

				// Interpreta la sentencia
				if (!sentence.IsEmpty())
					while (position < sentence.Length)
					{
						char chrChar = sentence[position];

							switch (chrChar)
							{
								case '[':
										result += ParseBraces(sentence, ref position);
									break;
								case '~':
								case '@':
										result += ParseVariable(sentence, ref position);
									break;
								default:
										result += chrChar;
										position++;
									break;
							}
					}
				// Devuelve el resultado
				return result.TrimIgnoreNull();
		}

		/// <summary>
		///		Interpreta las cadenas entre corchetes
		/// </summary>
		private string ParseBraces(string sentence, ref int position)
		{
			string inner;

				// Se salta la llave de apertura		
				position++;
				// Obtiene la cadena entre corchetes
				inner = GetSentenceTo(sentence, ']', ref position);
				// Divide la cadena por los |
				if (!inner.IsEmpty())
				{
					string[] parts = inner.Split('|');

						if (parts.Length != 0)
						{ 
							// Obtiene una cadena aleatoriamente
							inner = parts[_rnd.Next(parts.Length)];
							// Interpreta la cadena interna
							inner = Parse(inner);
						}
				}
				// Devuelve la cadena
				return inner.TrimIgnoreNull();
		}

		/// <summary>
		///		Interpreta una variable
		/// </summary>
		private string ParseVariable(string sentence, ref int position)
		{
			char start = sentence[position++];
			string nameVariable = GetNameVariable(sentence, ref position);
			string value = "";

				// Obtiene el valor de la variable
				if (!nameVariable.IsEmpty())
				{
					// Obtiene el valor de la variable
					if (start == '~')
					{
						value = GetValueSynonymous(nameVariable);
						if (value.IsEmpty())
							value = GetValueVariable(nameVariable);
					}
					else
					{
						value = GetValueVariable(nameVariable);
						if (value.IsEmpty())
							value = GetValueSynonymous(nameVariable);
					}
					// Añade el valor como valor de la variable
					if (!value.IsEmpty() && GetValueVariable(nameVariable).IsEmpty())
						AddVariable(nameVariable, value);
				}
				// Devuelve el valor de la variable
				return value.TrimIgnoreNull();
		}

		/// <summary>
		///		Obtiene una cadena desde la posición hasta el carácter final
		/// </summary>
		private string GetSentenceTo(string sentence, char end, ref int position)
		{
			string inner = "";

				// Añade los caracteres entre llaves
				while (position < sentence.Length && sentence[position] != end)
				{ 
					// Añade el carácter
					inner += sentence[position];
					// Incrementa la posición
					position++;
				}
				// Si se ha encontrado el carácter de fin, lo quita
				if (position < sentence.Length && sentence[position] == end)
					position++;
				// Devuelve la cadena
				return inner;
		}

		/// <summary>
		///		Obtiene un nombre de variable
		/// </summary>
		private string GetNameVariable(string sentence, ref int position)
		{
			string inner = "";

				// Añade los caracteres y dígitos
				while (position < sentence.Length && char.IsLetterOrDigit(sentence[position]))
				{ 
					// Añade el carácter
					inner += sentence[position];
					// Incrementa la posición
					position++;
				}
				// Devuelve el nombre de variable
				return inner;
		}

		/// <summary>
		///		Obtiene el valor de un sinónimo
		/// </summary>
		private string GetValueSynonymous(string name)
		{
			return Parse(FilesSentences.Synonymous.SearchSynonymous(_rnd, name));
		}

		/// <summary>
		///		Obtiene el valor de una variable
		/// </summary>
		private string GetValueVariable(string name)
		{
			// Normaliza el nombre de la variable
			if (!name.StartsWith("@"))
				name = "@" + name;
			// Busca el valor de la variable
			if (!Variables.TryGetValue(name, out string value))
				value = "";
			// Devuelve el valor de la variable
			return value.TrimIgnoreNull();
		}

		/// <summary>
		///		Compilador
		/// </summary>
		internal ProjectCompiler Compiler { get; }

		/// <summary>
		///		Archivos de sentencias
		/// </summary>
		internal FileSentencesModel FilesSentences
		{
			get
			{   
				// Si no se habían cargado los archivos, los crea
				if (_fileSentences == null)
					_fileSentences = CompactFileSentences();
				// Devuelve los archivos de sentencias
				return _fileSentences;
			}
		}

		/// <summary>
		///		Diccionario de variables
		/// </summary>
		private Dictionary<string, string> Variables { get; } = new Dictionary<string, string>();
	}
}
