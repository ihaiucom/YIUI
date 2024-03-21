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
    public abstract class LanguagePopupViewBase:BaseView
    {
        public const string PkgName = "Login";
        public const string ResName = "LanguagePopupView";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EViewWindowType ViewWindowType => EViewWindowType.Popup;
        public override EViewStackOption StackOption => EViewStackOption.VisibleTween;
        protected UIEventP1<string> u_EventSelectLangeuage { get; private set; }
        protected UIEventHandleP1<string> u_EventSelectLangeuageHandle { get; private set; }
        protected UIEventP0 u_EventClickClose { get; private set; }
        protected UIEventHandleP0 u_EventClickCloseHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_EventSelectLangeuage = EventTable.FindEvent<UIEventP1<string>>("u_EventSelectLangeuage");
            u_EventSelectLangeuageHandle = u_EventSelectLangeuage.Add(OnEventSelectLangeuageAction);
            u_EventClickClose = EventTable.FindEvent<UIEventP0>("u_EventClickClose");
            u_EventClickCloseHandle = u_EventClickClose.Add(OnEventClickCloseAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventSelectLangeuage.Remove(u_EventSelectLangeuageHandle);
            u_EventClickClose.Remove(u_EventClickCloseHandle);

        }
     
        protected virtual void OnEventSelectLangeuageAction(string p1){}
        protected virtual void OnEventClickCloseAction(){}
   
   
    }
}