using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
//Will become the component on the entity for a game object
public class PerlinCubePosProxyECS : MonoBehaviour, IConvertGameObjectToEntity
{
    //EntityManager is a pool for all entities
    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        //create a new component
        var data = new PerlinPositionForEntity
        { };

        //add component onto entity (cube) 
        entityManager.AddComponentData(entity, data);
    }
}