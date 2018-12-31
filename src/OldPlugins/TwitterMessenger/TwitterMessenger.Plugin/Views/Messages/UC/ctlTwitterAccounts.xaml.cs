using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Bau.Libraries.TwitterMessenger.Model.Accounts;
using Bau.Libraries.TwitterMessenger.ViewModel.Messages;

namespace Bau.Plugins.TwitterMessenger.Views.Messages.UC
{
	/// <summary>
	///		Control de usuario con las cuentas de Twitter
	/// </summary>
	public partial class ctlTwitterAccounts : UserControl
	{	// Variables privadas
			private TwitterAccountsViewModel objViewModel;

		public ctlTwitterAccounts()
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el control
		/// </summary>
		private void InitControl()
		{ DataContext = ViewModel;
		}

		/// <summary>
		///		ViewModel del control
		/// </summary>
		private TwitterAccountsViewModel ViewModel
		{ get 
				{ // Genera el ViewModel si no estaba en memoria
						if (objViewModel == null)
							objViewModel = new TwitterAccountsViewModel();
					// Devuelve el ViewModel
						return objViewModel;
				}
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{ InitControl();
		}
	}
}