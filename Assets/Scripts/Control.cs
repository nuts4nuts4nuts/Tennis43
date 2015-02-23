using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour
{
    //Movement
    public float maxSpeed = 10.0f;
    public AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);

    //Visuals
    public Image associatedTri;
    private float boostJuice = 1.0f;
    private float boostJuiceBonus = 0.3f;

    //Hitting
    public float baseCharge = 1.0f;
    private float currentCharge = 1.0f;
    private SphereCollider hitRange;
    private float hitCoefficient = 200;

    [HideInInspector]
    public List<BallScript> currentBalls;

    void Awake()
    {
        hitRange = (SphereCollider)GetComponent(typeof(SphereCollider));
    }
	
	// Update is called once per frame
	void Update ()
    {
        boostJuice += boostJuiceBonus;

        float movementX = Input.GetAxis("Horizontal");
        float movementZ = Input.GetAxis("Vertical");
        float triggerJuice = -Input.GetAxis("Trigger");

        triggerJuice += 1.2f; //Buffer

        float modulatedTriggerJuice = (triggerJuice + 8.75f) * 0.03f;

        if (boostJuice < modulatedTriggerJuice)
        {
            triggerJuice = 1;
            boostJuice -= modulatedTriggerJuice;
        }
        else
        {
            boostJuice -= modulatedTriggerJuice;
        }

        boostJuice = Mathf.Clamp(boostJuice, 0, 1.0f);
        associatedTri.fillAmount = boostJuice;

        float curvedMovementX = movementCurve.Evaluate(movementX);
        float curvedMovementZ = movementCurve.Evaluate(movementZ);

        Vector3 newVelocity = rigidbody.velocity;
        newVelocity.x = curvedMovementX * maxSpeed * (triggerJuice * 3) * Time.deltaTime;
        newVelocity.z = curvedMovementZ * maxSpeed * (triggerJuice * 3) * Time.deltaTime;
        rigidbody.velocity = newVelocity;

        { //Hitting the ball
            if (Input.GetButton("Hit"))
            {
                currentCharge += 0.5f * Time.deltaTime;
            }

            if (Input.GetButtonUp("Hit"))
            {
                //Hit the ball
                if(currentBalls.Count > 0)
                {
                    BallScript currentBall = currentBalls[0];
                    GameObject ball = currentBall.gameObject;
                    ball.transform.forward = gameObject.transform.forward;

                    Vector3 newVelo = new Vector3(movementX, 0, 0);

                    newVelo.z += currentCharge * 200 * Time.deltaTime;
                    newVelo.y += currentCharge * 200 * Time.deltaTime;

                    ball.rigidbody.velocity = newVelo;
                }

                currentCharge = baseCharge;
            }
        }
    }
}
