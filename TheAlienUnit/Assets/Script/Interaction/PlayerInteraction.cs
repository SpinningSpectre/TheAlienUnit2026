using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactablesInArea = new List<Interactable>();

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

    public void Interact()
    {
        Debug.Log("Interact");
        for(int i = 0; i < interactablesInArea.Count; i++)
        {
            interactablesInArea[i].Interact();
        }
        print("I go");
    }
}
