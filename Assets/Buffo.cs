using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffo : MonoBehaviour
{
    [SerializeField] GameObject BuffoOn;
    public bool PlayerInArea { get; set; }
    public string detectionTag = "Player";
    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(detectionTag))
        {
            PlayerInArea = true;
            BuffoOn.SetActive(true);
            Debug.Log("Entro");
        }
    }
    // Update is called once per frame
    void Update()
    {


    }
}
