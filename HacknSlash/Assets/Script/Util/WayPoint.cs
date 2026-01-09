using UnityEngine;

/// <summary>
/// 몬스터들의 로밍장소
/// WayPoint
/// </summary>
[RequireComponent (typeof(CircleCollider2D))]
public class WayPoint : MonoBehaviour
{
   // public event System.Action<WayPoint> onArrived;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //onArrived?.Invoke(this);
    }

  
}
