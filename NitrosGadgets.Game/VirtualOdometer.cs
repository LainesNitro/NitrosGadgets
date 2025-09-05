using UnityEngine;

namespace NitrosGadgets.Game
{
    public class VirtualOdometer : MonoBehaviour
    {
        private TrainCar? trainCar;
        
        private float absSpeedMPS = 0;

        public float DistanceTraveledKM { get; private set; } = 0;
        
        public void Start()
        {
            trainCar = this.GetComponent<TrainCar>();
            
            if (trainCar == null)
            {
                Main.Error("Cannot find TrainCar instace!");
                Destroy(this);
            }

            // Get last saved traveled distance if exists
            if (Main.SaveData.TryGetValue(trainCar!.logicCar.carGuid, out var value))
            {
                DistanceTraveledKM = value;
            }
        }

        public void Update()
        {
            absSpeedMPS = trainCar!.GetAbsSpeed();

            if (absSpeedMPS > 0.1f)
            {
                DistanceTraveledKM += absSpeedMPS * Time.deltaTime / 1000f;
            }
        }
    }
}
