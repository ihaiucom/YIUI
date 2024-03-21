
using UnityEditor.Animations;
using UnityEditor;
using UnityEngine;
using System.IO;
namespace Zeng.MdxImport
{
    public class AnimatorControllerUtils
    {
        public static AnimatorController Create(string ctrlPath, string clipDir)
        {
            AnimatorController controller = GetOrAdd(ctrlPath);
            ClearStates(controller);
            AddStateFromFolder(controller, clipDir);
            return controller;
        }


        /// <summary>
        /// 获取或者添加, 动画控制器
        /// </summary>
        public static AnimatorController GetOrAdd(string ctrlPath)
        {
            AnimatorController controller = null;
            controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(ctrlPath);

            if (controller == null)
            {
                controller = Create(ctrlPath);
            }

            return controller;
        }

        private static void AddStateFromFolder(AnimatorController controller, string clipDir)
        {
            if(!Directory.Exists(clipDir))
            {
                Debug.LogError($"[AnimatorControllerUtils] 目录不存在 clipDir={clipDir}");
                return;
            }

            AnimatorControllerLayer animatorControllerLayer = controller.layers[0];
            AnimatorStateMachine stateMachine = animatorControllerLayer.stateMachine;

            string[] files =  Directory.GetFiles(clipDir);
            for(int i = 0; i < files.Length; i ++)
            {
                string itemPath = files[i];
                string ext = Path.GetExtension(itemPath).ToLower();
                if (ext == ".meta") continue;

                AnimationClip animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(itemPath);
                if (animationClip == null) {
                    continue;
                }

                AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(animationClip);
                clipSetting.loopTime = !animationClip.isLooping;
                AnimationUtility.SetAnimationClipSettings(animationClip, clipSetting);


                var state = stateMachine.AddState(animationClip.name);
                state.motion = animationClip;

                // 设置默认动作
                if (stateMachine.defaultState == null && animationClip.name.ToLower().Contains("stand"))
                {
                    stateMachine.defaultState = state;
                }
            }

        }

        /// <summary>
        /// 清空 老的状态
        /// </summary>
        private static void ClearStates(AnimatorController controller)
        {
            AnimatorControllerLayer animatorControllerLayer = controller.layers[0];
            AnimatorStateMachine stateMachine = animatorControllerLayer.stateMachine;

            // 清理老的动画状态
            if (stateMachine.states.Length > 0)
            {
                for (int i = stateMachine.states.Length - 1; i >= 0; i--)
                {
                    stateMachine.RemoveState(stateMachine.states[i].state);
                }
            }

            stateMachine.defaultState = null;
        }

        /// <summary>
        /// 创建一个新的 动画控制器
        /// </summary>
        private static AnimatorController Create(string ctrlPath)
        {
            string dir = Path.GetDirectoryName(ctrlPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if(File.Exists(ctrlPath))
            {
                File.Delete(ctrlPath);
            }

            AnimatorController controller = new AnimatorController();
            controller.name = Path.GetFileNameWithoutExtension(ctrlPath);
            AssetDatabase.CreateAsset(controller, ctrlPath);
            controller.AddLayer("Base Layer");
            return controller;
        }
    }
}
