using UnityEngine;
using UnityEngine.Events;

public class MonoEvents : MonoBehaviour
{

    public UnityEvent onStart;
    public UnityEvent onAwake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        onAwake.Invoke();
    }
    
    void Start()
    {
        onStart.Invoke();
    }
    
}
