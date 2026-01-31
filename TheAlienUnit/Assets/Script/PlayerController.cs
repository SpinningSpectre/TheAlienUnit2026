using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float WhistleRadius;
    
    private Vector2 _moveInput;
    private bool _isBlobbing;
    public Rigidbody2D _rb;
    private Animator _anim;
    private bool canMove = true;
    
    private InputAction _blob;
    private InputAction _reset;
    private InputAction _move;
    private InputAction _whistle;
    
    [Header("Unity Events")]
    public UnityEvent onBlobStart;
    public UnityEvent onBlobEnd;

    [Header("Mom")]
    private bool hasMommed = false;
    [SerializeField] private GameObject momBeam;
    private GameObject currentMom;
    public bool isUrMom = false;
    [SerializeField] private float momSpeed = 6;
    public Vector2 startingPos;
    [SerializeField] private int areaGoFar = 10;
    [SerializeField] private Transform cam;
    [SerializeField] private Sprite tempMomBeam;
    [SerializeField] private LayerMask enemyLayer;
    private bool switchCooldown = false;

    [SerializeField] private List<dialoge> momCallDialoge = new List<dialoge>();
    [SerializeField] private AudioClip momCallSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       _rb = GetComponent<Rigidbody2D>(); 
       _anim = GetComponent<Animator>();
       
       _blob = InputSystem.actions.FindAction("Blob");
       _reset = InputSystem.actions.FindAction("Reset");
       _whistle = InputSystem.actions.FindAction("Whistle");
    }

    // Update is called once per frame
    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if ((keyboard.numpadMultiplyKey.wasPressedThisFrame))
        {
            ChangeMomState();
        }

        if(isUrMom && keyboard.numpadEnterKey.wasPressedThisFrame)
        {
            StartCoroutine(MomAttack());
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isUrMom)
        {
            if (currentMom.transform.position.x < startingPos.x - areaGoFar ||
                currentMom.transform.position.x > startingPos.x + areaGoFar ||
                currentMom.transform.position.y < startingPos.y - areaGoFar ||
                currentMom.transform.position.y > startingPos.y + areaGoFar)
            {
                ChangeMomState();
            }
        }
        var input = _moveInput;

        // Normalize only if input is stronger than 1
        if (input.magnitude > 1f)
            input = input.normalized;
        
        float currentSpeed = _isBlobbing ? blobSpeed : speed;
        currentSpeed = isUrMom ? momSpeed : currentSpeed;
        
        _rb.linearVelocity = input * currentSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _anim.SetBool("isWalking", true);
        

        if (context.canceled)
        {
            _anim.SetBool("isWalking", false);
            _anim.SetFloat("LastInputX", _moveInput.x);
            _anim.SetFloat("LastInputY", _moveInput.y);
        }
        
        _anim.SetFloat("InputX", _moveInput.x);
        _anim.SetFloat("InputY", _moveInput.y);
        
    }

    private void ChangeMomState()
    {
        if (hasMommed || switchCooldown) { return; }
        if (isUrMom)
        {
            _rb = GetComponent<Rigidbody2D>();
            isUrMom = false;
            cam.parent = transform;
            cam.transform.position = transform.position;
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
            Destroy(currentMom);
            StartCoroutine(MomSwitchCooldown());
            DialogeScript.NextDial();
            DialogeScript.NextDial();
            DialogeScript.NextDial();
            DialogeScript.NextDial();

            return;
        }
        currentMom = Instantiate(momBeam, transform.position, transform.rotation);
        startingPos = currentMom.transform.position;
        _rb = currentMom.GetComponent<Rigidbody2D>();
        cam.parent = currentMom.transform;
        cam.transform.position = currentMom.transform.position;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
        isUrMom = true;
        Soundsystem.PlaySound(momCallSound);
        StartCoroutine(MomSwitchCooldown(true));
    }

    private IEnumerator MomSwitchCooldown(bool call = false)
    {
        switchCooldown = true;
        yield return new WaitForSeconds(0.5f);
        if (call)
        {
            DialogeScript.StartDialoge(momCallDialoge);
        }
        switchCooldown = false;
    }

    private IEnumerator MomAttack()
    {
        DialogeScript.NextDial();
        currentMom.GetComponent<SpriteRenderer>().sprite = tempMomBeam;
        Collider2D[] colls = Physics2D.OverlapCircleAll(currentMom.transform.position, currentMom.transform.localScale.x, enemyLayer);
        for (int i = 0; i < colls.Length; i++)
        {
            Destroy(colls[i].gameObject);
        }
        canMove = false;
        yield return new WaitForSeconds(1.5f);

        DialogeScript.NextDial();
        canMove = true;
        ChangeMomState();
        hasMommed = true;
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
    
    public void Whistle(InputAction.CallbackContext context)
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, WhistleRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out Npc npc))
            {
                npc.noisePos = transform.position;
                Debug.Log(npc.noisePos);
            }
        }
    }
}
