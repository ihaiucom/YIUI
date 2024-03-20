using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;
using YIUI.Friends;
using YIUI.Login;
using YooAsset; //如果不是用的Yoo资源管理器就可以删除掉
using Object = UnityEngine.Object;

//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

namespace YIUIFramework
{
    /// <summary>
    /// 主入口 演示作用参考
    /// </summary>
    public class Main : MonoBehaviour
    {
        private ResourcePackage package;

        /// <summary>
        /// 重点都在这里  一定要吧你项目中的资源管理  同步加载 异步加载 释放 注入到YIUI框架中
        /// 用什么资源管理都可以 Demo中用的YooAsset为案例 并非必须使用此资源管理
        /// </summary>
        private void Awake()
        {
            //关联UI工具中自动生成绑定代码 Tools >> YIUI自动化工具 >> 发布 >> UI自动生成绑定替代反射代码
            UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
            
            //YIUI会用到的各种加载 需要自行实现 Demo中使用的是YooAsset 根据自己项目的资源管理器实现下面的方法
            YIUILoadDI.LoadAssetFunc           = LoadAsset;               //同步加载
            YIUILoadDI.LoadAssetAsyncFunc      = LoadAssetAsync;          //异步加载
            YIUILoadDI.ReleaseAction           = ReleaseAction;           //释放
            YIUILoadDI.VerifyAssetValidityFunc = VerifyAssetValidityFunc; //检查
            YIUILoadDI.ReleaseAllAction        = ReleaseAllAction;        //释放所有
        }
        
        //这里是简单的本地记录YooAsset 根据你项目应该有一个资源管理器统一管理这里只是演示所以很简陋
        private Dictionary<int, AssetHandle> m_AllHandle = new Dictionary<int, AssetHandle>();
        
        /// <summary>
        /// 释放方法
        /// </summary>
        /// <param name="hashCode">加载时所给到的唯一ID</param>
        private void ReleaseAction(int hashCode)
        {
            if (m_AllHandle.TryGetValue(hashCode, out var value))
            {
                value.Release();
                m_AllHandle.Remove(hashCode);
            }
            else
            {
                Debug.LogError($"释放了一个未知Code");
            }
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="arg1">包名</param>
        /// <param name="arg2">资源名</param>
        /// <param name="arg3">类型</param>
        /// <returns>返回值(obj资源对象,唯一ID)</returns>
        private async UniTask<(Object, int)> LoadAssetAsync(string arg1, string arg2, Type arg3)
        {
            var handle = package.LoadAssetAsync(arg2, arg3);
            //参考 https://github.com/tuyoogame/YooAsset/blob/main/Assets/YooAsset/Samples~/UniTask%20Sample/README.md
            await handle.ToUniTask(); //异步等待 需要实现YooAsset在UniTask中的异步扩展
            return LoadAssetHandle(handle);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="arg1">包名</param>
        /// <param name="arg2">资源名</param>
        /// <param name="arg3">类型</param>
        /// <returns>返回值(obj资源对象,唯一ID)</returns>
        private (Object, int) LoadAsset(string arg1, string arg2, Type arg3)
        {
            var handle = package.LoadAssetSync(arg2, arg3);
            return LoadAssetHandle(handle);
        }

        
        //Demo中对YooAsset加载后的一个简单返回封装
        //只有成功加载才返回 否则直接释放
        private (Object, int) LoadAssetHandle(AssetHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var hashCode = handle.GetHashCode();
                m_AllHandle.Add(hashCode, handle);
                return (handle.AssetObject, hashCode);
            }
            else
            {
                handle.Release();
                return (null, 0);
            }
        }

        //释放所有
        private void ReleaseAllAction()
        {
            foreach (var handle in m_AllHandle.Values)
            {
                handle.Release();
            }

            m_AllHandle.Clear();
        }
        
        //检查合法
        private bool VerifyAssetValidityFunc(string arg1, string arg2)
        {
            return package.CheckLocationValid(arg2);
        }
        
        #region 如果你是其他的资源管理 可初始化自己的
        
        /// <summary>
        /// 开始
        /// </summary>
        private void Start()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            // 创建默认的资源包  要根据你定义的包名对应
            package = YooAssets.CreatePackage("DefaultPackage");
            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(package);
            //YooAsset中需要初始化 且分编辑器下和运行时
            StartCoroutine(InitializeYooAsset());
        }

        #if !UNITY_EDITOR
        private IEnumerator InitializeYooAsset()
        {
            var initParameters = new OfflinePlayModeParameters();
            yield return package.InitializeAsync(initParameters);
            StartOpenPanel().Forget();
        }

        #else
        private IEnumerator InitializeYooAsset()
        {
            var initParameters = new EditorSimulateModeParameters();
            //要根据你定义的包名对应
            initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.ScriptableBuildPipeline,"DefaultPackage");
            yield return package.InitializeAsync(initParameters);
            StartOpenPanel().Forget();
        }
        #endif
        
        #endregion

        private async UniTaskVoid StartOpenPanel()
        {
            //以下是YIUI中已经用到的管理器 在这里初始化
            //不需要的功能可以删除
            //await MgrCenter.Inst.Register(I2LocalizeMgr.Inst);
            await MgrCenter.Inst.Register(CountDownMgr.Inst);
            //await MgrCenter.Inst.Register(RedDotMgr.Inst);
            await MgrCenter.Inst.Register(PanelMgr.Inst);
            
            //在这里打开你的第一个界面
            // PanelMgr.Inst.OpenPanel<LoginPanel>();
            PanelMgr.Inst.OpenPanel<FriendsPanel>();
        }

        
        [Button]
        public void UnloadUnusedAssets()
        {
            //调试用调用Yooasset卸载方法 不需要可以删除
            package.UnloadUnusedAssets();
        }
        
        
        [Button]
        public void I2CleanResourceCache()
        {
            //调试用多语言卸载缓存数据  不需要可以删除
            I2.Loc.ResourceManager.pInstance.CleanResourceCache();
        }
        
    }
}