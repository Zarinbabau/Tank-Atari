using UnityEngine;
using TMPro; // Necessário para o texto do placar
using System.Collections; // Necessário para o tempo do gelo

public class TiroCongelante : MonoBehaviour
{
    public TextMeshProUGUI scoreB;
    public GameObject bulletFX;
    private GameManager manager;

    void Start()
    {
        // Busca as referências igual na bala normal
        scoreB = GameObject.Find("ScoreB").GetComponent<TextMeshProUGUI>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se acertou o Tank A (Inimigo)
        if (other.gameObject.name == "TankA")
        {
            // Marca o ponto para o Jogador B
            manager.MarcarPontoB();
            scoreB.text = manager.scoreBNum.ToString();

            // Efeito visual
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);

            // Congela o Tanque A
            TankBehaviour tanqueAtingido = other.GetComponent<TankBehaviour>();
            if (tanqueAtingido != null)
            {
                tanqueAtingido.StartCoroutine(CongelarTanque(tanqueAtingido));
            }

            // Oculta a bala em vez de destruir na hora (pois o tempo do gelo está rodando nela)
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        // Se bater na parede/obstáculo (ignorando o próprio Tank B)
        else if (other.gameObject.name != "TankB") 
        {
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator CongelarTanque(TankBehaviour tanque)
    {
        tanque.estaCongelado = true;         // Trava o tanque
        yield return new WaitForSeconds(2f); // Espera 2 segundos (você pode mudar aqui)
        tanque.estaCongelado = false;        // Solta o tanque
        Destroy(gameObject);                 // Destrói a bala
    }
}