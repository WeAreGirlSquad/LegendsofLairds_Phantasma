using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LegendaryQuestActor))]

public class LegendaryQuestActorEditor : Editor
{
    LegendaryQuestActor legendaryQuestActor;

    private void OnEnable()
    {
        legendaryQuestActor = target as LegendaryQuestActor;
    }

    public override void OnInspectorGUI()
    {
        //Guard clause
        if (legendaryQuestActor.avatar == null)
            return;

        //Convert the weaponSprite (see SO script) to Texture
        Texture2D texture = AssetPreview.GetAssetPreview(legendaryQuestActor.avatar);
        //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
        //This allows us to place the image JUST UNDER our default inspector
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        //Draws the texture where we have defined our Label (empty space)
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

        //Draw whatever we already have in SO definition
        base.OnInspectorGUI();
    }

}
