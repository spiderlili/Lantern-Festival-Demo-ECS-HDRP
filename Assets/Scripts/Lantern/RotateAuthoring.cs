using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

//convert component into actual entity representation
public class RotateAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private float degreesPerSecond;
    [SerializeField] private float flySpeed;

    //convert gets called when close is pressed on the sub scene
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //pass the entity with all the components on, add rotation to it (convert it to a component on the entity)
        dstManager.AddComponentData(entity, new Rotate
        {
            //dots optimised maths functions (Unity.Mathematics)
            radiansPerSecond = math.radians(degreesPerSecond),
                flySpeedPerSecond = flySpeed
        });

        dstManager.AddComponentData(entity, new RotationEulerXYZ());
        dstManager.AddComponentData(entity, new Translation());
    }
}