using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace YIUI.Common
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class TopBarView:TopBarViewBase
    {

        #region 生命周期

        public BasePanel panel;
        
        public void InitPanel(BasePanel panel, string title)
        {
            this.panel = panel;
            this.Title = title;
        }
        
        protected override void Initialize()
        {
            Debug.Log($"TopBarView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"TopBarView Start");
            u_DataCoins.SetValue(Random.Range(1, 9999999));
            u_DataGems.SetValue(Random.Range(1, 9999999));
        }

        protected override void OnEnable()
        {
            Debug.Log($"TopBarView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"TopBarView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"TopBarView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"TopBarView OnOpen");
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }

        public string Title
        {
            get
            {
                return u_DataTitle.GetValue();
            }
            set
            {
                u_DataTitle.SetValue(value);
            }
        }

        #endregion

        #region Event开始


       
        protected override void OnEventClickBackBtnAction()
        {
            panel?.Close();
        }
       
        protected override void OnEventClickSettingBtnAction()
        {
            
        }
       
        protected override void OnEventClickNotificationBtnAction()
        {
            
        }
       
        protected override void OnEventClickCoinsBtnAction()
        {
            
        }
       
        protected override void OnEventClickGemsBtnAction()
        {
        }
        #endregion Event结束

    }
}