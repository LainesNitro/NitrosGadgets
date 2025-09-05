using HarmonyLib;
using System.Collections;

namespace NitrosGadgets.Game
{
    [HarmonyPatch(typeof(CarSpawner))]
    internal static class CarSpawnerPatches
    {
        [HarmonyPatch("Awake"), HarmonyPostfix]
        private static void AwakePostfix(CarSpawner __instance)
        {
            __instance.CarSpawned += SpawnSequence;
            __instance.CarAboutToBeDeleted += DespawnSequence;
        }
        private static void SpawnSequence(TrainCar car)
        {
            if (car.TryGetComponent<VirtualOdometer>(out _))
            {
                return;
            }

            car.StartCoroutine(WaitForGUIDCoroutine(car));
        }

        private static IEnumerator WaitForGUIDCoroutine(TrainCar car)
        {
            while (car.logicCar == null)
            {
                yield return null;
            }

            car.gameObject.AddComponent<VirtualOdometer>();
        }

        private static void DespawnSequence(TrainCar car)
        {
            if (car.TryGetComponent<VirtualOdometer>(out var comp))
            {
                UnityEngine.Object.Destroy(comp);
            }
        }
    }

}
