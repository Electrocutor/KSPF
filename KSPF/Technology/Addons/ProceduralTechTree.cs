using System;
using System.Collections.Generic;
using UnityEngine;

namespace KSPF.Technology.Addons
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class ProceduralTechTree : MonoBehaviour
    {
        public static bool IsDebug = true;

        public void Start()
        {
            DontDestroyOnLoad(this);

            //this.VariantAppliedEvent = new EventData<Part, PartVariant>.OnEvent(VariantAppliedEventHandler);
            //GameEvents.onVariantApplied.Add(this.VariantAppliedEvent);
        }
    }
}
