using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

//functions converted from monobehaviours & gameobjects into entities
//want to convert spawned prefabs into entities: anything with this monobehaviour on will be converted into an entity
public class SpawnerLanternAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private float maxDistanceFromSpawner;
    //add to referenced prefabs
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }

    //make an entity with data: anything with this monobehaviour on is going to have a SpawnerLantern component on it
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new SpawnerLantern
        {
            prefabLantern = conversionSystem.GetPrimaryEntity(prefab), //convert game object prefabs to entities
                maxDistFromSpawner = maxDistanceFromSpawner,
                //can call spawn every frame but for performance reasons only call it on conversion
                secondsBetweenSpawns = 1 / spawnRate,
                secondsToNextSpawn = 0f
        }); //need the entity to add it to & the data to add
    }
}