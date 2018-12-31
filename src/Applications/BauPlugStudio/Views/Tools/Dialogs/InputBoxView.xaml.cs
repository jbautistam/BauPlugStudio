using System;
using System.Windows;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Applications.BauPlugStudio.Views.Tools.Dialogs
{
	/// <summary>
	///		Ventana para mostrar un texto
	/// </summary>
	public partial class InputBoxView : Window
	{
		public InputBoxView(string message, string inputText)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa las propiedades
			Message = message;
			InputText = inputText;
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			lblMessage.Text = Message;
			txtInput.Text = InputText;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (txtInput.Text.IsEmpty())
					Globals.HostController.ControllerWindow.ShowMessage("Introduzca el texto");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{ 
				// Asigna el texto
				InputText = txtInput.Text;
				// Cierra el formulario
				DialogResult = true;
				Close();
			}
		}

		/// <summary>
		///		Mensaje
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		///		Texto introducido por el usuario
		/// </summary>
		public string InputText { get; set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void cmdSave_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}
	}
}
