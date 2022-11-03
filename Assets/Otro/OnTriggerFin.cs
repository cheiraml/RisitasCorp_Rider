using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerFin : MonoBehaviour
{
    [SerializeField] GameObject CuadroSalida;
    [SerializeField] GameObject CuadroMeta;
    [SerializeField] GameObject Felicitacion;
    [SerializeField] GameObject Cronometro;
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
            CuadroSalida.SetActive(true);
            Felicitacion.SetActive(true);
            CuadroMeta.SetActive(false);
            Cronometro.SetActive(false);
            Time.timeScale = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {


    }
}