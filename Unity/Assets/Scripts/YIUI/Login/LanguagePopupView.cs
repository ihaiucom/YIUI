using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;

namespace YIUI.Login
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.22
    /// </summary>
    public sealed partial class LanguagePopupView:LanguagePopupViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"LanguagePopupView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"LanguagePopupView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"LanguagePopupView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"LanguagePopupView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"LanguagePopupView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"LanguagePopupView OnOpen");
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


       
        protected override void OnEventSelectLangeuageAction(string p1)
        {
            I2LocalizeMgr.Inst.SetLanguage(p1,true);
            Close();
        }
       
        protected override void OnEventClickCloseAction()
        {
            Close();
        }
        #endregion Event结束

    }
}