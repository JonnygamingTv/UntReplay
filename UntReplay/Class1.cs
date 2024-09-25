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
        // TODO:
        // Add all needed event listeners and save needed data
        // Probably have a continous save system instead of saving on exit only (write stream)
        // Add a way to load the data so it can actually be used.
        List<String> Logs;
        void AddLog(string s) // make it easier to change save method
        {
            Logs.Add(s);
        }
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
            SDG.Unturned.Provider.onEnemyDisconnected += OPDC; // 18

            Console.WriteLine("Adding Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Adding zombie listeners..");
            /* Zombies */

            Console.WriteLine("Adding vehicle listeners..");
            /* Vehicles */
            SDG.Unturned.VehicleManager.onEnterVehicleRequested += OEV; // 19
            SDG.Unturned.VehicleManager.onExitVehicleRequested += OExV; // 20
            VehicleManager.onSwapSeatRequested += OSSR; // 21
            SDG.Unturned.VehicleManager.onDamageTireRequested += ODT; // 22
            SDG.Unturned.VehicleManager.OnVehicleExploded += OVE; // 23
        }

        public void shutdown()
        {
            Console.WriteLine("Unturned ReplayMod exiting..");
        }
        // Level handlers
        void OLL(int level) {
            Logs = new List<String>();
            foreach (InteractableVehicle Ve in SDG.Unturned.VehicleManager.vehicles)
                AddLog("01"+Ve.instanceID.ToString()+'|'+Ve.asset.GUID+'|'+Ve.transform.position.x+','+Ve.transform.position.y+','+Ve.transform.position.z+','+Ve.transform.rotation.w+','+Ve.transform.rotation.x+','+Ve.transform.rotation.y+','+Ve.transform.rotation.z+'|'+Ve.tires.ToArray().ToString());
        }
        void OLE() {
            System.IO.File.WriteAllLines(UnturnedPaths.RootDirectory.FullName+"/UntReplay.log", Logs);
        }

        // Barricade/structure spawns
        void oBS(SDG.Unturned.BarricadeRegion region, SDG.Unturned.BarricadeDrop drop)
        {
            AddLog("03"+drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.barricade.transform.parent?.GetInstanceID());
        }
        void oSS(SDG.Unturned.StructureRegion region, SDG.Unturned.StructureDrop drop)
        {
            AddLog("04"+drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.structure.transform.parent?.GetInstanceID());
        }
        // Deploy handlers, basically spawn handlers
        void ODBS(SDG.Unturned.Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if (shouldAllow && BarricadeManager.tryGetRegion(asset.barricade.transform, out byte x, out byte y, out ushort plant, out BarricadeRegion region))
            {
                AddLog("03" + region.drops.Count + "|" + asset.GUID + "|" + asset.barricade.transform.position.x + "," + asset.barricade.transform.position.y + "," + asset.barricade.transform.position.z + "," + asset.barricade.transform.rotation.w + "," + asset.barricade.transform.rotation.x + "," + asset.barricade.transform.rotation.y + "," + asset.barricade.transform.rotation.z + "|" + asset.barricade.transform.parent?.GetInstanceID());
            }
        } // 4
        void ODSS(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if(shouldAllow && StructureManager.tryGetRegion(asset.structure.transform, out byte x, out byte y, out StructureRegion region))
            {
                AddLog("04" + region.drops.Count + "|" + asset.GUID + "|" + asset.structure.transform.position.x + "," + asset.structure.transform.position.y + "," + asset.structure.transform.position.z + "," + asset.structure.transform.rotation.w + "," + asset.structure.transform.rotation.x + "," + asset.structure.transform.rotation.y + "," + asset.structure.transform.rotation.z + "|"+ asset.structure.transform.parent?.GetInstanceID());
            }
        } // 9
        // Move handlers
        void oTR(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Barricade
        {
            BarricadeDrop drop;
            if (shouldAllow && BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion) && (drop = barricadeRegion.drops.Find((BarricadeDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("05" + instanceID + "|" +drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 5
        void oTR(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Structure
        {
            StructureDrop drop;
            if (shouldAllow && StructureManager.tryGetRegion(x, y, out StructureRegion structureRegion) && (drop = structureRegion.drops.Find((StructureDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("10" + instanceID + "|" + drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 10

        // Salvage handlers
        void oSR(SDG.Unturned.BarricadeDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if(shouldAllow)
                AddLog("07"+structure.instanceID);
        } // 7
        void oSR(SDG.Unturned.StructureDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if (shouldAllow)
                AddLog("12" + structure.instanceID);
        } // 12
        // Damage handlers
        void oDBR(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) // Save when destroyed
        {
            BarricadeDrop drop;
            if (shouldAllow && (BarricadeManager.tryGetRegion(barricadeTransform, out byte x, out byte y, out ushort plant, out BarricadeRegion region) && (drop = region.drops.Find((o)=>o.instanceID == barricadeTransform.GetInstanceID())) != null) && (drop.asset.health - pendingTotalDamage <= 0))
                AddLog("06" + barricadeTransform.GetInstanceID());
        } // 6
        void oDSR(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) // Save when destroyed
        {
            StructureDrop drop;
            if (shouldAllow && (StructureManager.tryGetRegion(structureTransform, out byte x, out byte y, out StructureRegion region) && (drop = region.drops.Find((o) => o.instanceID == structureTransform.GetInstanceID())) != null) && (drop.asset.health - pendingTotalDamage <= 0))
                AddLog("11" + structureTransform.GetInstanceID());
        } // 11
        // Player handlers
        void oPC(Player player) // Create
        {
            AddLog("13" + player.GetInstanceID() + "|" + player.stance + "|" + player.transform.position.x + "," + player.transform.position.y + "," + player.transform.position.z + "," + player.transform.rotation.x + "," + player.transform.rotation.w + "|" + player.stance);
        } // 13
        void OPDC(SteamPlayer player) // Disconnected
        {
            AddLog("18"+player?.player?.GetInstanceID());
        } // 18

        void oPD(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator) // Death
        {
            AddLog("17" + sender.GetInstanceID() + "|" + instigator + "|" + cause + "|" + limb);
        } // 17
        void POGC(PlayerAnimator a, EPlayerGesture b) // Gesture
        {
            AddLog("14" + a.player.GetInstanceID() + "|" + b);
        } // 14
        void POSC(PlayerStance a) // Stance
        {
            AddLog("15"+a.player.GetInstanceID()+"|"+a.stance);
        } // 15
        void POP(PlayerEquipment a, EPlayerPunch b) // Punch
        {
            AddLog("16"+a.player.GetInstanceID()+"|"+b);
        } // 16
        // Vehicles
        void OSSR(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex) // Swap seat
        {
            AddLog("21" + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + toSeatIndex);
        } // 21
        void OEV(Player player, InteractableVehicle vehicle, ref bool shouldAllow) // Enter veh
        {
            if(vehicle.tryAddPlayer(out byte seat, player))
                AddLog("19"+vehicle.instanceID+'|'+player.GetInstanceID()+'|'+seat);
        } // 19
        void OExV(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) // Exit veh
        {
            if (shouldAllow)
            {
                AddLog("20" + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + pendingLocation.x + ',' + pendingLocation.y + ',' + pendingLocation.z + ',' + pendingYaw);
            }
        } // 20
        void ODT(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) // tire damage
        {
            if(shouldAllow)
            {
                AddLog("22"+vehicle.instanceID+"|"+tireIndex+"|"+damageOrigin);
            }
        } // 22
        void OVE(InteractableVehicle Veh) // Vehicle explode (destroyed)
        {
            AddLog("23" + Veh.instanceID + '|' + Veh.transform.position.x + ',' + Veh.transform.position.y + ',' + Veh.transform.position.z);
        } // 23
    }
}
