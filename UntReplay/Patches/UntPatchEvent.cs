using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UntReplay.Patches
{
    public static class UntPatchEvent
    {
        #region delegates
        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);
        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player player,
            Vector3 lastPosition);
        public delegate void VehicleSpawned(InteractableVehicle vehicle);
        public delegate void VehicleSpawnedFromSpawnpoint(VehicleSpawnpoint vehicleSpawnpoint,
            InteractableVehicle vehicle);

        #endregion
        #region events
        public static event VehicleMovementChanged OnVehicleMovementChanged;
        public static event VehicleMovementChangedByPlayer OnVehicleMovementChangedByPlayer;
        public static event VehicleSpawned OnVehicleSpawned;
        public static event VehicleSpawnedFromSpawnpoint OnVehicleSpawnedFromSpawnpoint;
        #endregion
        #region patches
        [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
            typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(float), typeof(float),
            typeof(float))]
        [HarmonyPrefix]
        internal static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance,
            out Vector3 __state,
            Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
            Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
        {
            // var shouldAllow = true;
            // OnPreVehicleMovementChangedByPlayer?.Invoke(__instance,
            //     __instance.passengers.ElementAtOrDefault(0)?.player?.player, ___lastUpdatedPos, ref shouldAllow);
            // return shouldAllow;
            __state = ___lastUpdatedPos;
        }

        [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
            typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(float), typeof(float),
            typeof(float))]
        [HarmonyPostfix]
        internal static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance, Vector3 __state, uint simulation, int recov, bool inputStamina,
            Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
        {
            OnVehicleMovementChangedByPlayer?.Invoke(__instance,
                __instance.passengers.ElementAtOrDefault(0)?.player?.player, __state);
        }

        [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset), typeof(ushort), typeof(ushort), typeof(float), typeof(Vector3), typeof(Quaternion), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(ushort), typeof(ushort), typeof(ushort), typeof(CSteamID), typeof(CSteamID), typeof(bool), typeof(byte[][]), typeof(byte))]
        [HarmonyPostfix]
        internal static void OnPreVehicleSpawnedInvoker(bool __state,
            InteractableVehicle __result,
            VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle,
            bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health,
            ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask)
        {
            if (!__state)
                return;

            OnVehicleSpawned?.Invoke(__result);
        }

        [HarmonyPatch(typeof(VehicleManager), "SpawnVehicleV3", typeof(VehicleAsset), typeof(ushort), typeof(ushort), typeof(float), typeof(Vector3), typeof(Quaternion), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(ushort), typeof(ushort), typeof(ushort), typeof(CSteamID), typeof(CSteamID), typeof(bool), typeof(byte[][]), typeof(byte), typeof(Color32))]
        [HarmonyPostfix]
        internal static void OnPreVehicleSpawnedInvoker(bool __state,
            InteractableVehicle __result,
            VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle,
            bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health,
            ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask, Color32 paintColor)
        {
            if (!__state)
                return;

            OnVehicleSpawned?.Invoke(__result);
        }

        [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
        [HarmonyPostfix]
        internal static void OnPreVehicleSpawnedFromSpawnpointInvoker(bool __state,
    InteractableVehicle __result, VehicleSpawnpoint spawn)
        {
            if (!__state)
                return;

            OnVehicleSpawnedFromSpawnpoint?.Invoke(spawn, __result);
        }

        [HarmonyPatch(typeof(InteractableVehicle), "UpdateSafezoneStatus")]
        [HarmonyPrefix]
        internal static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance, out Vector3 __state,
    Vector3 ___lastUpdatedPos, float deltaSeconds)
        {
            __state = ___lastUpdatedPos;
        }

        [HarmonyPatch(typeof(InteractableVehicle), "UpdateSafezoneStatus")]
        [HarmonyPostfix]
        internal static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance,
            Vector3 __state, float deltaSeconds)
        {
            if (__instance.transform.position == __state)
                return;

            OnVehicleMovementChanged?.Invoke(__instance, __state);
        }
        #endregion
    }
}
