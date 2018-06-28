using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(InputSystem))]
public class CrosshairSystem : JobComponentSystem {

    [BurstCompile]
    private struct CrosshairJob : IJobProcessComponentData<Position, MouseInput>
    {
        public void Execute(ref Position position, [ReadOnly] ref MouseInput mouseInput)
        {
            position = new Position(new float3(mouseInput.Pos.x, mouseInput.Pos.y, 0));
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CrosshairJob
        {

        };
        return job.Schedule(this, 1, inputDeps);
    }

}
