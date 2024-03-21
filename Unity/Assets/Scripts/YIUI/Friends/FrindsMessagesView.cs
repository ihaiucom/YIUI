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
        private YIUILoopScroll<FriendMessageData, FriendMessageItem> loopScroll;
        private List<FriendMessageData> dataList;

        #region 生命周期
        
        protected override void Initialize()
        {
            Debug.Log($"FrindsMessagesView Initialize");
            dataList = new List<FriendMessageData>();
            for (int i = 0; i < 50; i ++)
            {
                var data = new FriendMessageData();
                data.role = new RoleData();
                data.msg = new ChatMessageData();
                dataList.Add(data);
            }

            loopScroll = new YIUILoopScroll<FriendMessageData, FriendMessageItem>(u_ComLoopVerticalScroll, RenderItem);
        }

        private void RenderItem(int index, FriendMessageData data, FriendMessageItem item, bool select)
        {
            Debug.Log($"RenderItem {index}");
            item.SetData(data);
        }

        protected override void Start()
        {
            Debug.Log($"FrindsMessagesView Start dataList.Count={dataList.Count}");
            loopScroll.SetDataRefresh(dataList);
        }

        protected override void OnEnable()
        {
            Debug.Log($"FrindsMessagesView OnEnable dataList.Count={dataList.Count}");
            loopScroll.SetDataRefresh(dataList);
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