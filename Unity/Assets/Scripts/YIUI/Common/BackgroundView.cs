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
    /// Date    2024.3.22
    /// </summary>
    public sealed partial class BackgroundView:BackgroundViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"BackgroundView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"BackgroundView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"BackgroundView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"BackgroundView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"BackgroundView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"BackgroundView OnOpen");
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