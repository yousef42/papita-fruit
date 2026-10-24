
using UnityEngine;
using UnityEngine.UI;

public class CheckMobileNumber : MonoBehaviour {

    string m_MyString;

    public InputField m_MyInputField;
    public GameObject m_MyButton;
    public GameObject m_MyTextAlarm;
    bool hasspace;
    int activation2,activation;

    int a = 0;
    void Start()
    {
        //Call SubmitPasswordButton function when the user clicks the Button
        // m_MyButton.onClick.AddListener(SubmitPasswordButton);
        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
        activation = UnityEngine.PlayerPrefs.GetInt("activation");
    }

    void Update()
    {
        //Set the string to be that of the InputField
        m_MyString = m_MyInputField.text;

        if (activation2 > 0 | activation > 0)
        {

        }
        else
        {
            SubmitPasswordButton();
        }
    
       
       
    }

    //Check the length of the string and output a message to the console window
    public void SubmitPasswordButton()
    {
        //Check if length of the string is less than 8
        if (m_MyString.Length < 11 )
        {
           // Debug.Log("Password not long enough!");

           m_MyButton.SetActive(false);
           

        }
        //Check if the length of the string is more than or equal to 8
        if (m_MyString.Length >= 11 && System.Text.RegularExpressions.Regex.IsMatch(m_MyInputField.text, @"^(\+98|0)?9\d{9}$"))
        {
            // Debug.Log("Password Accepted!");
           
                try
                {
                    if (checkNetworkAvailability())
                    {
                        m_MyButton.SetActive(true);
                        m_MyTextAlarm.SetActive(false);
                    }
                    else
                    {
                     
                        m_MyTextAlarm.SetActive(true);
                        m_MyButton.SetActive(false);
                    }

                }
                catch (System.Exception)
                {
                }
           
          
          

         }
            
            
            else
            {
                      if(m_MyString.Length >= 11)
                        {
                              m_MyTextAlarm.SetActive(true);
                        }
          
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
}
