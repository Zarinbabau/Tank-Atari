using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    
    // Prefab do efeito ao atingir algo
    public GameObject bulletFX;

    private void OnTriggerEnter(Collider other)
    {
        // Instancia o efeito na posição da bala
        Instantiate(bulletFX, transform.position, Quaternion.identity);

        // Destrói a bala
        Destroy(gameObject);
    }
}