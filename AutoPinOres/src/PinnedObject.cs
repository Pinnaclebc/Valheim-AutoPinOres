using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace AutoPinOres
{
    class PinnedObject : MonoBehaviour
    {
        public Minimap.PinData pin;
        public string currentPinName;

        private MineRock5 mineRock5;
        private Destructible destructible;
        private MineRock mineRock;
        private float checkTimer = 0f;

        public void Init(string aName)
        {
            currentPinName = aName;

            mineRock5 = GetComponent<MineRock5>();
            destructible = GetComponent<Destructible>();
            mineRock = GetComponent<MineRock>();

            Minimap.PinData existingPin = FindExistingNearbyPin(aName, 12f);

            if (existingPin != null)
            {
                pin = existingPin;
            }
            else
            {
                pin = Minimap.instance.AddPin(transform.position, Minimap.PinType.Icon3, aName, true, false);
                Mod.Log.LogInfo(string.Format("Tracking: {0} at {1} {2} {3}", aName, transform.position.x, transform.position.y, transform.position.z));
            }
        }

        void Update()
        {
            if (pin == null)
            {
                return;
            }

            checkTimer += Time.deltaTime;
            if (checkTimer < 2f)
            {
                return;
            }
            checkTimer = 0f;

            if (IsDepositDepleted())
            {
                RemoveCurrentPin();
                Destroy(this);
            }
        }

        private bool IsDepositDepleted()
        {
 
            if (mineRock5 != null)
            {

                var hitAreas = Traverse.Create(mineRock5).Field("m_hitAreas").GetValue<System.Collections.IList>();
                if (hitAreas == null || hitAreas.Count == 0)
                {
                    return true;
                }

 
                bool anyRemaining = false;
                foreach (var area in hitAreas)
                {
                    if (area == null)
                    {
                        continue;
                    }

    
                    var comp = area as Component;
                    if (comp != null && comp.gameObject.activeInHierarchy)
                    {
                        anyRemaining = true;
                        break;
                    }


                    float health = Traverse.Create(area).Field("m_health").GetValue<float>();
                    if (health > 0f)
                    {
                        anyRemaining = true;
                        break;
                    }
                }


                return !anyRemaining;
            }


            if (mineRock != null)
            {
                var hitAreas = Traverse.Create(mineRock).Field("m_hitAreas").GetValue<System.Collections.IList>();
                return hitAreas == null || hitAreas.Count == 0;
            }

            return false;
        }

        private Minimap.PinData FindExistingNearbyPin(string name, float searchRadius)
        {
            if (Minimap.instance == null)
            {
                return null;
            }

            var pinsList = Traverse.Create(Minimap.instance).Field("m_pins").GetValue<List<Minimap.PinData>>();
            if (pinsList == null)
            {
                pinsList = Traverse.Create(Minimap.instance).Field("m_customPins").GetValue<List<Minimap.PinData>>();
            }

            if (pinsList == null)
            {
                return null;
            }

            foreach (var p in pinsList)
            {
                if (p.m_name == name && Vector3.Distance(p.m_pos, transform.position) <= searchRadius)
                {
                    return p;
                }
            }

            return null;
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

            return pinsList != null && pinsList.Contains(pin);
        }

        private void RemoveCurrentPin()
        {
            if (HasActivePin())
            {
                Minimap.instance.RemovePin(pin);
                Mod.Log.LogInfo(string.Format("Removed completed ore pin: {0}", currentPinName));
                pin = null;
            }
        }

        void OnDestroy()
        {
            if (destructible != null)
            {
                RemoveCurrentPin();
            }
        }
    }
}