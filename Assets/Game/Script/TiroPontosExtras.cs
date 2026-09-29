using UnityEngine;
using TMPro; // Necessário para o texto do placar

public class TiroPontosExtras : MonoBehaviour
{
    public TextMeshProUGUI scoreA;
    public GameObject bulletFX;
    private GameManager manager;

    void Start()
    {
        // Busca as referências igual na bala normal
        scoreA = GameObject.Find("ScoreA").GetComponent<TextMeshProUGUI>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se acertou o Tank B (Inimigo)
        if (other.gameObject.name == "TankB")
        {
            // Dá 3 pontos para o Jogador A
            manager.MarcarPontoA();
            manager.MarcarPontoA();
            manager.MarcarPontoA();

            // Atualiza o texto do placar
            scoreA.text = manager.scoreANum.ToString();

            // Efeito visual e destrói
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // Se bater na parede/obstáculo (ignorando o próprio Tank A)
        else if (other.gameObject.name != "TankA") 
        {
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}