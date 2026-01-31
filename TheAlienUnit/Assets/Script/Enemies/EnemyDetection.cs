using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private int viewLevel = 0;
    [SerializeField] private int viewRange = 10;
    [SerializeField] private LayerMask plalienLayer;
    public GameObject _player;
    [SerializeField] private float detectionTime = 3;
    private bool _isSpotting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = PlayerDetection.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (SeesPlayer() && !_isSpotting)
        {
            StartCoroutine(StartDetection());
        }
    }

    public IEnumerator StartDetection()
    {
        print("huh?");
        //do ex clam
        _isSpotting = true;
        yield return new WaitForSeconds((detectionTime / 3) * 1.5f);
        if(SeesPlayer() )
        {
            StartCoroutine(CloseDetection());
        }
    }

    public IEnumerator CloseDetection()
    {
        print("erm excuse me wha");
        yield return new WaitForSeconds((detectionTime / 3) * 1.5f);
        if(SeesPlayer() )
        {
            Detect();
        }
    }

    public void Detect()
    {
        print("KILL YOURSELF");
    }

    private bool SeesPlayer()
    {
        if (viewLevel >= PlayerDetection.Instance.currentMaskLevel && !PlayerDetection.Instance.currentMaskVoided)
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
}
