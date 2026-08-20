using UnityEngine;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    public Transform spawnPoint;     
    public GameObject bulletPrefab;  
    public float bulletSpeed = 10f;

    private Rigidbody rb;

    public float fireCooldown = 1f; 
    private float nextFireTime = 0f;

    public string botaoAtirar;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Trava TODAS as rotações causadas por colisão/física.
        // O script (rb.MoveRotation) continuará rotacionando normalmente.
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (Input.GetButtonDown(botaoAtirar) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;

            GameObject bullet = Instantiate(
                bulletPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = spawnPoint.right * bulletSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        // Rotação controlada exclusivamente pelo jogador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Quaternion deltaRotation = Quaternion.Euler(
            0f,
            horizontal * rotationSpeed * Time.fixedDeltaTime,
            0f
        );

        rb.MoveRotation(rb.rotation * deltaRotation);

        // Movimento (preserva a velocidade Y para gravidade)
        Vector3 targetVelocity = transform.right * vertical * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}