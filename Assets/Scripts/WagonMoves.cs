using UnityEngine;
using System.Collections;

public class WagonMoves : MonoBehaviour {
    
    private float lastMove;
    private Vector2 startPosition;
    private bool fallback = false;
    private Vector2 curMoveSpeed = Vector2.zero;

    public float moveInterval = 1f;
    public float moveMaxVelocity = 0.1f;

	// Use this for initialization
	void Start () {
        lastMove = Time.realtimeSinceStartup;
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (lastMove + moveInterval < Time.realtimeSinceStartup){
            if (curMoveSpeed == Vector2.zero){
                lastMove = Time.realtimeSinceStartup;
                curMoveSpeed.x = Random.Range(-moveMaxVelocity, moveMaxVelocity);
                curMoveSpeed.y = Random.Range(0.01f, moveMaxVelocity);
            }
            else if(fallback){
                fallback = false;
                curMoveSpeed = Vector2.zero;
                this.transform.position = startPosition;
            }
            else{
                fallback = true;
                curMoveSpeed = -curMoveSpeed;
                lastMove = Time.realtimeSinceStartup;
            }
        }
        this.transform.position += new Vector3(curMoveSpeed.x * Time.deltaTime, curMoveSpeed.y * Time.deltaTime, 0f);
	}
}
