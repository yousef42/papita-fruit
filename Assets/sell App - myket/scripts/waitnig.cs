using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitnig : MonoBehaviour
{
    public static waitnig instance;
    public GameObject anim, googleBtn;
    public bool phoneBtn,iranStore;
    int counter = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wait()
    {
        this.gameObject.SetActive(false);
        anim.SetActive(true);
        Invoke("comeback",4);

        if (iranStore)
        {
            counter = counter + 1;
            Debug.Log("clicking ..." + counter);
            
        }
      
    }

    void comeback()
    {
        if (phoneBtn)
        {
            if(UnityEngine.PlayerPrefs.GetInt("init phone") == 0)
            {
                this.gameObject.SetActive(true);
                anim.SetActive(false);
            }
         
        }
        
        else
        {
            if (counter >= 2)
            {
                Debug.Log("clicked");
                googleBtn.SetActive(true);
                this.gameObject.SetActive(false);
                anim.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
                anim.SetActive(false);
            }
         
        }
     
    }
}
