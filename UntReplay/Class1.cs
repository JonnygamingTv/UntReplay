﻿using SDG.Framework.Modules;
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
    public class Class1 : MonoBehaviour, IModuleNexus
    {
        // TODO:
        // Add all needed event listeners and save needed data
        // Probably have a continous save system instead of saving on exit only (write stream)
        // Add a way to load the data so it can actually be used.
        byte RecId;
        // List<String> Logs;
        UnicodeEncoding UniEncoding = new UnicodeEncoding();
        System.IO.FileStream SaveStream;
        void AddLog(string s) // make it easier to change save method
        {
            s = s + "\n";
            SaveStream?.WriteAsync(UniEncoding.GetBytes(s), 0, UniEncoding.GetByteCount(s));
            // Logs.Add(s);
        }
        public void initialize()
        {
            Console.WriteLine("Unturned ReplayMod starting..\nBrought to you by JonHosting.com!");
            Console.WriteLine("Adding level listeners..");
            /* Listen on level load to reset data, basically. */
            SDG.Unturned.Level.onPostLevelLoaded += OLL; // 1
            SDG.Unturned.Level.onLevelExited += OLE; // 2

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        public void shutdown()
        {
            Console.WriteLine("Unturned ReplayMod exiting..");
            Console.WriteLine("Removing level listeners..");
            /* Listen on level */
            SDG.Unturned.Level.onPostLevelLoaded -= OLL;
            SDG.Unturned.Level.onLevelExited -= OLE;
            StopRec();
        }

        void StartRec() // Would be cool to move this to a in-game button somewhere... Like a start recording / stop recording button.
        {
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
            SDG.Unturned.Provider.onEnemyConnected += OPC; // 13
            // Modify when moving or gesturing/stance
            UntPlayerEvents.OnPlayerUpdatePosition += OPUP; // 14
            SDG.Unturned.PlayerAnimator.OnGestureChanged_Global += POGC; // 15
            SDG.Unturned.PlayerStance.OnStanceChanged_Global += POSC; // 16
            SDG.Unturned.PlayerEquipment.OnPunch_Global += POP; // 17
            // SDG.Unturned.PlayerClothing.OnBackpackChanged_Global

            // Keep track of death
            SDG.Unturned.PlayerLife.onPlayerDied += oPD; // 18
            // Remove on leave
            SDG.Unturned.Provider.onEnemyDisconnected += OPDC; // 19

            Console.WriteLine("Adding Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Adding zombie listeners..");
            /* Zombies */

            Console.WriteLine("Adding vehicle listeners..");
            /* Vehicles */
            // Created
            Patches.UntPatchEvent.OnVehicleSpawned += OVS; // 20
            Patches.UntPatchEvent.OnVehicleSpawnedFromSpawnpoint += OVS; // 21
            // Interactivity
            SDG.Unturned.VehicleManager.onEnterVehicleRequested += OEV; // 22
            SDG.Unturned.VehicleManager.onExitVehicleRequested += OExV; // 23
            // Movement
            VehicleManager.onSwapSeatRequested += OSSR; // 24
            Patches.UntPatchEvent.OnVehicleMovementChanged += OVMC; // 25
            // Destroyed
            SDG.Unturned.VehicleManager.onDamageTireRequested += ODT; // 26
            SDG.Unturned.VehicleManager.OnVehicleExploded += OVE; // 27
            
            System.IO.Directory.CreateDirectory(UnturnedPaths.RootDirectory.FullName + "/UntReplay");
            RecId = 0;
            while (System.IO.File.Exists(UnturnedPaths.RootDirectory.FullName + "/UntReplay/" + RecId + ".log") && RecId < 255) { RecId++; }
            SaveStream = System.IO.File.OpenWrite(UnturnedPaths.RootDirectory.FullName + "/UntReplay/" + RecId + ".log");
            ResetRec();
        }
        void ResetRec()
        {
            SaveStream?.Seek(0, System.IO.SeekOrigin.Begin);
            // Logs = new List<String>();
        }
        void StopRec()
        {
            SaveStream?.Close();
            // System.IO.File.WriteAllLines(UnturnedPaths.RootDirectory.FullName + "/UntReplay"+RecId+".log", Logs);
            Console.WriteLine("Removing barricade listeners..");
            /* Barricades */
            // Add when placed
            SDG.Unturned.BarricadeManager.onBarricadeSpawned -= oBS;
            SDG.Unturned.BarricadeManager.onDeployBarricadeRequested -= ODBS;
            // Modify when changed
            SDG.Unturned.BarricadeManager.onTransformRequested -= oTR;
            // Remove when destroyed
            SDG.Unturned.BarricadeManager.onDamageBarricadeRequested -= oDBR;
            SDG.Unturned.BarricadeDrop.OnSalvageRequested_Global -= oSR;
            Console.WriteLine("Removing structure listeners..");
            /* Structures */
            // Add when placed
            SDG.Unturned.StructureManager.onStructureSpawned -= oSS;
            SDG.Unturned.StructureManager.onDeployStructureRequested -= ODSS;
            // Modify when changed
            SDG.Unturned.StructureManager.onTransformRequested -= oTR;
            // Remove when destroyed
            SDG.Unturned.StructureManager.onDamageStructureRequested -= oDSR;
            SDG.Unturned.StructureDrop.OnSalvageRequested_Global -= oSR;
            Console.WriteLine("Removing player listeners..");
            /* Players */
            // Add on join
            SDG.Unturned.Provider.onEnemyConnected -= OPC;
            // Modify when moving or gesturing/stance
            UntPlayerEvents.OnPlayerUpdatePosition -= OPUP;
            SDG.Unturned.PlayerAnimator.OnGestureChanged_Global -= POGC;
            SDG.Unturned.PlayerStance.OnStanceChanged_Global -= POSC;
            SDG.Unturned.PlayerEquipment.OnPunch_Global -= POP;
            // SDG.Unturned.PlayerClothing.OnBackpackChanged_Global

            // Keep track of death
            SDG.Unturned.PlayerLife.onPlayerDied -= oPD;
            // Remove on leave
            SDG.Unturned.Provider.onEnemyDisconnected -= OPDC;

            Console.WriteLine("Removing Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Removing zombie listeners..");
            /* Zombies */

            Console.WriteLine("Removing vehicle listeners..");
            /* Vehicles */
            // Created
            Patches.UntPatchEvent.OnVehicleSpawned -= OVS;
            Patches.UntPatchEvent.OnVehicleSpawnedFromSpawnpoint -= OVS;
            // Interactivity
            SDG.Unturned.VehicleManager.onEnterVehicleRequested -= OEV;
            SDG.Unturned.VehicleManager.onExitVehicleRequested -= OExV;
            // Movement
            VehicleManager.onSwapSeatRequested -= OSSR;
            Patches.UntPatchEvent.OnVehicleMovementChanged -= OVMC;
            // Destroyed
            SDG.Unturned.VehicleManager.onDamageTireRequested -= ODT;
            SDG.Unturned.VehicleManager.OnVehicleExploded -= OVE;

        }

        // Level handlers
        void OLL(int level) {
            StartRec();
            foreach (InteractableVehicle Ve in SDG.Unturned.VehicleManager.vehicles)
                AddLog("20" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString()+'|'+Ve.asset.GUID+'|'+Ve.transform.position.x+','+Ve.transform.position.y+','+Ve.transform.position.z+','+Ve.transform.rotation.w+','+Ve.transform.rotation.x+','+Ve.transform.rotation.y+','+Ve.transform.rotation.z+'|'+Ve.tires.ToArray().ToString());
        }
        void OLE() {
            StopRec();
        }

        // Barricade/structure spawns
        void oBS(SDG.Unturned.BarricadeRegion region, SDG.Unturned.BarricadeDrop drop)
        {
            AddLog("03" + DateTime.Now.Ticks + '|'+drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.barricade.transform.parent?.GetInstanceID());
        }
        void oSS(SDG.Unturned.StructureRegion region, SDG.Unturned.StructureDrop drop)
        {
            AddLog("04" + DateTime.Now.Ticks + '|' +drop.instanceID+"|"+drop.asset.GUID+"|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.structure.transform.parent?.GetInstanceID());
        }
        // Deploy handlers, basically spawn handlers
        void ODBS(SDG.Unturned.Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if (shouldAllow && BarricadeManager.tryGetRegion(asset.barricade.transform, out byte x, out byte y, out ushort plant, out BarricadeRegion region))
            {
                AddLog("03" + DateTime.Now.Ticks + '|' + region.drops.Count + "|" + asset.GUID + "|" + asset.barricade.transform.position.x + "," + asset.barricade.transform.position.y + "," + asset.barricade.transform.position.z + "," + asset.barricade.transform.rotation.w + "," + asset.barricade.transform.rotation.x + "," + asset.barricade.transform.rotation.y + "," + asset.barricade.transform.rotation.z + "|" + asset.barricade.transform.parent?.GetInstanceID());
            }
        } // 4
        void ODSS(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if(shouldAllow && StructureManager.tryGetRegion(asset.structure.transform, out byte x, out byte y, out StructureRegion region))
            {
                AddLog("04" + DateTime.Now.Ticks + '|' + region.drops.Count + "|" + asset.GUID + "|" + asset.structure.transform.position.x + "," + asset.structure.transform.position.y + "," + asset.structure.transform.position.z + "," + asset.structure.transform.rotation.w + "," + asset.structure.transform.rotation.x + "," + asset.structure.transform.rotation.y + "," + asset.structure.transform.rotation.z + "|"+ asset.structure.transform.parent?.GetInstanceID());
            }
        } // 9
        // Move handlers
        void oTR(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Barricade
        {
            BarricadeDrop drop;
            if (shouldAllow && BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion) && (drop = barricadeRegion.drops.Find((BarricadeDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("05" + DateTime.Now.Ticks + '|' + instanceID + "|" +drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 5
        void oTR(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Structure
        {
            StructureDrop drop;
            if (shouldAllow && StructureManager.tryGetRegion(x, y, out StructureRegion structureRegion) && (drop = structureRegion.drops.Find((StructureDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("10" + DateTime.Now.Ticks + '|' + instanceID + "|" + drop.asset.GUID + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 10

        // Salvage handlers
        void oSR(SDG.Unturned.BarricadeDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if(shouldAllow)
                AddLog("07" + DateTime.Now.Ticks + '|' +structure.instanceID);
        } // 7
        void oSR(SDG.Unturned.StructureDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {
            if (shouldAllow)
                AddLog("12" + DateTime.Now.Ticks + '|' + structure.instanceID);
        } // 12
        // Damage handlers
        void oDBR(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) // Save when destroyed
        {
            BarricadeDrop drop;
            if (shouldAllow && (BarricadeManager.tryGetRegion(barricadeTransform, out byte x, out byte y, out ushort plant, out BarricadeRegion region) && (drop = region.drops.Find((o)=>o.instanceID == barricadeTransform.GetInstanceID())) != null) && (drop.asset.health - pendingTotalDamage <= 0))
                AddLog("06" + DateTime.Now.Ticks + '|' + barricadeTransform.GetInstanceID());
        } // 6
        void oDSR(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin) // Save when destroyed
        {
            StructureDrop drop;
            if (shouldAllow && (StructureManager.tryGetRegion(structureTransform, out byte x, out byte y, out StructureRegion region) && (drop = region.drops.Find((o) => o.instanceID == structureTransform.GetInstanceID())) != null) && (drop.asset.health - pendingTotalDamage <= 0))
                AddLog("11" + DateTime.Now.Ticks + '|' + structureTransform.GetInstanceID());
        } // 11
        // Player handlers
        void OPC(SteamPlayer player) // Create
        {
            AddLog("13" + DateTime.Now.Ticks + '|' + player.player.GetInstanceID() + "|" + player.player.transform.position.x + "," + player.player.transform.position.y + "," + player.player.transform.position.z + "," + player.player.transform.rotation.x + "," + player.player.transform.rotation.w + "|" + player.player.stance.stance);
            player.player.gameObject.AddComponent<UntPlayerFeature>();
            player.player.gameObject.AddComponent<UntPlayerEvents>();
        } // 13
        void OPDC(SteamPlayer player) // Disconnected
        {
            AddLog("19" + DateTime.Now.Ticks + '|' +player?.player?.GetInstanceID());
        } // 19

        void oPD(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator) // Death
        {
            AddLog("18" + DateTime.Now.Ticks + '|' + sender.GetInstanceID() + "|" + instigator + "|" + cause + "|" + limb);
        } // 18
        void OPUP(Player player, Vector3 position) // on player move or rotate (look)
        {
            AddLog("14" + DateTime.Now.Ticks + '|' + player.GetInstanceID() + "|" + position.x + "," + position.y + "," + position.z + "," + player.transform.rotation.x + "," + player.transform.rotation.w);
        } // 14
        void POGC(PlayerAnimator a, EPlayerGesture b) // Gesture
        {
            AddLog("15" + DateTime.Now.Ticks + '|' + a.player.GetInstanceID() + "|" + b);
        } // 15
        void POSC(PlayerStance a) // Stance
        {
            AddLog("16" + DateTime.Now.Ticks + '|' +a.player.GetInstanceID()+"|"+a.stance);
        } // 16
        void POP(PlayerEquipment a, EPlayerPunch b) // Punch
        {
            AddLog("17" + DateTime.Now.Ticks + '|' +a.player.GetInstanceID()+"|"+b);
        } // 17
        // Vehicles
        void OVS(InteractableVehicle Ve)
        {
            AddLog("20" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.asset.GUID + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.tires.ToArray().ToString());
        } // 20
        void OVS(VehicleSpawnpoint vehicleSpawnpoint, InteractableVehicle Ve)
        {
            AddLog("20" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.asset.GUID + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.tires.ToArray().ToString());
        } // 20
        void OVMC(InteractableVehicle Ve, Vector3 lastPosition)
        {
            AddLog("25" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z);
        } // 25
        void OEV(Player player, InteractableVehicle vehicle, ref bool shouldAllow) // Enter veh
        {
            if(vehicle.tryAddPlayer(out byte seat, player))
                AddLog("22" + DateTime.Now.Ticks + '|' +vehicle.instanceID+'|'+player.GetInstanceID()+'|'+seat);
        } // 22
        void OExV(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) // Exit veh
        {
            if (shouldAllow)
            {
                AddLog("23" + DateTime.Now.Ticks + '|' + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + pendingLocation.x + ',' + pendingLocation.y + ',' + pendingLocation.z + ',' + pendingYaw);
            }
        } // 23
        void OSSR(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex) // Swap seat
        {
            AddLog("24" + DateTime.Now.Ticks + '|' + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + toSeatIndex);
        } // 24
        void ODT(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) // tire damage
        {
            if(shouldAllow)
            {
                AddLog("26" + DateTime.Now.Ticks + '|' +vehicle.instanceID+"|"+tireIndex+"|"+damageOrigin);
            }
        } // 26
        void OVE(InteractableVehicle Veh) // Vehicle explode (destroyed)
        {
            AddLog("27" + DateTime.Now.Ticks + '|' + Veh.instanceID + '|' + Veh.transform.position.x + ',' + Veh.transform.position.y + ',' + Veh.transform.position.z);
        } // 27
    }
}
