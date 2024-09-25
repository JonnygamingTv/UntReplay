using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public sealed class UntPlayerFeature : UntPlayerComp
    {
		private Vector3 oldPosition;
		private Quaternion oldRot;
		private void FixedUpdate()
		{
			if (this.oldPosition != base.Player.transform.position || this.oldRot != base.Player.transform.rotation)
			{
				UntPlayerEvents.fireOnPlayerUpdatePosition(base.Player);
				this.oldPosition = base.Player.transform.position;
				this.oldRot = base.Player.transform.rotation;
			}
		}
	}
}
