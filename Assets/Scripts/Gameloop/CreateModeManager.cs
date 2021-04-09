using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CreateModeManager : MonoBehaviour
{
    public enum Tool
    {

    }

    [Header("Data")]

    [SerializeField]
    private List<EntitySO> m_EntityDatas;

    [SerializeField]
    private List<BehaviourSO> m_BehaviourDatas;

    [Header("Reference")]

    [SerializeField]
    private Camera m_Camera;

    private List<EntityInstance> m_EntityInstances = new List<EntityInstance>();
    public ReadOnlyCollection<EntityInstance> EntityInstances { get { return m_EntityInstances.AsReadOnly(); } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        for (int i = 0; i < m_EntityDatas.Count; i++)
        {
            var entityData = m_EntityDatas[i];
        }

        for (int i = 0; i < m_BehaviourDatas.Count; i++)
        {
            var behaviourData = m_BehaviourDatas[i];
        }
    }
}
