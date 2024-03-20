using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUI.Friends
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class FrindsListViewBase:BaseView
    {
        public const string PkgName = "Friends";
        public const string ResName = "FrindsListView";
        
        public override EWindowOption WindowOption => EWindowOption.None;
        public override EViewWindowType ViewWindowType => EViewWindowType.View;
        public override EViewStackOption StackOption => EViewStackOption.VisibleTween;

        
        protected sealed override void UIBind()
        {

        }

        protected sealed override void UnUIBind()
        {

        }
     
   
   
    }
}