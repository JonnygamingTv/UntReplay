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
            SDG.Unturned.PlayerLife.onPlayerLifeUpdated += PLU; // 18
            SDG.Unturned.PlayerLife.onPlayerDied += oPD; // 19
            // Remove on leave
            SDG.Unturned.Provider.onEnemyDisconnected += OPDC; // 20

            Console.WriteLine("Adding Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Adding zombie listeners..");
            /* Zombies */

            Console.WriteLine("Adding vehicle listeners..");
            /* Vehicles */
            // Created
            Patches.UntPatchEvent.OnVehicleSpawned += OVS; // 71
            Patches.UntPatchEvent.OnVehicleSpawnedFromSpawnpoint += OVS; // 72
            // Interactivity
            SDG.Unturned.VehicleManager.onEnterVehicleRequested += OEV; // 73
            SDG.Unturned.VehicleManager.onExitVehicleRequested += OExV; // 74
            // Movement
            VehicleManager.onSwapSeatRequested += OSSR; // 75
            Patches.UntPatchEvent.OnVehicleMovementChanged += OVMC; // 76
            Patches.UntPatchEvent.OnVehicleMovementChangedByPlayer += OVMC; // 76
            // Destroyed
            SDG.Unturned.VehicleManager.onDamageTireRequested += ODT; // 77
            SDG.Unturned.VehicleManager.OnVehicleExploded += OVE; // 78

            /* Items */
            ItemManager.onItemDropAdded += IteSpawn; // 91
            ItemManager.onItemDropRemoved += IteDespawn; // 92

            /* Setup for save */
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
            SDG.Unturned.PlayerLife.onPlayerLifeUpdated -= PLU;
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
                AddLog("20" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString()+'|'+Ve.asset.GUID.ToString()+'|'+Ve.transform.position.x+','+Ve.transform.position.y+','+Ve.transform.position.z+','+Ve.transform.rotation.w+','+Ve.transform.rotation.x+','+Ve.transform.rotation.y+','+Ve.transform.rotation.z+'|'+Ve.tires.ToArray().ToString());
            foreach (SteamPlayer player in SDG.Unturned.Provider.clients)
            {
                AddLog("13" + DateTime.Now.Ticks + '|' + player.player.GetInstanceID() + "|" + player.player.transform.position.x + "," + player.player.transform.position.y + "," + player.player.transform.position.z + "," + player.player.transform.rotation.x + "," + player.player.transform.rotation.w + "|" + player.player.stance.stance);
                if (player.player.gameObject.GetComponent<UntPlayerFeature>() == null)
                {
                    player.player.gameObject.AddComponent<UntPlayerFeature>();
                    player.player.gameObject.AddComponent<UntPlayerEvents>();
                }
            }
            foreach(BarricadeRegion reg in BarricadeManager.regions)
            {
                foreach(BarricadeDrop drop in reg.drops) {
                    AddLog("03" + DateTime.Now.Ticks + '|' + drop.instanceID + "|" + drop.asset.GUID.ToString() + "|" + drop.asset.barricade.transform.position.x + "," + drop.asset.barricade.transform.position.y + "," + drop.asset.barricade.transform.position.z + "," + drop.asset.barricade.transform.rotation.w + "," + drop.asset.barricade.transform.rotation.x + "," + drop.asset.barricade.transform.rotation.y + "," + drop.asset.barricade.transform.rotation.z + "|" + drop.asset.barricade.transform.parent?.GetInstanceID());
                }
            }
            foreach(StructureRegion reg in StructureManager.regions)
            {
                foreach (StructureDrop drop in reg.drops) {
                    AddLog("04" + DateTime.Now.Ticks + '|' + drop.instanceID + "|" + drop.asset.GUID.ToString() + "|" + drop.asset.structure.transform.position.x + "," + drop.asset.structure.transform.position.y + "," + drop.asset.structure.transform.position.z + "," + drop.asset.structure.transform.rotation.w + "," + drop.asset.structure.transform.rotation.x + "," + drop.asset.structure.transform.rotation.y + "," + drop.asset.structure.transform.rotation.z + "|" + drop.asset.structure.transform.parent?.GetInstanceID());
                }
            }
        }
        void OLE() {
            StopRec();
        }

        // Barricade/structure spawns
        void oBS(SDG.Unturned.BarricadeRegion region, SDG.Unturned.BarricadeDrop drop)
        {
            AddLog("03" + DateTime.Now.Ticks + '|'+drop.instanceID+"|"+drop.asset.GUID.ToString() + "|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.barricade.transform.parent?.GetInstanceID());
        }
        void oSS(SDG.Unturned.StructureRegion region, SDG.Unturned.StructureDrop drop)
        {
            AddLog("04" + DateTime.Now.Ticks + '|' +drop.instanceID+"|"+drop.asset.GUID.ToString() + "|"+drop.model.position.x+","+drop.model.position.y+","+drop.model.position.z+","+drop.model.rotation.w+","+drop.model.rotation.x+","+drop.model.rotation.y+","+drop.model.rotation.z + "|" + drop.asset.structure.transform.parent?.GetInstanceID());
        }
        // Deploy handlers, basically spawn handlers
        void ODBS(SDG.Unturned.Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if (shouldAllow && BarricadeManager.tryGetRegion(asset.barricade.transform, out byte x, out byte y, out ushort plant, out BarricadeRegion region))
            {
                AddLog("03" + DateTime.Now.Ticks + '|' + region.drops.Count + "|" + asset.GUID.ToString() + "|" + asset.barricade.transform.position.x + "," + asset.barricade.transform.position.y + "," + asset.barricade.transform.position.z + "," + asset.barricade.transform.rotation.w + "," + asset.barricade.transform.rotation.x + "," + asset.barricade.transform.rotation.y + "," + asset.barricade.transform.rotation.z + "|" + asset.barricade.transform.parent?.GetInstanceID());
            }
        } // 4
        void ODSS(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            if(shouldAllow && StructureManager.tryGetRegion(asset.structure.transform, out byte x, out byte y, out StructureRegion region))
            {
                AddLog("04" + DateTime.Now.Ticks + '|' + region.drops.Count + "|" + asset.GUID.ToString() + "|" + asset.structure.transform.position.x + "," + asset.structure.transform.position.y + "," + asset.structure.transform.position.z + "," + asset.structure.transform.rotation.w + "," + asset.structure.transform.rotation.x + "," + asset.structure.transform.rotation.y + "," + asset.structure.transform.rotation.z + "|"+ asset.structure.transform.parent?.GetInstanceID());
            }
        } // 9
        // Move handlers
        void oTR(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Barricade
        {
            BarricadeDrop drop;
            if (shouldAllow && BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion) && (drop = barricadeRegion.drops.Find((BarricadeDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("05" + DateTime.Now.Ticks + '|' + instanceID + "|" +drop.asset.GUID.ToString() + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
            }
        } // 5
        void oTR(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Structure
        {
            StructureDrop drop;
            if (shouldAllow && StructureManager.tryGetRegion(x, y, out StructureRegion structureRegion) && (drop = structureRegion.drops.Find((StructureDrop o) => o.instanceID == instanceID)) != null) {
                AddLog("10" + DateTime.Now.Ticks + '|' + instanceID + "|" + drop.asset.GUID.ToString() + "|" + drop.model.position.x + "," + drop.model.position.y + "," + drop.model.position.z + "," + drop.model.rotation.w + "," + drop.model.rotation.x + "," + drop.model.rotation.y + "," + drop.model.rotation.z);
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
            if (player.player.gameObject.GetComponent<UntPlayerFeature>() == null)
            {
                player.player.gameObject.AddComponent<UntPlayerFeature>();
                player.player.gameObject.AddComponent<UntPlayerEvents>();
            }
        } // 13
        void OPDC(SteamPlayer player) // Disconnected
        {
            AddLog("20" + DateTime.Now.Ticks + '|' +player?.player?.GetInstanceID());
        } // 20
        void PLU(Player player)
        {
            if(player.life.IsAlive)
                AddLog("18" + DateTime.Now.Ticks + '|' + player.GetInstanceID() + "|" + player.life.health);
        } // 18

        void oPD(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator) // Death
        {
            AddLog("19" + DateTime.Now.Ticks + '|' + sender.GetInstanceID() + "|" + instigator + "|" + cause + "|" + limb);
        } // 19
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
            AddLog("71" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.asset.GUID.ToString() + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.tires.ToArray().ToString());
        } // 71
        void OVS(VehicleSpawnpoint vehicleSpawnpoint, InteractableVehicle Ve)
        {
            AddLog("72" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.asset.GUID.ToString() + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.tires.ToArray().ToString());
        } // 72
        void OVMC(InteractableVehicle Ve, Vector3 lastPosition)
        {
            string s = "76" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.ReplicatedSpeed;
            for (byte i = 0; i < Ve.tires?.Length; i++) s += "|" + Ve.tires?[i]?.wheel?.rpm + "," + Ve.tires?[i]?.wheel?.steerAngle + "," + Ve.tires?[i]?.wheel?.suspensionDistance;
            AddLog(s);
        } // 76
        void OVMC(InteractableVehicle Ve, Player player, Vector3 lastPosition) 
        {
            string s = "76" + DateTime.Now.Ticks + '|' + Ve.instanceID.ToString() + '|' + Ve.transform.position.x + ',' + Ve.transform.position.y + ',' + Ve.transform.position.z + ',' + Ve.transform.rotation.w + ',' + Ve.transform.rotation.x + ',' + Ve.transform.rotation.y + ',' + Ve.transform.rotation.z + '|' + Ve.ReplicatedSpeed;
            for(byte i = 0; i < Ve.tires?.Length; i++) s += "|" + Ve.tires?[i]?.wheel?.rpm + "," + Ve.tires?[i]?.wheel?.steerAngle + "," + Ve.tires?[i]?.wheel?.suspensionDistance;
            AddLog(s);
        }
        void OEV(Player player, InteractableVehicle vehicle, ref bool shouldAllow) // Enter veh
        {
            if(vehicle.tryAddPlayer(out byte seat, player))
                AddLog("73" + DateTime.Now.Ticks + '|' +vehicle.instanceID+'|'+player.GetInstanceID()+'|'+seat);
        } // 73
        void OExV(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) // Exit veh
        {
            if (shouldAllow)
            {
                AddLog("74" + DateTime.Now.Ticks + '|' + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + pendingLocation.x + ',' + pendingLocation.y + ',' + pendingLocation.z + ',' + pendingYaw);
            }
        } // 74
        void OSSR(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex) // Swap seat
        {
            AddLog("75" + DateTime.Now.Ticks + '|' + vehicle.instanceID + '|' + player.GetInstanceID() + '|' + toSeatIndex);
        } // 75
        void ODT(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) // tire damage
        {
            if(shouldAllow)
            {
                AddLog("77" + DateTime.Now.Ticks + '|' +vehicle.instanceID+"|"+tireIndex+"|"+damageOrigin);
            }
        } // 77
        void OVE(InteractableVehicle Veh) // Vehicle explode (destroyed)
        {
            AddLog("78" + DateTime.Now.Ticks + '|' + Veh.instanceID + '|' + Veh.transform.position.x + ',' + Veh.transform.position.y + ',' + Veh.transform.position.z);
        } // 78

        void IteSpawn(Transform model, InteractableItem interactableItem)
        {
            AddLog("91" + DateTime.Now.Ticks + '|' + interactableItem.transform.position.x + ',' + interactableItem.transform.position.y + ',' + interactableItem.transform.position.z + ',' + interactableItem.transform.rotation.x + ',' + interactableItem.transform.rotation.w + '|' + interactableItem.asset.GUID.ToString().ToString());
        } // 91
        void IteDespawn(Transform model, InteractableItem interactableItem)
        {
            AddLog("92" + DateTime.Now.Ticks + '|');
        } // 92
    }
}
