using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup healthBar;
    [SerializeField]
    private GameObject healthUnit;
    [SerializeField]
    private int health = 5;
    private List<GameObject> healthList = new List<GameObject>(); //same as C++ vector
    //ideally we need a way to access the health from the player instead of from here
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnHealth();
        //Invoke("CallChangeHealth", 5f); //tests changeHealth via CallChangeHealth
    }

    void SpawnHealth()
    {
        for (int i = 0; i < health; i++)
        {
            GameObject hp = Instantiate(healthUnit, healthBar.transform);
            healthList.Add(hp);
        }
    }

    /*
    private void CallChangeHealth() //helper function to test ChangeHealth
    {
        ChangeHealth(3);
    }
    */

    public void ChangeHealth(int newHealth) 
    {

        int changeBy = newHealth - health;
        health = newHealth;
        if (changeBy > 0)
        {
            for (int i = 0;i < changeBy;i++) 
            { 
                GameObject hp = Instantiate(healthUnit, healthBar.transform);
                healthList.Add(hp);
            }
        }
        else if (changeBy < 0)
        {
            for (int i = 0; i > changeBy; i--)
            {
                if (healthList.Count > 0)
                {
                    GameObject hp = healthList.Last<GameObject>();
                    healthList.Remove(hp);
                    Destroy(hp);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
