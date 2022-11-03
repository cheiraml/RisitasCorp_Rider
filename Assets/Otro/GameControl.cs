using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameControl : MonoBehaviour
{
    [SerializeField] Text Nombre;
    [SerializeField] GameObject CuadroSalida;
    [SerializeField] GameObject CuadroMeta;
    [SerializeField] GameObject Felicitaciones;
    [SerializeField] Text TimerUsuario;
    [SerializeField] GameObject Bienvenida;
    [SerializeField] GameObject TiempoFuera;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bufo;



    private static GameControl instance;
    private Thread receiveThread;
    private UdpClient _dataReceive;
    private IPEndPoint _receiveEndPointData;
    public string _ipData = "192.168.100.13";//IP Main
    public Vector3 TeleportPos;
    private float StartTime;
    private string mins;
    public bool PlayerInArea { get; set; }
    public string detectionTag = "Player";
    public Collider colision;

    public int _receivePortData = 3500;
    public int _sendPortData = 3100;
    private bool isInitialized;
    private Queue receiveQueue;

    private string _dataReceived;

    private void Awake()
    {
        colision = Bufo.GetComponent<Collider>();
        TeleportPos = CuadroMeta.transform.position;
        StartTime = Time.time;
        Initialize();
    }

    private void Initialize()
    {
        instance = this;
        _receiveEndPointData = new IPEndPoint(IPAddress.Parse(_ipData), _sendPortData);
        _dataReceive = new UdpClient(_receivePortData);

        receiveQueue = Queue.Synchronized(new Queue());


        receiveThread = new Thread(new ThreadStart(ReceiveDataListener));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        isInitialized = true;
    }
   
    private void OnDestroy()
    {
        TryKillThread();
    }
    private void OnApplicationQuit()
    {
        TryKillThread();
    }
    private void TryKillThread()
    {
        if (isInitialized)
        {
            receiveThread.Abort();
            receiveThread = null;

            _dataReceive.Close();
            _dataReceive = null;

            Debug.Log("Thread killed");
            isInitialized = false;
        }
    }

    private void ReceiveDataListener()
    {
        while (true)
        {
            try
            {
                byte[] dataPulse = _dataReceive.Receive(ref _receiveEndPointData);
                receiveQueue.Enqueue(dataPulse);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }

    private void sendStringDataMaster(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _dataReceive.Send(data, data.Length, _receiveEndPointData);
        }
        catch (System.Exception err)
        {
            print(err.ToString());
        }
    }


    

    
    // Update is called once per frame
    void Update()
    {
       

        float TimerControl = Time.time - StartTime;
        float TimerPlay = Time.time - StartTime;
        mins = ((int)TimerControl / 60).ToString("00");
        if (receiveQueue.Count != 0)
        {
            byte[] message = (byte[])receiveQueue.Dequeue();
            if (message == null)
            return;

                Debug.Log("Mensaje de llegada");
                _dataReceived = Encoding.Default.GetString(message); ;
                Debug.Log(_dataReceived);
                
                if (_dataReceived.Equals("Tiempo"))
                {
                    mins= mins+1;
                }
                else if (_dataReceived.Equals("NoTiempo"))
                {
                    mins = mins + (-1);    

                }
                else 
                {
                    Nombre.text = _dataReceived;
                }
            
        }


        string segs = (TimerControl % 60).ToString("00");
        string milisegs = ((TimerControl * 100) % 100).ToString("00");

        string TimerString = string.Format("{00}:{01}:{02}", mins, segs, milisegs);
        TimerUsuario.text = TimerString.ToString();

        string minsPlayer = ((int)TimerPlay / 60).ToString("00");
        string segsPlayer = (TimerPlay % 60).ToString("00");
        string milisegsPlayer = ((TimerPlay * 100) % 100).ToString("00");


        if (minsPlayer.Equals("07"))
        {
            Player.transform.position = TeleportPos;
            TiempoFuera.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
