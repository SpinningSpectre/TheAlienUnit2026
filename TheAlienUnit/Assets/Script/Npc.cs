using UnityEngine;
using UnityEngine.Events;

public class Npc : MonoBehaviour, IInteractable
{
    public bool isAlive { get; private set; }
    
    
    [Header("Unity Events")]
    public UnityEvent die;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
