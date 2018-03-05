using UnityEngine.UI;
using ZFramework;
using GGame;
using System.Collections;
using UnityEngine;

namespace GGame.UI
{ 
    public class LoginForm : UGuiForm
    {
        public InputField nameInputF;
        public GameObject btnOK;
        public Image logoBg;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            nameInputF.gameObject.SetActive(false);
            btnOK.SetActive(false);
            StartCoroutine(ShowAni());

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

        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);



        }
         

        public void OK()
        {
            if (string.IsNullOrEmpty(nameInputF.text))
            {
                Log.Info("请输入玩家名字");
            }
            else
            {
                Log.Info("开始登陆");
            }

            

            GameEntry.UI.OpenUIForm(UIFormId.SignInForm, null);



        }

        IEnumerator ShowAni()
        {
            yield return new WaitForSeconds(3f); 
            logoBg.CrossFadeAlpha(0, 1f, true);
            yield return new WaitForSeconds(1f);
            nameInputF.gameObject.SetActive(true);
            btnOK.SetActive(true); 
        }
    }
}
