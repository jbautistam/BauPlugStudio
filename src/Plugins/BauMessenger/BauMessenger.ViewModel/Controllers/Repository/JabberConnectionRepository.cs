using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibXmppClient;
using Bau.Libraries.LibXmppClient.Core;
using Bau.Libraries.LibXmppClient.Servers;
using Bau.Libraries.LibXmppClient.Users;

namespace Bau.Libraries.BauMessenger.ViewModel.Controllers.Repository
{
	/// <summary>
	///		Repository para las conexiones
	/// </summary>
	public class JabberConnectionRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "Connections";
		private const string TagConnection = "Connection";
		private const string TagServer = "Server";
		private const string TagAddress = "Address";
		private const string TagPort = "Port";
		private const string TagUseSsl = "UseSsl";
		private const string TagUser = "User";
		private const string TagLogin = "Login";
		private const string TagPassword = "Password";
		private const string TagStatus = "Status";

		/// <summary>
		///		Carga las conexiones
		/// </summary>
		public void Load(JabberManager manager, string fileName)
		{
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode connectionML in nodeML.Nodes)
								if (connectionML.Name == TagConnection)
								{
									JabberServer server = null;
									JabberUser user = null;

										// Carga los datos del servidor
										foreach (MLNode serverML in connectionML.Nodes)
											if (serverML.Name == TagServer)
												server = new JabberServer(serverML.Attributes[TagAddress].Value,
																		  serverML.Attributes[TagPort].Value.GetInt(5222),
																		  serverML.Attributes[TagUseSsl].Value.GetBool(true));
										// Carga los datos del usuario
										foreach (MLNode userML in connectionML.Nodes)
											if (userML.Name == TagUser)
											{ 
												// Crea el usuario
												user = new JabberUser(userML.Attributes[TagAddress].Value,
																	  userML.Attributes[TagLogin].Value,
																	  userML.Attributes[TagPassword].Value);
												// Y le asigna el estado
												user.Status.Status = userML.Attributes[TagStatus].Value.GetEnum(JabberContactStatus.Availability.Offline);
											}
										// Añade la conexión
										if (server != null && user != null)
											manager.AddConnection(server, user);
								}
		}

		/// <summary>
		///		Graba las conexiones
		/// </summary>
		public void Save(JabberManager manager, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode nodeML = fileML.Nodes.Add(TagRoot);

				// Asigna los nodos de las conexión
				foreach (JabberConnection connection in manager.Connections)
				{
					MLNode connectionML = nodeML.Nodes.Add(TagConnection);
					MLNode serverML = connectionML.Nodes.Add(TagServer);
					MLNode userML = connectionML.Nodes.Add(TagUser);

						// Añade los datos del servidor
						serverML.Attributes.Add(TagAddress, connection.Host.Address);
						serverML.Attributes.Add(TagPort, connection.Host.Port);
						serverML.Attributes.Add(TagUseSsl, connection.Host.UseTls);
						// Añade los datos del usuario
						userML.Attributes.Add(TagAddress, connection.User.Host);
						userML.Attributes.Add(TagLogin, connection.User.Login);
						userML.Attributes.Add(TagPassword, connection.User.Password);
						userML.Attributes.Add(TagServer, connection.User.Status.Status.ToString());
				}
				// Graba el archivo
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}
	}
}
