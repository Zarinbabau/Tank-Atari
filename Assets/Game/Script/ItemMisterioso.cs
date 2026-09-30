using UnityEngine;

public class ItemMisterioso : MonoBehaviour
{
    private GameManager manager;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem pegou foi um tanque
        if (other.gameObject.name == "TankA" || other.gameObject.name == "TankB")
        {
            // Avisa o GameManager para iniciar a sirene e o fim de jogo
            manager.DispararEventoFimDoMundo();

            // Destrói o item do mapa para ninguém pegar duas vezes
            Destroy(gameObject);
        }
    }
}