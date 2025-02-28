using HarmonyLib;
using Rewired.Utils.Platforms.Windows;
using System;

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using static UnityEngine.UI.Button;
// FROM TOWN OF HOST ENHANCED: https://github.com/EnhancedNetwork/TownofHost-Enhanced
[HarmonyPatch(typeof(MainMenuManager))]
public static class MainMenuManagerPatch
{
    private static PassiveButton template;
    private static PassiveButton ModsButton;

    [HarmonyPatch(nameof(MainMenuManager.Start)), HarmonyPostfix, HarmonyPriority(Priority.Normal)]
    public static void Start_Postfix(MainMenuManager __instance)
    {
        if (template == null) template = __instance.howToPlayButton;



        if (template == null) return;
       

        // donation Button
        if (ModsButton == null)
        {
            Action OpenMenuLayer = () =>
                                Blackstone.Mods();
            ModsButton = CreateButton(
                __instance.howToPlayButton.transform.parent,
                "ModManger",
                new(-2f, -5f, 1f),
                new(255, 255, 255, byte.MaxValue),
                new(255, 255, 255, byte.MaxValue),
                (UnityEngine.Events.UnityAction)OpenMenuLayer
                ,
                "Mods");
            // right now until i make a like ui scale / ui list object
            // this just fixes the positions
            ModsButton.transform.position = new(template.transform.position.x - 0.75f, template.transform.position.y, template.transform.position.x);
            PassiveButton howToPlayButton = __instance.howToPlayButton;
            PassiveButton freePlayButton = __instance.freePlayButton;
            howToPlayButton.transform.position = new(ModsButton.transform.position.x + 1.65f, ModsButton.transform.position.y, ModsButton.transform.position.x);
            freePlayButton.transform.position = new(howToPlayButton.transform.position.x + 1.5f, howToPlayButton.transform.position.y, howToPlayButton.transform.position.x);
            ModsButton.transform.localScale = new Vector3(0.9f,0.9f, 1);
            freePlayButton.transform.localScale = new Vector3(0.9f, 0.9f, 1);
            howToPlayButton.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
        ModsButton.gameObject.SetActive(true);
    }

    public static PassiveButton CreateButton(Transform startpos, string name, Vector3 localPosition, Color32 normalColor, Color32 hoverColor, UnityEngine.Events.UnityAction action, string label, Vector2? scale = null)
    {
        var button = Object.Instantiate(template, startpos);
        button.name = name;
        Object.Destroy(button.GetComponent<AspectPosition>());
        button.transform.localPosition = localPosition;

        button.OnClick = new();
        button.OnClick.AddListener(action);
        Transform ModeTextObject = button.transform.Find("ModeText");
        TextTranslatorTMP ModeText = ModeTextObject.GetComponent<TextTranslatorTMP>();

        if (ModeText)
        {
            ModeText.defaultStr = label;
            ModeText.TargetText = StringNames.None;
            ModeTextObject.GetComponent<TextMeshPro>().SetText(label);
            ModeText.ResetText();
        }
        var normalSprite = button.inactiveSprites.GetComponent<SpriteRenderer>();
        var hoverSprite = button.activeSprites.GetComponent<SpriteRenderer>();
        normalSprite.color = normalColor;
        hoverSprite.color = hoverColor;

        /*
        Town of Host E Center Text 
        var container = buttonText.transform.parent;
        Object.Destroy(container.GetComponent<AspectPosition>());
        Object.Destroy(buttonText.GetComponent<AspectPosition>());
        container.SetLocalX(0f);
        buttonText.transform.SetLocalX(0f);
        buttonText.horizontalAlignment = HorizontalAlignmentOptions.Center;
        */

        var buttonCollider = button.GetComponent<BoxCollider2D>();
        if (scale.HasValue)
        {
            normalSprite.size = hoverSprite.size = buttonCollider.size = scale.Value;
        }

        buttonCollider.offset = new(0f, 0f);

        return button;
    }
    public static void Modify(this PassiveButton passiveButton, UnityEngine.Events.UnityAction action)
    {
        if (passiveButton == null) return;
        passiveButton.OnClick = new ButtonClickedEvent();
        passiveButton.OnClick.AddListener(action);
    }
    public static T FindChild<T>(this MonoBehaviour obj, string name) where T : Object
    {
        string name2 = name;
        return obj.GetComponentsInChildren<T>().First((T c) => c.name == name2);
    }
    public static T FindChild<T>(this GameObject obj, string name) where T : Object
    {
        string name2 = name;
        return obj.GetComponentsInChildren<T>().First((T c) => c.name == name2);
    }
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        //if (source == null) throw new ArgumentNullException("source");
        if (source == null) throw new ArgumentNullException(nameof(source));

        IEnumerator<TSource> enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action(enumerator.Current);
        }

        enumerator.Dispose();
    }
}