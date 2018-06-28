using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class MoveSystem : JobComponentSystem {

    [BurstCompile]
    private struct MoveJob : IJobProcessComponentData<Position, MoveSpeed>
    {
        [ReadOnly] public float Dt;
        [ReadOnly] public int HorizontalLimit;

        public void Execute(ref Position position, [ReadOnly] ref MoveSpeed moveSpeed)
        {
            var pos = position;
            pos.Value.x += moveSpeed.Value * Dt;

            if(pos.Value.x >= HorizontalLimit)
            {
                pos.Value.x = 0;
            }

            position = pos;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MoveJob
        {
            Dt = Time.deltaTime,
            HorizontalLimit = Bootstrap.HorizontalLimit
        };
        return job.Schedule(this, 1, inputDeps);
    }

}
