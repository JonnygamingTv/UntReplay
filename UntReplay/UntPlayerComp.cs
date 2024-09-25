using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public class UntPlayerComp : MonoBehaviour
    {
		private Player player;
		public Player Player
		{
			get
			{
				return this.player;
			}
		}
		private void Awake()
        {
			this.player = base.gameObject.transform.GetComponent<Player>();
        }
		private void OnEnable()
		{
			this.Load();
		}

		private void OnDisable()
		{
			this.Unload();
		}

		protected virtual void Load()
		{
		}
		protected virtual void Unload()
		{
		}
	}
}
