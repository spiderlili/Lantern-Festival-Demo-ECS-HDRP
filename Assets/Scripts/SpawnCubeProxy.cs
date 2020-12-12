using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//inherit from IDeclareReferencedPrefabs & IConvertGameObjectToEntity to turn this into an entity spawning system
public class SpawnCubeProxy : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    //pass these values thhrough from the game object into the ECS system
    public GameObject cubepf;
    public int rows;
    public int columns;

    //IDeclareReferencedPrefabs needs its own method: takes care of declaring and creating prefabs
    public void DeclareReferencedPrefabs(List<GameObject> gameObjects)
    {
        gameObjects.Add(cubepf);
    }

    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        //act as an interface between the SpawnCubesECS game object & the SpawnCubesECS which is going to become an entity. 
        //need to declare a new SpawnCubesECS structure to pass information through to it
        var spawnerData = new SpawnCubesECS
        {
            //need a holder for each value: putting data & declare an instance of a structure
            ECSCubePrefab = conversionSystem.GetPrimaryEntity(cubepf),
            ECSRows = rows,
            ECSColumns = columns
        };
        entityManager.AddComponentData(entity, spawnerData);
    }
}