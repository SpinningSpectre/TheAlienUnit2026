using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float blobSpeed;
    [SerializeField] private float interactDistance;
    [SerializeField] private float interactRadius;
    
    private Vector2 _moveInput;
    private bool _isBlobbing;
    private Rigidbody2D _rb;

    private InputAction _blob;
    private InputAction _reset;
    
    [Header("Unity Events")]
    public UnityEvent onBlobStart;
    public UnityEvent onBlobEnd;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       _rb = GetComponent<Rigidbody2D>(); 
       _blob = InputSystem.actions.FindAction("Blob");
       _reset = InputSystem.actions.FindAction("Reset");
    }

    // Update is called once per frame
    private void Update()
    {
        var input = _moveInput;

        // Normalize only if input is stronger than 1
        if (input.magnitude > 1f)
            input = input.normalized;
        
        var currentSpeed = _isBlobbing ? blobSpeed : speed;
        
        _rb.linearVelocity = input * currentSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Blob(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isBlobbing = true;
            onBlobStart?.Invoke();
        }
        
        if (context.canceled)
        {
            _isBlobbing = false;
            onBlobEnd?.Invoke();
        }
    }

    public void Reset(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Vector2 center = transform.position + transform.forward * interactDistance;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, interactRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 center = transform.position + transform.forward * interactDistance;

        Gizmos.DrawWireSphere(center, interactRadius);
    }

}
