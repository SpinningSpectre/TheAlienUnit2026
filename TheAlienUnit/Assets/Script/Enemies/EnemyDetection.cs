using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private int viewLevel = 0;
    [SerializeField] private int viewRange = 10;
    [SerializeField] private LayerMask plalienLayer;
    [SerializeField] private LayerMask bodyLayer;
    public GameObject _player;
    [SerializeField] private float detectionTime = 3;
    private bool _isSpotting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = PlayerDetection.Instance.gameObject;
        StartCoroutine(CheckAround());
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
        print("huh?");
        //do ex clam
        _isSpotting = true;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds((detectionTime / 30) * 1.5f);
            if (!SeesPlayer())
            {
                _isSpotting = false;
                yield break;
            }
        }
        if (SeesPlayer() )
        {
            StartCoroutine(CloseDetection());
        }
        else
        {
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
            _isSpotting=false;
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
            Vector2 forward = transform.TransformDirection(Vector2.up);
            Vector2 toOther = Vector2.Normalize(PlayerDetection.Instance.transform.position - transform.position);
            if (Vector2.Dot(forward, toOther) > 0.3f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (_player.transform.position - transform.position), viewRange);
                if (hit && hit.collider.gameObject == _player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void SeesBody()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 10);
        for(int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.name == "Body")
            {
                print("We have a body!");
                Vector2 forward = transform.TransformDirection(Vector2.up);
                Vector2 toOther = Vector2.Normalize(cols[i].transform.position - transform.position);
                if (Vector2.Dot(forward, toOther) > 0.3f)
                {
                    print("In ma damn vision!");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, (cols[i].transform.position - transform.position), viewRange);
                    if (hit && hit.collider.gameObject == cols[i].gameObject)
                    {
                        if(_player.GetComponent<PlayerDetection>().faceCurrentBody == cols[i].gameObject)
                        {
                            _player.GetComponent<PlayerDetection>().currentMaskVoided = true;
                        }
                        print("Bro lied");
                    }
                    if (hit)
                    {
                        print(hit.collider.gameObject.name + " and also " + cols[i].gameObject);
                    }
                }
            }
        }

    }
}
