using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public sealed class UntPlayerEvents : UntPlayerComp
    {
        public delegate void PlayerUpdatePosition(Player player, Vector3 position);
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;
		internal static void fireOnPlayerUpdatePosition(Player player)
		{
			OnPlayerUpdatePosition?.Invoke(player, player.transform.position);
		}
	}
}
