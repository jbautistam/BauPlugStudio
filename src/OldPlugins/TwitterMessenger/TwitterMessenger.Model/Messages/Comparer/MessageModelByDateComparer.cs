using System;

namespace Bau.Libraries.TwitterMessenger.Model.Messages.Comparer
{
	/// <summary>
	///		Clase para comparación de fechas de un mensaje
	/// </summary>
	internal class MessageModelByDateComparer : LibDataStructures.Tools.Comparers.AbstractBaseComparer<MessageModel>
	{
		internal MessageModelByDateComparer(bool ascending) : base(ascending) { }

		/// <summary>
		///		Compara dos mensajes
		/// </summary>
		protected override int CompareData(MessageModel first, MessageModel second)
		{
			return first.DateNew.CompareTo(second.DateNew);
		}
	}
}
