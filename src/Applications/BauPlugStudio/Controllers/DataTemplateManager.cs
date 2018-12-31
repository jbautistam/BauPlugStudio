using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Bau.Applications.BauPlugStudio.Controllers
{
	/// <summary>
	///		Manager de <see cref="DataTemplate"/>
	/// </summary>
	public class DataTemplateManager
	{   
		// Variables privadas
		private Dictionary<Type, object> _templates = new Dictionary<Type, object>();

		/// <summary>
		///		Registra una plantilla de datos
		/// </summary>
		public void RegisterDataTemplate<TViewModel, TView>() where TView : FrameworkElement
		{
			RegisterDataTemplate(typeof(TViewModel), typeof(TView));
		}

		/// <summary>
		///		Registra una plantilla de datos
		/// </summary>
		public void RegisterDataTemplate(Type viewModelType, Type objViewType)
		{
			if (!_templates.ContainsKey(viewModelType))
			{
				DataTemplate template = CreateTemplate(viewModelType, objViewType);
				object key = template.DataTemplateKey;

					// Añade la plantilla a los recursos de aplicación
					Application.Current.Resources.Add(key, template);
					// Añade la plantilla al diccionario
					_templates.Add(viewModelType, key);
			}
		}

		/// <summary>
		///		Crea la plantilla de datos
		/// </summary>
		private DataTemplate CreateTemplate(Type typeViewModel, Type typeView)
		{
			string xaml = string.Format("<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>",
										typeViewModel.Name, typeView.Name,
										typeViewModel.Namespace, typeView.Namespace);
			ParserContext parserContext = new ParserContext();

				// Crea el mapper y asigna las instrucciones de procesamiento
				parserContext.XamlTypeMapper = new XamlTypeMapper(new string [0]);
				parserContext.XamlTypeMapper.AddMappingProcessingInstruction("vm", typeViewModel.Namespace, typeViewModel.Assembly.FullName);
				parserContext.XamlTypeMapper.AddMappingProcessingInstruction("v", typeView.Namespace, typeView.Assembly.FullName);
				// Añade los diccionarios
				parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
				parserContext.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
				parserContext.XmlnsDictionary.Add("vm", "vm");
				parserContext.XmlnsDictionary.Add("v", "v");
				// Devuelve la plantilla creada
				return (DataTemplate) XamlReader.Parse(xaml, parserContext);
		}

		/// <summary>
		///		Obtiene una plantilla de datos
		/// </summary>
		public DataTemplate GetDataTemplate(Type viewModelType)
		{
			DataTemplate template = null;

				// Obiene la plantilla del diccionario
				if (_templates.ContainsKey(viewModelType))
					if (_templates.TryGetValue(viewModelType, out object key))
						template = Application.Current.Resources [key] as DataTemplate;
				// Devuelve la plantilla
				return template;
		}
	}
}
