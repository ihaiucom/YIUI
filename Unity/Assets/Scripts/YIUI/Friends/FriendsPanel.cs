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
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class FriendsPanel:FriendsPanelBase
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FriendsPanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"FriendsPanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"FriendsPanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"FriendsPanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"FriendsPanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"FriendsPanel OnOpen");
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


        #endregion Event结束

    }
}