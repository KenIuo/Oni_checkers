using UnityEngine;

public class LayersTags
{
    public static readonly LayerMask ARROW_LAYER = LayerMask.GetMask("UI_Arrows");
    public static readonly LayerMask PF_LAYER = LayerMask.GetMask("Playing_Field");
    public static readonly LayerMask HIDER_LAYER = LayerMask.GetMask("UI_Hidable");
}

public class NamesTags
{
    public static readonly string MAIN_MENU_SCENE = "MainMenuScene";
    public static readonly string ARENA_1_SCENE = "Arena1Scene";
    public static readonly string ARENA_2_SCENE = "Arena2Scene";
    public static readonly string ARENA_3_SCENE = "Arena3Scene";

    public static readonly string PLAYING_FIELD = "PlayingField";
    public static readonly string BGM_TAG = "GameMusic";

    public static readonly string VOLUME_MASTER = "MasterVolume";
    public static readonly string VOLUME_MUSIC = "MusicVolume";
    public static readonly string VOLUME_EFFECTS = "EffectsVolume";
    public static readonly string VOLUME_ENVIRONMENT = "EnvironmentVolume";

    public static readonly string SHADER_COLOR = "_Color";
    public static readonly string SHADER_SECOND_COLOR = "_Shadow";

    public static readonly string VFX_COLOR = "Color2";
    public static readonly string DISSOLVE = "_DissolveAmount";
}

public class AnimationTags
{
    public const string KILL = "Dead";
    public const string PULL = "Pulled";
}