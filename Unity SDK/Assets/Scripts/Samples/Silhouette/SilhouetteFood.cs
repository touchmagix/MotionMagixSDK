using UnityEngine;
using System.Collections;

public class SilhouetteFood : MonoBehaviour 
{
	Spawner spawner;
	Renderer _renderer;
	// Use this for initialization
	void Start () 
	{
		spawner = Camera.main.GetComponent<Spawner> ();
		_renderer = GetComponentInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) 
		{
			Vector2 coord = hit.textureCoord;

			Color pixel = MMData.SillhouetteTexture.GetPixel((int)(coord.x * MMData.SillhouetteTexture.width),(int)(coord.y * MMData.SillhouetteTexture.height));

			if(pixel.a != 0.0f)
			{
				spawner.Score++;
				_renderer.material.mainTexture = null;
				rigidbody.useGravity = false;
				Destroy(gameObject,0.2f);
			}

		}
	}
}
