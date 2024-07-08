using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;
    private Rigidbody rb;

    public GameObject bullet;
    public Transform bulletShootPos;
    public int health = 3;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.drag = 0;  // Ensure no drag affects the player's stopping
    }

    void Update()
    {
        // Rotate the player to face the cursor
        RotatePlayerToCursor();
    }

    void FixedUpdate()
    {
        // Move the player
        MovePlayer();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;
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

    void Shoot()
    {
        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
    }

    public void dealDmg()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
