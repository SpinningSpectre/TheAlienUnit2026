using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private int viewLevel = 0;
    [SerializeField] private int viewRange = 10;
    [SerializeField] private LayerMask plalienLayer;
    [SerializeField] private LayerMask memyselfandILayer;
    [SerializeField] private LayerMask bodyLayer;
    public GameObject _player;
    [SerializeField] private float detectionTime = 3;
    [SerializeField] private GameObject rayPoint;
    private bool _isSpotting = false;

    public GameObject smallVisualObject;
    public Sprite exclamSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = PlayerDetection.Instance.gameObject;
        StartCoroutine(CheckAround());
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rayPoint.transform.position, (_player.transform.position - rayPoint.transform.position), Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green);
    }

    // Update is called once per frame
    public IEnumerator CheckAround()
    {
        while (true)
        {
            if (SeesPlayer() && !_isSpotting)
            {
                StartCoroutine(StartDetection());
            }
            SeesBody();
            yield return new WaitForSeconds(0.1f)
;        }
    }

    public IEnumerator StartDetection()
    {
        print("Wha??");
        if (smallVisualObject) { smallVisualObject.SetActive(true); }

        //do ex clam
        _isSpotting = true;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds((detectionTime / 30) * 1.5f);
            if (!SeesPlayer())
            {
                print("nvm");
                _isSpotting = false;
                yield break;
            }
        }
        if (SeesPlayer())
        {
            StartCoroutine(CloseDetection());
        }
        else
        {
            smallVisualObject.SetActive(false);
            _isSpotting = false;
        }
    }

    public IEnumerator CloseDetection()
    {
        print("erm excuse me wha");

        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds((detectionTime / 30) * 1.5f);
            if (!SeesPlayer())
            {
                _isSpotting = false;
                yield break;
            }
        }
        if(SeesPlayer() )
        {
            Detect();
        }
        else
        {
            smallVisualObject.SetActive(false);
            _isSpotting =false;
        }
    }

    public void Detect()
    {
        print("KILL YOURSELF");
    }

    private bool SeesPlayer()
    {
        if (viewLevel >= PlayerDetection.Instance.currentMaskLevel || PlayerDetection.Instance.currentMaskVoided)
        {
            print("in area " + gameObject.name +" also my name");
            Vector3 forward = rayPoint.transform.TransformDirection(Vector2.up);
            Vector3 toOther = Vector3.Normalize(_player.transform.position - transform.position);
            if (Vector3.Dot(forward, toOther) > 0.3f)
            {
                print(" in sight" + gameObject.name + " also my name");
                RaycastHit2D hit = Physics2D.Raycast(rayPoint.transform.position, (_player.transform.position - rayPoint.transform.position), viewRange, ~memyselfandILayer);
                if(hit && hit.collider.gameObject == _player.gameObject)
                {
                    print(" in vision" + gameObject.name + " also my name");
                    return true;
                }
            }
        }
        return false;
    }

    private void SeesBody()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(rayPoint.transform.position, 10);
        for(int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.name == "Body")
            {
                Vector2 forward = rayPoint.transform.TransformDirection(Vector2.up);
                Vector2 toOther = Vector2.Normalize(cols[i].transform.position - rayPoint.transform.position);
                if (Vector2.Dot(forward, toOther) > 0.3f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(rayPoint.transform.position, (cols[i].transform.position - rayPoint.transform.position), viewRange, ~memyselfandILayer);
                    if (hit && hit.collider.gameObject == cols[i].gameObject)
                    {
                        if(_player.GetComponent<PlayerDetection>().faceCurrentBody == cols[i].gameObject)
                        {
                            _player.GetComponent<PlayerDetection>().currentMaskVoided = true;
                        }
                    }
                }
            }
        }

    }
}
