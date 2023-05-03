using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TempleManager : MonoBehaviour
{
    public GameObject door;
    public bool EnableCinématique;
    public GameObject Escalier1;
    public GameObject Escalier2;
    
    void Start()
    {
        StartCoroutine(CinématiqueStart());
    }

    public void ActivateEscalier1()
    {
        Escalier1.transform.DOMove(
            new Vector3(Escalier1.transform.position.x, Escalier1.transform.position.y + 10,
                Escalier1.transform.position.z), 2f);
    }
    
    public void ActivateEscalier2()
    {
        Escalier2.transform.DOMove(
            new Vector3(Escalier2.transform.position.x, Escalier2.transform.position.y + 20,
                Escalier2.transform.position.z), 2f);
    }
        
    
    IEnumerator CinématiqueStart()
    {
        if (EnableCinématique)
        {
            Controller.instance.canMove = false;
            Controller.instance.canJump = false;
            Controller.instance.transform.DOMove(new Vector3(Controller.instance.transform.position.x, Controller.instance.transform.position.y,
                Controller.instance.transform.position.z + 6),1.5f);
            yield return new WaitForSeconds(1.5f);
            door.transform.DOMove(new Vector3(door.transform.position.x, door.transform.position.y + 5, door.transform.position.z), 1f);
            Controller.instance.canMove = true;
            Controller.instance.canJump = true;
        }
    }
}
