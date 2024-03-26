using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Test
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.22
    /// </summary>
    public sealed partial class TestPanel:TestPanelBase
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"TestPanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"TestPanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"TestPanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"TestPanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"TestPanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"TestPanel OnOpen");
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