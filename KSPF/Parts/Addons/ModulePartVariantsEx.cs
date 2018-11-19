using System;
using System.Collections.Generic;
using UnityEngine;

namespace KSPF.Parts.Addons
{
    /// <summary>
    /// Addon used to hook the existing ModulePartVariants and extend it
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class ModulePartVariantsEx : MonoBehaviour
    {
        private bool _Debug = false;
        private EventData<Part, PartVariant>.OnEvent VariantAppliedEvent;

        public void Start()
        {
            this.VariantAppliedEvent = new EventData<Part, PartVariant>.OnEvent(VariantAppliedEventHandler);
            GameEvents.onVariantApplied.Add(this.VariantAppliedEvent);
        }

        private void VariantAppliedEventHandler(Part oPart, PartVariant oVariant)
        {
            string sDebug = "";
            string sValue;
            float fValue;
            double dValue;
            double dValue2;

            if (oPart == null || oVariant == null)
            { return; }

            #region "Resources"

            PartResource oResource;
            ConfigNode oConfigNode;

            foreach (PartResourceDefinition oResourceDef in PartResourceLibrary.Instance.resourceDefinitions)
            {
                sValue = oVariant.GetExtraInfoValue("Resource/" + oResourceDef.name);

                if (!string.IsNullOrEmpty(sValue))
                {
                    oResource = oPart.Resources[oResourceDef.name];

                    if (oResource != null)
                    {
                        if (sValue.ToLower() == "none")
                        { oPart.RemoveResource(oResource); }
                        else if (double.TryParse(sValue, out dValue))
                        {
                            dValue2 = oResource.amount / oResource.maxAmount;
                            oResource.maxAmount = dValue;

                            if (HighLogic.LoadedSceneIsEditor)
                            { oResource.amount = dValue * dValue2; }
                        }
                    }
                    else if (double.TryParse(sValue, out dValue))
                    {
                        oConfigNode = new ConfigNode("RESOURCE");
                        oConfigNode.AddValue("name", oResourceDef.name);

                        if (HighLogic.LoadedSceneIsEditor)
                        { oConfigNode.AddValue("amount", dValue); }
                        else
                        { oConfigNode.AddValue("amount", 0.0); }

                        oConfigNode.AddValue("maxAmount", dValue);

                        oPart.AddResource(oConfigNode);
                    }
                }
            }

            #endregion "Resources"

            #region "Module Properties"

            sValue = oVariant.GetExtraInfoValue("AddModule1");
            for (int i = 2; !string.IsNullOrEmpty(sValue); i++)
            {
                oPart.AddModule(sValue);
                sValue = oVariant.GetExtraInfoValue("AddModule" + i.ToString());
            }

            List<string> oRemoveModuleList = new List<string>();
            sValue = oVariant.GetExtraInfoValue("RemoveModule1");
            for (int i = 2; !string.IsNullOrEmpty(sValue); i++)
            {
                oRemoveModuleList.Add(sValue);
                sValue = oVariant.GetExtraInfoValue("RemoveModule" + i.ToString());
            }

            foreach (PartModule oModule in oPart.Modules)
            {
                if (_Debug)
                { sDebug = "Module Fields [" + oModule.moduleName + "]:\r\n"; }

                foreach (BaseField oField in oModule.Fields)
                {
                    if (_Debug)
                    { sDebug += oField.name + "\r\n"; }

                    sValue = oVariant.GetExtraInfoValue(oModule.moduleName + "/" + oField.name);

                    if (!string.IsNullOrEmpty(sValue))
                    {
                        oField.SetValue(Convert.ChangeType(sValue, oField.FieldInfo.FieldType), oField.host);

                        if (_Debug)
                        { sDebug += "  Appled: " + oField.name + " = " + sValue + "\r\n"; }
                    }
                }

                if (_Debug)
                { Debug.Log(sDebug); }
            }

            #endregion"Module Properties"

            #region "Update UI"

            if (HighLogic.LoadedSceneIsEditor)
            {
                GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, oPart);
                GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
            }
            else if (HighLogic.LoadedSceneIsFlight)
            {
                GameEvents.onVesselWasModified.Fire(oPart.vessel);
            }

            UIPartActionWindow[] oWindows = Resources.FindObjectsOfTypeAll<UIPartActionWindow>();
            foreach (UIPartActionWindow oWindow in oWindows)
            {
                if (oWindow.part == oPart)
                { oWindow.displayDirty = true; }
            }

            #endregion "Update UI"
        }
    }
}
