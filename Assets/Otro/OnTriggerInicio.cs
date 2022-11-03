using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerInicio : MonoBehaviour
{
    [SerializeField] GameObject CuadroSalida;
    [SerializeField] GameObject CuadroMeta;
    [SerializeField] GameObject Bienvenida;
    public bool PlayerInArea { get; private set; }
    public string detectionTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            PlayerInArea = true;
            CuadroSalida.SetActive(false);
            Bienvenida.SetActive(false);
            CuadroMeta.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {

        
    }
}
