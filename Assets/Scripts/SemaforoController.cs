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
        Debug.Log("2");
        yield return new WaitForSeconds(3);
        m_rojo.SetActive(false);
        m_amarillo.SetActive(true);

    }
    IEnumerator luzVerde()
    {
        Debug.Log("1");
        Debug.Log("Go!!!!");
        yield return new WaitForSeconds(3);
        m_amarillo.SetActive(false);
        m_verde.SetActive(true);
   
    }
    IEnumerator apagar()
    {
        yield return new WaitForSeconds(6);

        m_verde.SetActive(false);
    }
}
