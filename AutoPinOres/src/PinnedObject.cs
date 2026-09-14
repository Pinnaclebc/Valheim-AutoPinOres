using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace AutoPinOres
{
    class PinnedObject : MonoBehaviour
    {
        public Minimap.PinData pin;
        public string currentPinName;

        public void Init(string aName)
        {
            currentPinName = aName;
            pin = Minimap.instance.AddPin(transform.position, Minimap.PinType.Icon3, aName, true, false);
            Mod.Log.LogInfo(string.Format("Tracking: {0} at {1} {2} {3}", aName, transform.position.x, transform.position.y, transform.position.z));
        }

        public bool HasActivePin()
        {
            if (pin == null || Minimap.instance == null)
            {
                return false;
            }

            var pinsList = Traverse.Create(Minimap.instance).Field("m_pins").GetValue<List<Minimap.PinData>>();
            if (pinsList == null)
            {
                pinsList = Traverse.Create(Minimap.instance).Field("m_customPins").GetValue<List<Minimap.PinData>>();
            }

            if (pinsList != null)
            {
                return pinsList.Contains(pin);
            }

            return false;
        }

    }
}