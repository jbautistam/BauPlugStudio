using System;

namespace Bau.Libraries.PluginsInterface
{
		/// <summary>
		///		Interface para los plugins de MEF
		/// </summary>
    public interface IPluginChild
    {
			/// <summary>
			///		Operación
			/// </summary>
			string Operate(string strValue);
    }
}
