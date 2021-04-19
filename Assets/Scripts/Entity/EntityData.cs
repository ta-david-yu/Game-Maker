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

        public BehaviourParamData Clone()
        {
            return new BehaviourParamData
            {
                BehaviourParamSO = BehaviourParamSO,
                Value = Value
            };
        }
    }

    public BehaviourTypeSO BehaviourSO;
    public List<BehaviourParamData> ParamDatas;

    public BehaviourData Clone()
    {
        var clone = new BehaviourData()
        {
            BehaviourSO = BehaviourSO,
            ParamDatas = new List<BehaviourParamData>()
        };

        for (int i = 0; i < ParamDatas.Count; i++)
        {
            var paramData = ParamDatas[i];
            clone.ParamDatas.Add(paramData.Clone());
        }

        return clone;
    }
}

/// <summary>
/// EntityData represents an entity
/// </summary>
[System.Serializable]
public struct EntityData
{
    public string EntityName;
    public EntityTypeSO EntitySO;
    public List<BehaviourData> BehaviourDatas;

    public EntityData Clone()
    {
        var clone = new EntityData()
        {
            EntityName = EntityName,
            EntitySO = EntitySO,
            BehaviourDatas = new List<BehaviourData>()
        };

        for (int i = 0; i < BehaviourDatas.Count; i++)
        {
            var behaviourData = BehaviourDatas[i];
            clone.BehaviourDatas.Add(behaviourData.Clone());
        }

        return clone;
    }
}
