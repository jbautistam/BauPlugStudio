using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibTwitter;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.TwitterMessenger.ViewModel.Accounts
{
	/// <summary>
	///		ViewModel para la definición de cuentas
	/// </summary>
	public class AccountViewModel : Bau.Libraries.BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		public AccountViewModel(TwitterAccount account) 
		{
			ValidateCommand = new BaseCommand(parameter => ValidateAccount(parameter), 
											  parameter => Account.NeedAuthorization);
			if (account == null)
				Account = new TwitterAccount(TwitterMessengerViewModel.Instance.TwitterMessenger.ManagerTwitter);
			else
				Account = account;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false; // ... supone que los datos no son correctos

				// Comprueba los datos
				if (User.IsEmpty())
					TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el código de usuario");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos de la cuenta
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{ 
				// Añade la cuenta
				Account.Manager.Accounts.RemoveByScreenName(Account.ScreenName);
				Account.Manager.Accounts.Add(Account);
				// Graba los datos
				AccountsRepository.Save(Account.Manager.Accounts, TwitterMessengerViewModel.Instance.FileAccounts);
				// Indica que no ha habido modificaciones
				IsUpdated = false;
				SaveCommand.OnCanExecuteChanged();
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Valida la cuenta contra Twitter
		/// </summary>
		private void ValidateAccount(object parameter)
		{
			if (ValidateData() && 
					TwitterMessengerViewModel.Instance.ViewsController.OpenFormValidateAccount(Account) == SystemControllerEnums.ResultType.Yes)
			{ 
				// Graba la cuenta
				AccountsRepository.Save(Account.Manager.Accounts, TwitterMessengerViewModel.Instance.FileAccounts);
				// Indica que se ha modificado
				IsUpdated = true;
				TwitterMessengerViewModel.Instance.ControllerWindow.ShowMessage("Validado");
			}
		}

		/// <summary>
		///		Código de usuario
		/// </summary>
		public string User
		{
			get { return Account.ScreenName; }
			set 
			{
				if (Account.ScreenName != value)
				{
					Account.ScreenName = value;
					OnPropertyChanged(nameof(User));
				}
			}
		}

		/// <summary>
		///		Comando de validación de cuenta
		/// </summary>
		public BaseCommand ValidateCommand { get; }

		/// <summary>
		///		Cuenta
		/// </summary>
		private TwitterAccount Account { get; }
	}
}