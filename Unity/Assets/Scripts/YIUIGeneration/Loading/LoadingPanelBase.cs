using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Loading
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class LoadingPanelBase:BasePanel
    {
        public const string PkgName = "Loading";
        public const string ResName = "LoadingPanel";
        
        public override EWindowOption WindowOption => EWindowOption.BanTween;
        public override EPanelLayer Layer => EPanelLayer.Top;
        public override EPanelOption PanelOption => EPanelOption.TimeCache;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;
        protected override float CachePanelTime => 10;

        public YIUIBind.UIDataValueFloat u_DataProgess { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_DataProgess = DataTable.FindDataValue<YIUIBind.UIDataValueFloat>("u_DataProgess");

        }

        protected sealed override void UnUIBind()
        {

        }
     
   
   
    }
}