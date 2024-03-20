using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Friends
{

    public enum EFriendsPanelViewEnum
    {
        FrindsListView = 1,
        FrindsRequestView = 2,
        FrindsMessagesView = 3,
    }

    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class FriendsPanelBase:BasePanel
    {
        public const string PkgName = "Friends";
        public const string ResName = "FriendsPanel";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EPanelLayer Layer => EPanelLayer.Panel;
        public override EPanelOption PanelOption => EPanelOption.None;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;
        public YIUI.Common.TopBarView u_UITopBarView { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_UITopBarView = CDETable.FindUIBase<YIUI.Common.TopBarView>("TopBarView");

        }

        protected sealed override void UnUIBind()
        {

        }
     
   
   
    }
}