using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

// i'm leaving this up to @dankyblox, i am really bad at unity ui lol!

namespace BlackStone.Mod_List
{
    internal class UiElements
    {
        private static GameObject modListPanel;
        private static Transform modListContainer;

        public static void LoadModManager(Scene scene)
        {
            // todo: make it look better
            GameObject canvas = new GameObject("ModListCanvas");
            Canvas canvasComp = canvas.AddComponent<Canvas>();
            //canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();

            modListPanel = new GameObject("ModListPanel");
            modListPanel.transform.SetParent(canvas.transform);
            RectTransform rect = modListPanel.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(600, 800);
            Image panelImage = modListPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f); 
    
            GameObject scrollView = new GameObject("ScrollView");
            scrollView.transform.SetParent(modListPanel.transform);
            RectTransform scrollRectTrans = scrollView.AddComponent<RectTransform>();
            scrollRectTrans.sizeDelta = new Vector2(580, 780);
            ScrollRect scrollRect = scrollView.AddComponent<ScrollRect>();
            modListContainer = new GameObject("ModListContainer").transform;
            modListContainer.SetParent(scrollView.transform);
            scrollRect.content = modListContainer.GetComponent<RectTransform>();
            FetchModList();
        }

        
        public static void FetchModList()
        {
            string json = BlackStone.RequestHelper.sendrequest("https://github.com/Blackstone-rock-loader/index/blob/main/Index/list.json?raw=please");
            Dictionary<string, ModInfo> modList = null;
            try
            {
                modList = JsonSerializer.Deserialize<Dictionary<string, ModInfo>>(json);
            } catch (Exception ex)
            {
                Debug.Log($"Error fetching or deserializing JSON: {ex.Message}");
            }
            Debug.Log(json + modList);
            PopulateModList(modList);
        }



        private static void PopulateModList(Dictionary<string, ModInfo> modList)
        {
            foreach (Transform child in modListContainer)
            {
                GameObject.Destroy(child.gameObject);
            }
            // todo fix
            foreach (var modEntry in modList)
            {
               /* GameObject Holder = new GameObject(modEntry.Key.ToString());
                GameObject modItem = new GameObject("Text_"+modEntry.Key.ToString());

                Holder.transform.SetParent(modListContainer, false);
                modItem.transform.SetParent(Holder.transform, false);
                RectTransform rectTransform = Holder.AddComponent<RectTransform>();

                rectTransform.sizeDelta = new Vector2(400, 100); 

                Image panelImage = Holder.AddComponent<Image>();
                panelImage.color = new Color(0, 0, 0, 0.8f); 

                Text modText = modItem.AddComponent<Text>();
                modText.text = $"{modEntry.Value.name}\n{modEntry.Value.description}";
                modText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                modText.fontSize = 25;
                modText.color = Color.white;
                modText.alignment = TextAnchor.MiddleCenter; 

                modText.horizontalOverflow = HorizontalWrapMode.Wrap;
                modText.verticalOverflow = VerticalWrapMode.Truncate;

                LayoutElement layoutElement = Holder.AddComponent<LayoutElement>();
                layoutElement.preferredWidth = 400;
                layoutElement.preferredHeight = 100;

                VerticalLayoutGroup layoutGroup = Holder.AddComponent<VerticalLayoutGroup>();
                layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                layoutGroup.padding.left = 10;
                layoutGroup.padding.right = 10;
                layoutGroup.padding.top = 10;
                layoutGroup.padding.bottom = 10;

                rectTransform.ForceUpdateRectTransforms();*/

            }
        }

        private static IEnumerator LoadModIcon(string url, GameObject modItem)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Failed to load mod icon: " + request.error);
                yield break;
            }

            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            GameObject iconObject = new GameObject("ModIcon");
            iconObject.transform.SetParent(modItem.transform);

            Image iconImage = iconObject.AddComponent<Image>();
            iconImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
    public class ModInfo
    {
        public ModType type { get; set; }
        public string downloadlink { get; set; }
        public string hash { get; set; }
        public string icon { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    public class ModType
    {
        public bool Host { get; set; }
        public bool Roles { get; set; }
        public bool Cosmetics { get; set; }
    }
}
