using _3C;
using UnityEngine;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper instance;
    public bool isHat;
    public GameObject prefabHat;
    
    /*public bool isFullScreen;
    public bool isUI;
    public int currentQuality;
    public int currentResolution;
    public float currentVolume;
    public float currentMusic;
    public float currentSFX;*/
    
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
            Debug.Log("STAAAAAR §§§§");
            GameObject hat = Instantiate(prefabHat, Controller.instance.transform.position, Quaternion.identity);
            hat.GetComponent<HatManager>().PutHat();
        }
    }
}
