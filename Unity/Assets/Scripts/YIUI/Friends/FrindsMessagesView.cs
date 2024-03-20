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
    public sealed partial class FrindsMessagesView:FrindsMessagesViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FrindsMessagesView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"FrindsMessagesView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"FrindsMessagesView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"FrindsMessagesView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"FrindsMessagesView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"FrindsMessagesView OnOpen");
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