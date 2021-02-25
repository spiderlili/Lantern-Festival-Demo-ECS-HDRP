using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//entities have a list of component data
public struct SpawnerLantern : IComponentData
{
    //prefab to spawn
    public Entity prefabLantern;

    //how far away from spawner = radius
    public float maxDistanceFromSpawner;

    //how often to spawn: how many seconds between each spawn
    public float secondsBetweenSpawns;

    //how long it has been since last spawn
    public float secondsToNextSpawn;
}