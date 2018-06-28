using Unity.Entities;
using Unity.Mathematics;

public struct DuckTag : IComponentData { }
public struct PlayerTag : IComponentData { }

public struct MouseInput : IComponentData
{
    // Bool's are not Blittable, and therefor is represented with 0=false and 1=true
    // And I can't get bool1 to work
    public int Fire;
    public float2 Pos;
}

public struct Health : IComponentData
{
    public float Value;
}

public struct MoveSpeed : IComponentData
{
    public float Value;
}