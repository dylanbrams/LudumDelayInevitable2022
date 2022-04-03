using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BulldozerMover : MonoBehaviour
{
    [SerializeField] private float bulldozerMaxSpeed;
    [SerializeField] private float bulldozerInitialForce;
    [SerializeField] private float bulldozerForceGrowth;
    [SerializeField] private Vector2 bulldozerMoveDirection;
    [SerializeField] private TextMeshProUGUI bulldozerSpeedDisplay;
    [SerializeField] private TextMeshProUGUI bulldozerForceDisplay;
    [SerializeField] private TextMeshProUGUI houseLifespanDisplay;
    [SerializeField] private TextMeshProUGUI totalSecondsSurvived;
    [SerializeField] private TextMeshProUGUI houseMoveDistance;
    [SerializeField] private GameObject LoseObjects;
    [SerializeField] private HouseTracker[] houses;

    private Rigidbody2D myRigidBody2D;
    private float bulldozerForce;
    private Stopwatch myGameTime;
    void Start()
    {
        bulldozerForce = bulldozerInitialForce; 
        myRigidBody2D = this.GetComponent<Rigidbody2D>();
        myGameTime = new Stopwatch();
        Time.timeScale = 0;
    }
    void Update()
    {
        MoveBulldozerForward();
        DisplayBulldozerSpeed();
        RecenterBulldozer();
        CheckForLoss();
    }

    public void StartGame()
    {
        myGameTime.Start();
        Time.timeScale = 1;
    }
    private void RecenterBulldozer()
    {
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private void DisplayBulldozerSpeed()
    {
        bulldozerSpeedDisplay.text = myRigidBody2D.velocity.magnitude.ToString("F3");
        bulldozerForceDisplay.text = ((int)bulldozerForce).ToString();
        houseLifespanDisplay.text = myGameTime.Elapsed.ToString(@"mm\:ss\.fff");
    }
    private void MoveBulldozerForward()
    {
        Vector2 dozerMovement = (bulldozerMoveDirection * bulldozerForce) * Time.deltaTime;
        myRigidBody2D.AddForce(new Vector3(dozerMovement.x, dozerMovement.y, 0));
        if (myRigidBody2D.velocity.x > bulldozerMaxSpeed)
        {
            myRigidBody2D.AddForce(new Vector3( -dozerMovement.x / 2, -dozerMovement.y / 2, 0 ));
        }
        if (myRigidBody2D.velocity.y > 0)
        {
            myRigidBody2D.velocity = new Vector2(myRigidBody2D.velocity.x, 0);
        }
        IncreaseBulldozerForce();
    }
    private void IncreaseBulldozerForce()
    {
        bulldozerForce += bulldozerForceGrowth * Time.deltaTime * (10 ^ (1 + myGameTime.ElapsedMilliseconds / 2500)) / 10;
    }

    private void CheckForLoss()
    {
        if (transform.position.x >= 25.5f)
        {
            LoseGame();
        }
    }

    private void LoseGame()
    {
        LoseObjects.SetActive(true);
        houseMoveDistance.text = CalcHousesMoved().ToString();
        totalSecondsSurvived.text = houseLifespanDisplay.text;
        Time.timeScale = 0;
        myGameTime.Stop();
    }

    private int CalcHousesMoved()
    {
        float moveDistanceFloat = 0;
        foreach (HouseTracker houseTracker in houses)
        {
            moveDistanceFloat += houseTracker.houseMoveDistance();
        }
        return (int)moveDistanceFloat;
    } 
    
}
