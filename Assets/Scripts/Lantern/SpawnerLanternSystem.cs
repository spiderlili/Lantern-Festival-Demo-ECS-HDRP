using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Time = UnityEngine.Time;

public class SpawnerLanternSystem : JobComponentSystem
{
    //when a simulation ends: simulated the frame but not done the rendering.
    //if use multiple cores on PC processor: want to use as many as you can - ECS utilises across multiple cores
    //command buffer system: only want them to happen at a certain time, store a buffer of all things to do when it's done
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    //gets called when the system is created
    protected override void OnCreate()
    {
        //if there's no system with this type already: make a new one 
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    //this happens on everything that has a spawner lantern, LocalToWorld is a transform that allows you to get a relative position to the world
    private struct SpawnerJob : IJobForEachWithEntity<SpawnerLantern, LocalToWorld>
    {
        private EntityCommandBuffer.Concurrent entityCommandBuffer;
        //don't have access to deltaTime - need to pass it in as it's on a separate core
        private readonly float deltaTime;

        private Random random;

        //have to be passed in through the job scheduler
        public SpawnerJob(EntityCommandBuffer.Concurrent entityCommandBuffer, Random random, float deltaTime)
        {
            this.entityCommandBuffer = entityCommandBuffer;
            this.random = random;
            this.deltaTime = deltaTime;
        }

        //similar to update loop
        //don't need the entity but need the index, make spawner position read only
        public void Execute(Entity entity, int index, ref SpawnerLantern spawner, [ReadOnly] ref LocalToWorld localToWorld)
        {
            //can't use entity command buffer with burst but it will be compatible eventually, can still use jobs to use multiple cores
            spawner.secondsToNextSpawn -= deltaTime;

            if (spawner.secondsToNextSpawn >= 0)
            {
                return;
            }

            //add not set to prevent negative values
            spawner.secondsToNextSpawn += spawner.secondsBetweenSpawns;
            Entity instance = entityCommandBuffer.Instantiate(index, spawner.prefabLantern);
            entityCommandBuffer.SetComponent(index, instance, new Translation
            {
                //performant random vector3 different to normal unity random
                //get original position, add a random direction * a random amount between 0 to maxDistFromSpawner
                Value = localToWorld.Position + random.NextFloat3Direction() * random.NextFloat() * spawner.maxDistFromSpawner
            });
        }
    }

    //set up spawner job
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var spawnerJob = new SpawnerJob(endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            new Random((uint) UnityEngine.Random.Range(0, float.MaxValue)), //try int.MaxValue
            Time.deltaTime
        );

        JobHandle jobHandle = spawnerJob.Schedule(this, inputDeps);
        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}