using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogeScript : MonoBehaviour
{
    //This is so you can call this script anywhere
    public static DialogeScript instance;

    [SerializeField] private List<dialoge> testDialoges = new List<dialoge>();

    //it copies dialoge onto this
    [SerializeField] private List<dialoge> currentTextDialoges = new List<dialoge>();

    //references to ui elements
    [SerializeField] private GameObject dialogeBox;
    [SerializeField] private TMP_Text dialogeText;
    [SerializeField] private TMP_Text voiceText;
    [SerializeField] private Image voiceImage;
    [SerializeField] private Button[] optionButtons;

    //this is for volume settings
    PlayerInput playerInput;
    //keeping track of current dialoge
    bool inDialoge = false;
    int currentDiaIndex;

    //keeping track of current audio
    public List<GameObject> spawnedSources = new List<GameObject>();

    //idk prob used somewhere
    [SerializeField] private bool playerCanMove = true;

    void Start()
    {
        instance = this;
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        //This is mainly for testing, you can press L to start some dialoge
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
#if UNITY_EDITOR
        if (keyboard.lKey.wasPressedThisFrame)
        {
            StartDialoge(testDialoges, 0);
        }
#endif
        if (inDialoge)
        {
            //if the player goes to the next part and they dont have to answer a question
            //if (keyboard.numpadPlusKey.wasPressedThisFrame && currentTextDialoges[currentDiaIndex].hasQuestion == false)
            //{
            //    NextDia();
            //}
        }
    }

    public void StartDialoge(List<dialoge> dialoges, int index)
    {
        if (inDialoge) { return; }
        //stop player from moving

        //turn on dialoge
        dialogeBox.SetActive(true);
        WriteDialoge(index, dialoges);
        inDialoge = true;
    }

    public void WriteDialoge(int index, List<dialoge> dialoges)
    {
        dialoge currentDia = dialoges[index];

        //calls the events
        currentDia.startDialogEvent.Invoke();

        //make sound play
        if (currentDia.playsSound)
        {
            spawnedSources.Add(Soundsystem.PlaySound(currentDia.clip));
        }

        currentDiaIndex = index;
        //Names things broken for if they break (no way)
        string text = "BROKEN";
        string voice = "NONE";
        string[] buttonText = { "BROKE", "BROKE", "BROKE" };

        //sets the actual texts and images
        text = currentDia.text;
        buttonText = currentDia.questionAnswers;
        voice = currentDia.voiceName;
        voiceText.text = voice;
        voiceImage.sprite = currentDia.voiceImage;
        dialogeText.text = text;

        //turn off all buttons
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        //turn them back on with settings if the question needs it
        if (currentDia.hasQuestion)
        {
            for (int i = 0; i < buttonText.Length; i++)
            {
                //turn buttons on and set text
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = buttonText[i];

                //set functionality such as end of dialoge events
                int moveTo = currentDia.questionMoveTo[i];
                optionButtons[i].GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                optionButtons[i].GetComponent<Button>().onClick.AddListener(() => currentTextDialoges[currentDiaIndex].endDialogEvent.Invoke());
                optionButtons[i].GetComponent<Button>().onClick.AddListener(() => KillSound());
                optionButtons[i].GetComponent<Button>().onClick.AddListener(() => WriteDialoge(moveTo, dialoges));
            }
        }
        currentTextDialoges = dialoges;
    }
    public void StopDialoge()
    {
        //make player be able to move again

        //do any end of dialoge events
        currentTextDialoges[currentDiaIndex].endDialogEvent.Invoke();

        //turn off dialoge
        dialogeBox.SetActive(false);
        inDialoge = false;
    }

    private void KillSound()
    {
        for (int i = 0; i < spawnedSources.Count; i++)
        {
            Destroy(spawnedSources[i]);
        }
    }

    public static void StartDialoge(List<dialoge> dia)
    {
        instance.StartDialoge(dia, 0);
    }

    public void NextDia()
    {
        if(!currentTextDialoges[currentDiaIndex].hasQuestion == false) { return; }
        KillSound();
        //continue or end dialoge
        if (!currentTextDialoges[currentDiaIndex].isEnd)
        {
            WriteDialoge(currentTextDialoges[currentDiaIndex].nextDiaIfNotQuestion, currentTextDialoges);
        }
        else
        {
            StopDialoge();
        }
    }

    public static void NextDial()
    {
        instance.NextDia();
    }
}

[System.Serializable]
public class dialoge
{
    [Header("Text")]
    public string text;
    public bool isEnd;

    //what dialoge "id" it needs to go to, the id is based on list order
    public int nextDiaIfNotQuestion;

    //whatever character said it
    public string voiceName;
    public Sprite voiceImage;

    [Header("Sound")]
    public bool playsSound;
    public AudioClip clip;

    [Header("Questions")]
    public bool hasQuestion;
    public string[] questionAnswers;

    //what dialoge "id" it needs to go to
    public int[] questionMoveTo;

    [Header("Events")]
    //These events are called at the start and end of dialoge, and should all be set through the inspector
    public UnityEvent startDialogEvent;
    public UnityEvent endDialogEvent;

}
