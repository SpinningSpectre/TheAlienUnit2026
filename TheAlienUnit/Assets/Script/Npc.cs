using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Npc : MonoBehaviour, IInteractable
{
    public bool isAlive { get; private set; }
    
    
    [Header("Unity Events")]
    public UnityEvent die;
    private NavMeshAgent _agent;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        isAlive = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    
    private void Die()
    {
        isAlive = false;
        die?.Invoke();
        Debug.Log("DIE");
    }

    public void Interact()
    {
        Die();
    }
    
    public bool CanInteract()
    {
        return isAlive;
    }
    

}
