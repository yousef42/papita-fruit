using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class type : MonoBehaviour {
    public static type instance;
    public bool testbazaar;
    string word = null;
    int wordIndex = -1;
    string alpha=null,alpha2=null;
    public Text myName = null;
    char[] nameChar = new char[30];
    public Text index=null;
    public GameObject mybtn,mybtn2;
    public SlideManager slidemanager;
  
    int show;
    bool mustcheck = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start()
    {
      
    }
  
    void Update()
    {

        if (mustcheck)
        {
            SubmitPasswordButton();
        }



    }

    public void alphabetFunction(string alphabet)
    {
        if (wordIndex <= 9)
        {
            wordIndex++;
            char[] keepchar = alphabet.ToCharArray();
            nameChar[wordIndex] = keepchar[0];
            alpha = nameChar[wordIndex].ToString();
            word = word + alpha;
            myName.text = word;
            myName.color = Color.black;
            index.text = wordIndex.ToString();
        }
  
   
       


    }
    public void del()
    {

        if (wordIndex >= 0)
        {
            


            mybtn.SetActive(false);
            mybtn2.SetActive(false);
            wordIndex--;
            alpha2 = null;
            for(int i = 0; i < wordIndex + 1; i++)
            {
                alpha2 = alpha2 + nameChar[i].ToString();
            }

            word = alpha2;
            myName.text = word;
            index.text = wordIndex.ToString();
            mustcheck = true;
        }


    }

    bool checkNetworkAvailability()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SubmitPasswordButton()
    {
        //Check if length of the string is less than 8

        //Check if the length of the string is more than or equal to 8
        if (System.Text.RegularExpressions.Regex.IsMatch(myName.text, @"^(\+98|0)?9\d{9}$"))
        {
            // Debug.Log("Password Accepted!");



            try
            {
                if (checkNetworkAvailability())
                {
                    if (testbazaar)
                    {
                        mybtn.SetActive(true);
                    }
                    else
                    {
                        Invoke("change", 1f);
                    }


                }
                else
                {

                    mybtn.SetActive(false);
                    mybtn2.SetActive(false);
                }

            }
            catch (System.Exception)
            {
            }

        }




    }
    GameObject btnNum;
    void change()
    {
        //  btnNum = GameObject.Find("back num + btn");
        // btnNum.GetComponent<Button>().interactable = true;
        // mybtn.SetActive(true);

        mybtn2.SetActive(true);
        mustcheck = false;

        // slidemanager.kpi.SetActive(true);
        slidemanager.keyboard.SetActive(false);

    }

    void OnMouseDown()
    {
        Debug.Log("sss");
    }

    public void sabtshomare()
    {
      
        Invoke("fadeout", 2f);
    }

    void fadeout()
    {
     //   internumber.SetActive(false);
    }
}
