using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SO holding behaviour information
/// </summary>
[CreateAssetMenu(fileName = "Behaviour Param SO", menuName = "Game Data/Behaviour Param SO")]
public class BehaviourParamSO : ScriptableObject
{
    public enum ParamType
    {
        Integer,
        Float,
        Bool,
        String
    }

    public string Name;
    public ParamType Type;
    public string DefaultValue;

    public bool IsEnum = false;
    public List<string> EnumValues;
}
