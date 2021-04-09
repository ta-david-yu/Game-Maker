using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviourData represents a behaviour, including parameters' values
/// </summary>
[System.Serializable]
public struct BehaviourData
{
    [System.Serializable]
    public struct BehaviourParamData
    {
        /// <summary>
        /// Parameter Information
        /// </summary>
        public BehaviourParamSO BehaviourParamSO;

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string Value;
    }

    public BehaviourSO BehaviourSO;
    public List<BehaviourParamData> ParamDatas;
}

/// <summary>
/// EntityData represents an entity
/// </summary>
[System.Serializable]
public struct EntityData
{
    public EntitySO EntitySO;
    public List<BehaviourData> BehaviourDatas;
}
