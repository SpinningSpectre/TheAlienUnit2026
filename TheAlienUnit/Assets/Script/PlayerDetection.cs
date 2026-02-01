using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDetection : MonoBehaviour
{
    public static PlayerDetection Instance;
    public int currentMaskLevel = 0;
    public Sprite[] maskSkinLevels;
    public Sprite[] maskMiscLevels;
    public GameObject maskSkinVisual;
    public GameObject maskMiscVisual;
    public GameObject faceCurrentBody;
    public bool currentMaskVoided = false;
    public TMP_Text SecurityLevelUI;
    [SerializeField] private TilemapCollider2D[] doors;

    public GameObject maskPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    public static void SetMaskLevel(int level,Color maskColor)
    {
        Instance.SetMask(level,maskColor);
    }

    public void SetMask(int level, Color maskColor)
    {
        SecurityLevelUI.text = level.ToString();
        currentMaskLevel = level;
        currentMaskVoided = false;
        maskSkinVisual.SetActive(level != 0);
        maskMiscVisual.SetActive(level != 0);

        if(level == 0) { return; }
        maskSkinVisual.GetComponent<SpriteRenderer>().sprite = maskSkinLevels[level - 1];
        maskMiscVisual.GetComponent<SpriteRenderer>().sprite = maskMiscLevels[level - 1];
        maskSkinVisual.GetComponent<SpriteRenderer>().color = maskColor;
        SwitchDoors(true);
    }

    public void DropMask()
    {
        if(currentMaskLevel == 0) return;
        Mask mas = Instantiate(maskPrefab,transform.position,transform.rotation).GetComponent<Mask>();
        mas.maskLevel = currentMaskLevel;
        mas.maskColor = maskSkinVisual.GetComponent<SpriteRenderer>().color;
        mas.body = faceCurrentBody;
        maskSkinVisual.SetActive(false);
        maskMiscVisual.SetActive(false);


        SwitchDoors(false);
        currentMaskLevel = 0;
        faceCurrentBody = null;
    }

    public void SwitchDoors(bool enabled)
    {
        for (int i = 0; i < currentMaskLevel - 1; i++) {
            doors[i].enabled = enabled;
        }
    }
}
