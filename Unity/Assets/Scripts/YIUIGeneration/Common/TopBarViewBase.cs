using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Common
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class TopBarViewBase:BaseView
    {
        public const string PkgName = "Common";
        public const string ResName = "TopBarView";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EViewWindowType ViewWindowType => EViewWindowType.View;
        public override EViewStackOption StackOption => EViewStackOption.VisibleTween;
        public YIUIBind.UIDataValueString u_DataTitle { get; private set; }
        public YIUIBind.UIDataValueInt u_DataCoins { get; private set; }
        public YIUIBind.UIDataValueInt u_DataGems { get; private set; }
        protected UIEventP0 u_EventClickBackBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickBackBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickSettingBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickSettingBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickNotificationBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickNotificationBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickCoinsBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickCoinsBtnHandle { get; private set; }
        protected UIEventP0 u_EventClickGemsBtn { get; private set; }
        protected UIEventHandleP0 u_EventClickGemsBtnHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_DataTitle = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataTitle");
            u_DataCoins = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataCoins");
            u_DataGems = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataGems");
            u_EventClickBackBtn = EventTable.FindEvent<UIEventP0>("u_EventClickBackBtn");
            u_EventClickBackBtnHandle = u_EventClickBackBtn.Add(OnEventClickBackBtnAction);
            u_EventClickSettingBtn = EventTable.FindEvent<UIEventP0>("u_EventClickSettingBtn");
            u_EventClickSettingBtnHandle = u_EventClickSettingBtn.Add(OnEventClickSettingBtnAction);
            u_EventClickNotificationBtn = EventTable.FindEvent<UIEventP0>("u_EventClickNotificationBtn");
            u_EventClickNotificationBtnHandle = u_EventClickNotificationBtn.Add(OnEventClickNotificationBtnAction);
            u_EventClickCoinsBtn = EventTable.FindEvent<UIEventP0>("u_EventClickCoinsBtn");
            u_EventClickCoinsBtnHandle = u_EventClickCoinsBtn.Add(OnEventClickCoinsBtnAction);
            u_EventClickGemsBtn = EventTable.FindEvent<UIEventP0>("u_EventClickGemsBtn");
            u_EventClickGemsBtnHandle = u_EventClickGemsBtn.Add(OnEventClickGemsBtnAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventClickBackBtn.Remove(u_EventClickBackBtnHandle);
            u_EventClickSettingBtn.Remove(u_EventClickSettingBtnHandle);
            u_EventClickNotificationBtn.Remove(u_EventClickNotificationBtnHandle);
            u_EventClickCoinsBtn.Remove(u_EventClickCoinsBtnHandle);
            u_EventClickGemsBtn.Remove(u_EventClickGemsBtnHandle);

        }
     
        protected virtual void OnEventClickBackBtnAction(){}
        protected virtual void OnEventClickSettingBtnAction(){}
        protected virtual void OnEventClickNotificationBtnAction(){}
        protected virtual void OnEventClickCoinsBtnAction(){}
        protected virtual void OnEventClickGemsBtnAction(){}
   
   
    }
}