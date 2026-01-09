
using UnityEngine;

public class MonsterB : Monster
{
    [SerializeField] private ProjectileLauncher m_launcher = null;

    new protected void Start()
    {
        base.Start();
        m_launcher = GetComponent<ProjectileLauncher>(); 
    }

   


}
