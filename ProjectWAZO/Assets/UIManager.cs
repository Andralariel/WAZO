using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject weightCanvas;
    
    public void InvokeUI(Transform position)
    {
        GameObject canvas = Instantiate(weightCanvas, position.position, Quaternion.identity);
    }
}
