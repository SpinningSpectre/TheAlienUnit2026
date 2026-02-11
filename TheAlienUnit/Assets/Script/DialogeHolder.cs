using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DialogeHolder : MonoBehaviour
{
    public List<dialoge> Dialoge1;

    public void PlayDialoge1()
    {
        DialogeScript.instance.StartDialoge(Dialoge1,0);
        StartCoroutine(DialogeScript.instance.DoAutoDia());
    }
}
