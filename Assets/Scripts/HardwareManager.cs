using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public enum Sensor
{
    LeftWheel = 0,
    RightWheel = 1,
    Leg = 2
}

public class HardwareManager : MonoBehaviour
{

    public static HardwareManager Instance { get; private set; }

    [HideInInspector]
    public UnityEvent<Hardware> TrackerConnected;
    [HideInInspector]
    public UnityEvent<Hardware> TrackerDisconnected;

    public Dictionary<int, Hardware> HardwareObjects { get => hardwareObjects; }

    [SerializeField] private int port = 8888;
    [Space]
    [SerializeField] private Dictionary<int, Hardware> hardwareObjects = new Dictionary<int, Hardware>();
    [SerializeField] private GameObject hardwarePrefab;
    [SerializeField] private Transform hardwareParent;

    private Socket socket;
    private Queue<string> data = new Queue<string>();
    private bool isRunning = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Initialize();
        Task.Run(ReceiveData);

        TrackerDisconnected.AddListener((hardware) =>
        {
            hardwareObjects.Remove(hardware.ID);
        });

    }

    void OnDisable()
    {
        if (isRunning)
        {
            isRunning = false;
            socket.Close();
        }
    }

    void Update()
    {
        while (data.Count > 0)
        {
            //Debug.Log("Parsing data.");
            string message = data.Dequeue();
            Debug.Log(message);
            string[] packet = message.Split(',');
            int id = int.Parse(packet[0]);
            float gyroX = float.Parse(packet[1]);
            float gyroY = float.Parse(packet[2]);
            float gyroZ = float.Parse(packet[3]);

            if (!hardwareObjects.ContainsKey(id))
            {
                Hardware hardware = Instantiate(hardwarePrefab, hardwareParent).GetComponent<Hardware>();
                hardware.Init(id);
                hardwareObjects[id] = hardware;
            }

            hardwareObjects[id].ReceiveGyro(gyroX, gyroY, gyroZ);
        }
    }

    public void Initialize()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ReceiveBufferSize = 1024;
        socket.Bind(ep);
        isRunning = true;
        Debug.Log("Initialized.");
    }

    public async void ReceiveData()
    {
        while (isRunning)
        {
            try
            {
                byte[] buffer = new byte[1024];
                await socket.ReceiveFromAsync(buffer, SocketFlags.None, new IPEndPoint(IPAddress.Any, 0));
                string message = Encoding.ASCII.GetString(buffer);
                data.Enqueue(message);
            }
            catch (ObjectDisposedException)
            {
                //Debug.Log("Socket has been closed");
            }
            catch (System.Exception e)
            {
                //Debug.LogError(e);
            }
        }
    }



}
