using System.Collections;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using UnityEngine;
using MotionMagixSimulator.Model;
using MotionMagixSimulator.Utility;
using UnityEngine.UI;

public class MMClient :MonoBehaviour
{
    IPAddress[] ipAddress = Dns.GetHostAddresses("localhost");
    IPEndPoint ipEnd;
    Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    
	public static List<Quaternion> SkeletonJointQuats = new List<Quaternion>();
	public static List<Vector3> SkeletonJoints = new List<Vector3>();

	public FeedType FeedType = FeedType.POINT;
	public int Width;
	public int Height;

	Texture2D TempTex;
	byte[] temp;

	public static event EventHandler<EventArgs> ConnectionClosed;

    // Use this for initialization
    public void Start()
    {
		TempTex = new Texture2D (300, 220,TextureFormat.ARGB32,false);

		MMData.FeedType = FeedType;
		MMData.Width = Width;
		MMData.Height = Height;

        ipEnd = new IPEndPoint(ipAddress[0], 12345);
        try
        {
            clientSock.Connect(ipEnd);
			StateObject so2 = new StateObject();
			so2.workSocket = clientSock;
			clientSock.BeginReceive(so2.buffer, 0, StateObject.BUFFER_SIZE,0,new AsyncCallback(OnData), so2);

			MMData.Status = "Connected.";
        }
        catch (Exception e)
        {
			 MMData.Status = "Connection could not be made";
        }
    }

	void FixedUpdate()
	{
		if (MMData.SillhouetteTexture != null) 
		{
			
			TempTex.LoadImage(temp);
			Color[] pixels = TempTex.GetPixels();
			if(pixels.Length == 300 * 220)
			{
				for(int i = 0; i< pixels.Length;i++) 
				{
					if(pixels[i].r > 0.93f && pixels[i].g > 0.93f && pixels[i].b > 0.93f)
						pixels[i].a = 0;
				}
				MMData.SillhouetteTexture.SetPixels(pixels);
				MMData.SillhouetteTexture.Apply();
			}
		}
	}

    public void OnData(IAsyncResult ar)
    {
        try
        {
			StateObject so = (StateObject) ar.AsyncState;
			Socket s = so.workSocket;
			
			int read = s.EndReceive(ar);
			
			if (read > 0) 
			{
				try
				{
					switch(MMData.FeedType)
					{
						case FeedType.POINT :
							so.sb = (Encoding.ASCII.GetString(so.buffer, 0, read));
							MultiPoint multiPoint = CustomSerilization.DeserializeData(so.sb.ToString(),new MultiPoint()) as MultiPoint;
							MMData.AddBlob(multiPoint.MultiPointCoordinates[0].XCoordinate,multiPoint.MultiPointCoordinates[0].YCoordinate);
							break;
						case FeedType.SKELETON :
							so.sb = (Encoding.ASCII.GetString(so.buffer, 0, read));
							MMData.SkeletonObject = CustomSerilization.DeserializeData(so.sb.ToString(),new Skeleton()) as Skeleton;
							break;
						case FeedType.SILHOUETTE :
							temp = new byte[so.buffer.Length-4];
							Array.Copy(so.buffer,4,temp,0,so.buffer.Length-4);
							break;
					}
					MMData.Status = "Receiving data";
				}
				catch(Exception ex)
				{
					MMData.Status = "Problem receiving data";
				}

				s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0, new AsyncCallback(OnData), so);
			}
			else
			{
				if (so.sb.Length > 1) 
				{
					if(ConnectionClosed != null)
						ConnectionClosed(null,null);
					MMData.Status = "Connection closed";
				}
				s.Close();
			}
		}
        catch (Exception e)
        {
            Debug.Log (e.Message);
        }
    }

	void OnApplicationQuit()
	{
		if (clientSock != null && clientSock.Connected) 
		{
			clientSock.Close();
		}
	}
}