using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ObjectListSO : ScriptableObject
{
    public List<ObjectData> GridObjectData; //A list with data for objects (Used by other scripts)
}

[Serializable] public class ObjectData 
{
    [field: SerializeField] public string Name {get; private set;} //Name of the object
    [field: SerializeField] public int Id {get; private set;} //The identification number of the object
    [field: SerializeField] public Vector2Int Size {get; private set;} = Vector2Int.one; //The size of the object
    [field: SerializeField] public GameObject Prefab {get; private set;} //The prefab of the object

}
