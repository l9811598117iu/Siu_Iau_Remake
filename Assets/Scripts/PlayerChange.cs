using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour
{
    private ChangeController data;

    void Start()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<ChangeController>();
    }

    public GameObject ChangeMesh()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        switch (data.GetStance())
        {
            case ChangeController.Stance_Type.player1:
                transform.GetChild(0).gameObject.SetActive(true);
                return transform.GetChild(0).gameObject;
            case ChangeController.Stance_Type.player2:
                transform.GetChild(1).gameObject.SetActive(true);
                return transform.GetChild(1).gameObject;
            
            default:
                return null;
        }

    }
}