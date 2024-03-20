using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Home
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class HomePanelBase:BasePanel
    {
        public const string PkgName = "Home";
        public const string ResName = "HomePanel";
        
        public override EWindowOption WindowOption => EWindowOption.BanTween;
        public override EPanelLayer Layer => EPanelLayer.Scene;
        public override EPanelOption PanelOption => EPanelOption.None;
        public override EPanelStackOption StackOption => EPanelStackOption.VisibleTween;
        public override int Priority => 0;

        
        protected sealed override void UIBind()
        {

        }

        protected sealed override void UnUIBind()
        {

        }
     
   
   
    }
}