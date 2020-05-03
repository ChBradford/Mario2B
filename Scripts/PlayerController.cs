using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //physics stuff
    float speed = 6f;
    float verticalSpeed;
    float jumpSpeed = 15;
    float gravity = 16;
    public int direction;
    Vector3 movement_direction = Vector3.zero;
    public Collider other;

    float t;

    CharacterController controller;

    Animator animate;

    bool Jumping = true; //For stopping jump animation when landing
    bool death = false;  //For when player dies
    bool win = false;    //For player win (Reaches pill)

    public int score = 0;
    public int coins = 0;
    public Text score_text;
    public Text coins_text;
    public Text timer;
    public Text WinText;
    float y;
    public GameObject Water2;
    public Transform reference;

    //For coin/score
    string scoretemp;
    string coinscoretemp; 

    void Start()
    {
        scoretemp = string.Format("{0:000000}", score);
        score_text.text = scoretemp.ToString();
        coinscoretemp = string.Format("{0:00}", coins);
        coins_text.text = coinscoretemp.ToString();
        controller = GetComponent<CharacterController>();
        animate = GetComponent<Animator>();
        direction = 1;

        
    }

    void Update()
    {
        
        //timer counting down
        if (!death)
        {
            t = 100f - Time.time;
        }
        
        
        //if the timer reaches zero, you lose
        if(t <= 0 && !death)
        {
            
            t = 1;
            death = true;
            score_text.text = "Player 1\nStatus: Time's up";
            Time.timeScale = 0f; 
        }

        //update the text
        if (!death && !win)
        {
            scoretemp = string.Format("{0:000000}", score);
            score_text.text = "Player 1\n" + scoretemp.ToString();
            coinscoretemp = string.Format("{0:00}", coins);
            coins_text.text = "x" + coinscoretemp.ToString();
            timer.text = "Time \n" + t.ToString("f0");
        }

        

        //if character is on the ground
        if (controller.isGrounded)
        {
            //when Ethan lands, stop him
            if (Jumping)
            {
                Jumping = false;
                movement_direction = new Vector3(0, 0, 0);
                animate.SetInteger("isWalking", 0);
            }
            animate.SetInteger("inAir", 0);
            //movement to the right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if(direction == 1)
                {
                    animate.SetInteger("isWalking", 1);
                    movement_direction = new Vector3(1, 0, 0);
                    movement_direction *= speed;
                }
                //Rotation based on direction
                else
                {
                    direction = 1;
                    transform.Rotate(0, 180, 0);
                }
                
                
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                animate.SetInteger("isWalking", 0);
                movement_direction = new Vector3(0, 0, 0);
            }
            //movement to the left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if(direction == 0)
                {
                    animate.SetInteger("isWalking", 1);
                    movement_direction = new Vector3(-1, 0, 0);
                    movement_direction *= speed;
                }
                else
                {
                    direction = 0;
                    transform.Rotate(0, 180, 0);
                }
                
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                animate.SetInteger("isWalking", 0);
                movement_direction = new Vector3(0, 0, 0);
            }

            //Jump
            if (Input.GetKey(KeyCode.Space))
            {
                animate.SetInteger("isRunning", 0);
                speed = 6f;
                animate.SetInteger("isWalking", 0);
                movement_direction = new Vector3(movement_direction.x, jumpSpeed, 0);
                

            }
            if (Input.GetKey(KeyCode.Space))
            {
                animate.SetInteger("isWalking", 0);
                movement_direction = new Vector3(movement_direction.x, jumpSpeed, 0);

            }

            //Sprint
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                animate.SetInteger("isRunning", 1);
                speed = 12f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                animate.SetInteger("isRunning", 0);
                speed = 6f;
            }
        }

        //in the air
        else
        {
            Jumping = true;
            animate.SetInteger("inAir", 1);
        }


        //Gravity applied & allows movement. Freezes Ethan upon death, not allowing the player to continue
        if (!death)
        {
            movement_direction.y -= gravity * Time.deltaTime;
            controller.Move(movement_direction * Time.deltaTime);
            y = transform.position.y;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //bricks will 'break' when just walking over them or sprinting into them... it works well enough though!
        if(collision.collider.name == "Brick(Clone)" || collision.collider.name == "Brick")
        {
            score += 100;
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.name == "Water gravity(Clone)" || collision.collider.name == "Water gravity")
        {
            score_text.text = "Player 1\nStatus: drowned";
            WinText.text = "Ethan can't swim! D:";
            WinText.color = Color.blue;
            death = true;
            Time.timeScale = 0f;
            Debug.Log("Ded");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Win Condition
        if(other.name == "Pill(Clone)" || other.name == "Pill")
        {
            if (!win)
            {
                win = true;
                Debug.Log("Win");
                WinText.text = "You win!!! :D";
                WinText.color = Color.green;
                scoretemp = string.Format("{0:000000}", score);
                score_text.text = "Player 1\n" + scoretemp.ToString() + "\nStatus: Victory!";
            }
            
        }
        //Coin pickup
        if(other.name == "Coin(Clone)" || other.name == "Coin")
        {
            coins = coins + 1;
            score = score + 100;
        }
        //Die by Water
        if(other.name == "Water(Clone)" || other.name == "Water")
        {
            score_text.text = "Player 1\nStatus: drowned";
            WinText.text = "Ethan can't swim! D:";
            WinText.color = Color.blue;
            death = true;
            Time.timeScale = 0f;
            Debug.Log("Ded");
        }

    }
}
