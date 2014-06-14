using UnityEngine;
using System.Collections;

public class WagonMapElement : MonoBehaviour {
    private int wagonID = 0;
    public Transform mapElement = null;
	// Use this for initialization
	void Start () {
	    foreach(var a in this.name.Split('-')){
            int id = 0;
            if(int.TryParse(a, out id)){
                wagonID = id;
                if (mapElement){
                    Instantiate(mapElement, new Vector3(0.055f * id, 0f, 0f), Quaternion.identity);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
