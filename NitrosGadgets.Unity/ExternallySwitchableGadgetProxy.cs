using custom_item_components;
using System;
using UnityEngine;

namespace NitrosGadgets.Unity
{
    [Serializable]
    public class ExternallySwitchableGadgetProxy : GadgetBase
    {
        public enum TrainCarPresence : byte
        {
            [Tooltip("Can be only placed on a TrainCar interior")]
            RequireInterior,
            [Tooltip("Can be only placed on a TrainCar")]
            RequireTrainCar,
            [Tooltip("No additional restrictions")]
            None
        }
        public TrainCarPresence trainCarPresence;
        public bool baseControls;
        public bool electricsFuse;
        public bool muSupport;
        public bool cabin;

        public bool requireSoldering;
    }
}
