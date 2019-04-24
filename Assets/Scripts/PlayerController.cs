using UnityEngine;
using System.Collections;
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public Time timeScale;

    private Rigidbody rb;
    private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1.0f;
        speed = 10;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.velocity = movement * speed;
            if (Input.GetKeyDown(KeyCode.Space))
                 {
            if (Time.timeScale == 1.0f)
            {
                speed = 20;
            }
            if(Time.timeScale == 0.5f)
            {
                speed = 10;
            }
            if (speed == 10) 
            {
                speed = 20;
            }
            if (speed == 20)
            {
                speed = 10;
            }
        }
        rb.position = new Vector3
            (
                 Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                 0.0f,
                 Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );

            rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
        }

    }