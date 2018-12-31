using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Bau.Libraries.TwitterMessenger.Controller.TwitterMessenger;

namespace Bau.Plugins.TwitterMessenger.Views.Messages.UC
{
	/// <summary>
	///		Control para mostrar los mensajes de Twitter
	/// </summary>
	public partial class ctlTwitterMessages : UserControl
	{ // Propiedades de dependencia
			public static readonly DependencyProperty MessagesProperty = DependencyProperty.Register("Messages", typeof(TwitterMessagesCollection), 
																																															 typeof(ctlTwitterMessages));

		public ctlTwitterMessages()
		{ // Inicializa el componente
				InitializeComponent();
		}

		/// <summary>
		///		Mensajes (propiedad de dependencia)
		/// </summary>
		[Description("Mensajes mostrados en la lista")]
		public TwitterMessagesCollection Messages
		{ get { return (TwitterMessagesCollection) GetValue(MessagesProperty); }
			set 
				{ SetValue(MessagesProperty, value); 
					lstMessages.ItemsSource = value;
				}
		}
	}
}
