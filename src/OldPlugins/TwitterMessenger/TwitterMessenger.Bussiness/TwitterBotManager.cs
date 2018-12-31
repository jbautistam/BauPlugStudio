using System;

using Bau.Libraries.LibTwitter;

namespace Bau.Libraries.TwitterMessenger
{
	/// <summary>
	///		Manager de TwitterBot
	/// </summary>
	public class TwitterBotManager
	{ 
		// Variables privadas
		private ManagerTwitter manager = null;

		/// <summary>
		///		Manager de Twitter
		/// </summary>
		public ManagerTwitter ManagerTwitter
		{
			get
			{ // Obtiene el manager de Twitter
				if (manager == null)
					manager = new ManagerTwitter();
				// Devuelve el manager de Twitter
				return manager;
			}
		}
	}
}