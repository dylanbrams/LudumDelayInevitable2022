using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] private BulldozerMover bulldozerMover;

    public void startGame()
    { 
        bulldozerMover.StartGame();
        this.gameObject.SetActive(false);
    }
}
