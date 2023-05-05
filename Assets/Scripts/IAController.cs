using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IAController : MonoBehaviour
{
    [SerializeField]
    private float speed = 25f, rotationSpeed = 180f;
    [SerializeField] private bool playerCar = false;
    private bool salida = false;

    private Rigidbody rb;
    private float translation, rotation;
    private GameObject target;

    private void Awake()
    {
      rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if(!playerCar&&!target)
            return;

        if(playerCar)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
                translation = Input.GetAxis("Vertical") * speed;
            }
            else
            {
                rotation = 0f;
                translation = 0f;
            }
        }
    }
    private void FixedUpdate()
    {
        if (playerCar && !salida)
        {
            rb.velocity = transform.forward * translation;
            transform.Rotate(Vector3.up * rotation);
        }
        else 
        {
            StartCoroutine(arrancar());
            
        }
    }
    IEnumerator arrancar()
    {
        yield return new WaitForSeconds(6);

        rb.angularVelocity = rotationSpeed * rotation * new Vector3(0, 1, 0);
        rb.velocity = transform.forward * speed;
    }
}
