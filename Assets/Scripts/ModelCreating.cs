using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelCreating : MonoBehaviour
{

	public GameObject[] objects;
	private GameObject inst_obj;
	void Start()
	{
		
		int rand = Random.Range(0, objects.Length - 1);
		inst_obj = Instantiate(objects[rand], objects[rand].transform.position, Quaternion.identity);
		inst_obj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

}

