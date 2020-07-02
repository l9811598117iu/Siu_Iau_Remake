using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeController : MonoBehaviour
{
    public RuntimeAnimatorController player1Anim;
    public RuntimeAnimatorController player2Anim;
    public PlayerChange playerController;
    public Avatar player1Avatar;
    public Avatar player2Avatar;

    private static bool Q = false;
    private static bool E = false;

    public float changecooldownTime = 6f;
    private float nextChangeTime = 0f;

    public enum Stance_Type
    {
        player1,
        player2,
        TOTAL_STANCE,
    }

    public Stance_Type currStance;
    void Start()
    {
        Q = true;
        E = false;
    }
    void Update()
    {
       

        if (Time.time > nextChangeTime && Input.GetKeyDown(KeyCode.Q)&&!Q)  //范
        {
            Q = true;
            E = false;
            NextStance();
            nextChangeTime = Time.time + changecooldownTime;

        }

        if (Time.time > nextChangeTime && Input.GetKeyDown(KeyCode.E)&&!E)  //范
        {
            Q = false;
            E = true;
            NextStance2();
            nextChangeTime = Time.time + changecooldownTime;

        }
    }

    public Stance_Type GetStance()
    {
        return currStance;
    }

    public void NextStance()
    {
        currStance = Stance_Type.player1;
        if (currStance == Stance_Type.TOTAL_STANCE)
            currStance = 0;
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;
                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }
    }
    public void NextStance2()
    {
        currStance = Stance_Type.player2;
        if (currStance == Stance_Type.TOTAL_STANCE)
            currStance = 0;
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;

                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }

    }
    public void PrevStance()
    {
        if (currStance != 0)
        {
            currStance--;
        }
        else
        {
            currStance = Stance_Type.TOTAL_STANCE - 1;
        }
        playerController.ChangeMesh();
        switch (currStance)
        {
            case Stance_Type.player1:
                GetComponent<Animator>().runtimeAnimatorController = player1Anim;
                GetComponent<Animator>().avatar = player1Avatar;
                break;
            case Stance_Type.player2:
                GetComponent<Animator>().runtimeAnimatorController = player2Anim;
                GetComponent<Animator>().avatar = player2Avatar;
                break;

            default:
                break;
        }
    }
}