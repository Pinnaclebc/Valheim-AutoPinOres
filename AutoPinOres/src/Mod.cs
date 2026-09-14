using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace AutoPinOres
{
    [BepInPlugin("net.pinnaclebc.valheim.autopinores", "Auto Map Pins", "1.0.1")]
    class Mod : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        void Awake()
        {
            var harmony = new Harmony("net.pinnaclebc.valheim.autopinores");
            harmony.PatchAll();
            Log = Logger;
        }
    }
}
