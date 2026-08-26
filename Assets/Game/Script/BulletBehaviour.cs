using UnityEngine;
using TMPro;

public class BulletBehaviour : MonoBehaviour
{
    // Variáveis do TextMeshPro
    public TextMeshProUGUI scoreA;
    public TextMeshProUGUI scoreB;

    // Prefab do efeito ao atingir algo
    public GameObject bulletFX;

    // O método Start precisa do modificador de acesso e da maiúscula inicial
    void Start()
    {
        // Corrigida a sintaxe do tipo TextMeshProUGUI no GetComponent
        scoreA = GameObject.Find("ScoreA").GetComponent<TextMeshProUGUI>();
        scoreB = GameObject.Find("ScoreB").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TankA")
        {
            // Busca o objeto GameManager e incrementa o ponto
            GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            manager.scoreBNum++;

            // Atualiza o texto do placar
            scoreB.text = manager.scoreBNum.ToString();

            // Instancia o efeito e destrói a bala
            Instantiate(bulletFX, transform.position, Quaternion.identity);

            GameObject.Find("GameManager").GetComponent<GameManager>().termina();

            Destroy(gameObject);
        }
        else
        {
            // Instancia o efeito na posição da bala se atingir outro objeto
            Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.gameObject.name == "TankB")
        {
            // Busca o objeto GameManager e incrementa o ponto
            GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            manager.scoreANum++;

            // Atualiza o texto do placar
            scoreA.text = manager.scoreANum.ToString();

            // Instancia o efeito e destrói a bala
            Instantiate(bulletFX, transform.position, Quaternion.identity);
 GameObject.Find("GameManager").GetComponent<GameManager>().termina();

            Destroy(gameObject);
        }
        else
        {
            // Instancia o efeito na posição da bala se atingir outro objeto
            Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}