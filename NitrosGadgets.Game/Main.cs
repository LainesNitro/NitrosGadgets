using custom_item_mod;
using HarmonyLib;
using NitrosGadgets.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityModManagerNet;
using Object = UnityEngine.Object;

namespace NitrosGadgets.Game
{
    public static class Main
    {
        public static UnityModManager.ModEntry Instance { get; private set; } = null!;

        public static Dictionary<string, float> SaveData = new Dictionary<string, float>();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Instance = modEntry;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            CustomGadgetBaseMap.RegisterGadgetImplementation(typeof(ExternallySwitchableGadgetProxy), typeof(GadgetWheelslipIndicator), CustomizeWheelslipIndicator);
            CustomGadgetBaseMap.RegisterGadgetImplementation(typeof(custom_item_components.GadgetBase), typeof(GadgetAnalogOdometer), CustomizeAnalogOdometer);

            return true;
        }

        public static void CustomizeWheelslipIndicatorItem(GameObject target)
        {
            ProcessTextMeshProComponent(target);
        }

        public static void CustomizeWheelslipIndicator(custom_item_components.GadgetBase sourceGadget, ref DV.Customization.Gadgets.GadgetBase targetGadget)
        {
            var target = targetGadget as GadgetWheelslipIndicator;
            
            if (target == null)
                return;

            var source = sourceGadget as ExternallySwitchableGadgetProxy;

            if (source == null)
                return;

            target.requirements.trainCarPresence = (DV.Customization.TrainCarCustomization.TrainCarCustomizerBase.CustomizerTrainCarRequirements)source.trainCarPresence;
            target.requirements.baseControls = source.baseControls;
            target.requirements.electricsFuse = source.electricsFuse;
            target.requirements.muSupport = source.muSupport;
            target.requirements.cabin = source.cabin;
            target.requireSoldering = source.requireSoldering;

            ProcessTextMeshProComponent(target.gameObject);

            var lampControlProxyList = target.gameObject.GetComponentsInChildren<LampControlProxy>();

            var powerLEDProxy = lampControlProxyList.FirstOrDefault(c => c.gameObject.name == "L_On");
            var powerLED = powerLEDProxy.gameObject.AddComponent<LampControl>();
            
            var indicatorEmissionProxy = powerLED.gameObject.GetComponentInChildren<IndicatorEmissionProxy>();
            var indicatorEmission = indicatorEmissionProxy.gameObject.AddComponent<IndicatorEmission>();
            
            powerLED.lampInd = indicatorEmission;
            target.powerLED = powerLED;

            Object.Destroy(powerLEDProxy);
            Object.Destroy(indicatorEmissionProxy);

            var activeLEDProxy = lampControlProxyList.FirstOrDefault(c => c.gameObject.name == "L_Active");
            var activeLED = activeLEDProxy.gameObject.AddComponent<LampControl>();
            
            indicatorEmissionProxy = activeLED.gameObject.GetComponentInChildren<IndicatorEmissionProxy>();
            indicatorEmission = indicatorEmissionProxy.gameObject.AddComponent<IndicatorEmission>();

            activeLED.lampInd = indicatorEmission;
            activeLED.onStateBuzzingLoopAudio = activeLEDProxy.onStateBuzzingLoopAudio;
            activeLED.lampAudioMixerGroup = NAudio.Default3DMixerGroup.audioMixer.FindMatchingGroups("Cab")[0];
            target.activeLED  = activeLED;

            Object.Destroy(activeLEDProxy);
            Object.Destroy(indicatorEmissionProxy);
        }

        public static void CustomizeAnalogOdometer(custom_item_components.GadgetBase sourceGadget, ref DV.Customization.Gadgets.GadgetBase targetGadget)
        {
            var target = targetGadget as GadgetAnalogOdometer;

            if (target == null)
                return;

            target.requirements.trainCarPresence = DV.Customization.TrainCarCustomization.TrainCarCustomizerBase.CustomizerTrainCarRequirements.RequireTrainCar;
        }

        private static void ProcessTextMeshProComponent(GameObject parent)
        {
            var tmpProxy = parent.GetComponentInChildren<TextMeshPro>();
            var tmp = Object.Instantiate(Resources.Load<GameObject>("Label").GetComponentInChildren<TextMeshPro>(true), tmpProxy.transform.position, tmpProxy.transform.rotation);

            tmp.transform.SetParent(tmpProxy.transform.parent);
            tmp.text = tmpProxy.text;

            Object.Destroy(tmpProxy.gameObject);
        }


        public static void Log(string message)
        {
            Instance.Logger.Log(message);
        }

        public static void Warning(string message)
        {
            Instance.Logger.Warning(message);
        }

        public static void Error(string message)
        {
            Instance.Logger.Error(message);
        }
    }
}

