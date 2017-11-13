using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System;

/// <summary>
/// Script for export button behavior. Initializes network connection and serializes points to server
/// </summary>
public class ExportButtonScript : Singleton<ExportButtonScript>
{
    public Button exportButton; // UI element

    //networking
    static TcpListener serverSocket;
    static TcpClient clientSocket;
    static NetworkStream networkStream;

    /// <summary>
    /// Initializes network connection
    /// </summary>
    void Start()
    {
        Button btn = exportButton.GetComponent<Button>();
        btn.onClick.AddListener(delegate { Export(); });
		btn.gameObject.SetActive (false);

        // networking
        serverSocket = new TcpListener(IPAddress.Any, 8888);
        clientSocket = default(TcpClient);
        networkStream = null;

        serverSocket.Start();
        print("Server started");

        Thread tid1 = new Thread(new ThreadStart(ExportButtonScript.CheckForConnection));
        tid1.Start();
    }

    /// <summary>
    /// Set export button to active when user finishes clicking "generate table" button
    /// </summary>
    public void Initialize()
    {
		Button btn = exportButton.GetComponent<Button>();
		btn.gameObject.SetActive (true);    
    }

    /// <summary>
	/// Update is called once per frame
    /// </summary>
    void Update()
    {

    }

	/// <summary>
	/// collects input from the user in the table and compiles it into list of lists. called when user hits "export" button in 2nd stage
	/// </summary>
    public List<List<int>> Export()
    {
        List<string> unformatted = GenerateTableButtonScript.Instance.inputVals;
        int numCols = GenerateTableButtonScript.Instance.numCols;
        List<int> unformattedToInt = new List<int>(unformatted.Count);
        unformattedToInt = (Enumerable.Repeat(0, unformatted.Count)).ToList();

        for (int i = 0; i < unformattedToInt.Count; i++)
        {
			if (unformatted [i].Length > 0) {
				unformattedToInt [i] = int.Parse (unformatted [i]);
			} else {
				unformattedToInt [i] = -1;
			}
        }

        List<int> unformattedToOrigKeys = new List<int>(unformatted.Count);
        unformattedToOrigKeys = (Enumerable.Repeat(0, unformatted.Count)).ToList();
        for (int i = 0; i < unformattedToOrigKeys.Count; i++)
        {
            if (unformattedToInt[i] != -1)
            {
                unformattedToOrigKeys[i] = ObjManager.Instance.GetVirtualMemory()[unformattedToInt[i]];
            }
            else
            {
                unformattedToOrigKeys[i] = -1;
            }
        }

        //convert from row-wise to column-wise
        List<List<int>> result = new List<List<int>>(numCols);
        for (int j = 0; j < numCols + 1; j++)
        {
            result.Add(new List<int>());
            for (int i = 0; i < unformattedToOrigKeys.Count; i++)
            {
                if (i % numCols == j)
                {
                    result[j].Add(unformattedToOrigKeys[i]);
                }
            }
        }
        
		// send points over server
        sendPoints(result, ObjManager.Instance.GetPointNormals());
        exportPoints(result, ObjManager.Instance.GetPointNormals());
        return result;
    }

    /// <summary>
	/// Checks for network connection
    /// </summary>
    static void CheckForConnection()
    {
        while(true)
        {
            clientSocket = serverSocket.AcceptTcpClient();
            print("Client connected to");
            networkStream = clientSocket.GetStream();
        }
    }

	/// <summary>
	/// writes information to server over network
	/// </summary>
	/// <param name="information">Information.</param>
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

	/// <summary>
	/// converts points to json before calling Send over network
	/// </summary>
	/// <param name="actorList">Actor list.</param>
	/// <param name="positions">Positions.</param>
    void sendPoints(List<List<int>> actorList, Dictionary<int, PositionNormals> positions)
    {
        String serializedPoints = convertPoints(actorList, positions, false);
        print(serializedPoints);
        Send(serializedPoints);
    }
		
	/// <summary>
	/// convert points to json and writes to a file
	/// </summary>
	/// <param name="actorList">Actor list.</param>
	/// <param name="positions">Positions.</param>
    void exportPoints(List<List<int>> actorList, Dictionary<int, PositionNormals> positions)
    {
        String serializedPoints = convertPoints(actorList, positions, true);
        String originalPathname = FileUpload.Instance.path;
        StringBuilder sb = new StringBuilder();
        int ind = originalPathname.Length - 1;

        // Ignore the file extension
        while (originalPathname[ind--] != '.') ;

        // Move up until it reaches the end of the filename
        while (originalPathname[ind] != '\\' && originalPathname[ind] != '/') sb.Append(originalPathname[ind--]);
        char[] filenameChars = sb.ToString().ToCharArray();
        Array.Reverse(filenameChars);
        String filename = new string(filenameChars) + ".json";
        String pathname = originalPathname.Substring(0, ind + 1) + filename;

        // Save the json in the same directory as the .obj
        System.IO.File.WriteAllText(pathname, serializedPoints);
    }

	/// <summary>
	/// converts points to json
	/// </summary>
	/// <returns>The points.</returns>
	/// <param name="actorList">Actor list.</param>
	/// <param name="positions">Positions.</param>
	/// <param name="pretty">If set to <c>true</c> pretty.</param>
    String convertPoints(List<List<int>> actorList, Dictionary<int, PositionNormals> positions, bool pretty)
    {
        Wrapper moveList = new Wrapper(new List<Actor>());
        foreach (List<int> gestureList in actorList)
        {
            Actor actor = new Actor(new List<Vertex>());
            foreach (int gesture in gestureList)
            {
                if (!positions.ContainsKey(gesture)) continue;
                positions[gesture].pos.Normalize();
                actor.gest.Add(new Vertex(positions[gesture].pos, positions[gesture].norm));
            }
            if (actor.gest.Count != 0) moveList.act.Add(actor);
        }

        return UnityEngine.JsonUtility.ToJson(moveList, pretty);
    }
		
	/// <summary>
	/// wraps position and normals for each vertex
	/// </summary>
    [Serializable]
    class Vertex
    {
        public Vector3 p;
        public Quaternion q;

        public Vertex(Vector3 position, Quaternion normal)
        {
            this.p = position;
            this.q = normal;
        }
    }

	/// <summary>
	/// each actor has a list of vertices to make a gesture
	/// </summary>
    [Serializable]
    class Actor
    {
        public List<Vertex> gest;

        public Actor(List<Vertex> gestures)
        {
            this.gest = gestures;
        }
    }

	/// <summary>
	/// list of actors
	/// </summary>
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