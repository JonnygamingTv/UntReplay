using SDG.Framework.Modules;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay
{
    public class Class1 : SDG.Unturned.BarricadeManager, IModuleNexus
    {
        List<String> Logs;
        public void initialize()
        {
            Console.WriteLine("Unturned ReplayMod starting..\nBrought to you by JonHosting.com!");
            Console.WriteLine("Adding level listeners..");
            /* Listen on level load to reset data, basically. */
            SDG.Unturned.Level.onPostLevelLoaded += OLL; // 1
            SDG.Unturned.Level.onLevelExited += OLE; // 2
            Console.WriteLine("Adding barricade listeners..");
            /* Barricades */
            // Add when placed
            SDG.Unturned.BarricadeManager.onBarricadeSpawned += oBS; // 3
            SDG.Unturned.BarricadeManager.onDeployBarricadeRequested += ODBS; // 4
            // Modify when changed
            SDG.Unturned.BarricadeManager.onTransformRequested += oTR; // 5
            // Remove when destroyed
            SDG.Unturned.BarricadeManager.onDamageBarricadeRequested += oDBR; // 6
            SDG.Unturned.BarricadeDrop.OnSalvageRequested_Global += oSR; // 7
            Console.WriteLine("Adding structure listeners..");
            /* Structures */
            // Add when placed
            SDG.Unturned.StructureManager.onStructureSpawned += oSS; // 8
            SDG.Unturned.StructureManager.onDeployStructureRequested += ODSS; // 9
            // Modify when changed
            SDG.Unturned.StructureManager.onTransformRequested += oTR; // 10
            // Remove when destroyed
            SDG.Unturned.StructureManager.onDamageStructureRequested += oDSR; // 11
            SDG.Unturned.StructureDrop.OnSalvageRequested_Global += oSR; // 12
            Console.WriteLine("Adding player listeners..");
            /* Players */
            // Add on join
            SDG.Unturned.Player.onPlayerCreated += oPC; // 13
            // Modify when moving or gesturing/stance

            SDG.Unturned.PlayerAnimator.OnGestureChanged_Global += POGC; // 14
            SDG.Unturned.PlayerStance.OnStanceChanged_Global += POSC; // 15
            SDG.Unturned.PlayerEquipment.OnPunch_Global += POP; // 16
            // SDG.Unturned.PlayerClothing.OnBackpackChanged_Global

            // Keep track of death
            SDG.Unturned.PlayerLife.onPlayerDied += oPD; // 17
            // Remove on leave
            

            Console.WriteLine("Adding Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Adding zombie listeners..");
            /* Zombies */

            Console.WriteLine("Adding vehicle listeners..");
            /* Vehicles */
            SDG.Unturned.VehicleManager.onEnterVehicleRequested += OEV; // 18
            SDG.Unturned.VehicleManager.onExitVehicleRequested += OExV; // 19
            VehicleManager.onSwapSeatRequested += OSSR; // 20
            SDG.Unturned.VehicleManager.onDamageTireRequested += ODT; // 21
            SDG.Unturned.VehicleManager.OnVehicleExploded += OVE; // 22
        }

        public void shutdown()
        {
            Console.WriteLine("Unturned ReplayMod exiting..");
        }
        // Level handlers
        void OLL(int level) {
            Logs = new List<String>();
            foreach (InteractableVehicle Ve in SDG.Unturned.VehicleManager.vehicles)
                Logs.Add("01"+Ve.instanceID.ToString()+'|'+Ve.asset.GUID+'|'+Ve.transform.position.x+','+Ve.transform.position.y+','+Ve.transform.position.z+','+Ve.transform.rotation.w+','+Ve.transform.rotation.x+','+Ve.transform.rotation.y+','+Ve.transform.rotation.z+'|'+Ve.tires.ToArray().ToString());
        }
        void OLE() {
            System.IO.File.WriteAllLines(UnturnedPaths.RootDirectory.FullName+"/UntReplay.log", Logs);
        }

        // Barricade/structure spawns
        void oBS(SDG.Unturned.BarricadeRegion region, SDG.Unturned.BarricadeDrop drop)
        {
            Logs.Add("03"+drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.barricade.transform.parent?.GetInstanceID());
        }
        void oSS(SDG.Unturned.StructureRegion region, SDG.Unturned.StructureDrop drop)
        {
            Logs.Add("04"+drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.structure.transform.parent?.GetInstanceID());
        }
        // Deploy handlers, basically spawn handlers
        void ODBS(SDG.Unturned.Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if (BarricadeManager.tryGetRegion(asset.barricade.transform, out byte x, out byte y, out ushort plant, out BarricadeRegion region))
            {
                Logs.Add("03" + region.drops.Count + "|" + asset.GUID + "|" + asset.barricade.transform.position.x + "," + asset.barricade.transform.position.y + "," + asset.barricade.transform.position.z + "," + asset.barricade.transform.rotation.w + "," + asset.barricade.transform.rotation.x + "," + asset.barricade.transform.rotation.y + "," + asset.barricade.transform.rotation.z + "|" + asset.barricade.transform.parent?.GetInstanceID());
            }
        } // 4
        void ODSS(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if(StructureManager.tryGetRegion(asset.structure.transform, out byte x, out byte y, out StructureRegion region))
            {
                Logs.Add("04" + region.drops.Count + "|" + asset.GUID + "|" + asset.structure.transform.position.x + "," + asset.structure.transform.position.y + "," + asset.structure.transform.position.z + "," + asset.structure.transform.rotation.w + "," + asset.structure.transform.rotation.x + "," + asset.structure.transform.rotation.y + "," + asset.structure.transform.rotation.z + "|"+ asset.structure.transform.parent?.GetInstanceID());
            }
        } // 9
        // Move handlers
        void oTR(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Barricade
        {
            BarricadeDrop drop;
            if (BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion) && (drop = barricadeRegion.drops.Find((BarricadeDrop o) => o.instanceID == instanceID)) != null) {
                Logs.Add("05" + instanceID + "|" +drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 5
        void oTR(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Structure
        {
            StructureDrop drop;
            if (StructureManager.tryGetRegion(x, y, out StructureRegion structureRegion) && (drop = structureRegion.drops.Find((StructureDrop o) => o.instanceID == instanceID)) != null) {
                Logs.Add("10" + instanceID + "|" + drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 10

        // Salvage handlers
        void oSR(SDG.Unturned.BarricadeDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {

        } // 7
        void oSR(SDG.Unturned.StructureDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {

        } // 12
        // Damage handlers
        void oDBR(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {

        } // 6
        void oDSR(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {

        } // 11
        // Player handlers
        void oPC(Player player) // Create
        {
            Logs.Add("13" + player.GetInstanceID() + "|" + player.stance + "|" + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z + "," + player.transform.rotation.x + "," + player.transform.rotation.w + "|" + player.stance);
        } // 13

        void oPD(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator) // Death
        {
            Logs.Add("17" + sender.GetInstanceID() + "|" + instigator + "|" + cause + "|" + limb);
        } // 17
        void POGC(PlayerAnimator a, EPlayerGesture b) // Gesture
        {
            Logs.Add("14" + a.player.GetInstanceID() + "|" + b);
        } // 14
        void POSC(PlayerStance a) // Stance
        {
            Logs.Add("15"+a.player.GetInstanceID()+"|"+a.stance);
        } // 15
        void POP(PlayerEquipment a, EPlayerPunch b) // Punch
        {
            Logs.Add("16"+a.player.GetInstanceID()+"|"+b);
        } // 16
        // Vehicles
        void OSSR(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex) // Swap seat
        {
            Logs.Add("20" + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + toSeatIndex);
        } // 20
        void OEV(Player player, InteractableVehicle vehicle, ref bool shouldAllow) // Enter veh
        {
            if(vehicle.tryAddPlayer(out byte seat, player))
                Logs.Add("18"+vehicle.instanceID+'|'+player.GetInstanceID()+'|'+seat);
        } // 18
        void OExV(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) // Exit veh
        {
            if (shouldAllow)
            {
                Logs.Add("19" + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + pendingLocation.x + ',' + pendingLocation.y + ',' + pendingLocation.z + ',' + pendingYaw);
            }
        } // 19
        void ODT(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) // tire damage
        {
            if(shouldAllow)
            {
                Logs.Add("21"+vehicle.instanceID+"|"+tireIndex+"|"+damageOrigin);
            }
        } // 21
        void OVE(InteractableVehicle Veh) // Vehicle explode (destroyed)
        {
            Logs.Add("22" + Veh.instanceID + '|' + Veh.transform.position.x + ',' + Veh.transform.position.y + ',' + Veh.transform.position.z);
        } // 22
    }
}
