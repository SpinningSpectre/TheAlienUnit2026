using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MomController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;

    private InputAction _reset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _reset = InputSystem.actions.FindAction("Reset");
    }

    // Update is called once per frame
    private void Update()
    {
        var input = _moveInput;

        // Normalize only if input is stronger than 1
        if (input.magnitude > 1f)
            input = input.normalized;

        var currentSpeed = speed;

        _rb.linearVelocity = input * currentSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Reset(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
