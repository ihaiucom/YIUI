using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Friends
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class FrindsMessagesViewBase:BaseView
    {
        public const string PkgName = "Friends";
        public const string ResName = "FrindsMessagesView";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EViewWindowType ViewWindowType => EViewWindowType.View;
        public override EViewStackOption StackOption => EViewStackOption.VisibleTween;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComLoopVerticalScroll { get; private set; }
        public YIUI.Friends.FriendMessageItem u_UIFriendListMessage { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_ComLoopVerticalScroll = ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComLoopVerticalScroll");
            u_UIFriendListMessage = CDETable.FindUIBase<YIUI.Friends.FriendMessageItem>("FriendListMessage");

        }

        protected sealed override void UnUIBind()
        {

        }
     
   
   
    }
}