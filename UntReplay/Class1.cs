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
        public void initialize()
        {
            Console.WriteLine("Unturned ReplayMod starting..\nBrought to you by JonHosting.com!");
            Console.WriteLine("Adding level listeners..");
            /* Listen on level load to reset data, basically. */
            SDG.Unturned.Level.onPostLevelLoaded += OLL;
            SDG.Unturned.Level.onLevelExited += OLE;
            Console.WriteLine("Adding barricade listeners..");
            /* Barricades */
            // Add when placed
            SDG.Unturned.BarricadeManager.onBarricadeSpawned += oBS;
            SDG.Unturned.BarricadeManager.onDeployBarricadeRequested += ODBS;
            // Modify when changed
            SDG.Unturned.BarricadeManager.onTransformRequested += oTR;
            // Remove when destroyed
            SDG.Unturned.BarricadeManager.onDamageBarricadeRequested += oDBR;
            SDG.Unturned.BarricadeDrop.OnSalvageRequested_Global += oSR;
            Console.WriteLine("Adding structure listeners..");
            /* Structures */
            // Add when placed
            SDG.Unturned.StructureManager.onStructureSpawned += oSS;
            SDG.Unturned.StructureManager.onDeployStructureRequested += ODSS;
            // Modify when changed
            SDG.Unturned.StructureManager.onTransformRequested += oTR;
            // Remove when destroyed
            SDG.Unturned.StructureManager.onDamageStructureRequested += oDSR;
            SDG.Unturned.StructureDrop.OnSalvageRequested_Global += oSR;
            Console.WriteLine("Adding player listeners..");
            /* Players */
            // Add on join
            SDG.Unturned.Player.onPlayerCreated += oPC;
            // Modify when moving or gesturing/stance

            SDG.Unturned.PlayerAnimator.OnGestureChanged_Global += POGC;
            SDG.Unturned.PlayerStance.OnStanceChanged_Global += POSC;
            SDG.Unturned.PlayerEquipment.OnPunch_Global += POP;
            // SDG.Unturned.PlayerClothing.OnBackpackChanged_Global

            // Keep track of death
            SDG.Unturned.PlayerLife.onPlayerDied += oPD;
            // Remove on leave
            

            Console.WriteLine("Adding Animal listeners..");
            /* Animals */

            // Keep track of death

            Console.WriteLine("Adding zombie listeners..");
            /* Zombies */

            Console.WriteLine("Adding vehicle listeners..");
            /* Vehicles */
            SDG.Unturned.VehicleManager.onEnterVehicleRequested += OEV;
            SDG.Unturned.VehicleManager.onExitVehicleRequested += OExV;
            VehicleManager.onSwapSeatRequested += OSSR;
            SDG.Unturned.VehicleManager.onDamageTireRequested += ODT;
            SDG.Unturned.VehicleManager.OnVehicleExploded += OVE;
        }

        public void shutdown()
        {
            Console.WriteLine("Unturned ReplayMod exiting..");
        }
        // Level handlers
        void OLL(int level) { }
        void OLE() { }

        // Barricade/structure spawns
        void oBS(SDG.Unturned.BarricadeRegion region, SDG.Unturned.BarricadeDrop drop)
        {

        }
        void oSS(SDG.Unturned.StructureRegion region, SDG.Unturned.StructureDrop drop)
        {

        }
        // Deploy handlers, basically spawn handlers
        void ODBS(SDG.Unturned.Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {

        }
        void ODSS(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
        }
        // Move handlers
        void oTR(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Barricade
        {

        }
        void oTR(CSteamID instigator, byte x, byte y, uint instanceID, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldAllow) // Structure
        {

        }

        // Salvage handlers
        void oSR(SDG.Unturned.BarricadeDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {

        }
        void oSR(SDG.Unturned.StructureDrop structure, SDG.Unturned.SteamPlayer instigatorClient, ref bool shouldAllow)
        {

        }
        // Damage handlers
        void oDBR(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {

        }
        void oDSR(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {

        }
        // Player handlers
        void oPC(Player player) // Create
        {

        }

        void oPD(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator) // Death
        {
        }
        void POGC(PlayerAnimator a, EPlayerGesture b)
        {

        }
        void POSC(PlayerStance a)
        {

        }
        void POP(PlayerEquipment a, EPlayerPunch b)
        {

        }
        // Vehicles
        void OSSR(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex) { } // Swap seat
        void OEV(Player player, InteractableVehicle vehicle, ref bool shouldAllow) { } // Enter veh
        void OExV(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw) { } // Exit veh
        void ODT(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin) { }
        void OVE(InteractableVehicle Veh) { }
    }
}
