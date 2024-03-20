using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace YIUI.Loading
{
    /// <summary>
    /// Author  YIUI
    /// Date    2024.3.20
    /// </summary>
    public sealed partial class LoadingPanel:LoadingPanelBase, IYIUIOpen<string, Action<float>, Action<bool>>
    {
    
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"LoadingPanel Initialize");
        }

        protected override void Start()
        {
            Debug.Log($"LoadingPanel Start");
        }

        protected override void OnEnable()
        {
            Debug.Log($"LoadingPanel OnEnable");
        }

        protected override void OnDisable()
        {
            Debug.Log($"LoadingPanel OnDisable");
        }

        protected override void OnDestroy()
        {
            Debug.Log($"LoadingPanel OnDestroy");
        }

        protected override async UniTask<bool> OnOpen()
        {
            await UniTask.CompletedTask;
            Debug.Log($"LoadingPanel OnOpen");
            return true;
        }

        private string openModule;
        private Action<float> openProgressCallback;
        private Action<bool> openEndCallback;
        public async UniTask<bool> OnOpen(string p1, Action<float> p2, Action<bool> p3)
        {
            openModule = p1;
            openProgressCallback = p2;
            openEndCallback = p3;
            SimulteProgress().Forget();
            return true;
        }

        private async UniTask<bool> SimulteProgress()
        {
            u_DataProgess.SetValue(0);
            m_PanelMgr.OpenPanel(openModule);
            await DOTween.To(() => u_DataProgess.GetValue(),
                (x) =>
                {
                    u_DataProgess.SetValue(x);
                    openProgressCallback?.Invoke(x);
                },
                100, 3
            );
            openEndCallback?.Invoke(true);
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