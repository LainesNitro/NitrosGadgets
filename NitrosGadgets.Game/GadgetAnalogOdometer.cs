using custom_item_mod;
using DV.Customization;
using DV.Customization.Gadgets;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NitrosGadgets.Game
{
    public class GadgetAnalogOdometer : GadgetBase
    {
        private VirtualOdometer? virtualOdometer;
        private Transform[]? rotaryTransforms;
        private float[]? rotaryValues;

        public void Start()
        {
            rotaryTransforms = this.transform.GetChild(0).GetChild(0).GetChildren().Skip(1).ToArray();

            if (rotaryTransforms == null)
            {
                Main.Error("Rotaries are missing!");
                return;
            }

            virtualOdometer = base.TrainCar.GetComponent<VirtualOdometer>();

            if (virtualOdometer == null)
            {
                Main.Error("VirtualOdometer is missing!");
                return;
            }
        }
        public void OnEnable()
        {
            if (base.TrainCar == null) return;

            virtualOdometer = base.TrainCar.GetComponent<VirtualOdometer>();

            if (virtualOdometer == null)
            {
                Main.Error("VirtualOdometer is missing!");
                return;
            }
        }

        public void Update()
        {
            if (virtualOdometer == null) return;

            if (rotaryTransforms == null) return;
            
            rotaryValues = new float[6];

            // calculate rotation of 1/10th rotary first, since it has to be smooth
            rotaryValues[5] = virtualOdometer.DistanceTraveledKM * 10f % 10f;

            // calculate rotation for the rest of rotaries
            for (int i = 4; i >= 0; i--)
            {
                rotaryValues[i] = Mathf.Floor(virtualOdometer.DistanceTraveledKM * Mathf.Pow(10f, i - 4) % 10f);

                // only add a fraction for smooth rotation when about to change digit
                if (rotaryValues[i + 1] >= 9f)
                {
                    rotaryValues[i] += rotaryValues[i + 1] % 1f;
                }
            }

            for (int i = 0; i < 6; i++)
            {
                rotaryTransforms[i].localRotation = Quaternion.Euler(0f, 0f, rotaryValues[i] * 36f);
            }
        }
    }
}
