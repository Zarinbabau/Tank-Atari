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
    public AudioClip somTiroNormal; // NOVO: Som do tiro normal

    [Header("Tiro Especial")]
    public GameObject specialBulletPrefab; 
    public float specialBulletSpeed = 15f; 
    public float specialFireCooldown = 5f; 
    public string botaoEspecial;           
    private float nextSpecialFireTime = 0f;
    public AudioClip somTiroEspecial; // NOVO: Som do tiro especial

    [Header("Controles")]
    public string eixo_horizontal;
    public string eixo_vertical;

    [Header("Áudio")]
    public AudioSource tankAudio; // NOVO: O "alto-falante" do tanque

    public bool estaCongelado = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (estaCongelado) return;

        // TIRO NORMAL
        if (Input.GetButtonDown(botaoAtirar) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            // Agora enviamos o som normal junto com o tiro
            Atirar(bulletPrefab, bulletSpeed, somTiroNormal); 
        }

        // TIRO ESPECIAL
        if (Input.GetButtonDown(botaoEspecial) && Time.time >= nextSpecialFireTime)
        {
            if (specialBulletPrefab != null)
            {
                nextSpecialFireTime = Time.time + specialFireCooldown;
                // Agora enviamos o som especial junto com o tiro
                Atirar(specialBulletPrefab, specialBulletSpeed, somTiroEspecial); 
            }
        }
    }

    // Função única para lidar com o disparo (Agora recebe também o som)
    void Atirar(GameObject prefabDaBala, float velocidadeBala, AudioClip somDoTiro)
    {
        GameObject bullet = Instantiate(prefabDaBala, spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = spawnPoint.right * velocidadeBala;
        }

        // NOVO: Toca o som do tiro se o alto-falante e o som estiverem configurados
        if (tankAudio != null && somDoTiro != null)
        {
            tankAudio.PlayOneShot(somDoTiro); // PlayOneShot permite tocar vários tiros sem um cortar o outro
        }
    }

    void FixedUpdate()
    {
        if (estaCongelado)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            rb.angularVelocity = Vector3.zero; 
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