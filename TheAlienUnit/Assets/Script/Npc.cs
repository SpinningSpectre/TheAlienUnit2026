using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Npc : MonoBehaviour, IInteractable
{
    public bool isAlive { get; private set; }

    [Header("Unity Events")]
    public UnityEvent die;

    private NavMeshAgent _agent;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int _currentPatrolIndex = 0;
    public float waitTime = 2f;

    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    [Header("Search Settings")]
    public Vector2 noisePos;
    public float searchDuration = 3f;
    private bool _isSearching = false;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        isAlive = true;

        if (patrolPoints.Length > 0)
            _agent.SetDestination(patrolPoints[0].position);
    }

    private void Update()
    {
        if (!isAlive) return;

        if (noisePos == Vector2.zero && !_isSearching)
        {
            Patrol();
        }
        else
        {
            Search();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;

            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                GoToNextPatrolPoint();
            }

            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
        {
            _isWaiting = true;
            _waitTimer = waitTime;
            _agent.isStopped = true;
        }
    }

    private void GoToNextPatrolPoint()
    {
        _agent.isStopped = false;

        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
    }

    private void Search()
    {
        // If we just detected noise, start moving toward it
        if (!_isSearching)
        {
            _isSearching = true;
            _agent.isStopped = false;
            _agent.SetDestination(noisePos);
        }

        // When NPC reaches the noise location, begin searching
        if (!_agent.pathPending && _agent.remainingDistance <= 0.5f)
        {
            _agent.isStopped = true;
            _waitTimer -= Time.deltaTime;

            if (_waitTimer <= 0f)
            {
                // Done searching
                _isSearching = false;
                noisePos = Vector2.zero;
                _waitTimer = searchDuration;

                // Resume patrol
                GoToNextPatrolPoint();
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        _agent.isStopped = true;
        die?.Invoke();
    }

    public void Interact()
    {
        Die();
    }

    public bool CanInteract()
    {
        return isAlive;
    }
}
