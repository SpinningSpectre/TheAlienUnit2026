using UnityEngine;

public class NukeCodeInteract : MonoBehaviour
{
    public AudioClip typing;
    public void GetCode()
    {
        ObjectiveScript.Instance.amountOfCodes += 1;
        Soundsystem.PlaySound(typing);
        if(ObjectiveScript.Instance.amountOfCodes == 1)
        {
            ObjectiveScript.UpdateObjective("Find the nuclear codes (1/2)");
        }
        else
        {
            ObjectiveScript.UpdateObjective("Find the nuke room");
        }
    }
}
