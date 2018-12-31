using System;

namespace Bau.Libraries.DevConference.Application.Models
{
    public abstract class BaseModel
    {
		// Variables privadas
		private string _id;

		/// <summary>
		///		Id del elemento
		/// </summary>
		public string Id
		{
			get
			{
				// Calcula el Id si no existía
				if (string.IsNullOrWhiteSpace(_id))
					_id = Guid.NewGuid().ToString();
				// Devuelve el Id
				return _id;
			}
			set { _id = value; }
		}

		/// <summary>
		///		Título
		/// </summary>
		public string Title { get; set; }
    }
}
