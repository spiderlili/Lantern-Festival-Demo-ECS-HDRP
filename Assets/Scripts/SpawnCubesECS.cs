using Unity.Entities;
using UnityEngine;

public struct SpawnCubesECS : IComponentData
{
    //the entity for the SpawnCubesECS gets created, it now has a spawner component on it & has passed through that information from the inspector
    public Entity ECSCubePrefab;
    public int ECSRows;
    public int ECSColumns;
}