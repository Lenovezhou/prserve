using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour 
{
	public float Speed;
	public float MinX;
	public float MaxX;
	public float RandomMaxY;
	public float RandomMinY;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.left * Speed*Time.deltaTime);

		if(transform.localPosition.x>MaxX)
		{
			transform.localPosition=new Vector3(MinX,Random.Range(RandomMaxY,RandomMinY),transform.localPosition.z);
		}
		if(transform.localPosition.x<MinX)
		{
			transform.localPosition=new Vector3(MaxX,Random.Range(RandomMaxY,RandomMinY),transform.localPosition.z);
		}

	}
}
