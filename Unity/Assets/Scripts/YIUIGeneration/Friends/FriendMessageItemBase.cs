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
    public abstract class FriendMessageItemBase:BaseComponent
    {
        public const string PkgName = "Friends";
        public const string ResName = "FriendMessageItem";
        
        protected UIEventP0 u_EventClickClaim { get; private set; }
        protected UIEventHandleP0 u_EventClickClaimHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_EventClickClaim = EventTable.FindEvent<UIEventP0>("u_EventClickClaim");
            u_EventClickClaimHandle = u_EventClickClaim.Add(OnEventClickClaimAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventClickClaim.Remove(u_EventClickClaimHandle);

        }
     
        protected virtual void OnEventClickClaimAction(){}
   
   
    }
}