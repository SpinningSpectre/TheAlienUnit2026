using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactablesInArea = new List<Interactable>();

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        if (keyboard.leftMetaKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>())
        {
            Interactable newGuy = collision.GetComponent<Interactable>();
            interactablesInArea.Add(newGuy);
            newGuy.InArea();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>())
        {
            Interactable oldGuy = collision.GetComponent<Interactable>();
            interactablesInArea.Remove(oldGuy);
            oldGuy.OutArea();
        }
    }

    private void Interact()
    {
        for(int i = 0; i < interactablesInArea.Count; i++)
        {
            interactablesInArea[i].Interact();
        }
        print("I go");
    }
}
