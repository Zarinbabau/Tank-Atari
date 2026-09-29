using UnityEngine;
using TMPro;

public class BulletBehaviour : MonoBehaviour
{
    // Variáveis do TextMeshPro
    public TextMeshProUGUI scoreA;
    public TextMeshProUGUI scoreB;

    // Prefab do efeito ao atingir algo
    public GameObject bulletFX;

    // NOVO: Guarda a referência do GameManager para não ter que procurar toda hora
    private GameManager manager;

    void Start()
    {
        scoreA = GameObject.Find("ScoreA").GetComponent<TextMeshProUGUI>();
        scoreB = GameObject.Find("ScoreB").GetComponent<TextMeshProUGUI>();
        
        // Busca o GameManager uma vez só no começo
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Se acertou o Tank A (Ponto para o Jogador B)
        if (other.gameObject.name == "TankA")
        {
            manager.MarcarPontoB(); // Chama APENAS a função do GameManager
            scoreB.text = manager.scoreBNum.ToString(); // Atualiza o texto
            
            Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // 2. Se acertou o Tank B (Ponto para o Jogador A)
        else if (other.gameObject.name == "TankB") // IMPORTANTE: Usar 'else if' aqui
        {
            manager.MarcarPontoA(); // Chama APENAS a função do GameManager
            scoreA.text = manager.scoreANum.ToString(); // Atualiza o texto
            
            Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // 3. Se não for nenhum dos tanques (acertou parede, obstáculo, etc)
        else
        {
            Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}