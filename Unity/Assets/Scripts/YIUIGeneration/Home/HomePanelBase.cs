using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Home
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class HomePanelBase:BasePanel
    {
        public const string PkgName = "Home";
        public const string ResName = "HomePanel";
        
        public override EWindowOption WindowOption => EWindowOption.BanTween;
        public override EPanelLayer Layer => EPanelLayer.Panel;
        public override EPanelOption PanelOption => EPanelOption.None;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;
        public YIUIFramework.UI3DDisplay u_ComYIUI3DDisplayUI3DDisplay { get; private set; }
        protected UIEventP0 u_EventClickBackBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickBackBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickCharacterPreBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickCharacterPreBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickCharacterNextBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickCharacterNextBtnHandle { get; private set; }
        protected UIEventP1<string> u_EventOpenPanel { get; private set; }
        protected UIEventHandleP1<string> u_EventOpenPanelHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_ComYIUI3DDisplayUI3DDisplay = ComponentTable.FindComponent<YIUIFramework.UI3DDisplay>("u_ComYIUI3DDisplayUI3DDisplay");
            u_EventClickBackBtn = EventTable.FindEvent<UIEventP0>("u_EventClickBackBtn");
            u_EventClickBackBtnHandle = u_EventClickBackBtn.Add(OnEventClickBackBtnAction);
            u_EventClickCharacterPreBtn = EventTable.FindEvent<UIEventP0>("u_EventClickCharacterPreBtn");
            u_EventClickCharacterPreBtnHandle = u_EventClickCharacterPreBtn.Add(OnEventClickCharacterPreBtnAction);
            u_EventClickCharacterNextBtn = EventTable.FindEvent<UIEventP0>("u_EventClickCharacterNextBtn");
            u_EventClickCharacterNextBtnHandle = u_EventClickCharacterNextBtn.Add(OnEventClickCharacterNextBtnAction);
            u_EventOpenPanel = EventTable.FindEvent<UIEventP1<string>>("u_EventOpenPanel");
            u_EventOpenPanelHandle = u_EventOpenPanel.Add(OnEventOpenPanelAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventClickBackBtn.Remove(u_EventClickBackBtnHandle);
            u_EventClickCharacterPreBtn.Remove(u_EventClickCharacterPreBtnHandle);
            u_EventClickCharacterNextBtn.Remove(u_EventClickCharacterNextBtnHandle);
            u_EventOpenPanel.Remove(u_EventOpenPanelHandle);

        }
     
        protected virtual void OnEventClickBackBtnAction(){}
        protected virtual void OnEventClickCharacterPreBtnAction(){}
        protected virtual void OnEventClickCharacterNextBtnAction(){}
        protected virtual void OnEventOpenPanelAction(string p1){}
   
   
    }
}