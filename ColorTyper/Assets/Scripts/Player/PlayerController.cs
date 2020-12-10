using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            GameOverEvent e = new GameOverEvent();
            _EventBus.Publish<GameOverEvent>(e);
        }
    }
}
