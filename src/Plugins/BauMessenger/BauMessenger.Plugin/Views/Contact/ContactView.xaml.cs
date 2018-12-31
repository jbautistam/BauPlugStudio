using System;
using System.Windows;

using Bau.Libraries.BauMessenger.ViewModel.Contacts;

namespace Bau.Plugins.BauMessenger.Views.Contact
{
	/// <summary>
	///		Formulario para el mantenimiento de un contacto
	/// </summary>
	public partial class ContactView : Window
	{
		public ContactView(ContactViewModel viewModel)
		{    
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el viewmodel
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}
	}
}
