using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public class VehicleController
	{
		private Character controller;
		private Vehicle controlledVehicle;

		public VehicleController(Vehicle controlledVehicle, Character controller)
		{
			this.controller = controller;
			this.controlledVehicle = controlledVehicle;

			GameManager.Instance.keyboard.KeyPressed += Keyboard_KeyPressed;
		}

		public void RotateTurrent(float angle)
		{
			var turrentPart = controlledVehicle.GetVehiclePart(VehiclePartType.VPT_Turrent);
			if (turrentPart == null)
			{
				turrentPart = controlledVehicle.GetVehiclePart(VehiclePartType.VPT_Body);
			}
			turrentPart.Node.Rotate(Vector3.UNIT_Y, new Radian(new Degree(angle)));
		}

		public void Move(Vector3 movement, float speed)
		{
			var bodyPart = controlledVehicle.GetVehiclePart(VehiclePartType.VPT_Body);
			bodyPart.Node.Position = bodyPart.Node.Position + movement;

			if (controlledVehicle.Flags.Exists(o => o == VehicleFlags.VF_Wheels))
			{
				var wheelParts = controlledVehicle.GetVehicleParts(VehiclePartType.VPT_Wheels);
				for (int i = 0; i < wheelParts.Count; i++)
				{
					wheelParts[i].Node.Rotate(Vector3.UNIT_X, new Radian(new Degree(speed)));
				}
			}
			else if (controlledVehicle.Flags.Exists(o => o == VehicleFlags.VF_Tracked))
			{
				//Play Vertex Animation
			}
		}

		private bool Keyboard_KeyPressed(MOIS.KeyEvent arg)
		{
			switch (arg.key)
			{
				case MOIS.KeyCode.KC_W:
					break;
				case MOIS.KeyCode.KC_A:
					break;
				case MOIS.KeyCode.KC_S:
					break;
				case MOIS.KeyCode.KC_D:
					break;
			}
			return true;
		}

		public void Update(float timeSinceLastFrame)
		{

		}
	}
}
