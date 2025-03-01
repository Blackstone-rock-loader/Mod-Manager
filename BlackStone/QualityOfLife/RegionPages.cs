using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// FROM TOWN OF HOST ENHANCED: https://github.com/EnhancedNetwork/TownofHost-Enhanced
namespace BlackStone
{
    [HarmonyPatch(typeof(RegionMenu))]
    public static class RegionMenuPatch
    {
        [HarmonyPatch(nameof(RegionMenu.OnEnable))]
        [HarmonyPostfix]
        public static void AdjustButtonPositions_Postfix(RegionMenu __instance)
        {
            const int maxColumns = 4;
            int buttonsPerColumn = 8;
            float buttonSpacing = 0.6f;
            float buttonSpacingSide = 2.25f;

            List<UiElement> buttons = __instance.controllerSelectable.ToArray().ToList();

            int columnCount = (buttons.Count + buttonsPerColumn - 1) / buttonsPerColumn;

            while (columnCount > maxColumns)
            {
                buttonsPerColumn++;
                columnCount = (buttons.Count + buttonsPerColumn - 1) / buttonsPerColumn;
            }

            float totalWidth = (columnCount - 1) * buttonSpacingSide;
            float totalHeight = (buttonsPerColumn - 1) * buttonSpacing;

            Vector3 startPosition = new Vector3(-totalWidth / 2, totalHeight / 2, 0f);

            for (int i = 0; i < buttons.Count; i++)
            {
                int col = i / buttonsPerColumn;
                int row = i % buttonsPerColumn;
                buttons[i].transform.localPosition = startPosition + new Vector3(col * buttonSpacingSide, -row * buttonSpacing, 0f);
            }
        }
    }
}