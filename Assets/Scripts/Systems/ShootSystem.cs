using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(MoveSystem))]
[UpdateAfter(typeof(CrosshairSystem))]
public class ShootSystem : JobComponentSystem {

    private struct PlayerGroup
    {
        [ReadOnly] public ComponentDataArray<Position> Positions;
        [ReadOnly] public ComponentDataArray<MouseInput> Inputs;
        [ReadOnly] public ComponentDataArray<PlayerTag> Tags;
        public int Length;
    }

    [Inject] PlayerGroup _players;

    [BurstCompile]
    [RequireComponentTag(typeof(DuckTag))]
    private struct ShootJob : IJobProcessComponentData<Position, Health>
    {
        [ReadOnly] public ComponentDataArray<MouseInput> Inputs;
        [ReadOnly] public ComponentDataArray<Position> PlayerPos;
        [ReadOnly] public float Accuracy;

        public void Execute([ReadOnly] ref Position position, ref Health health)
        {
            for (int i = 0; i < Inputs.Length; i++)
            {
                if (Inputs[i].Fire == 1 && math.distance(PlayerPos[i].Value, position.Value) <= Accuracy)
                {
                    var _health = health;
                    _health.Value -= 1;
                    health = _health;
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ShootJob
        {
            Inputs = _players.Inputs,
            PlayerPos = _players.Positions,
            Accuracy = Bootstrap.Accuracy
        };
        return job.Schedule(this, 1, inputDeps);
    }

}
