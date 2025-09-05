using custom_item_mod;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NitrosGadgets.Game
{
    [HarmonyPatch(typeof(CustomItem))]
    public static class PatchCustomItem
    {
        [HarmonyPatch(MethodType.Constructor,
            new Type[] {
        typeof(CustomItemInfo),
        typeof(GameObject),
        typeof(Sprite),
        typeof(Sprite),
        typeof(GameObject),
        typeof(List<Shop>),
        typeof(bool),
        typeof(bool),
        typeof(bool)})]
        [HarmonyPostfix]
        public static void PatchCustomItemConstructor(CustomItem __instance)
        {
            if (__instance.ItemPrefab.name == "WheelslipIndicator")
            {
                Main.CustomizeWheelslipIndicatorItem(__instance.ItemPrefab);
            }
        }
    }
}
