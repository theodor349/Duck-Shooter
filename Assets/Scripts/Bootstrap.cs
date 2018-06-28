using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

public class Bootstrap {

    public static MeshInstanceRenderer DuckLook;

    public static EntityArchetype _playerArchtype;
    public static EntityArchetype _duckArchtype;

    private const int Amount = 5;
    private const int DuckHealth = 1;
    private const int MoveSpeed = 5;
    private const int MoveSpeedVariation = 2;
    private const int VertivalLimit = 10;

    public static int HorizontalLimit = 20;
    public static float Accuracy = 0.5f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        var entitymanager = World.Active.GetOrCreateManager<EntityManager>();
        CreateArchtypes(entitymanager);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitWithScene()
    {
        var entityManager = World.Active.GetExistingManager<EntityManager>();
        CreateActors(entityManager);
    }

    private static void CreateActors(EntityManager entityManager)
    {
        var player = entityManager.CreateEntity(_playerArchtype);

        for (int i = 0; i < Amount; i++)
        {
            var duck = entityManager.CreateEntity(_duckArchtype);
            entityManager.SetSharedComponentData(duck, GetLook("DuckLook"));
            entityManager.SetComponentData(duck, new Position
            {
                Value = new float3( Random.Range(0,HorizontalLimit), 
                                    Random.Range(0,VertivalLimit), 
                                    0)
            });
            entityManager.SetComponentData(duck, new MoveSpeed
            {
                Value = Random.Range(MoveSpeed - MoveSpeedVariation,
                                        MoveSpeed + MoveSpeedVariation)
            });
            entityManager.SetComponentData(duck, new Health {
                Value = DuckHealth
            });
        }
    }

    private static void CreateArchtypes(EntityManager entityManager)
    {
        _playerArchtype = entityManager.CreateArchetype(
            typeof(TransformMatrix),
            typeof(Position),
            typeof(PlayerTag),
            typeof(MouseInput)
            );

        _duckArchtype = entityManager.CreateArchetype(
            typeof(TransformMatrix),
            typeof(Position),
            typeof(MeshInstanceRenderer),
            typeof(DuckTag),
            typeof(Health),
            typeof(MoveSpeed)
            );
    }

    private static MeshInstanceRenderer GetLook(string protoType)
    {
        var prototype = GameObject.Find(protoType);
        var look = prototype.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(prototype);
        return look;
    }

}
