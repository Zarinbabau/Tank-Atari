using UnityEngine;

public class TankBehaviour : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    public Transform spawnPoint;     
    
    [Header("Tiro Normal")]
    public GameObject bulletPrefab;  
    public float bulletSpeed = 10f;
    public float fireCooldown = 1f; 
    public string botaoAtirar;
    private float nextFireTime = 0f;

    [Header("Tiro Especial")]
    public GameObject specialBulletPrefab; // NOVO: Prefab do tiro especial
    public float specialFireCooldown = 5f; // NOVO: Tempo de recarga maior
    public string botaoEspecial;           // NOVO: Botão para atirar o especial
    private float nextSpecialFireTime = 0f;

    [Header("Controles")]
    public string eixo_horizontal;
    public string eixo_vertical;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // TIRO NORMAL
        if (Input.GetButtonDown(botaoAtirar) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            Atirar(bulletPrefab, bulletSpeed);
        }

        // TIRO ESPECIAL (NOVO)
        if (Input.GetButtonDown(botaoEspecial) && Time.time >= nextSpecialFireTime)
        {
            // Verifica se o tanque tem um especial equipado (evita erros se não colocar nada no Inspector)
            if (specialBulletPrefab != null)
            {
                nextSpecialFireTime = Time.time + specialFireCooldown;
                Atirar(specialBulletPrefab, bulletSpeed); // Você pode até criar uma specialBulletSpeed se quiser
            }
        }
    }

    // Criei essa função para não repetir código de atirar duas vezes
    void Atirar(GameObject prefabDaBala, float velocidadeBala)
    {
        GameObject bullet = Instantiate(prefabDaBala, spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = spawnPoint.right * velocidadeBala;
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis(eixo_horizontal);
        float vertical = Input.GetAxis(eixo_vertical);

        Quaternion deltaRotation = Quaternion.Euler(0f, horizontal * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        Vector3 targetVelocity = transform.right * vertical * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}