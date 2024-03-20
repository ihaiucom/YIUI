using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Login
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class LoginPanelBase:BasePanel
    {
        public const string PkgName = "Login";
        public const string ResName = "LoginPanel";
        
        public override EWindowOption WindowOption => EWindowOption.BanTween;
        public override EPanelLayer Layer => EPanelLayer.Panel;
        public override EPanelOption PanelOption => EPanelOption.TimeCache;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;
        protected override float CachePanelTime => 10;

        public TMPro.TMP_InputField u_ComEmailInput { get; private set; }
        public TMPro.TMP_InputField u_ComPassworldInput { get; private set; }
        public UnityEngine.UI.Toggle u_ComRememberCheckoutbox { get; private set; }
        public UnityEngine.UI.Button u_ComLoginBtn { get; private set; }
        public UnityEngine.UI.Button u_ComSignupBtn { get; private set; }
        public UnityEngine.UI.Button u_ComForgotPasswordBtn { get; private set; }
        public YIUIBind.UIDataValueString u_DataTitle { get; private set; }
        protected UIEventP0 u_EventLogin { get; private set; }
        protected UIEventHandleP0 u_EventLoginHandle { get; private set; }
        protected UIEventP1<int> u_EventSignup { get; private set; }
        protected UIEventHandleP1<int> u_EventSignupHandle { get; private set; }
        protected UIEventP1<string> u_EventInputEmail { get; private set; }
        protected UIEventHandleP1<string> u_EventInputEmailHandle { get; private set; }
        protected UIEventP1<string> u_EventInputPassword { get; private set; }
        protected UIEventHandleP1<string> u_EventInputPasswordHandle { get; private set; }
        protected UIEventP1<bool> u_EventRemember { get; private set; }
        protected UIEventHandleP1<bool> u_EventRememberHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_ComEmailInput = ComponentTable.FindComponent<TMPro.TMP_InputField>("u_ComEmailInput");
            u_ComPassworldInput = ComponentTable.FindComponent<TMPro.TMP_InputField>("u_ComPassworldInput");
            u_ComRememberCheckoutbox = ComponentTable.FindComponent<UnityEngine.UI.Toggle>("u_ComRememberCheckoutbox");
            u_ComLoginBtn = ComponentTable.FindComponent<UnityEngine.UI.Button>("u_ComLoginBtn");
            u_ComSignupBtn = ComponentTable.FindComponent<UnityEngine.UI.Button>("u_ComSignupBtn");
            u_ComForgotPasswordBtn = ComponentTable.FindComponent<UnityEngine.UI.Button>("u_ComForgotPasswordBtn");
            u_DataTitle = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataTitle");
            u_EventLogin = EventTable.FindEvent<UIEventP0>("u_EventLogin");
            u_EventLoginHandle = u_EventLogin.Add(OnEventLoginAction);
            u_EventSignup = EventTable.FindEvent<UIEventP1<int>>("u_EventSignup");
            u_EventSignupHandle = u_EventSignup.Add(OnEventSignupAction);
            u_EventInputEmail = EventTable.FindEvent<UIEventP1<string>>("u_EventInputEmail");
            u_EventInputEmailHandle = u_EventInputEmail.Add(OnEventInputEmailAction);
            u_EventInputPassword = EventTable.FindEvent<UIEventP1<string>>("u_EventInputPassword");
            u_EventInputPasswordHandle = u_EventInputPassword.Add(OnEventInputPasswordAction);
            u_EventRemember = EventTable.FindEvent<UIEventP1<bool>>("u_EventRemember");
            u_EventRememberHandle = u_EventRemember.Add(OnEventRememberAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventLogin.Remove(u_EventLoginHandle);
            u_EventSignup.Remove(u_EventSignupHandle);
            u_EventInputEmail.Remove(u_EventInputEmailHandle);
            u_EventInputPassword.Remove(u_EventInputPasswordHandle);
            u_EventRemember.Remove(u_EventRememberHandle);

        }
     
        protected virtual void OnEventLoginAction(){}
        protected virtual void OnEventSignupAction(int p1){}
        protected virtual void OnEventInputEmailAction(string p1){}
        protected virtual void OnEventInputPasswordAction(string p1){}
        protected virtual void OnEventRememberAction(bool p1){}
   
   
    }
}