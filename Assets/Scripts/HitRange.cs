using UnityEngine;
using System.Collections;

public class HitRange : MonoBehaviour
{
    private Control player;

    void Awake()
    {
        player = (Control)GetComponentInParent(typeof(Control));
    }

    void OnTriggerEnter(Collider other)
    {
        BallScript bs = (BallScript)other.GetComponent(typeof(BallScript));
        
        if(bs) //lol
        {
            player.currentBalls.Add(bs);
        }
    }

    void OnTriggerExit(Collider other)
    {
        BallScript bs = (BallScript)other.GetComponent(typeof(BallScript));

        if(bs)
        {
            player.currentBalls.Remove(bs);
        }
    }
}
