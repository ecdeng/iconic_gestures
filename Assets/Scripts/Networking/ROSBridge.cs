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
        print("waiting for connection");
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
        if (Input.GetKeyDown("space"))
        {
            Dictionary<int, Vector3> positions = new Dictionary<int, Vector3>();
            positions.Add(0, new Vector3(3.2f, 0.1f, -1.1f));

            Dictionary<int, Quaternion> quarternions = new Dictionary<int, Quaternion>();
            quarternions.Add(0, new Quaternion(0, 0, 0, 1));

            PositionNormals positionNormals = new PositionNormals(new Vector3(3.2f, 0.1f, -1.1f), new Quaternion(0, 0, 0, 1));
            Dictionary<int, PositionNormals> map = new Dictionary<int, PositionNormals>();
            map.Add(0, positionNormals);

            List<List<int>> topList = new List<List<int>>();
            List<int> bottomList = new List<int>();
            bottomList.Add(0);

            topList.Add(bottomList);
            topList.Add(bottomList);

            sendPoints(topList, map);
            print("send");
        }
    }

    public void sendPoints(object[] point_normals)
    {
        // sendPoints ((List<Vector3>)point_normals [0], (List<Vector3>)
        // point_normals [1]);
    }

    void sendPoints(List<List<int>> actorList, Dictionary<int, PositionNormals> positions)
    {
        Wrapper moveList = new Wrapper(new List<Actor>());
        foreach (List<int> gestureList in actorList)
        {
            Actor actor = new Actor(new List<Vertex>());
            foreach (int gesture in gestureList)
            {
                if (gesture == -1) continue;
                actor.gest.Add(new Vertex(positions[gesture].pos, positions[gesture].norm));
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
        public Quaternion q;

        public Vertex(Vector3 position, Quaternion quarternion)
        {
            this.p = position;
            this.q = quarternion;
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
