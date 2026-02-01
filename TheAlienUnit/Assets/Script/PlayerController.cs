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
    [SerializeField] private Animator _maskAnim;
    [SerializeField] private Animator _miscMaskAnim;
    private bool canMove = true;
    
    private InputAction _blob;
    private InputAction _reset;
    private InputAction _move;
    private InputAction _whistle;
    private InputAction _mom;
    private InputAction _maskState;
    
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

    [Header("Maskinator5948S")]
    [SerializeField] private GameObject maskBeam;
    public bool isUrMask = false;
    [SerializeField] private float maskSpeed = 6;
    [SerializeField] private AudioClip maskCallSound;
    [SerializeField] private LayerMask ignoreTheseYouDamnRay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
       _rb = GetComponent<Rigidbody2D>(); 
       _anim = GetComponent<Animator>();
       
       _blob = InputSystem.actions.FindAction("Blob");
       _reset = InputSystem.actions.FindAction("Reset");
       _whistle = InputSystem.actions.FindAction("Whistle");
       _mom = InputSystem.actions.FindAction("Mom");
       _maskState = InputSystem.actions.FindAction("MaskState");
    }

    // Update is called once per frame
    private void Update()
    {
        

         /*if(isUrMom && keyboard.numpadEnterKey.wasPressedThisFrame)
         {
             StartCoroutine(MomAttack());
         }

         if ((keyboard.numpadDivideKey.wasPressedThisFrame))
         {
             ChangeMaskState();
         }

         if (isUrMask && keyboard.numpad0Key.wasPressedThisFrame)
         {
             MaskAttack();
         }*/
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
        if (isUrMask)
        {
            if (currentMom.transform.position.x < startingPos.x - 7 ||
                currentMom.transform.position.x > startingPos.x + 7 ||
                currentMom.transform.position.y < startingPos.y - 7 ||
                currentMom.transform.position.y > startingPos.y + 7)
            {
                ChangeMaskState();
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
        if (context.canceled && !isUrMom && !isUrMask)
        {
            _anim.SetBool("isWalking", false);
            _anim.SetFloat("LastInputX", _moveInput.x);
            _anim.SetFloat("LastInputY", _moveInput.y);

            if (_maskAnim.transform.parent.gameObject.activeSelf)
            {
                _maskAnim.SetBool("isWalking", false);
                _maskAnim.SetFloat("LastInputX", _moveInput.x);
                _maskAnim.SetFloat("LastInputY", _moveInput.y);
            }

            if (_miscMaskAnim.transform.parent.gameObject.activeSelf)
            {
                _miscMaskAnim.SetBool("isWalking", false);
                _miscMaskAnim.SetFloat("LastInputX", _moveInput.x);
                _miscMaskAnim.SetFloat("LastInputY", _moveInput.y);
            }
        }
        _moveInput = context.ReadValue<Vector2>();
        if (context.canceled || isUrMom || isUrMask) return;
        _anim.SetBool("isWalking", true);
        _anim.SetFloat("InputX", _moveInput.x);
        _anim.SetFloat("InputY", _moveInput.y);

        if (_maskAnim.transform.parent.gameObject.activeSelf)
        {
            _maskAnim.SetBool("isWalking", true);
            _maskAnim.SetFloat("InputX", _moveInput.x);
            _maskAnim.SetFloat("InputY", _moveInput.y);
        }

        if (_miscMaskAnim.transform.parent.gameObject.activeSelf)
        {
            _miscMaskAnim.SetBool("isWalking", true);
            _miscMaskAnim.SetFloat("InputX", _moveInput.x);
            _miscMaskAnim.SetFloat("InputY", _moveInput.y);
        }

    }

    private void ChangeMomState()
    {
        if (hasMommed || switchCooldown || isUrMask) { return; }
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
        _rb.linearVelocity = Vector2.zero;

        _anim.SetBool("isWalking", false);
        _anim.SetFloat("LastInputX", _moveInput.x);
        _anim.SetFloat("LastInputY", _moveInput.y);

        if (_maskAnim.transform.parent.gameObject.activeSelf)
        {
            _maskAnim.SetBool("isWalking", false);
            _maskAnim.SetFloat("LastInputX", _moveInput.x);
            _maskAnim.SetFloat("LastInputY", _moveInput.y);
        }

        if (_miscMaskAnim.transform.parent.gameObject.activeSelf)
        {
            _miscMaskAnim.SetBool("isWalking", false);
            _miscMaskAnim.SetFloat("LastInputX", _moveInput.x);
            _miscMaskAnim.SetFloat("LastInputY", _moveInput.y);
        }

        currentMom = Instantiate(momBeam, transform.position, transform.rotation);
        startingPos = currentMom.transform.position;
        _rb = currentMom.GetComponent<Rigidbody2D>();
        cam.parent = currentMom.transform;
        cam.transform.position = currentMom.transform.position;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
        isUrMom = true;
        Soundsystem.PlaySound(momCallSound);
        _moveInput = Vector2.zero;
        StartCoroutine(MomSwitchCooldown(true));
    }

    public void ChangeMaskStateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeMaskState();
        }
    }
    public void ChangeMaskState()
    {
        if (switchCooldown || isUrMom) { return; }
        if (isUrMask)
        {
            _rb = GetComponent<Rigidbody2D>();
            isUrMask = false;
            cam.parent = transform;
            cam.transform.position = transform.position;
            cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10);
            Destroy(currentMom);
            StartCoroutine(MomSwitchCooldown());
            return;
        }
        _rb.linearVelocity = Vector2.zero;


        _anim.SetBool("isWalking", false);
        _anim.SetFloat("LastInputX", _moveInput.x);
        _anim.SetFloat("LastInputY", _moveInput.y);

        if (_maskAnim.transform.parent.gameObject.activeSelf)
        {
            _maskAnim.SetBool("isWalking", false);
            _maskAnim.SetFloat("LastInputX", _moveInput.x);
            _maskAnim.SetFloat("LastInputY", _moveInput.y);
        }

        if (_miscMaskAnim.transform.parent.gameObject.activeSelf)
        {
            _miscMaskAnim.SetBool("isWalking", false);
            _miscMaskAnim.SetFloat("LastInputX", _moveInput.x);
            _miscMaskAnim.SetFloat("LastInputY", _moveInput.y);
        }

        currentMom = Instantiate(maskBeam, transform.position, transform.rotation);
        startingPos = currentMom.transform.position;
        _rb = currentMom.GetComponent<Rigidbody2D>();
        isUrMask = true;
        _moveInput = Vector2.zero;
        StartCoroutine(MomSwitchCooldown());
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
            Destroy(colls[i].transform.parent.parent.gameObject);
        }
        canMove = false;
        yield return new WaitForSeconds(1.5f);

        DialogeScript.NextDial();
        canMove = true;
        ChangeMomState();
        hasMommed = true;
    }

    public void MaskAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MaskAttack();
        }
    }
    private void MaskAttack()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(currentMom.transform.position, currentMom.transform.localScale.x, enemyLayer);
        Soundsystem.PlaySound(maskCallSound, false, true, 0.4f);
        bool failure = false;
        for (int i = 0; i < colls.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (colls[i].transform.position - transform.position),7, ~ignoreTheseYouDamnRay);

            if(hit && hit.collider.gameObject == colls[i].gameObject)
            {
                colls[i].transform.parent.GetComponent<Npc>().Interact();
            }
            if (hit)
            {
                print(hit.collider.gameObject.name);
            }
            //else { failure = true; break; }
            //Destroy(colls[i].transform.parent.parent.gameObject);
        }
        if (failure) { return; }
        canMove = true;
        ChangeMaskState();
    }

    public void Blob(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isBlobbing = true;
            GetComponent<PlayerDetection>().DropMask();
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
            var interactable = col.GetComponentInParent<IInteractable>();
            interactable?.Interact();
        }
    }
    
    public void Whistle(InputAction.CallbackContext context)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, WhistleRadius);

        foreach (var col in colliders)
        {
            Debug.Log(col.name);

            var npc = col.GetComponentInParent<Npc>();
            if (npc != null)
            {
                npc.noisePos = transform.position;
            }
        }
    }
    
    public void Mom(InputAction.CallbackContext context)
    {
        if (context.performed)
            ChangeMomState();
        
    }
    public void MomFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            StartCoroutine(MomAttack());
    }
}
