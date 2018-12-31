using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Bau.Applications.BauPlugStudio
{
	/// <summary>
	///		Clase principal de la aplicación
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			//? Wpf tiene un error al formatear las fechas. No recoge la cultura actual si no que siempre lo pone en formato inglés.
			//? Para evitarlo se utiliza la siguiente línea (que tiene que estar antes de empezar a abrir ventanas)
			//? http://www.nbdtech.com/Blog/archive/2009/02/22/wpf-data-binding-cheat-sheet-update-the-internationalization-fix.aspx
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
															   new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
			// Inicializa los manejadores de eventos para las excepciones no controladas
			// En principio sólo haría falta el primero, el resto son para excepciones de otros hilos
			DispatcherUnhandledException += (sender, evntArgs) => 
												{ 
													TreatException(evntArgs.Exception);
													evntArgs.Handled = true;
												};
			AppDomain.CurrentDomain.UnhandledException += (sender, evntArgs) => TreatException(evntArgs.ExceptionObject as Exception);
			TaskScheduler.UnobservedTaskException += (sender, args) => TreatException(args.Exception);
		}

		/// <summary>
		///		Trata las excepciones
		/// </summary>
		private void TreatException(Exception exception)
		{ 
			#if !DEBUG
				MessageBox.Show($"Excepción no tratada {exception.Message}.{Environment.NewLine}{exception.StackTrace.ToString()}");
			#endif
		}
	}
}
