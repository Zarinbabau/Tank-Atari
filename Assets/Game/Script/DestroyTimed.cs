using UnityEngine;

public class DestroyTimed : MonoBehaviour
{



    void Start()
    {
        Invoke(nameof(AutoDestroy), 1f);
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
