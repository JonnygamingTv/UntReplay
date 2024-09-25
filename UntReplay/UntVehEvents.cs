using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public sealed class UntVehEvents : UntVehComp
    {
        public delegate void VehicleUpdatePosition(InteractableVehicle player, Vector3 position);
        public static event VehicleUpdatePosition OnVehicleUpdatePosition;
		internal static void fireOnVehicleUpdatePosition(InteractableVehicle vehicle)
		{
			OnVehicleUpdatePosition?.Invoke(vehicle, vehicle.transform.position);
		}
	}
}
