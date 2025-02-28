using BlackStone.Mod_List;
using Il2CppSystem.Reflection;

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class Blackstone
{
    private static Action<Scene, LoadSceneMode> sceneLoadedListener;
    public static bool mods = false;
    public static void loadSceneHook()
    {
        // look, ok i know i could do this better, i needed a emptyish scene.
        sceneLoadedListener = (scene, loadMode) =>
        {
            if (scene.name == "HowToPlay" && mods)
            {
                Blackstone.mods = false;
                var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

                foreach (var obj in allObjects)
                {
                    if (obj && obj.gameObject && obj.name != "HowToPlayScene" && obj.scene == scene && !GetParentCheck(obj, "HowToPlayScene"))
                    {
                        obj.gameObject.active = false;
                    }
                }
                UiElements.LoadModManager(scene);
            }
        };
        SceneManager.add_sceneLoaded(sceneLoadedListener);
    }

    public static void Mods()
    {
        mods = true;
        SceneManager.LoadScene("HowToPlay");
    }

    private static bool GetParentCheck(GameObject obj, string parentName)
    {
        Transform parentTransform = obj.transform.parent;

        while (parentTransform != null)
        {
            if (parentTransform.name == parentName)
            {
                return true;
            }

            parentTransform = parentTransform.parent; 
        }

        return false;
    }




}