using UnityEngine;

public class Mask : MonoBehaviour
{
    public int maskLevel = 0;
    public GameObject body;
    public Color maskColor;
    public void GrabMask() 
    {
        PlayerDetection.SetMaskLevel(maskLevel,maskColor);
        PlayerDetection.Instance.faceCurrentBody = body;
    }
}
