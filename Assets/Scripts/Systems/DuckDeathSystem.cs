using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(ShootSystem))]
public class DuckDeathSystem : JobComponentSystem {

    private class KillBarrier : BarrierSystem
    {
    }

    private struct DuckHealthGroup
    {
        [ReadOnly] public ComponentDataArray<Health> Healths;
        [ReadOnly] public ComponentDataArray<DuckTag> Tags;
        [ReadOnly] public EntityArray Entities;
        public int Length;
    }

    [Inject] KillBarrier _killBarrier;
    [Inject] DuckHealthGroup _ducks;
    
    [BurstCompile]
    private struct HealthJob : IJob
    {
        public EntityCommandBuffer Commands;
        [ReadOnly] public ComponentDataArray<Health> Healths;
        [ReadOnly] public EntityArray Entities;

        public void Execute()
        {
            for (int i = 0; i < Entities.Length; i++)
            {
                if(Healths[i].Value <= 0)
                {
                    Commands.DestroyEntity(Entities[i]);
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new HealthJob
        {
            Commands = _killBarrier.CreateCommandBuffer(),
            Healths = _ducks.Healths,
            Entities = _ducks.Entities
        }.Schedule(inputDeps);
    }

}
