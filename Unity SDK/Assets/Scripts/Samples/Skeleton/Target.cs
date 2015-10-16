using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour 
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
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag.Equals("Player")) 
		{
			spawner.Score++;
			_renderer.material.mainTexture = null;
			rigidbody.useGravity = false;
			Destroy(gameObject,0.2f);
		}
	}
}
