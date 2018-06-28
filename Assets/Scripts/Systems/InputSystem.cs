using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class InputSystem : JobComponentSystem {

    private struct InputGroup
    {
        public ComponentDataArray<MouseInput> Inpus;
        public int Length;
    }

    [Inject] InputGroup _inputs;

    [BurstCompile]
    private struct InputJob : IJobProcessComponentData<MouseInput>
    {
        [ReadOnly] public int Fire;
        [ReadOnly] public float3 MousePos;

        public void Execute(ref MouseInput mouseInput)
        {
            var input = mouseInput;
            input.Fire = Fire;
            input.Pos = new float2(MousePos.x, MousePos.y);
            mouseInput = input;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new InputJob
        {
            Fire = (Input.GetMouseButtonDown(0)) ? 1 : 0,
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition)
        };
        return job.Schedule(this, 1, inputDeps);
    }

}
