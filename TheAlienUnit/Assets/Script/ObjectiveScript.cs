using TMPro;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    public static ObjectiveScript Instance;
    [SerializeField] private TMP_Text textBox;

    private void Start()
    {
        Instance = this;
        UpdateObj("Find the nuclear codes (0/2)");
    }

    public static void UpdateObjective(string text)
    {
        Instance.UpdateObj(text);
    }
    public void UpdateObj(string text)
    {
        textBox.text = text;
    }
}
