using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static PlayerDetection Instance;
    public int currentMaskLevel = 0;
    public GameObject faceCurrentBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
    }

    public static void SetMaskLevel(int level)
    {
        Instance.currentMaskLevel = level;
    }
}
