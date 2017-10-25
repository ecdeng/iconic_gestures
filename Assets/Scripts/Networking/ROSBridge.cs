using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System;
using UnityEngine;

public class ROSBridge : MonoBehaviour {
	static TcpListener serverSocket;
	static TcpClient clientSocket;
	static NetworkStream networkStream;

	// Use this for initialization
	void Start () {
		serverSocket = new TcpListener(IPAddress.Any, 8888);
		clientSocket = default(TcpClient);
		networkStream = null;

		serverSocket.Start();
		print("Server started");

		Initialize();
	}

	void Initialize ()
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

	// Update is called once per frame
	void Update () {
        Coordinate c1 = new Coordinate(new Vector3(1, 1, 1), new Vector3(2, 2, 2));
        Coordinate c2 = new Coordinate(new Vector3(2, 2, 2), new Vector3(3, 3, 3));
        Coordinate c3 = new Coordinate(new Vector3(3, 3, 3), new Vector3(4, 4, 4));
        Coordinate c4 = new Coordinate(new Vector3(4, 4, 4), new Vector3(5, 5, 5));

        List<Coordinate> hand1 = new List<Coordinate>();
        hand1.Add(c1); hand1.Add(c2);
        List<Coordinate> hand2 = new List<Coordinate>();
        hand2.Add(c3); hand2.Add(c4);

        List<List<Coordinate>> gestures = new List<List<Coordinate>>();
        gestures.Add(hand1); gestures.Add(hand2);

        sendPoints(gestures);
    }

	public void sendPoints(object[] point_normals) 
	{
		// sendPoints ((List<Vector3>)point_normals [0], (List<Vector3>) point_normals [1]);
	}

	void sendPoints(List<List<Coordinate>> coordinates)
	{
		string serializedPoints = UnityEngine.JsonUtility.ToJson(coordinates[0][0]);
		//Send(serializedPoints + "\n");
		print (serializedPoints);
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
}

// Wrapper class to be able to serialize out the points
public class Points
{
    public List<List<Coordinate>> coordinates;

    public Points(List<List<Coordinate>> coordinates)
    {
        this.coordinates = coordinates;
    }

    public List<List<Coordinate>> getCoordinates()
    {
        return coordinates;
    }
}

public class Coordinate
{
    public Vector3 point;
    public Vector3 normal;

    public Coordinate(Vector3 point, Vector3 normal)
    {
        this.point = point;
        this.normal = normal;
    }

    public Vector3 getPoint()
    {
        return point;
    }

    public Vector3 getNormal()
    {
        return normal;
    }
}