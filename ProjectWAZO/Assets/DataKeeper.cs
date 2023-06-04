using _3C;
using UnityEngine;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper instance;
    public bool isHat;
    public GameObject prefabHat;

    void Awake()
    {
        if (instance != default && instance!=this)
        {
            DestroyImmediate(this);
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
       
    }

    public void CheckHat()
    {
        if (isHat)
        {
            GameObject hat = Instantiate(prefabHat, Controller.instance.transform.position, Quaternion.identity);
            hat.GetComponent<HatManager>().PutHat();
        }
    }
}
