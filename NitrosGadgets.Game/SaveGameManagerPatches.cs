using DV.JObjectExtstensions;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace NitrosGadgets.Game
{
    [HarmonyPatch(typeof(SaveGameManager))]
    internal class SaveGameManagerPatches
    {
        [HarmonyPatch("DoSaveIO"), HarmonyPrefix]
        public static void InjectSaveData(SaveGameData data)
        {
            var loadedData = data.GetJObject("virtual_odometer");
            loadedData ??= new JObject();

            Main.SaveData.Clear();
            Main.SaveData = CarSpawner.Instance.AllCars.Where(x => x.logicCar != null && x.TryGetComponent<VirtualOdometer>(out _)).ToDictionary(
                x => x.logicCar.carGuid,
                x => (x.GetComponent<VirtualOdometer>().DistanceTraveledKM));

            loadedData.SetObjectViaJSON("data", new DataHolder(Main.SaveData));
            data.SetJObject("virtual_odometer", loadedData);
        }


        [HarmonyPatch(nameof(SaveGameManager.FindStartGameData)), HarmonyPostfix]
        public static void ExtractSaveData(SaveGameManager __instance)
        {
            if (__instance.data == null) return;

            var data = __instance.data.GetJObject("virtual_odometer");

            if (data == null) return;

            var holder = data.GetObjectViaJSON<DataHolder>("data");

            if (holder == null) return;

            Main.SaveData = holder.ToData();
        }

        private class DataHolder
        {
            public string[] Keys;
            public float[] Values;

            public DataHolder()
            {
                Keys = new string[0];
                Values = new float[0];
            }

            public DataHolder(Dictionary<string, float> data)
            {
                Keys = data.Keys.ToArray();
                Values = data.Values.ToArray();
            }

            public Dictionary<string, float> ToData()
            {
                Dictionary<string, float> data = new Dictionary<string, float>();
                int length = Keys.Length;

                for (int i = 0; i < length; i++)
                {
                    data.Add(Keys[i], Values[i]);
                }

                return data;
            }
        }
    }
}
