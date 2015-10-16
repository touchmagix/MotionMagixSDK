using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Silhouette : MonoBehaviour 
{
	
	public RawImage RawImage;
	public Renderer ImageRenderer;

	// Use this for initialization
	void Start () 
	{
		if (MMData.FeedType == FeedType.SILHOUETTE) 
		{
			MMData.SillhouetteTexture = new Texture2D (300, 220,TextureFormat.ARGB32,false);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		ImageRenderer.material.mainTexture = MMData.SillhouetteTexture;
	}
}
