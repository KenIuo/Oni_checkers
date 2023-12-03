using UnityEngine;

public class LayersTags
{
    public static readonly LayerMask ARROW_LAYER = LayerMask.GetMask("UI_Arrows");
    public static readonly LayerMask PF_LAYER = LayerMask.GetMask("Playing_Field");
    public static readonly LayerMask HIDER_LAYER = LayerMask.GetMask("UI_Hideable");
}

public class NamesTags
{
    public static readonly string MAIN_MENU_SCENE = "MainMenuScene";
    public static readonly string ARENA_1_SCENE = "Arena1Scene";
    public static readonly string ARENA_2_SCENE = "Arena2Scene";
    public static readonly string ARENA_3_SCENE = "Arena3Scene";

    public static readonly string PLAYING_FIELD = "PlayingField";
    public static readonly string BGM_TAG = "GameMusic";
}