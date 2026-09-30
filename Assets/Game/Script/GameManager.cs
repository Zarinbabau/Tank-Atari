using UnityEngine;
using TMPro;
using System.Collections; // Necessário para a Coroutine de tempo

public class GameManager : MonoBehaviour
{
    public GameObject scoreA;
    public GameObject scoreB;

    public GameObject logo;
    public GameObject texto; 

    public GameObject playerA;
    public GameObject playerB;

    public int scoreANum = 0;
    public int scoreBNum = 0;

    public float tempoDeJogo = 90f; 
    private bool jogoRodando = false; 

    public TextMeshProUGUI timerText; 
    public GameObject TextTimer;

    public GameObject telaVitoriaA;
    public GameObject telaVitoriaB;
    public GameObject telaEmpate;
    
    private Vector3 posicaoInicialA;
    private Quaternion rotacaoInicialA;
    private Vector3 posicaoInicialB;
    private Quaternion rotacaoInicialB;

    [Header("Evento Especial (Fim do Mundo)")]
    public GameObject itemEspecialPrefab;    // O Prefab do item
    public Transform[] pontosDeSpawnItem;    // Lugares onde ele pode nascer
    [Range(0f, 100f)] public float chanceDeSpawn = 5f; // 5% de chance de aparecer
    public Light luzDoMundo;                 // A Directional Light do seu jogo
    public AudioClip somSirene;              // O som da sirene
    public AudioSource gameManagerAudio;     // O AudioSource que vai tocar a sirene
    public GameObject telaFimDoMundo;        // A nova tela final animada

    private float intensidadeLuzInicial;
    private GameObject itemSpawnadoAtual;    // Guarda o item caso precise deletar ao reiniciar

    void Start()
    {
        posicaoInicialA = playerA.transform.position;
        rotacaoInicialA = playerA.transform.rotation;

        posicaoInicialB = playerB.transform.position;
        rotacaoInicialB = playerB.transform.rotation;

        // Salva como a luz do seu jogo é normalmente para poder voltar ao normal depois
        if (luzDoMundo != null)
        {
            intensidadeLuzInicial = luzDoMundo.intensity;
        }
    }

    public void TerminaJogo()
    {
        jogoRodando = false; 
        playerA.GetComponent<TankBehaviour>().enabled = false;
        playerB.GetComponent<TankBehaviour>().enabled = false;

        AtualizarTextoCronometro(0f);
        TextTimer.SetActive(false);
        scoreA.SetActive(false);
        scoreB.SetActive(false);

        if (scoreANum > scoreBNum) telaVitoriaA.SetActive(true);
        else if (scoreBNum > scoreANum) telaVitoriaB.SetActive(true);
        else telaEmpate.SetActive(true);
        
        texto.SetActive(true); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !jogoRodando)
        {
            IniciarPartida();
        }

        if (jogoRodando)
        {
            tempoDeJogo -= Time.deltaTime;
            AtualizarTextoCronometro(tempoDeJogo);

            if (tempoDeJogo <= 0f)
            {
                tempoDeJogo = 0f; 
                TerminaJogo();
            }
        }
    }

    public void MarcarPontoA() { if (jogoRodando) scoreANum++; }
    public void MarcarPontoB() { if (jogoRodando) scoreBNum++; }

    public void IniciarPartida()
    {
        jogoRodando = true;
        tempoDeJogo = 90f; 
        scoreANum = 0;      
        scoreBNum = 0;

        if(telaVitoriaA != null) telaVitoriaA.SetActive(false);
        if(telaVitoriaB != null) telaVitoriaB.SetActive(false);
        if(telaEmpate != null) telaEmpate.SetActive(false);
        if(telaFimDoMundo != null) telaFimDoMundo.SetActive(false); // Esconde a tela nova

        // Reseta a luz do mundo
        if (luzDoMundo != null) luzDoMundo.intensity = intensidadeLuzInicial;

        // Limpa o item antigo se ainda estiver no mapa
        if (itemSpawnadoAtual != null) Destroy(itemSpawnadoAtual);

        // LÓGICA DE SPAWN DO ITEM (5% de chance)
        if (itemEspecialPrefab != null && pontosDeSpawnItem.Length > 0)
        {
            float sorteio = Random.Range(0f, 100f);
            if (sorteio <= chanceDeSpawn)
            {
                // Escolhe um ponto aleatório da lista e spawna o item lá
                int indexSpawn = Random.Range(0, pontosDeSpawnItem.Length);
                itemSpawnadoAtual = Instantiate(itemEspecialPrefab, pontosDeSpawnItem[indexSpawn].position, Quaternion.identity);
            }
        }

        scoreA.SetActive(true);
        scoreB.SetActive(true);
        scoreA.GetComponent<TextMeshProUGUI>().text = "0";
        scoreB.GetComponent<TextMeshProUGUI>().text = "0";

        logo.SetActive(false);
        texto.SetActive(false);

        playerA.transform.position = posicaoInicialA;
        playerA.transform.rotation = rotacaoInicialA;
        playerB.transform.position = posicaoInicialB;
        playerB.transform.rotation = rotacaoInicialB;

        playerA.GetComponent<TankBehaviour>().enabled = true; 
        playerB.GetComponent<TankBehaviour>().enabled = true;

        TextTimer.SetActive(true);
    }

    private void AtualizarTextoCronometro(float tempoRestante)
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tempoRestante % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // --- NOVA FUNÇÃO CHAMADA PELO ITEM ---
    public void DispararEventoFimDoMundo()
    {
        // Começa a sequência cinemática
        StartCoroutine(RotinaFimDoMundo());
    }

    private IEnumerator RotinaFimDoMundo()
    {
        // Trava o jogo e os tanques (ninguém se move mais, o tempo para)
        jogoRodando = false; 
        playerA.GetComponent<TankBehaviour>().enabled = false;
        playerB.GetComponent<TankBehaviour>().enabled = false;

        // 1. Espera 3 segundos em silêncio absoluto / suspense
         

        // 2. Toca a sirene
        if (gameManagerAudio != null && somSirene != null)
        {
            gameManagerAudio.PlayOneShot(somSirene);
        }
        
        yield return new WaitForSeconds(3f);
        
        // 3. Aumenta a luz suavemente até "estourar" a tela de branco
        if (luzDoMundo != null)
        {
            float tempoClareando = 0f;
            float duracaoDoBrilho = 10f; // Leva 10 segundos para a luz estourar
            
            while (tempoClareando < duracaoDoBrilho)
            {
                tempoClareando += Time.deltaTime;
                // Transforma a intensidade da luz de normal para 700 (Muito forte)
                luzDoMundo.intensity = Mathf.Lerp(intensidadeLuzInicial, 700f, tempoClareando / duracaoDoBrilho);
                yield return null; // Espera o próximo frame
            }
        }

        // 4. Esconde a interface normal e mostra a tela final
        TextTimer.SetActive(false);
        scoreA.SetActive(false);
        scoreB.SetActive(false);
        
        if (telaFimDoMundo != null) telaFimDoMundo.SetActive(true);
        texto.SetActive(true); // Mostra o texto "Aperte X para reiniciar"
    }
}