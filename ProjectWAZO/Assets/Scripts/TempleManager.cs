using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TempleManager : MonoBehaviour
{
    public GameObject door;
    void Start()
    {
        StartCoroutine(CinématiqueStart());
    }

    IEnumerator CinématiqueStart()
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
