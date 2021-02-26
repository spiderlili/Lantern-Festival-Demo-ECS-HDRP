using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

//rotate system: certain systems can't use jobs - jobs work on multithreading, certain things only happen on the main thread
public class RotateSystem : JobComponentSystem
{
    //job done for each entity that has the component passed in: RotationEulerXYZ & Rotate
    private struct RotateJob : IJobForEach<RotationEulerXYZ, Rotate>
    {
        public float deltaTime;
        public void Execute(ref RotationEulerXYZ euler, ref Rotate rotate)
        {
            euler.Value.y += rotate.radiansPerSecond * deltaTime;
            euler.Value.z = 180;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotateJob
        {
            deltaTime = UnityEngine.Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}