using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int ran = Random.Range(0, possibleColors.Length);
        Color chosenColor = possibleColors[ran];
        GetComponent<SpriteRenderer>().color = chosenColor;

        transform.parent.GetComponent<Npc>().npcColor = chosenColor;
    }
}
