using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Behaviour Collection", menuName = "Game Data/Behaviour Collection")]
public class BehaviourCollectionSO : ScriptableObject
{
    [SerializeField]
    private List<BehaviourTypeSO> m_BehaviourTypes;
    public ReadOnlyCollection<BehaviourTypeSO> BehaviourTypes { get { return m_BehaviourTypes.AsReadOnly(); } }
}
