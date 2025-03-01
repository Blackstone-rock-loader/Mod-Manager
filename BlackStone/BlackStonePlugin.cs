using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Networking;
using Reactor.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace BlackStone;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class BlackStonePlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public ConfigEntry<string> ConfigName { get; private set; }

    public override void Load()
    {
        //  ConfigName = Config.Bind("Fake", "Name", ":>");
        BlackStone.AnnouncementPatch.GetAnnouncements();
        ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]); // remove non modded lol
        Harmony.PatchAll();
        Blackstone.loadSceneHook();
    }

    /*[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class ExamplePatch
    {
        public static void Postfix(PlayerControl __instance)
        {
            __instance.cosmetics.nameText.text = PluginSingleton<BlackStonePlugin>.Instance.ConfigName.Value;
        }
    }*/

}
