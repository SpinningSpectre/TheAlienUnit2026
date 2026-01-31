using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static PlayerDetection Instance;
    public int currentMaskLevel = 0;
    public GameObject faceCurrentBody;
    public bool currentMaskVoided = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    public static void SetMaskLevel(int level)
    {
        Instance.currentMaskLevel = level;
        Instance.currentMaskVoided = false;
    }
}
