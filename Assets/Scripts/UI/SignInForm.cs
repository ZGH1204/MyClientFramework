using GGame;
using GGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInForm : UGuiForm
{ 
    public GameObject btnOK;
    public Image logoBg;

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);

    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);
         

    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

    }

    public void OK()
    {
         
        GameEntry.UI.OpenUIForm(UIFormId.LoginForm, null);
         

    }

}
