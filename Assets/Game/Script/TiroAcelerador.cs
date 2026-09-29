using UnityEngine;
using TMPro; 
using System.Collections; 

public class TiroAcelerador : MonoBehaviour
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
            // Marca 1 ponto normal para o Jogador A
            manager.MarcarPontoA();
            scoreA.text = manager.scoreANum.ToString();

            // Efeito visual da explosão/impacto
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);

            // Acelera o Tanque B
            TankBehaviour tanqueAtingido = other.GetComponent<TankBehaviour>();
            if (tanqueAtingido != null)
            {
                tanqueAtingido.StartCoroutine(AcelerarTanque(tanqueAtingido));
            }

            // Oculta a bala em vez de destruir na hora (pois o tempo da aceleração está rodando nela)
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        // Se bater na parede/obstáculo (ignorando o próprio Tank A)
        else if (other.gameObject.name != "TankA") 
        {
            if (bulletFX != null) Instantiate(bulletFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator AcelerarTanque(TankBehaviour tanque)
    {
        // 1. Salva a velocidade normal do tanque para não perdê-la
        float velocidadeNormal = tanque.moveSpeed; 
        
        // 2. Dobra a velocidade do tanque (você pode alterar esse multiplicador se quiser mais rápido)
        tanque.moveSpeed = velocidadeNormal * 2f;  

        // 3. Espera 3 segundos (tempo que o inimigo ficará rápido)
        yield return new WaitForSeconds(3f);       

        // 4. Devolve o tanque para a velocidade normal original
        tanque.moveSpeed = velocidadeNormal;       
        
        // 5. Destrói a bala invisível que estava contando o tempo
        Destroy(gameObject);                       
    }
}