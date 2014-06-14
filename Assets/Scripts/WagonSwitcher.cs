using UnityEngine;
using System.Collections;

public class WagonSwitcher : MonoBehaviour {
    public Transform fadeOnCollide = null;
    public Transform camera = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D collision){
        if (fadeOnCollide){
            StartCoroutine("Fade");
            var go = GameObject.FindGameObjectsWithTag("Foreground");
            foreach (var g in go){
                if (g.transform != fadeOnCollide){
                    StartCoroutine("FadeOther", g.transform);
                }
            }
        }
        if (camera){
            StartCoroutine("BringCamera");
        }
    }

    IEnumerator FadeOther(Transform g)
    {
        if (g.renderer.material.color.a == 1.0f) yield break;
        for (float f = 0.0f; f <= 1.0f; f += 0.05f){
            Color c = g.renderer.material.color;
            c.a = f;
            g.renderer.material.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        g.renderer.material.color = Color.white;
    }

    IEnumerator Fade(){
        if (fadeOnCollide.renderer.material.color.a == 0.0f) yield break;
        for (float f = 1.0f; f >= 0.0f; f -= 0.05f){
            Color c = fadeOnCollide.renderer.material.color;
            c.a = f;
            fadeOnCollide.renderer.material.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        Color col = fadeOnCollide.renderer.material.color;
        col.a = 0.0f;
        fadeOnCollide.renderer.material.color = col;
    }

    IEnumerator BringCamera(){
        if (camera.position.x < this.GetComponentInParent<Transform>().position.x){
            float incr = 0.4f;
            for (float f = camera.position.x; f < this.GetComponentInParent<Transform>().position.x; f += incr)
            {
                camera.position = new Vector3(f, camera.position.y, camera.position.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else{
            float incr = -0.4f;
            for (float f = camera.position.x; f > this.GetComponentInParent<Transform>().position.x; f += incr)
            {
                camera.position = new Vector3(f, camera.position.y, camera.position.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
