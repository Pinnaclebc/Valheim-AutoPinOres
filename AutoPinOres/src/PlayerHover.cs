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


            if (hoverObj.GetComponentInParent<ItemDrop>() != null)
            {
                return;
            }


            MineRock5 mr5 = hoverObj.GetComponentInParent<MineRock5>();
            MineRock mr = hoverObj.GetComponentInParent<MineRock>();
            Destructible dest = hoverObj.GetComponentInParent<Destructible>();

            if (mr5 == null && mr == null && dest == null)
            {
                return;
            }

            Transform rootTransform = hoverObj.transform.root;
            GameObject rootObj = rootTransform != null ? rootTransform.gameObject : hoverObj;
            GameObject parentObj = hoverObj.transform.parent != null ? hoverObj.transform.parent.gameObject : null;

            string pinName = CheckOreName(hoverObj.name)
                ?? (parentObj != null ? CheckOreName(parentObj.name) : null)
                ?? CheckOreName(rootObj.name);

            if (pinName == null && IsCopperNode(hoverObj, mr5, dest))
            {
                pinName = "Copper";
            }

            if (pinName == null)
            {
                return;
            }

            GameObject targetObj = mr5 != null ? mr5.gameObject : (mr != null ? mr.gameObject : (dest != null ? dest.gameObject : (parentObj != null ? parentObj : rootObj)));

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


            if (lower.EndsWith("ore") || lower.Contains("scrap") || lower.Contains("item"))
            {
                return null;
            }

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

        private static bool IsCopperNode(GameObject obj, MineRock5 mr5, Destructible dest)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.name.ToLower().Contains("copper"))
            {
                return true;
            }

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