using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using YIUI.Friends;
using YIUI.Home;
using YIUI.Loading;

namespace YIUI.Login
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class LoginPanel:LoginPanelBase
    {
        private string email;
        private string password;
        private bool remeber;
        
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"LoginPanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"LoginPanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"LoginPanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"LoginPanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"LoginPanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"LoginPanel OnOpen");
            u_DataTitle.SetValue("登录");
            u_ComLoginBtn.onClick.AddListener(onClickLoginBtn);
            return true;
        }

        private void onClickLoginBtn()
        {
            Debug.Log("onClickLoginBtn");
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


       
        protected override void OnEventLoginAction()
        {
            Debug.Log($"OnEventLoginAction");
            DoLogin().Forget();
        }
        
        private async UniTask<bool> DoLogin()
        { 
            Debug.Log("DoLogin Begin");
            await PanelMgr.Inst.OpenPanelAsync<LoadingPanel, string,Action<float>, Action<bool>>(HomePanel.ResName, 
                (float progress) =>
                {
                    Debug.Log($"Open Loading Callback progress={progress}");
                },
                (bool isOk) =>
                {
                    Debug.Log($"Open Loading Callback end={isOk}");
                    m_PanelMgr.ClosePanel<LoadingPanel>();
                    Close();
                }
            );
            Debug.Log("DoLogin End");
            return true;
        }
       
        protected override void OnEventSignupAction(int p1)
        {
            Debug.Log($"OnEventSignupAction: {p1}");
            PanelMgr.Inst.OpenPanel<FriendsPanel>();
        }
       
        protected override void OnEventInputEmailAction(string p1)
        {
            email = p1;
            Debug.Log($"OnEventInputEmailAction: {p1}");
        }
       
        protected override void OnEventInputPasswordAction(string p1)
        {
            password = p1;
            Debug.Log($"OnEventInputPasswordAction: {p1}");
            
        }
       
        protected override void OnEventRememberAction(bool p1)
        {
            remeber = p1;
            Debug.Log($"OnEventRememberAction: {p1}");
            
        }
        #endregion Event结束

    }
}