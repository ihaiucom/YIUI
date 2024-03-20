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
    public sealed partial class FrindsRequestView:FrindsRequestViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FrindsRequestView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"FrindsRequestView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"FrindsRequestView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"FrindsRequestView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"FrindsRequestView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"FrindsRequestView OnOpen");
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