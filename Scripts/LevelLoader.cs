using Godot;
using System;
using System.ComponentModel;

public class LevelLoader : Node
{
    [Export]
    public PackedScene levelTemplate;

    [Export]
    public PackedScene testGeometry;

    //[Export]
    //public LevelContainer[] levels;

    private ResourceInteractiveLoader loader = null;
    private bool look = false;
    public void LoadGameplayLevel()
    {
        GetTree().ChangeSceneTo(levelTemplate);

        look = true;
    }

    public void LoadTestAsync()
    {
        GD.Print("LevelLoader LoadTest");

        loader = ResourceLoader.LoadInteractive("res://Assets/Levels/TestLevelContainer.res");

        if (loader != null)
        {
            GD.Print("not null");
        }
        else
        {
            GD.Print("null");
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if(look)
        {
            GeometryLoader tileLoader = GetNode<GeometryLoader>("/root/Main/GeometryLoader");

            if (tileLoader != null)
            {
                tileLoader.Load(testGeometry);
                look = false;
            }
        }

        if(loader == null)
        {
            return;
        }

        Error err = loader.Poll();

        if(err == Error.Ok)
        {
            GD.Print("still loading");
            return;
        }
        if(err == Error.FileEof)
        {
            GD.Print("load finished");
            LoadFromResource(loader.GetResource());
            loader = null;
            return;
        }
        GD.Print($"load error: {err}");
        loader = null;
    }

    private void LoadFromResource(Resource resource)
    {
        GD.Print("LoadFromResource...");
        LevelContainer container = resource as LevelContainer;
        if(container == null) {
            GD.Print("LoadFromResource failed");
            return;
        }
        GetTree().ChangeSceneTo(container.Scene);
    }

    public void ResourceLoadImmediate()
    {
        GD.Print("LevelLoader LoadTest");
        PackedScene data = ResourceLoader.Load<PackedScene>("res://Scenes/Main.tscn");
        GD.Print("LevelLoader 0");

        GetTree().ChangeSceneTo(data);

        GD.Print("LevelLoader 1");
        //GeometryLoader geoLoader = GetNode<GeometryLoader>("/root/Main/GeometryLoader");
        //GD.Print("LevelLoader 2");
        //if (geoLoader == null)
        //{
        //    GD.Print("bummer");
        //    return;
        //}
        //geoLoader.Load(testGeometry);
        //GD.Print("LevelLoader 3");
    }

    public void PackedSceneLoad()
    {
        GD.Print("LevelLoader PackedSceneLoad");
        GetTree().ChangeSceneTo(levelTemplate);
        //GetTree().ChangeScene("res://Scenes/Main.tscn");

        GD.Print("LevelLoader 1");
        GeometryLoader geoLoader = GetNode<GeometryLoader>("/root/Main/GeometryLoader");
        GD.Print("LevelLoader 2");
        if (geoLoader == null)
        {
            GD.Print("bummer");
            return;
        }
        geoLoader.Load(testGeometry);
        GD.Print("LevelLoader 3");
    }
}
