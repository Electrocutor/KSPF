using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPF.Parts.PartModules
{
    /// <summary>
    /// Allows persisting of part-level properties per instance of part that are usually only at the AvailablePart definition level
    /// </summary>
    public class ModulePartVariantsEx : PartModule
    {
        [KSPField(isPersistant = true)]
        public string internalModel = null;

        [KSPField(isPersistant = true)]
        public float rescaleFactor = 0.0f;

        [KSPField(isPersistant = true)]
        public float scale = 0.0f;

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
            if (string.IsNullOrEmpty(this.internalModel))
            { return; }

            ConfigNode oConfig = new ConfigNode("INTERNAL");
            oConfig.AddValue("name", this.internalModel);

            this.part.partInfo.internalConfig = oConfig;
        }

        private void SetRescaleFactor()
        {
            if (this.rescaleFactor == 0.0f)
            { return; }

            this.part.rescaleFactor = this.rescaleFactor;
        }

        private void SetScale()
        {
            if (this.scale == 0.0f)
            { return; }

            this.part.scaleFactor = this.scale;
        }
    }
}
