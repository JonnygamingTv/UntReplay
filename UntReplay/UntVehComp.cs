using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public class UntVehComp : MonoBehaviour
    {
		private InteractableVehicle vehicle;
		public InteractableVehicle Vehicle
		{
			get
			{
				return this.vehicle;
			}
		}
		private void Awake()
        {
			this.vehicle = base.gameObject.transform.GetComponent<InteractableVehicle>();
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
