
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Apache License. Copyright (C) Bobardo Studio - All Rights Reserved.
 * Unauthorized publishing the plugin with different name is strictly prohibited.
 * This plugin is free and no one has right to sell it to others.
 * http://bobardo.com
 * http://opensource.org/licenses/Apache-2.0
 */

[RequireComponent(typeof(StoreHandler))]
public class InAppStore : MonoBehaviour
{
    public static InAppStore instance;
    public Product[] products;
    public addStatusBazar addStatus;
 
    public MainAppController mainApp;
    public int application_id;
    public int witchTerm;

    // public GameObject 
    private int selectedProductIndex;

    void Awake()
    {
        if (instance == null)
            instance = this;



    }

    void Start()
    {
        mainApp = GameObject.Find("manager").GetComponent<MainAppController>();
       
        application_id = mainApp.application_id;
        witchTerm = mainApp.witchTerm;
    }

    public void purchasedSuccessful(Purchase purchase)
    {
        // purchase was successful, give user the pruduct

        switch (selectedProductIndex)
        {
            case 0: // kharid ghadimi
                UnityEngine.PlayerPrefs.SetInt("status", application_id);

                UnityEngine.PlayerPrefs.SetString("tarriff_id", application_id.ToString());


                addStatus.SetStatus();


                break;
            case 1: // package1
                UnityEngine.PlayerPrefs.SetInt("status", witchTerm);



                addStatus.SetStatus();

                break;
            case 2: // package2

                UnityEngine.PlayerPrefs.SetInt("status", 13);
                UnityEngine.PlayerPrefs.SetInt("buyTypeAllApps", 1);


                addStatus.SetStatus();

                break;
            case 3: // buy just this
                UnityEngine.PlayerPrefs.SetInt("status", application_id);

                addStatus.SetStatus();
                break;

            case 4: // package1off

                UnityEngine.PlayerPrefs.SetInt("status", witchTerm);
                UnityEngine.PlayerPrefs.SetInt("hasDico", 1);

                addStatus.SetStatus();
                break;
            case 5: // package2off

                UnityEngine.PlayerPrefs.SetInt("status", 13);
                UnityEngine.PlayerPrefs.SetInt("hasDico", 1);

                addStatus.SetStatus();
                break;
            case 6: // buyJustThisOff

                UnityEngine.PlayerPrefs.SetInt("status", application_id);
                UnityEngine.PlayerPrefs.SetInt("hasDico", 1);

                addStatus.SetStatus();
                break;
            case 7: // buytype18

                UnityEngine.PlayerPrefs.SetInt("status", 13);
                UnityEngine.PlayerPrefs.SetInt("hasDico", 1);

                addStatus.SetStatus();
                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
        
    }

    public void purchasedFailed(int errorCode, string info)
    {
        // purchase failed. show user the proper message
        switch (errorCode)
        {
            case 1: // error connecting cafeBazaar
            case 2: // error connecting cafeBazaar
            case 4: // error connecting cafeBazaar
            case 5: // error connecting cafeBazaar

                break;
            case 6: // user canceled the purchase

                break;
            case 7: // purchase failed

                break;
            case 8: // failed to consume product. but the purchase was successful.

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }

    public void userHasThisProduct(Purchase purchase)
    {
        // user already has this product
        switch (selectedProductIndex)
        {
            case 0: // first product

                break;
            case 1: // second product

                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
    }

    public void failToGetUserInventory(int errorCode, string info)
    {
        // user has not this product or some error happened
        switch (errorCode)
        {
            case 3:  // error connecting cafeBazaar
            case 10: // error connecting cafeBazaar

                break;
            case 9: // user didn't login to cafeBazaar

                break;
            case 11: // user has not this product

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }
    string buyType;
    public void purchaseProduct(int productIndex)
    {
        if (UnityEngine.PlayerPrefs.GetInt("init phone") == 0)
        {
            LockLevelsManager.instance.witchProductForBuy = productIndex;
            LockLevelsManager.instance.userBtn();
            UnityEngine.PlayerPrefs.SetInt("comeForBuy", 1);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt("goForBuy", 1);

            if (productIndex == 1 | productIndex == 4)
            {

                buyType = witchTerm.ToString();
                UnityEngine.PlayerPrefs.SetString("tarriff_id", buyType);
            }
            else if (productIndex == 2 | productIndex == 5 | productIndex == 7)
            {

                if (UnityEngine.PlayerPrefs.GetInt("status") == 14 | UnityEngine.PlayerPrefs.GetInt("status") == 15
                    | UnityEngine.PlayerPrefs.GetInt("status") == 16 | UnityEngine.PlayerPrefs.GetInt("status") == 17)
                {

                    buyType = "18";
                }
                else
                {
                    buyType = "13";
                }

                UnityEngine.PlayerPrefs.SetString("tarriff_id", buyType);
            }
            else if (productIndex == 3 | productIndex == 6)
            {

                buyType = application_id.ToString();
                UnityEngine.PlayerPrefs.SetString("tarriff_id", buyType);
            }

            this.GetComponent<StoreHandler>().payload = "ver :" + Application.version + " / " + UnityEngine.PlayerPrefs.GetString("phoneNumber");


            selectedProductIndex = productIndex;
            Product product = products[productIndex];
            if (product.type == Product.ProductType.Consumable)
            {
                GetComponent<StoreHandler>().BuyAndConsume(product.productId);
            }
            else if (product.type == Product.ProductType.NonConsumable)
            {
                GetComponent<StoreHandler>().BuyProduct(product.productId);
            }
        }
  
    }

    public void checkIfUserHasProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        GetComponent<StoreHandler>().CheckInventory(products[productIndex].productId);
    }

    void afterbuy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
  

}

