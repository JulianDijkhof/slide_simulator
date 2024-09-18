using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //---------------------\\
    // Max play area limits
    // X limit 95-9
    // Z limit 35-75
    //---------------------\\
    public Vector2 currentPos;
    public float moveSpeed = 5f;
    private Camera mainCamera;
    private Rigidbody rb;
    private Vector3 vert;
    private Vector3 horz;

    public GameObject bullet;
    public Transform bulletShootPos;
    public int health = 3;

    public float cooldown = 1;
    public bool cdActive = false;
    public float lastShotTime = 0;

    private Collider playerCollider;
    public Animator animator;

    public GameManager manager;
    public bool shieldActive = false;

    // Basic setup
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.drag = 0;
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Rotate the player to face the cursor
        RotatePlayerToCursor();

        // Animation stuff, doens't work very well yet though.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        // Shoot if the gun isn't on cooldown and space is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > lastShotTime)
            {
                lastShotTime = Time.time + cooldown;
                if (bullet != null && bulletShootPos != null)
                {
                    GameObject newBullet = Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
                    Collider bulletCollider = newBullet.GetComponent<Collider>();
                    if (bulletCollider != null && playerCollider != null)
                    {
                        Physics.IgnoreCollision(bulletCollider, playerCollider);
                    }
                }
                else
                {
                    Debug.LogError("Bullet or bulletShootPos is not assigned.");
                }
            }

        }
    }

    void FixedUpdate()
    {
        // Setting play area limits
        if (gameObject.transform.position.x > 95)
        {
            gameObject.transform.position = new Vector3 (95, transform.position.y, transform.position.z);
        }
        if (gameObject.transform.position.x < 9)
        {
            gameObject.transform.position = new Vector3(9, transform.position.y, transform.position.z);
        }
        if (gameObject.transform.position.z < 35)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 35);
        }
        if (gameObject.transform.position.z > 75)
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 75);
        }
    }

    // Triggered by InputSystem
    public void OnMovement(InputValue value)
    {
        var v = value.Get<Vector2>();
        vert.x = v.x;
        horz.z = v.y; // <- Converting Vector2 values to Vector3 values, can't have the player start to fly now can we?
        MovePlayer();
    }

    // Keep looking at the cursor, don't you dare get distracted!
    void RotatePlayerToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = pointToLook - transform.position;
            direction.y = 0f; // Ensure the player stays upright, lest he might fall over and die...

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(targetRotation);
        }
    }

    // Take damage if hit without an active shield. Ouch!
    public void dealDmg()
    {
        if (!shieldActive)
        {
            health -= 1;
            if (health <= 0)
            {
                manager.EndGame();
                Destroy(gameObject);

            }
        }
        else
        {
            return;
        }
    }

    // Rapid fire time! BRRRRRRRR
    public void FireSpeedPu(float duration)
    {
        cooldown = 0.2f;
        StartCoroutine(DeactivateFireSpeedPU(duration));
    }

    // Fun is over, back to normal shooting speed.
    private IEnumerator DeactivateFireSpeedPU(float duration)
    {
        yield return new WaitForSeconds(duration);
        cooldown = 1f;
    }

    // Move your ass
    void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(-vert.x, 0, -horz.z).normalized;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
    }
}
