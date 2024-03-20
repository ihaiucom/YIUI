using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Home
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class HomePanel:HomePanelBase
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"HomePanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"HomePanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"HomePanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"HomePanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"HomePanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"HomePanel OnOpen");
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