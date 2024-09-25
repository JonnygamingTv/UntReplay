using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public sealed class UntVehFeature : UntVehComp
    {
		private Vector3 oldPosition;
		private Quaternion oldRot;
		private void FixedUpdate()
		{
			if (this.oldPosition != base.Vehicle.transform.position || this.oldRot != base.Vehicle.transform.rotation)
			{
				UntVehEvents.fireOnVehicleUpdatePosition(base.Vehicle);
				this.oldPosition = base.Vehicle.transform.position;
				this.oldRot = base.Vehicle.transform.rotation;
			}
		}
	}
}
