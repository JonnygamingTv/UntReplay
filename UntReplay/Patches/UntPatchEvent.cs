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
        // Vehicles
        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);
        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player player, Vector3 lastPosition);
        public delegate void VehicleSpawned(InteractableVehicle vehicle);
        public delegate void VehicleSpawnedFromSpawnpoint(VehicleSpawnpoint vehicleSpawnpoint, InteractableVehicle vehicle);
        // Animals
        public delegate void AnimalDamaged(Animal animal, ushort damage, EPlayerKill kill, uint xp);
        public delegate void AnimalKilled(Animal animal, ref Vector3 ragdoll, ref ERagdollEffect ragdollEffect);
        public delegate void AnimalMovementChanged(Animal animal, Vector3 lastPosition);
        public delegate void AnimalSpawned(Animal animal, Vector3 position, byte angle);
        // Player vehicle interact
        public delegate void PlayerInteractedStereoTrack(Player player, InteractableStereo stereo, Guid track);
        public delegate void PlayerInteractedStereoVolume(Player player, InteractableStereo stereo, byte volume);
        // Player interactions
        public delegate void PlayerInteractedDisplay(Player player, InteractableStorage display, byte rot);
        public delegate void PlayerInteractedFire(Player player, InteractableFire fire, bool lit);
        public delegate void PlayerInteractedGenerator(Player player, InteractableGenerator generator, bool powered);
        public delegate void PlayerInteractedOven(Player player, InteractableOven oven, bool lit);
        public delegate void PlayerInteractedOxygenator(Player player, InteractableOxygenator oxygenator, bool powered);
        public delegate void PlayerInteractedSafezone(Player player, InteractableSafezone safezone, bool powered);
        public delegate void PlayerInteractedSign(Player player, InteractableSign sign, string text);
        public delegate void PlayerInteractedSpot(Player player, InteractableSpot spot, bool powered);
        public delegate void PlayerInteractedStorage(Player player, InteractableStorage storage, bool quickGrab);
        public delegate void PlayerInteractedLibrary(Player player, InteractableLibrary library, byte transaction, uint delta);
        // Mannequins
        public delegate void PlayerInteractedMannequinPose(Player player, InteractableMannequin mannequin, byte pose);
        public delegate void PlayerInteractedMannequinUpdate(Player player, InteractableMannequin mannequin, EMannequinUpdateMode updateMode);
        // Other?
        #endregion
        #region events
        // Vehicles
        public static event VehicleMovementChanged OnVehicleMovementChanged;
        public static event VehicleMovementChangedByPlayer OnVehicleMovementChangedByPlayer;
        public static event VehicleSpawned OnVehicleSpawned;
        public static event VehicleSpawnedFromSpawnpoint OnVehicleSpawnedFromSpawnpoint;
        // Animals
        public static event AnimalDamaged OnAnimalDamaged;
        public static event AnimalKilled OnAnimalKilled;
        public static event AnimalMovementChanged OnAnimalMovementChanged;
        public static event AnimalSpawned OnAnimalSpawned;
        // Player vehicle interact
        public static event PlayerInteractedStereoTrack OnPlayerInteractedStereoTrack;
        public static event PlayerInteractedStereoVolume OnPlayerInteractedStereoVolume;
        // Player interactions
        public static event PlayerInteractedDisplay OnPlayerInteractedDisplay;
        public static event PlayerInteractedFire OnPlayerInteractedFire;
        public static event PlayerInteractedGenerator OnPlayerInteractedGenerator;
        public static event PlayerInteractedOven OnPlayerInteractedOven;
        public static event PlayerInteractedOxygenator OnPlayerInteractedOxygenator;
        public static event PlayerInteractedSafezone OnPlayerInteractedSafezone;
        public static event PlayerInteractedSign OnPlayerInteractedSign;
        public static event PlayerInteractedSpot OnPlayerInteractedSpot;
        public static event PlayerInteractedLibrary OnPlayerInteractedLibrary;
        // Mannequins
        public static event PlayerInteractedMannequinPose OnPlayerInteractedMannequinPose;
        public static event PlayerInteractedMannequinUpdate OnPlayerInteractedMannequinUpdate;
        // Other?
        #endregion
        #region patches
        // Vehicles
        [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
            typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(float), typeof(float),
            typeof(float))]
        [HarmonyPrefix]
        internal static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance,
            out Vector3 __state,
            Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
            Vector3 point, Quaternion angle, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
        {
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

        // Animals
        [HarmonyPatch(typeof(Animal), "tick")]
        [HarmonyPostfix]
        internal static void OnAnimalMovementChangedInvoker(Vector3 __state, Animal __instance)
        {
            if (__instance.transform.position == __state)
                return;

            OnAnimalMovementChanged?.Invoke(__instance, __state);
        }
        [HarmonyPatch(typeof(AnimalManager), "sendAnimalAlive")]
        [HarmonyPostfix]
        internal static void OnPreAnimalSpawnedInvoker(bool __state, Animal animal, Vector3 newPosition,
    byte newAngle)
        {
            if (!__state)
                return;

            OnAnimalSpawned?.Invoke(animal, newPosition, newAngle);
        }
        [HarmonyPatch(typeof(Animal), "askDamage")]
        [HarmonyPostfix]
        internal static void OnAnimalKilledInvoker(bool __state, Animal __instance, ushort amount,
    ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
    ref ERagdollEffect ragdollEffect)
        {
            if (!__state)
                return;

            OnAnimalDamaged?.Invoke(__instance, amount, kill, xp);
        }
        [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalDead")]
        [HarmonyPostfix]
        internal static void OnAnimalKilledInvoker(bool __state, ushort index, ref Vector3 newRagdoll,
    ref ERagdollEffect newRagdollEffect)
        {
            if (!__state)
                return;

            OnAnimalKilled?.Invoke(AnimalManager.animals.ElementAtOrDefault(index), ref newRagdoll,
                ref newRagdollEffect);
        }
        // Player vehicle interaction
        [HarmonyPatch(typeof(InteractableStereo), "ReceiveTrackRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedStereoInvoker(bool __state, InteractableStereo __instance,
    in ServerInvocationContext context, Guid newTrack)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedStereoTrack?.Invoke(player, __instance, newTrack);
        }
        [HarmonyPatch(typeof(InteractableStereo), "ReceiveChangeVolumeRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedStereoInvoker(bool __state, InteractableStereo __instance,
    in ServerInvocationContext context, byte newVolume)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedStereoVolume?.Invoke(player, __instance, newVolume);
        }
        // Player interactions

        [HarmonyPatch(typeof(InteractableStorage), "ReceiveRotDisplayRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedDisplayInvoker(bool __state, InteractableStorage __instance,
    in ServerInvocationContext context, byte rotComp)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedDisplay?.Invoke(player, __instance, rotComp);
        }
        [HarmonyPatch(typeof(InteractableFire), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedFireInvoker(bool __state, InteractableFire __instance,
            in ServerInvocationContext context, bool desiredLit)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedFire?.Invoke(player, __instance, desiredLit);
        }
        [HarmonyPatch(typeof(InteractableGenerator), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedGeneratorInvoker(bool __state,
    InteractableGenerator __instance,
    in ServerInvocationContext context, bool desiredPowered)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedGenerator?.Invoke(player, __instance, desiredPowered);
        }
        [HarmonyPatch(typeof(InteractableOven), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedOvenInvoker(bool __state, InteractableOven __instance,
    in ServerInvocationContext context, bool desiredLit)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedOven?.Invoke(player, __instance, desiredLit);
        }
        [HarmonyPatch(typeof(InteractableOxygenator), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedOxygenatorInvoker(bool __state,
    InteractableOxygenator __instance, in ServerInvocationContext context, bool desiredPowered)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedOxygenator?.Invoke(player, __instance, desiredPowered);
        }
        [HarmonyPatch(typeof(InteractableSafezone), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedSafezoneInvoker(bool __state, InteractableSafezone __instance,
    in ServerInvocationContext context, bool desiredPowered)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedSafezone?.Invoke(player, __instance, desiredPowered);
        }
        [HarmonyPatch(typeof(InteractableSign), "ReceiveChangeTextRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedSignInvoker(bool __state, InteractableSign __instance,
    in ServerInvocationContext context, string newText)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedSign?.Invoke(player, __instance, newText);
        }
        [HarmonyPatch(typeof(InteractableSpot), "ReceiveToggleRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedSpotInvoker(bool __state, InteractableSpot __instance,
    in ServerInvocationContext context, bool desiredPowered)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedSpot?.Invoke(player, __instance, desiredPowered);
        }
        [HarmonyPatch(typeof(InteractableLibrary), "ReceiveTransferLibraryRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedLibraryInvoker(bool __state, InteractableLibrary __instance,
    in ServerInvocationContext context, byte transaction, uint delta)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedLibrary?.Invoke(player, __instance, transaction, delta);
        }
        // Mannequins
        [HarmonyPatch(typeof(InteractableMannequin), "ReceiveUpdateRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedMannequinInvoker(bool __state,
    InteractableMannequin __instance,
    in ServerInvocationContext context, EMannequinUpdateMode updateMode)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedMannequinUpdate?.Invoke(player, __instance, updateMode);
        }
        [HarmonyPatch(typeof(InteractableMannequin), "ReceivePoseRequest")]
        [HarmonyPostfix]
        internal static void OnPlayerInteractedMannequinInvoker(bool __state,
    InteractableMannequin __instance, in ServerInvocationContext context, byte poseComp)
        {
            if (!__state)
                return;

            var player = context.GetPlayer();
            if (player == null)
                return;

            OnPlayerInteractedMannequinPose?.Invoke(player, __instance, poseComp);
        }
        // Other?
        #endregion
    }
}
