using System.Collections;
using System.Collections.Generic;
using Unity.Entities;

//component: store data, no logic - all by reference (struct)
public struct Rotate : IComponentData
{
    //use radians which is faster to work with mathematically 
    public float radiansPerSecond;

    public float flySpeedPerSecond;
}