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
    public sealed partial class FriendsPanel:FriendsPanelBase,IYIUIOpen<EFriendsPanelViewEnum>
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FriendsPanel Initialize");
            u_UITopBarView.InitPanel(this, PkgName);
            u_DataTab.AddValueChangeAction(OnTabChange);
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
            u_DataTab.RemoveValueChangeAction(OnTabChange);
        }

        protected override async UniTask<bool> OnOpen()
        {
            Debug.Log($"FriendsPanel OnOpen");
            await OnOpen(EFriendsPanelViewEnum.FrindsListView);
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


        #endregion Event结束

        public async UniTask<bool> OnOpen(EFriendsPanelViewEnum p1)
        {
            u_DataTab.SetValue((int)p1, false, false);
            await OpenViewAsync(p1.ToString());
            return true;
        }
        private void OnTabChange(int now, int old)
        {
            OpenViewAsync(((EFriendsPanelViewEnum)now).ToString()).Forget();
        }
    }
}