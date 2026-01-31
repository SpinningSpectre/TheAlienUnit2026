using UnityEngine;

public class NukeLaunchInteract : MonoBehaviour
{
    public void LaunchNuke()
    {
        if(ObjectiveScript.Instance.amountOfCodes == 2)
        {
            print("Nuke goes, start cutscene");
        }
    }
}
