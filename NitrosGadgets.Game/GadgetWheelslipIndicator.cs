using System;
using DV.Customization.Gadgets.Implementations;

namespace NitrosGadgets.Game
{
    public class GadgetWheelslipIndicator : ExternallySwitchableGadget
    {
        public LampControl powerLED;
        public LampControl activeLED;

        private bool isActivated;

        public bool IsActivated
        {
            get
            {
                return isActivated;
            }
            private set
            {
                if (isActivated != value)
                {
                    isActivated = value;
                    this.IsActivatedChanged?.Invoke();
                }
            }
        }

        public event Action? IsActivatedChanged;

        public void Start()
        {
            IsActivatedChanged += UpdateLamps;
        }

        public override void OnPowerStateChanged(bool _ = false)
        {
            UpdateLamps();
        }

        public void Update()
        {
            if (!base.PowerState || !base.IsOnTrainCar)
            {
                IsActivated = false;
            }
            else
            {
                if (!base.TrainCar.adhesionController.wheelslipController.IsSome(out var value))
                {
                    return;
                }
                IsActivated = value.IsWheelslipping;
            }
        }

        public void UpdateLamps()
        {
            powerLED.SetLampState(base.PowerState ? LampControl.LampState.On : LampControl.LampState.Off);
            activeLED.SetLampState(IsActivated ? LampControl.LampState.On : LampControl.LampState.Off);
        }
    }
}
