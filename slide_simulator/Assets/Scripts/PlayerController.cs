using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // X limit 95-9
    // Z limit 35-75
    public float moveSpeed = 5f;
    private Camera mainCamera;
    private Rigidbody rb;

    public GameObject bullet;
    public Transform bulletShootPos;
    public int health = 3;

    public float cooldown = 1;
    public float lastShotTime = 0;

    private Collider playerCollider;
    public Animator animator;

    public GameManager manager;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.drag = 0;  // Ensure no drag affects the player's stopping
        playerCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Rotate the player to face the cursor
        RotatePlayerToCursor();

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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }

    void FixedUpdate()
    {
        // Move the player
        MovePlayer();

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

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(-moveX, 0, -moveY).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    void RotatePlayerToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = pointToLook - transform.position;
            direction.y = 0f; // Ensure the player stays upright

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(targetRotation);
        }
    }

    public void dealDmg()
    {
        health -= 1;
        if (health <= 0)
        {
            manager.EndGame();
            Destroy(gameObject);

        }
    }

    public void FireSpeedPu(float duration)
    {
        cooldown = 0.2f;
        StartCoroutine(DeactivateFireSpeedPU(duration));
    }

    private IEnumerator DeactivateFireSpeedPU(float duration)
    {
        yield return new WaitForSeconds(duration);
        cooldown = 1f;
    }
}
