using UnityEngine;

public class Mask : MonoBehaviour
{
    public int maskLevel = 0;
    public GameObject body;
    public void GrabMask() 
    {
        PlayerDetection.SetMaskLevel(maskLevel);
        PlayerDetection.Instance.faceCurrentBody = body;
    }
}
