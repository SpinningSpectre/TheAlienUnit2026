using UnityEngine;

public class ObjectiveWalker : MonoBehaviour
{
    bool beenTrigged = false;
    public string text;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (beenTrigged) return;
        if(collision.gameObject.layer == 8)
        {
            beenTrigged = true;
            ObjectiveScript.UpdateObjective(text);
        }
    }
}
