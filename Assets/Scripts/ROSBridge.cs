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
		List<Vector3> points = new List<Vector3>();
		points.Add(new Vector3(1, 2, 3));
		points.Add(new Vector3(4, 5, 6));

		List<Vector3> normals = new List<Vector3>();
		normals.Add(new Vector3(0, 1, 0));
		normals.Add(new Vector3(1, 0, 0));

		sendPoints(points, normals);
	}

	void sendPoints(List<Vector3> points, List<Vector3> normals)
	{
		string serializedPoints = UnityEngine.JsonUtility.ToJson(new Points(points, normals));
		Send(serializedPoints);
	}

	private void Send(string information)
	{
		if (networkStream == null) return;
		try
		{
			byte[] sendBytes = Encoding.ASCII.GetBytes(information);
			networkStream.Write(sendBytes, 0, sendBytes.Length);
			networkStream.Flush();
			print(information);

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
	public List<Vector3> points;
	public List<Vector3> normals;

	public Points(List<Vector3> points, List<Vector3> normals)
	{
		this.normals = normals;
		this.points = points;
	}
}