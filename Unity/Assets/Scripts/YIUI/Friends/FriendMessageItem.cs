using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Friends
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.21
    /// </summary>
    public sealed partial class FriendMessageItem:FriendMessageItemBase
    {
        public FriendMessageData data;
        public void SetData(FriendMessageData data)
        {
            this.data = data;
        }
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FriendMessageItem Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"FriendMessageItem Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"FriendMessageItem OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"FriendMessageItem OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"FriendMessageItem OnDestroy");
        }

        #endregion

        #region Event开始


       
        protected override void OnEventClickClaimAction()
        {
            
        }
        #endregion Event结束

    }
}