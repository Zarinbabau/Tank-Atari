using UnityEngine;
using TMPro; // Corrigido de tmpPro para TMPro

public class GameManager : MonoBehaviour
{
    // Tipos de dados corrigidos de gameobjet/gameobjetc/gamect para GameObject
    public GameObject scoreA;
    public GameObject scoreB;

    public GameObject logo;
    public GameObject texto;

    public GameObject playerA;
    public GameObject playerB;


    public int scoreANum;
    public int scoreBNum;


public void termina()
    {
        
if (scoreANum == 5)
        {
            
Debug.Log("A ganhou");

        }

        if (scoreBNum == 5)
        {
            Debug.Log("B ganhou");


        }

    }

    // Update is called once per frame
    void Update()
    {
        // Corrigida a sintaxe de verificação do Input
        if (Input.GetButtonDown("Start"))
        {
            // Métodos no C# começam com letra maiúscula (SetActive)
            scoreA.SetActive(true);
            scoreB.SetActive(true);

            logo.SetActive(false);
            texto.SetActive(false);

            // Método GetComponent corrigido (certifique-se de que o nome da classe tankBehaviour está correto no seu projeto)
            playerA.GetComponent<TankBehaviour>().enabled = true; // Alterado para true para ativar o script do jogador no início do jogo
            playerB.GetComponent<TankBehaviour>().enabled = true;
        }
    }
}