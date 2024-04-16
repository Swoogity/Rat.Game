 using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // ----------------------------------------------------------------VARIABLES---------------------------------------------------------------//
    // Keybinds
    public KeyCode MoveLeft;
    public KeyCode MoveRight;
    public KeyCode Jump;

    // horizontal movement
    public float speed = 1.0f;
    public float accelerationRate = 5.0f;
    public float maxSpeed = 50.0f;
    public bool topSpeed = false;

    // vertical movement
    public float jumpHeight = 10.0f;
    public float jumpChargeRate = 5.0f;
    public float maxHeight = 50.0f;
    public bool highJump = false;
    public float wallJumpHeight = 700.0f;

    // collision
    Rigidbody2D rBody;
    public bool isOnGround = true;
    public bool touchWall = false;
    public bool stuck = false;
    public float bounceHeight = 10.0f;

    // health
    public float health = 3;
    public GameObject health3;
    public GameObject health2;
    public GameObject health1;

    // -----------------------------------------------------------START---------------------------------------------------------//
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // -----------------------------------------------------------UPDATE---------------------------------------------------------//
    // Update is called once per frame
    void Update()
    {
        // ----------------------------------------WALKING-------------------------------------------//
        // walk right
        if (Input.GetKey(MoveRight))
        {
            rBody.AddForce(transform.right * speed, ForceMode2D.Force);
        }
        // walk left
        if (Input.GetKey(MoveLeft))
        {
            rBody.AddForce(transform.right * -speed, ForceMode2D.Force);
        }

        // walking right acceleration 
        if (Input.GetKey(MoveRight) && stuck == false)
        {
            // walking speed
            speed += accelerationRate * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
                topSpeed = true;
            }
            // jump height
            jumpHeight += jumpChargeRate * Time.deltaTime;
            if (jumpHeight > maxHeight) 
            { 
                jumpHeight = maxHeight;
                highJump = true;
            }
        }
        if (Input.GetKeyUp(MoveRight))
        {
            speed = 110.0f;
            jumpHeight = 400.0f;
            topSpeed = false;
            highJump = false;
        }

        // walking left acceleration 
        if (Input.GetKey(MoveLeft) && stuck == false)
        {
            // walking speed
            speed += accelerationRate * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
                topSpeed = true;
            }
            // jump height
            jumpHeight += jumpChargeRate * Time.deltaTime;
            if (jumpHeight > maxHeight)
            {
                jumpHeight = maxHeight;
                highJump = true;
            }
        }
        if (Input.GetKeyUp(MoveLeft))
        {
            speed = 110.0f;
            jumpHeight = 400.0f;
            topSpeed = false;
            highJump = true;
        }
        // if player gets stuck in speedtrap:
        if (stuck == true) 
        {
            topSpeed = false;
            highJump = false;
            speed = 110;
            jumpHeight = 400;
        }

        // ----------------------------------------JUMPING-------------------------------------------//
        // jump
        if (Input.GetKeyDown(Jump) && isOnGround && touchWall == false)
        {
            rBody.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            isOnGround = false;
        }
        // wall jump
        if (Input.GetKeyDown(Jump) && isOnGround == false && touchWall) 
        {
            rBody.AddForce(transform.up * wallJumpHeight, ForceMode2D.Impulse);
        }

        // ----------------------------------------HEALTH-------------------------------------------//
        if (health == 3) 
        {
            health3.SetActive(true);
            health2.SetActive(false);
            health1.SetActive(false);
        }
        if (health == 2)
        {
            health3.SetActive(false);
            health2.SetActive(true);
            health1.SetActive(false);
        }
        if (health == 1)
        {
            health3.SetActive(false);
            health2.SetActive(false);
            health1.SetActive(true);
        }
        if (health == 0) 
        {
            SceneManager.LoadScene("GameOver_1");
        }
    }
    // ----------------------------------------PLAYER ENVIROMENT COLLISION-------------------------------------------//
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // resets jump after landing
        if (collision2D.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            highJump = false;
        }
    }
    // wall jumping
    private void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("wall"))
        {
            touchWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("wall"))
        {
            touchWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // break through breakable walls if collide while at top speed
        if (other.CompareTag("Breakable") && topSpeed)
        {
            Destroy(other.gameObject);
        }

        // speed trap / sticky ground
        if (other.CompareTag("SpeedTrap")) 
        {
            stuck = true;
        }

        // damage trap
        if (other.CompareTag("DamageTrap")) 
        {
            health -= 1;
        }

            // if the player gets caught by the chasing thing
            if (other.CompareTag("Death")) 
        {
            SceneManager.LoadScene("GameOver_1");
        }

        // bounce pad
        if (other.CompareTag("BouncePad"))
        {
            rBody.AddForce(Vector3.up * bounceHeight, ForceMode2D.Impulse);
        }

        // win
        if (other.CompareTag("Win")) 
        {
            SceneManager.LoadScene("WinScreen_1");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // if player escapes speed trap
        if (other.CompareTag("SpeedTrap"))
        {
            stuck = false;
        }
    }
}
