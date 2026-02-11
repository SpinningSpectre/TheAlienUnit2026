using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactablesInArea = new List<Interactable>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactable>() && !interactablesInArea.Contains(collision.GetComponent<Interactable>()))
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

    bool canInteract = true;
    public void Interact()
    {
        Debug.Log("Interact");
        if (!canInteract) return;
        StartCoroutine(InteractCooldown());
        for(int i = 0; i < interactablesInArea.Count; i++)
        {
            if (!interactablesInArea[i].enabled) { return; }
            interactablesInArea[i].Interact();
        }
        print("I go");
    }

    public IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSecondsRealtime(2);
        canInteract = true;
    }
}
