using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

public class LuaPowerTag
{
    public static void MakeTag (string name, bool isCharacter = false)
    {
        if (LuaPowerData.customEnums[typeof(Tag)].Contains(name))
        {
            Debug.Log("ERROR: A Tag named " + name + " already exists.");
            return;
        }
        Tag tag = (Tag)(LuaPowerData.customEnums[typeof(Tag)].Count);
        LuaPowerData.customEnums[typeof(Tag)].Add(name);
        if (isCharacter)
            LuaPowerData.characterTagEnums.Add(tag);
    }
}

//Fix for making sure Character Name style tags function properly.
[HarmonyPatch(typeof(ItemManager), "LoadItemData")]
public static class MoreLuaPower_CustomTags
{
    public static void Postfix(ItemManager __instance)
    {
        foreach (Tag tag in LuaPowerData.characterTagEnums)
        {
            if (!S.I.runCtrl.ctrl.currentHeroObj.tags.Contains(tag))
            {
                /*
                Debug.Log("Tag " + tag + " not on currentHeroObj");
                foreach (ArtifactObject obj in __instance.nonBaseArtList.Where<ArtifactObject>((Func<ArtifactObject, bool>)(t => t.tags.Contains(tag))).ToList<ArtifactObject>()) {
                    Debug.Log(obj.nameString);
                }*/
                __instance.nonBaseArtList = __instance.nonBaseArtList.Where<ArtifactObject>((Func<ArtifactObject, bool>)(t => !t.tags.Contains(tag))).ToList<ArtifactObject>();
            }
        }
    }
}