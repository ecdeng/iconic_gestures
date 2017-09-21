using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class ROSBridge : MonoBehaviour {
    TcpListener serverSocket;
    TcpClient clientSocket;
    NetworkStream networkStream;
	// Use this for initialization
	void Start () {
        serverSocket = new TcpListener(IPAddress.Any, 8888);
        clientSocket = default(TcpClient);
        serverSocket.Start();
        print("Server started");
        clientSocket = serverSocket.AcceptTcpClient();
        print("Client connected to");

        networkStream = clientSocket.GetStream();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void sendPoints(List<Vector3> points)
    {

    }
}
