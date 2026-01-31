using UnityEngine;
using UnityEngine.InputSystem;

public class IMCALLING : MonoBehaviour
{
    public AudioClip ARJENTELLSYOUYOURGONNADIE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        if (keyboard.fKey.wasPressedThisFrame)
        {
            print("ALIGN GAYM");
            Soundsystem.PlaySound(ARJENTELLSYOUYOURGONNADIE, false, true, 0.1f);
        }
    }
}
