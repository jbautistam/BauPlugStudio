using System;

using Bau.Libraries.FtpManager.Model.Connections;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.FtpManager.ViewModel.Controllers
{
	/// <summary>
	///		Manager para la creación de los nodos de esquema
	/// </summary>
	internal class TreeNodesSchemaManager
	{
		/// <summary>
		///		Carga los datos de una conexión
		/// </summary>
		private FtpConnectionModel LoadConnection(FileModel file)
		{
			return new Application.Bussiness.FtpConnectionBussiness().Load(file.FullFileName);
		}
	}
}
