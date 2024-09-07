using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Sensor
{
    LeftWheel = 0,
    RightWheel = 1,
    LeftLeg = 2,
    RightLeg = 3,
}

public class HardwareManager : MonoBehaviour
{

    public static HardwareManager Instance { get; private set; }

    public Hardware[] HardwareObjects { get => hardwareObjects; }

    [SerializeField] private int port = 7777;
    [Space]
    [SerializeField] private Hardware[] hardwareObjects;

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
            string message = data.Dequeue();
            string[] packet = message.Split(',');
            int id = int.Parse(packet[0]);
            float gyroX = float.Parse(packet[1]);
            float gyroY = float.Parse(packet[2]);
            float gyroZ = float.Parse(packet[3]);
            float accX = float.Parse(packet[4]);
            float accY = float.Parse(packet[5]);
            float accZ = float.Parse(packet[6]);
            hardwareObjects[id].ReceiveGyro(gyroX, gyroY, gyroZ);
            hardwareObjects[id].ReceiveAccel(accX, accY, accZ);
        }
    }

    public void Initialize()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ReceiveBufferSize = 1024;
        socket.Bind(ep);
        isRunning = true;
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
                Debug.Log("Socket has been closed");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }



}
