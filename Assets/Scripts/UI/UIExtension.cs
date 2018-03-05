using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZFramework;
using ZFramework.Runtime;

namespace GGame.UI
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        //public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        //{
        //    return uiComponent.HasUIForm((int)uiFormId, uiGroupName);
        //}

        //public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        //{
        //    IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
        //    DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
        //    if (drUIForm == null)
        //    {
        //        return false;
        //    }

        //    string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
        //    if (string.IsNullOrEmpty(uiGroupName))
        //    {
        //        return uiComponent.HasUIForm(assetName);
        //    }

        //    IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
        //    if (uiGroup == null)
        //    {
        //        return false;
        //    }

        //    return uiGroup.HasUIForm(assetName);
        //}

        //public static UGuiForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        //{
        //    return uiComponent.GetUIForm((int)uiFormId, uiGroupName);
        //}

        //public static UGuiForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        //{
        //    IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
        //    DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
        //    if (drUIForm == null)
        //    {
        //        return null;
        //    }

        //    string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
        //    UIForm uiForm = null;
        //    if (string.IsNullOrEmpty(uiGroupName))
        //    {
        //        uiForm = uiComponent.GetUIForm(assetName);
        //        if (uiForm == null)
        //        {
        //            return null;
        //        }

        //        return (UGuiForm)uiForm.Logic;
        //    }

        //    IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
        //    if (uiGroup == null)
        //    {
        //        return null;
        //    }

        //    uiForm = (UIForm)uiGroup.GetUIForm(assetName);
        //    if (uiForm == null)
        //    {
        //        return null;
        //    }

        //    return (UGuiForm)uiForm.Logic;
        //}

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static bool OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static bool OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            UIFormCSV uiForm = GGame.GameEntry.DateTable.GetTypeById<UIFormCSV>(DataTableName.UIForm, uiFormId);

            if (uiForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(uiForm.AssetName); 
            return uiComponent.OpenUIForm(assetName, uiForm.UIGroupName, uiForm.PauseCoveredUIForm, userData);
        }

        //public static void OpenDialog(this UIComponent uiComponent, DialogParams dialogParams)
        //{
        //    if (((ProcedureBase)GameEntry.Procedure.CurrentProcedure).UseNativeDialog)
        //    {
        //        OpenNativeDialog(dialogParams);
        //    }
        //    else
        //    {
        //        uiComponent.OpenUIForm(UIFormId.DialogForm, dialogParams);
        //    }
        //}

        //private static void OpenNativeDialog(DialogParams dialogParams)
        //{
        //    throw new System.NotImplementedException("OpenNativeDialog");
        //}
    }
}