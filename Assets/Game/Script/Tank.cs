using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade = 5f;
    public float velocidadeRotacao = 100f;

    void Update()
    {
        // Rotação
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -velocidadeRotacao * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, velocidadeRotacao * Time.deltaTime, 0);
        }

        // Movimento para frente
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
        }
    }
}