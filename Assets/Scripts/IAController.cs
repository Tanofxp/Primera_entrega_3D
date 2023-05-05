using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IAController : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Distance = 10f;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var l_diffVector = m_Target.position - transform.position;
        Chase(l_diffVector);
    }
    private void Chase(Vector3 l_diffVector)
    {
        if (m_Distance < l_diffVector.magnitude) 
        {
            Move(l_diffVector.normalized);   
        }
    }
    private void Move(Vector3 p_direction)
    {
        transform.position += m_MoveSpeed * Time.deltaTime * p_direction;
    }
}
