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
    public GameObject specialBulletPrefab; 
    public float specialBulletSpeed = 15f; 
    public float specialFireCooldown = 5f; 
    public string botaoEspecial;           
    private float nextSpecialFireTime = 0f;

    [Header("Controles")]
    public string eixo_horizontal;
    public string eixo_vertical;

    // NOVO: Variável pública que a bala especial (TiroCongelante) vai acessar
    public bool estaCongelado = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // NOVO: Se o tanque estiver congelado, ele ignora o resto do código (não atira)
        if (estaCongelado) return;

        // TIRO NORMAL
        if (Input.GetButtonDown(botaoAtirar) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            Atirar(bulletPrefab, bulletSpeed); 
        }

        // TIRO ESPECIAL
        if (Input.GetButtonDown(botaoEspecial) && Time.time >= nextSpecialFireTime)
        {
            if (specialBulletPrefab != null)
            {
                nextSpecialFireTime = Time.time + specialFireCooldown;
                Atirar(specialBulletPrefab, specialBulletSpeed); 
            }
        }
    }

    // Função única para lidar com o disparo
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
        // NOVO: Se o tanque estiver congelado, ele para de se mover e de rotacionar
        if (estaCongelado)
        {
            // Zera a velocidade de andar (mas preserva o Y para ele não flutuar caso caia)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            
            // Zera qualquer força de rotação residual
            rb.angularVelocity = Vector3.zero; 
            
            // O 'return' faz o código parar aqui, impedindo que ele leia os controles abaixo
            return; 
        }

        float horizontal = Input.GetAxis(eixo_horizontal);
        float vertical = Input.GetAxis(eixo_vertical);

        Quaternion deltaRotation = Quaternion.Euler(0f, horizontal * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);

        Vector3 targetVelocity = transform.right * vertical * moveSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}