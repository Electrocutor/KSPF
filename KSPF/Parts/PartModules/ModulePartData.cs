using System;
using System.Collections.Generic;

namespace KSPF.Parts.PartModules
{
    /// <summary>
    /// Allows persisting of part-level properties per instance of part that are usually only at the AvailablePart definition level
    /// </summary>
    public class ModulePartData : PartModule
    {
        [KSPField(isPersistant = true)]
        public int CrewCapacity = -1;

        [KSPField(isPersistant = true)]
        public string InternalModel = null;

        [KSPField(isPersistant = true)]
        public float RescaleFactor = 0.0f;

        [KSPField(isPersistant = true)]
        public float Scale = 0.0f;

        [KSPField(isPersistant = true)]
        public float BreakingForce = 0.0f;

        [KSPField(isPersistant = true)]
        public float BreakingTorque = 0.0f;

        [KSPField(isPersistant = true)]
        public float CrashTolerance = 0.0f;

        [KSPField(isPersistant = true)]
        public float GTolerance = 0.0f;

        [KSPField(isPersistant = true)]
        public float MaxTemp = 0.0f;

        [KSPField(isPersistant = true)]
        public float SkinMaxTemp = 0.0f;

        public override void OnAwake()
        {
            // rescaleFactor and scale must be set here, but KSP does not set PartModule fields until onLoad
        }

        public override void OnLoad(ConfigNode node)
        {
            this.part.partInfo = new AvailablePart(this.part.partInfo);
            
            // add/remove attach nodes
            SetProperties();
            SetInternalModel();
            // set scale and rescale
        }

        private void SetProperties()
        {
            if (this.CrewCapacity == -1)
            { return; }
            else
            { this.part.CrewCapacity = this.CrewCapacity; }

            if (this.BreakingForce == 0.0f)
            { return; }
            else
            { this.part.breakingForce = this.BreakingForce; }

            if (this.BreakingTorque == 0.0f)
            { return; }
            else
            { this.part.breakingTorque = this.BreakingTorque; }

            if (this.CrashTolerance == 0.0f)
            { return; }
            else
            { this.part.crashTolerance = this.CrashTolerance; }

            if (this.GTolerance == 0.0f)
            { return; }
            else
            { this.part.gTolerance = this.GTolerance; }

            if (this.MaxTemp == 0.0f)
            { return; }
            else
            { this.part.maxTemp = this.MaxTemp; }

            if (this.SkinMaxTemp == 0.0f)
            { return; }
            else
            { this.part.skinMaxTemp = this.SkinMaxTemp; }

            //this.part.minDepth;
            //this.part.maxDepth;

            //this.part.maxPressure;

            //this.part.dragModel;
            //this.part.minimum_drag;
            //this.part.maximum_drag;
            //this.part.angularDrag;

            //this.part.fuelCrossFeed; //other resources?
            //this.part.vesselType;

            //this.part.explosionPotential;
            //this.part.gExplodeChance;
            //this.part.presExplodeChance;
            //this.part.tempExplodeChance;

            //this.part.emissiveConstant;
            //this.part.CoLOffset;
            //this.part.CoMOffset;
            //this.part.CoPOffset;

            //this.part.heatConductivity;
        }

        private void SetInternalModel()
        {
            if (string.IsNullOrEmpty(this.InternalModel))
            { return; }

            ConfigNode oConfig = new ConfigNode("INTERNAL");
            oConfig.AddValue("name", this.InternalModel);

            this.part.partInfo.internalConfig = oConfig;
        }

        private void SetRescaleFactor()
        {
            if (this.RescaleFactor == 0.0f)
            { return; }

            this.part.rescaleFactor = this.RescaleFactor;
        }

        private void SetScale()
        {
            if (this.Scale == 0.0f)
            { return; }

            this.part.scaleFactor = this.Scale;
        }
    }
}
