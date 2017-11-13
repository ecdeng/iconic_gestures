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

// script for export button behavior
public class ExportButtonScript : Singleton<ExportButtonScript>
{
    public Button exportButton; // UI element

    //networking
    static TcpListener serverSocket;
    static TcpClient clientSocket;
    static NetworkStream networkStream;

    // Use this for initialization
    void Start()
    {
        Button btn = exportButton.GetComponent<Button>();
        btn.onClick.AddListener(delegate { Export(); });
		btn.gameObject.SetActive (false);

        //        //networking
        serverSocket = new TcpListener(IPAddress.Any, 8888);
        clientSocket = default(TcpClient);
        networkStream = null;

        serverSocket.Start();
        print("Server started");
        //
        //        Initialize();
        Thread tid1 = new Thread(new ThreadStart(ExportButtonScript.CheckForConnection));
        tid1.Start();
    }

    //networking
    public void Initialize()
    {
		Button btn = exportButton.GetComponent<Button>();
		btn.gameObject.SetActive (true);    
    }

    // Update is called once per frame
    void Update()
    {
        exportPoints(null, null);
    }

    public List<List<int>> Export()
    {
        //Debug.Log ("Attempting to export.");

        List<string> unformatted = GenerateTableButtonScript.Instance.inputVals;
        //Debug.Log ("unformatted size: " + unformatted.Count);
        int numCols = GenerateTableButtonScript.Instance.numCols;
        //int numRows = GenerateTableButtonScript.Instance.numRows;
        List<int> unformattedToInt = new List<int>(unformatted.Count);
        unformattedToInt = (Enumerable.Repeat(0, unformatted.Count)).ToList();

        //Debug.Log ("unformatted to int size: " + unformattedToInt.Count);
        for (int i = 0; i < unformattedToInt.Count; i++)
        {
			if (unformatted [i].Length > 0) {
				unformattedToInt [i] = int.Parse (unformatted [i]);
			} else {
				unformattedToInt [i] = -1;
			}
            //Debug.Log (unformattedToInt [i] + ", ");
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
        Debug.Log("unformatted to original keys: " + unformattedToOrigKeys.Count);

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
                    //result [j-1][i] = unformattedToInt [i];
                }
            }
        }
        //		Debug.Log ("firstcol0: " + result[0].Count);
        //		Debug.Log ("secondcol1: " + result[1].Count);
        //Debug.Log(result[0][0] + "," + result[0][1] + "," + result[0][2]);

        sendPoints(result, ObjManager.Instance.GetPointNormals());

        return result;
    }

    //networking
    static void CheckForConnection()
    {
        while(true)
        {
            clientSocket = serverSocket.AcceptTcpClient();
            print("Client connected to");
            networkStream = clientSocket.GetStream();
        }
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

    void sendPoints(List<List<int>> actorList, Dictionary<int, PositionNormals> positions)
    {
        String serializedPoints = convertPoints(actorList, positions, false);
        print(serializedPoints);
        Send(serializedPoints);
    }

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
                //if (positions[gesture].norm.x < 0 && positions[gesture].norm.y < 0 && positions[gesture].norm.z < 0 && positions[gesture].norm.w < 0)
                //{
                //    positions[gesture].norm.x *= -1;
                //    positions[gesture].norm.y *= -1;
                //    positions[gesture].norm.z *= -1;
                //    positions[gesture].norm.w *= -1;
                //}
                actor.gest.Add(new Vertex(positions[gesture].pos, positions[gesture].norm));
            }
            if (actor.gest.Count != 0) moveList.act.Add(actor);
        }

        return UnityEngine.JsonUtility.ToJson(moveList, pretty);
    }

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