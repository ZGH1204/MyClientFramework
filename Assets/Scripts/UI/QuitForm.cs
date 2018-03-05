using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZFramework;


namespace GGame.UI
{
    public class QuitForm : UGuiForm
    {
        public Image bg;
        public Text txt;

        Sprite loadingBg1;
        Sprite loadingBg2;
        float durtion;

        //读取场景的进度，它的取值范围在0 - 1 之间。    
        float progress = 0; 
        //异步对象    
        AsyncOperation async;

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            loadingBg1 = GameEntry.Resource.LoadSprite("Image/BG/loading_01");
            loadingBg2 = GameEntry.Resource.LoadSprite("Image/BG/loading_02");

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadSceneMode) =>
            {
                txt.text = "加载中 100%";
                GameEntry.UI.CloseAllLoadedUIForms();
            };
             
            bg.sprite = UnityEngine.Random.Range(1, 3) == 2 ? loadingBg2 : loadingBg1;

            //进入loadScene方法。    
            StartCoroutine(loadScene()); 
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            progress += elapseSeconds * 15f;
            if (async != null)
            { 
                if (progress < (int)(async.progress * 100))
                {
                    progress = (int)(async.progress * 100);
                }
            }

            progress = progress > 100 ? 100 : progress;
            txt.text = "加载中 " + Convert.ToInt32(progress) + "%"; 
        }

        protected internal override void OnClose(object userData)
        {
            base.OnClose(userData);
        }
 
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }
          
 
        IEnumerator loadScene()
        {
            yield return new WaitForSeconds(3f);
            yield return new WaitForEndOfFrame();//加上这么一句就可以先显示加载画面然后再进行加载  
            async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Battle", UnityEngine.SceneManagement.LoadSceneMode.Single); 
        }

    }
}