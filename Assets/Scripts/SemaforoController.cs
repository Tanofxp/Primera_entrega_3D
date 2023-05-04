using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class SemaforoController : MonoBehaviour
{
    [SerializeField] private GameObject m_verde;
    [SerializeField] private GameObject m_amarillo;
    [SerializeField] private GameObject m_rojo;
    // Start is called before the first frame update
    void Start()
    {
        m_verde.SetActive(false);
        m_amarillo.SetActive(false);
        m_rojo.SetActive(true);
        Debug.Log("3");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rojo.activeSelf == true) {
            StartCoroutine(luzRoja());
        }
        if (m_amarillo.activeSelf == true) {
            StartCoroutine(luzVerde());
            StartCoroutine(apagar());
        }
        
    }
    IEnumerator luzRoja()
    {
        yield return new WaitForSeconds(3);
        m_rojo.SetActive(false);
        m_amarillo.SetActive(true);
        Debug.Log("2");

    }
    IEnumerator luzVerde()
    {
        yield return new WaitForSeconds(3);
        m_amarillo.SetActive(false);
        m_verde.SetActive(true);
        Debug.Log("1");
        Debug.Log("Go!!!!");

    }
    IEnumerator apagar()
    {
        yield return new WaitForSeconds(3);

        m_verde.SetActive(false);
        
        


    }
}
