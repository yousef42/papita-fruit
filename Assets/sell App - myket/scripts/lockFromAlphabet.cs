using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lockFromAlphabet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] levels;
    public string[] events;
    private int until;
    void Start()
    {
        until = UnityEngine.PlayerPrefs.GetInt("fromAlphabet");
        for(int i=until; i < levels.Length; i++)
        {
           // levels[i].GetComponent<Image>().color = new Color32(255, 255, 225, 50);
          //  levels[i].GetComponent<GoAnimLayoutMain>().isLock = true;
        }
    }
    public void sendEvent(int i)
    {
        FirebaseBootstrap.SafeLogEvent("main_menu_game_btn", events[i], 1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
