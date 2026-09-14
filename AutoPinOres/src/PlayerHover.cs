using HarmonyLib;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace AutoPinOres
{

    [HarmonyPatch(typeof(Player), "UpdateHover")]
    class PlayerHoverPatch
    {
        private static void Postfix(ref Player __instance)
        {
            if (__instance != Player.m_localPlayer)
            {
                return;
            }

            GameObject hoverObj = __instance.GetHoverObject();
            if (hoverObj == null)
            {
                return;
            }

            Transform rootTransform = hoverObj.transform.root;
            GameObject targetObj = rootTransform != null ? rootTransform.gameObject : hoverObj;

            var existingPo = targetObj.GetComponent<PinnedObject>();
            if (existingPo == null)
            {
                existingPo = hoverObj.GetComponent<PinnedObject>();
            }

            if (existingPo != null && existingPo.HasActivePin())
            {
                return;
            }

            string targetName = targetObj.name.Replace("(Clone)", "").Trim();
            string hoverName = hoverObj.name.Replace("(Clone)", "").Trim();
            string pinName = null;

            pinName = CheckOreName(targetName);
            if (pinName == null)
            {
                pinName = CheckOreName(hoverName);
            }

            if (pinName != null)
            {
                if (existingPo != null)
                {
                    existingPo.Init(pinName);
                }
                else
                {
                    var po = targetObj.AddComponent<PinnedObject>();
                    po.Init(pinName);
                }
            }
        }

        private static string CheckOreName(string name)
        {
            switch (name)
            {
                case "MineRock_Tin":
                    return "Tin";
                case "MineRock_Copper":
                case "rock4_copper":
                    return "Copper";
                case "MineRock_Obsidian":
                    return "Obsidian";
                case "MineRock_Silver":
                case "silvervein":
                    return "Silver";
                case "MineRock_Meteorite":
                case "MineRock_Flametal":
                case "LeviathanLava":
                    return "Flametal";
                case "MineRock_Iron":
                    return "Iron";
                default:
                    return null;
            }
        }
    }
}