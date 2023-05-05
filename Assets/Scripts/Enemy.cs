using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Distance = 10f;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Vector3.Normalize(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        var l_diffVector = m_Target.position - transform.position;
        Debug.Log(gameObject.tag);
        switch (gameObject.tag)
        {
            case "Enemy1":
                Debug.Log("Te miro "+ m_Target.gameObject.tag);
                View(l_diffVector);
                break;
            case "Enemy2":
                Debug.Log("Te Persigo " + m_Target.gameObject.tag);
                Chase(l_diffVector);
                break;

        };
        
    }
    private void Chase(Vector3 l_diffVector)
    {
        
        if (m_Distance < l_diffVector.magnitude) 
        {
            View(l_diffVector);
          
            Move(l_diffVector.normalized);
            
        }
    }

    private void View(Vector3 l_diffVector) 
    {   
        Quaternion l_newRotarion = Quaternion.LookRotation(l_diffVector.normalized);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, l_newRotarion, m_rotationSpeed);
    }
    private void Move(Vector3 p_direction)
    {
        transform.position += p_direction * m_MoveSpeed * Time.deltaTime;
    }
}
