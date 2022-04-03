using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private Transform obstaclesParent;
    private bool objectHeld = false;
    private GameObject currentHeldObject;
    private Collider2D myCollider;
    private LudumDelayInevitable2022 characterInput;
    private float heldObjectPreviousMass;
    private Collider2D heldObjectCollider;
    private AudioSource myAudioSource;
    private Rigidbody2D myRigidBody2D;
    


    [SerializeField] private float characterForce;
    [SerializeField] private float characterMaxSpeed;
    [SerializeField] private AudioClip liftClip;
    [SerializeField] private AudioClip dropClip;
    
    
    
    void Awake()
    {
        myRigidBody2D = this.GetComponent<Rigidbody2D>();
        myCollider = this.GetComponent<Collider2D>();
        myAudioSource = this.GetComponent<AudioSource>();
        characterInput = new LudumDelayInevitable2022();
        characterInput.Player.Enable();
        
    }

    void Update()
    {
        characterInput.Player.Fire.performed += Fire;
        characterInput.Player.Move.performed += Walk;
        if (objectHeld)
            currentHeldObject.transform.localPosition = new Vector2(.4f, 0);
    }
    

    void Fire(InputAction.CallbackContext ctx)
    {
        Debug.Log("PickedUp / Dropped Something.");
        activateSouthButton();
    }

    void Walk(InputAction.CallbackContext ctx)
    {
        Vector2 vectorIn = ctx.ReadValue<Vector2>().normalized;
        Vector2 characterMovement = ( characterMaxSpeed * vectorIn) * Time.deltaTime;
        if (characterMovement.magnitude > .002) 
            rotateCharacter(vectorIn);
        myRigidBody2D.velocity = (new Vector3(characterMovement.x, characterMovement.y, 0));
        Debug.Log(characterMovement);
        if (myRigidBody2D.velocity.magnitude > characterMaxSpeed)
        {
            myRigidBody2D.AddForce(new Vector3( -characterMovement.x / 2, -characterMovement.y / 2, 0 ));
        }
    }
    private void rotateCharacter(Vector2 vectorIn)
    {
        float angle = Mathf.Atan2(vectorIn.y, vectorIn.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }



    void activateSouthButton()
    {
        GameObject pickupTarget;
        if (!objectHeld)
        {
            pickupTarget = getFacingObject();
            if (pickupTarget != null)
            {
                Debug.Log( "Picking up extant object.");
                pickupObject(pickupTarget);
            }
        }
        else
        {
            dropObject();
        }
    }

    private GameObject getFacingObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 3); // Shoot raycast
        Debug.Log("Raycast right: " + transform.right);
        if (hit.collider) {
            //Debug.Log("Raycast hitted to: " + objectHit.collider);
            return (hit.collider.gameObject);
        }
        return null;
    }

    private void pickupObject(GameObject gameObjectIn)
    {
        Rigidbody2D currentHeldObjectRB = gameObjectIn.GetComponent<Rigidbody2D>();
        if (currentHeldObjectRB.mass < 7500)
        {
            heldObjectCollider = gameObjectIn.GetComponent<Collider2D>();
            heldObjectCollider.enabled = false;
            currentHeldObjectRB.velocity = Vector2.zero;
            gameObjectIn.transform.SetParent(transform);
            currentHeldObject = gameObjectIn;
            heldObjectPreviousMass = currentHeldObjectRB.mass;        
            currentHeldObjectRB.mass = .001f;
            Physics2D.IgnoreCollision(myCollider, heldObjectCollider);
            objectHeld = true;
            myAudioSource.PlayOneShot(liftClip);
        }
    }

    private void dropObject()
    {
        Rigidbody2D currentHeldObjectRB = currentHeldObject.GetComponent<Rigidbody2D>();
        currentHeldObjectRB.mass = heldObjectPreviousMass;
        currentHeldObjectRB.velocity = Vector2.zero;
        currentHeldObject.transform.parent = obstaclesParent;
        currentHeldObject.transform.position = transform.position;
        myAudioSource.PlayOneShot(dropClip);

        heldObjectCollider.enabled = true;
        objectHeld = false;
    }
}
