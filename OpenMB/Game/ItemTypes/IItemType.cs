using Mogre;
using OpenMB.Interfaces;
using OpenMB.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game.ItemTypes
{
	public interface IItemType : IRenderPreview
	{
		string Name { get; }

		Item Item { get; set; }

		ModData ModData { get; set; }

		IItemController ItemController { get; }

		/// <summary>
		/// Call when item equip into the character
		/// if this kind of item can't be equip into the character
		/// like vehicle or aircraft
		/// just leave away
		/// </summary>
		/// <param name="character"></param>
		void Equip(Character character);

		/// <summary>
		/// Call when this kind of item will spawn into the world
		/// when enter this scene
		/// some item types like artillery without limber will just spawn
		/// but other item types like vehicle or aircraft will attach to the character
		/// </summary>
		void Spawn();

		/// <summary>
		/// Call when use this item
		/// like shoot the rifle, fire the artillery
		/// </summary>
		/// <param name="param"></param>
		void Use(params object[] param);
	}

	/// <summary>
	/// control this item used by some controllable item like horse, vehicle or aircrafts
	/// </summary>
	public interface IItemController
    {
		void MoveForward();

		void MoveBackward();

		void TurnLeft();

		void TurnRight();

		/// <summary>
		/// Horse need this
		/// </summary>
		void Jump();

		void InjectKeyPressed();
		void InjectKeyReleased();
		void InjectMouseMove();
    }
}
