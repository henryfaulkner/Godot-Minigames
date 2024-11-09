using Godot;

public class ForegroundFactory
{
    private readonly StringName EGG_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/EggCharacter.tscn");
    private readonly StringName COW_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/CowCharacter.tscn");

    private PackedScene _eggScene;
    private PackedScene _cowScene;

    public ForegroundFactory()
    {
        _eggScene = (PackedScene)ResourceLoader.Load(EGG_SCENE_PATH);
        _cowScene = (PackedScene)ResourceLoader.Load(COW_SCENE_PATH);
    }

    public static EggCharacter SpawnEggMesh()
    {
        return _eggScene.Instantiate<EggCharacter>();
    }

    public static CowCharacter SpawnCowMesh()
    {
        return _cowScene.Instantiate<CowCharacter>();
    }
}