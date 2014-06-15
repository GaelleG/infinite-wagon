using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    private Vector2 move = new Vector2(0,0);
    private Vector2 forceLimit;
    public Vector2 forceLimitUp = new Vector2(40.0f, 500.0f);
    public Vector2 forceLimitCrouched = new Vector2(20.0f, 0.0f);
    private Vector2 velocityMax;
    private Vector2 velocityMin;
    public Vector2 velocityMaxUp = new Vector2(5.0f, 10.0f);
    public Vector2 velocityMinUp = new Vector2(-5.0f, 0);
    public Vector2 velocityMaxCrouched = new Vector2(2.5f, 0);
    public Vector2 velocityMinCrouched = new Vector2(-2.5f, 0);
    private bool touchingFloor = false;
    private GameObject equipedWeapon = null;

    void Start() {
        forceLimit = forceLimitUp;
        velocityMax = velocityMaxUp;
        velocityMin = velocityMinUp;
    }

    void Update() {
        // horizontal
        if (Input.GetKeyDown(KeyCode.RightArrow)) move.x++;
        if (Input.GetKeyUp(KeyCode.RightArrow)) move.x--;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) move.x--;
        if (Input.GetKeyUp(KeyCode.LeftArrow)) move.x++;
        if (move.x > 1) move.x = 1;
        if (move.x < -1) move.x = -1;

        // crouch
        Vector3 localScale = this.GetComponent<Transform>().localScale;
        Vector3 position = this.GetComponent<Transform>().position;
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            localScale.y = 1;
            position.y -= 0.5f;
            forceLimit = forceLimitCrouched;
            velocityMax = velocityMaxCrouched;
            velocityMin = velocityMinCrouched;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            localScale.y = 2;
            position.y += 0.5f;
            forceLimit = forceLimitUp;
            velocityMax = velocityMaxUp;
            velocityMin = velocityMinUp;
        }
        this.GetComponent<Transform>().localScale = localScale;
        this.GetComponent<Transform>().position = position;
        
        // jump
        if (Input.GetKeyDown(KeyCode.Space) && touchingFloor) move.y++;

        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(move.x * forceLimit.x, move.y * forceLimit.y));
        move.y = 0;
        Vector2 velocity = this.GetComponent<Rigidbody2D>().velocity;
        if (velocity.x > velocityMax.x) velocity.x = velocityMax.x;
        if (velocity.x < velocityMin.x) velocity.x = velocityMin.x;
        if (velocity.y > velocityMax.y) velocity.y = velocityMax.y;
        if (move.x == 0) velocity.x = 0;
        this.GetComponent<Rigidbody2D>().velocity = velocity;

        // weapons
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        GameObject availableWeapon = null;
        foreach (GameObject weapon in weapons) {
            if (weapon.transform.position.x > this.transform.position.x-this.transform.localScale.x/2
                && weapon.transform.position.x < this.transform.position.x+this.transform.localScale.x/2) {
                    availableWeapon = weapon;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            if (equipedWeapon != null) {
                equipedWeapon.transform.parent = null;
                equipedWeapon.GetComponent<Rigidbody2D>().isKinematic = false;
                equipedWeapon.GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 0));
                equipedWeapon = null;
            }
            else if (availableWeapon != null) {
                equipedWeapon = availableWeapon;
                equipedWeapon.GetComponent<Rigidbody2D>().isKinematic = true;
                equipedWeapon.transform.parent = this.transform;
                Vector3 weaponPosition = equipedWeapon.transform.localPosition;
                weaponPosition.x = this.transform.localScale.x/2 + equipedWeapon.transform.localScale.x/2;
                weaponPosition.y = 0;
                equipedWeapon.transform.localPosition = weaponPosition;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        touchingFloor = true;
    }

    void OnCollisionExit2D(Collision2D collision) {
        touchingFloor = false;
    }
}
