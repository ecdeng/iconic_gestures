using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System;
using UnityEngine;

public class ROSBridge : MonoBehaviour
{
    static TcpListener serverSocket;
    static TcpClient clientSocket;
    static NetworkStream networkStream;

    // Use this for initialization
    void Start()
    {
        serverSocket = new TcpListener(IPAddress.Any, 8888);
        clientSocket = default(TcpClient);
        networkStream = null;

        serverSocket.Start();
        print("Server started");

        Initialize();
    }

    void Initialize()
    {
        Thread tid1 = new Thread(new ThreadStart(ROSBridge.CheckForConnection));
        tid1.Start();
    }

    static void CheckForConnection()
    {
        clientSocket = serverSocket.AcceptTcpClient();
        print("Client connected to");
        networkStream = clientSocket.GetStream();
    }

    private void Send(string information)
    {
        if (networkStream == null) return;
        try
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(information);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }
        catch (Exception ex)
        {
            print(ex.ToString());
            networkStream.Close();
            networkStream = null;
            Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();
        //positions.Add(0, new Vector3(1, 2, 3));
        //positions.Add(1, new Vector3(2, 3, 4));

        //Dictionary<int, Vector3> normals = new Dictionary<int, Vector3>();
        //normals.Add(0, new Vector3(4, 5, 6));
        //normals.Add(1, new Vector3(6, 7, 8));

        //List<List<int>> topList = new List<List<int>>();
        //List<int> bottomList = new List<int>();
        //bottomList.Add(0);
        //topList.Add(bottomList);
        //bottomList = new List<int>();
        //bottomList.Add(1);
        //topList.Add(bottomList);
        //for (int i = 0; i < 2; i++)
        //{
        //    topList.Add(bottomList);
        //}
        //sendPoints(topList, positions, normals);
    }

    public void sendPoints(object[] point_normals)
    {
        // sendPoints ((List<Vector3>)point_normals [0], (List<Vector3>)
        // point_normals [1]);
    }

    void sendPoints(List<List<int>> actorList, Dictionary<int, Vector3>
positions, Dictionary<int, Vector3> normals)
    {
        Wrapper moveList = new Wrapper(new List<Actor>());
        foreach (List<int> gestureList in actorList)
        {
            Actor actor = new Actor(new List<Vertex>());
            foreach (int gesture in gestureList)
            {
                if (gesture == -1) continue;
                actor.gest.Add(new Vertex(positions[gesture],
normals[gesture]));
            }
            moveList.act.Add(actor);
        }

        string serializedPoints = UnityEngine.JsonUtility.ToJson(moveList);
        Send(serializedPoints);
        //print(serializedPoints);
    }

    [Serializable]
    class Vertex
    {
        public Vector3 p;
        public Vector3 n;

        public Vertex(Vector3 position, Vector3 normal)
        {
            this.p = position;
            this.n = normal;
        }
    }

    [Serializable]
    class Actor
    {
        public List<Vertex> gest;

        public Actor(List<Vertex> gestures)
        {
            this.gest = gestures;
        }
    }

    [Serializable]
    class Wrapper
    {
        public List<Actor> act;

        public Wrapper(List<Actor> actors)
        {
            this.act = actors;
        }
    }

}
