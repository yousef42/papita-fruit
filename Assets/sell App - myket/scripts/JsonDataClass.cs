using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonDataClass 
{
    public string status;
    public List<buttonsList> applicationsData;

}
public class NumDataClass
{
    public string status;
    public List<NumList> interData;

}
public class Invention
{
    public string status;
    public List<InviteData> interData;

}
public class BuyDataClass
{
    public string status;
    public List<buyList> interData;

}
public class checkBuyDataClass
{
    public string status;
    public List<CheckbuyList> interData;

}

public class BuyInMarkets
{
    public string status;
    public List<CheckListMarket> interData;

}

public class checkDicoDataClass
{
    public string status;
    public List<CheckDicoList> interData;

}
public class interDataClass
{
    public string firstEpisod;
    public List<dataList> interData;

}

[System.Serializable]
public class buttonsList
{
    public string id;
    public string name;
    public string app_status;
    public string price;
    public string old_price;
    public string show_old_price;
    public string show_discount;
    public string link;
    public string free_code_google;
    public string free_code_bazar;
    public string free_code_ios;
    public string link_insta;
    public string link_whatsapp;
    public string link_telegram;
    public string free_see_at_first_time;
    public string show_gift;
    public string show_button_in_gift;
    public string gift_text;
    public string gift_link;
    public string show_video;
    public string video_link;
    public string number_of_invitations_to_be_free;



}
[System.Serializable]
public class NumList
{
  
    public string status;
    public int[] tariffs;
    public string invate_code;

}
[System.Serializable]
public class dataList
{
   
    public string status;
    public string name;

}
[System.Serializable]
public class InviteData
{

    public string status;
    public string Number_of_invitations;

}
[System.Serializable]
public class buyList
{
  
    public string status;
    public string link;
}

[System.Serializable]
public class CheckbuyList
{

    public string status;
    public int[] tariffs;

}

[System.Serializable]
public class CheckListMarket
{

    public string status;

}

[System.Serializable]
public class CheckDicoList
{

    public string status;
    public string new_price;
    public string discount;

}




[System.Serializable]
public class inAppmessage
{
    public string status;
    public List<inAppmessageData> interData;

}
[System.Serializable]
public class inAppmessageData
{
    public string status;
    public int inAppPurchase;
    public int PlaythegameOnce;
    public int sellWhatsapp;
    public string matn;
    public string matn2;
    public string matn3;
    public int fromAlphabet;
    public int until;
    public int fontSize;
    public int inappmessageTime;
    public string inappmessage;
    public string TitleInappMessage;
    public int ShowAd;
    public string PhoneNumberWhatsapp;


}








