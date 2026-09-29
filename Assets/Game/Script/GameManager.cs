using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject scoreA;
    public GameObject scoreB;

    public GameObject logo;
    public GameObject texto; // Esse é o seu texto "Aperte Start", talvez você queira mudar na UI para "Aperte X"

    public GameObject playerA;
    public GameObject playerB;

    public int scoreANum = 0;
    public int scoreBNum = 0;

    public float tempoDeJogo = 180f; 
    private bool jogoRodando = false; 

    public TextMeshProUGUI timerText; 
    public GameObject TextTimer;

    public GameObject telaVitoriaA;
    public GameObject telaVitoriaB;
    public GameObject telaEmpate;
    
    // NOVAS VARIÁVEIS: Para guardar onde os tanques nascem
    private Vector3 posicaoInicialA;
    private Quaternion rotacaoInicialA;
    private Vector3 posicaoInicialB;
    private Quaternion rotacaoInicialB;

    // NOVO: O método Start roda uma única vez quando o jogo abre
    void Start()
    {
        // Salva a posição e rotação exata de cada tanque no começo do jogo
        posicaoInicialA = playerA.transform.position;
        rotacaoInicialA = playerA.transform.rotation;

        posicaoInicialB = playerB.transform.position;
        rotacaoInicialB = playerB.transform.rotation;
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

        if (scoreANum > scoreBNum)
        {
            telaVitoriaA.SetActive(true);
        }
        else if (scoreBNum > scoreANum)
        {
            telaVitoriaB.SetActive(true);
        }
        else
        {
            telaEmpate.SetActive(true);
        }
        
        texto.SetActive(true); 
    }

    void Update()
    {
        // SUBSTITUÍDO: Agora verifica se a tecla X foi pressionada e se o jogo não está rodando
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

    public void MarcarPontoA()
    {
        if (jogoRodando)
        {
            scoreANum++;
            // Atualize o texto de pontuação aqui se não estiver atualizando no tiro
        }
    }

    public void MarcarPontoB()
    {
        if (jogoRodando)
        {
            scoreBNum++;
            // Atualize o texto de pontuação aqui se não estiver atualizando no tiro
        }
    }

    public void IniciarPartida()
    {
        jogoRodando = true;
        tempoDeJogo = 90f; 
        scoreANum = 0;      
        scoreBNum = 0;

        if(telaVitoriaA != null) telaVitoriaA.SetActive(false);
        if(telaVitoriaB != null) telaVitoriaB.SetActive(false);
        if(telaEmpate != null) telaEmpate.SetActive(false);

        scoreA.SetActive(true);
        scoreB.SetActive(true);

        // Zera visualmente os placares quando reinicia
        scoreA.GetComponent<TextMeshProUGUI>().text = "0";
        scoreB.GetComponent<TextMeshProUGUI>().text = "0";

        logo.SetActive(false);
        texto.SetActive(false);

        // NOVO: Devolve os tanques para os lugares iniciais
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
}