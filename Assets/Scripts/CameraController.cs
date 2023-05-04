using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject m_cam1, m_cam2;
    [SerializeField] private GameObject m_rear;
    // Start is called before the first frame update
    void Start()
    {
        m_cam1.SetActive(false);
        m_cam2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.V)){
            m_cam1.SetActive(!m_cam1.activeSelf);
            m_cam2.SetActive(!m_cam2.activeSelf);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.C))
        {
            m_rear.SetActive(!m_rear.activeSelf);
        }
        
    }
    private void RearCam()
    {
        
    }
}
