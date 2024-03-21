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

        private YIUI3DDisplayExtend model3DDisplayExtend;
        private List<string> modleList = new List<string>();
        private int modelIndex = 0;
        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"HomePanel Initialize");
            model3DDisplayExtend = new YIUI3DDisplayExtend(u_ComYIUI3DDisplayUI3DDisplay);
            modleList = new List<string>() { "Cube", "Sphere"};
            modleList = new List<string>() { "Footman", "Peasant", "phoenix", "Pig", "Sheep", "Ballista"};
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
            SetShowModel(modelIndex);
            return true;
        }

        protected override async UniTask<bool> OnOpen(ParamVo param)
        {
            return await base.OnOpen(param);
        }
        
        #endregion

        #region Event开始


       
        protected override void OnEventClickBackBtnAction()
        {
            Close();
        }
       
        protected override void OnEventClickCharacterPreBtnAction()
        {
            int index = modelIndex - 1;
            if (index < 0)
            {
                index = modleList.Count - 1;
            }
            SetShowModel(index);
        }
       
        protected override void OnEventClickCharacterNextBtnAction()
        {
            int index = modelIndex + 1;
            if (index >= modleList.Count)
            {
                index = 0;
            }
            SetShowModel(index);
        }
        
        #endregion Event结束

        private void SetShowModel(int index)
        {
            modelIndex = index;
            string modelName = modleList[index];
            Debug.Log($"SetShowModel: {index}, {modelName}");
            model3DDisplayExtend.Show(modelName);
        }

    }
}