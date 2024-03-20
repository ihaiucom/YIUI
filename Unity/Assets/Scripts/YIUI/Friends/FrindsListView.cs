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
    public sealed partial class FrindsListView:FrindsListViewBase
    {

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FrindsListView Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"FrindsListView Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"FrindsListView OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"FrindsListView OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"FrindsListView OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"FrindsListView OnOpen");
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