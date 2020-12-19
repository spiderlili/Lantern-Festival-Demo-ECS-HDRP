using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnCubesECSSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //SpawnCubesECS becomes its own entity, cause it exist & run
    //use [BurstCompile] attribute to compile a job with Burst
    [BurstCompile]
    struct SpawnJob : IJobForEachWithEntity<SpawnCubesECS, LocalToWorld>
    {
        public EntityCommandBuffer commandBuffer;

        //use job to create the world
        public void Execute(Entity entity, int index, [ReadOnly] ref SpawnCubesECS cubeSpawner, [ReadOnly] ref LocalToWorld location)
        {
            //reference SpawnCubes.cs to spawn cubes
            for (int x = 0; x < cubeSpawner.ECSRows; x++)
            {
                for (int z = 0; z < cubeSpawner.ECSColumns; z++)
                {
                    //like creating game object with instantiate: create an entity instead. 
                    //passed through the cube prefab to the spawn of proxy which then pass through a spawn structure
                    var instance = commandBuffer.Instantiate(cubeSpawner.ECSCubePrefab);

                    //create a position of where to put this entity instance in the world - different than doing vector3 but maths is familiar
                    //want a local to the world - using the the world coordinate system: creating a transform for entity based on world coordinates & actual position in world
                    //using new noise notation of ECS as can't use Mathf library
                    var pos = math.transform(location.Value, new float3(x, noise.cnoise(new float2(x, z) * 0.2f), z));

                    //apply the transform to the entity created 
                    commandBuffer.SetComponent(instance, new Translation
                    {
                        Value = pos
                    });
                }
            }
            //remove the spawn job from the commmand buffer: only want to run it once. Otherwise it will keep executing over and over again 
            commandBuffer.DestroyEntity(entity);
        }
    }

    //gets update code running in the background outside of normal monobehaviour update loop: OnUpdate runs on the main thread
    // Any previously scheduled jobs reading/writing from/tp SpawnJob will automatically be included in the inputDependencies.
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        //construct the job, inputDeps = input handle to job. 
        //IJobProcessComponentData uses a parallel Job, so a Concurrent Entity Command Buffer is required => use ScheduleSingle() to schedule the job on a single thread instead
        var job = new SpawnJob
        {
            commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer()
        }.ScheduleSingle(this, inputDependencies);

        //When writing to a command buffer from a Job: must add to the buffer system's dependency list with AddJobHandleForProducer(JobHandle).
        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);

        //if keep passing the job handle: the system will know how far through that job is of being run in the background
        return job;
    }
}