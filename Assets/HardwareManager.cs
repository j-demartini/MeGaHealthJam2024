using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class HardwareManager : MonoBehaviour
{

    public static HardwareManager Instance { get; private set; }

    private Socket socket;
    private Queue<string> data = new Queue<string>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ReceiveBufferSize = 1024;
        Task.Run(ReceiveData);
    }

    public async void ReceiveData()
    {
        while(true)
        {

            byte[] buffer = new byte[1024];
            await socket.ReceiveAsync(buffer, SocketFlags.None);



        }
    }



}
