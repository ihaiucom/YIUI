using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Common
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class TopBarView:TopBarViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"TopBarView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"TopBarView Start");
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
        
        #endregion

        #region Event开始


        #endregion Event结束

    }
}