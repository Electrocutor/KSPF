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
        public string InternalModel = null;

        [KSPField(isPersistant = true)]
        public float RescaleFactor = 0.0f;

        [KSPField(isPersistant = true)]
        public float Scale = 0.0f;

        public override void OnAwake()
        {
            // rescaleFactor and scale must be set here, but KSP does not set PartModule fields until onLoad
        }

        public override void OnLoad(ConfigNode node)
        {
            this.part.partInfo = new AvailablePart(this.part.partInfo);

            SetInternalModel();
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
