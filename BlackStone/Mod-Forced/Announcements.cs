using AmongUs.Data.Player;
using Assets.InnerNet;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Reactor.Networking;
using System.Linq;
using System.Text.Json;
using UnityEngine.ProBuilder;

namespace BlackStone
{
    [HarmonyPatch(typeof(PlayerAnnouncementData))]
    public static class AnnouncementPatch
    {
        public class AnnouncementJson
        {
            public string Title { get; set; }
            public string Id { get; set; }  
            public uint Language { get; set; }
            public string SubTitle { get; set; }
            public int Number { get; set; }
            public bool PinState { get; set; }
            public string Date { get; set; }  
            public string ShortTitle { get; set; }
            public string Text { get; set; }
        }

        static System.Collections.Generic.Dictionary<string, AnnouncementJson> Announcements;
        public static void GetAnnouncements()
        {
            try
            {
                string json = BlackStone.RequestHelper.sendrequest("https://github.com/Blackstone-rock-loader/Mod-Manager/blob/main/Anouncements.json?raw=please");
                Log.PrintToConsole(json);
                Announcements = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, AnnouncementJson>>(json);
            } catch (System.Exception e)
            {
                Log.PrintToConsole ($"Error fetching announcements: {e.Message}");
            }
        }
        [HarmonyPatch(nameof(PlayerAnnouncementData.AddAnnouncement))]
        public static bool Prefix(PlayerAnnouncementData __instance, Assets.InnerNet.Announcement a)
        {
            return true;
        }

        [HarmonyPatch(nameof(PlayerAnnouncementData.SetAnnouncements))]
        public static bool Prefix(PlayerAnnouncementData __instance, Announcement[] aRange)
        {
                __instance.allAnnouncements.Clear();
            try
            {
                if (Announcements.Count < 0)
                {
                    GetAnnouncements();
                    if (Announcements.Count < 0)
                    {
                        return false;
                    }
                }
            } catch
            {
                return false;
            }
                foreach (var json in Announcements)
                {
                    try
                    {
                        __instance.AddAnnouncement(
                            new Announcement
                            {
                                Id = json.Value.Id,
                                Title = json.Value.Title,
                                Language = json.Value.Language,
                                SubTitle = json.Value.SubTitle,
                                Number = json.Value.Number,
                                PinState = json.Value.PinState,
                                Date = json.Value.Date,
                                ShortTitle = json.Value.ShortTitle,
                                Text = json.Value.Text
                            });
                    } finally { };
                }
               

            return false;
        }
    }
}