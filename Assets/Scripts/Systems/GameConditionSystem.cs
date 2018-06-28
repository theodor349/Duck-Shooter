using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

[AlwaysUpdateSystem]
public class GameConditionSystem : JobComponentSystem {

    private struct DuckGroup
    {
        public ComponentDataArray<DuckTag> Tags;
        public int Length;
    }

    [Inject] DuckGroup _ducks;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (_ducks.Length == 0)
        {
            Debug.Log("Game Over");
        }
        return inputDeps;
    }

}
