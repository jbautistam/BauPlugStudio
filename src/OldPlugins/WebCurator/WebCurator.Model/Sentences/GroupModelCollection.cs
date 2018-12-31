using System;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.WebCurator.Model.Sentences
{
	/// <summary>
	///		Colección de <see cref="GroupModel"/>
	/// </summary>
	public class GroupModelCollection : Bau.Libraries.LibDataStructures.Base.BaseModelCollection<GroupModel>
	{
		/// <summary>
		///		Compacta un grupo
		/// </summary>
		internal void Compact(GroupModelCollection groups)
		{
			foreach (GroupModel source in groups)
			{
				GroupModel target = this.FirstOrDefault(group => group.Level.EqualsIgnoreCase(source.Level) &&
																 group.Order == source.Order);

					// Crea un objeto si no se ha encontrado ninguno
					if (target == null)
					{ 
						// Copia las propiedades del objeto
						target = new GroupModel
										{
											Level = source.Level,
											Order = source.Order,
											Maximum = source.Maximum,
											Probability = source.Probability
										};
						// Añade el grupo a la colección
						Add(target);
					}
					// Añade las sentencias
					target.Sentences.AddRange(source.Sentences);
			}
		}

		/// <summary>
		///		Ordena la colección por el orden numérico
		/// </summary>
		public void SortByOrder()
		{
			Sort((first, second) => first.Order.CompareTo(second.Order));
		}
	}
}
