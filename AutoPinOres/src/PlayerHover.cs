using HarmonyLib;
using UnityEngine;

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
            GameObject rootObj = rootTransform != null ? rootTransform.gameObject : hoverObj;
            GameObject parentObj = hoverObj.transform.parent != null ? hoverObj.transform.parent.gameObject : null;

            string pinName = CheckOreName(hoverObj.name)
                ?? (parentObj != null ? CheckOreName(parentObj.name) : null)
                ?? CheckOreName(rootObj.name);

            if (pinName == null && IsCopperNode(hoverObj))
            {
                pinName = "Copper";
            }

            if (pinName == null)
            {
                return;
            }

            MineRock5 mr5 = hoverObj.GetComponentInParent<MineRock5>();
            GameObject targetObj = mr5 != null ? mr5.gameObject : (parentObj != null ? parentObj : rootObj);

            var existingPo = targetObj.GetComponent<PinnedObject>()
                ?? hoverObj.GetComponent<PinnedObject>();

            if (existingPo != null && existingPo.HasActivePin())
            {
                return;
            }

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

        private static string CheckOreName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            string lower = name.ToLower();

            if (lower.Contains("copper"))
            {
                return "Copper";
            }

            if (lower.Contains("tin"))
            {
                return "Tin";
            }

            if (lower.Contains("obsidian"))
            {
                return "Obsidian";
            }

            if (lower.Contains("silver"))
            {
                return "Silver";
            }

            if (lower.Contains("meteorite") || lower.Contains("flametal") || lower.Contains("leviathanlava"))
            {
                return "Flametal";
            }

            if (lower.Contains("iron") || lower.Contains("bogiron"))
            {
                return "Iron";
            }

            return null;
        }

        private static bool IsCopperNode(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.name.ToLower().Contains("copper"))
            {
                return true;
            }

            MineRock5 mr5 = obj.GetComponentInParent<MineRock5>();
            if (mr5 != null)
            {
                if (mr5.m_name == "$piece_deposit" || mr5.m_name.ToLower().Contains("copper"))
                {
                    return true;
                }

                if (mr5.gameObject.name.ToLower().Contains("copper"))
                {
                    return true;
                }
            }

            Destructible dest = obj.GetComponentInParent<Destructible>();
            if (dest != null)
            {
                HoverText ht = dest.GetComponent<HoverText>();
                if (ht != null && (ht.m_text == "$piece_deposit" || ht.m_text.ToLower().Contains("copper")))
                {
                    return true;
                }
            }

            return false;
        }
    }
}