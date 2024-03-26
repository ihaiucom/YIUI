/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Framework\UObject\UEffect.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-4-13      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    public sealed class UEffect : MonoBehaviour
    {
        private Action<UEffect> onDestroyed = null;
        private Action<UEffect> onLifeTimeEnd = null;
        private float mCurTime = 0;
        private float mLifeTime = 0;
        private List<ParticleSystem> mParticleSystemList = new List<ParticleSystem>();
        private List<Animator> mAnimatorList = new List<Animator>();
        private bool mLoop = false;

        void Awake()
        {
            mParticleSystemList.AddRange(transform.GetComponentsInChildren<ParticleSystem>());
            mAnimatorList.AddRange(transform.GetComponentsInChildren<Animator>());

            using (var itr = mParticleSystemList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    mLoop = itr.Current.main.loop;
                    if (mLoop)
                        break;
                }
            }
        }

        void Update()
        {
            mCurTime += Time.deltaTime;

            if (mCurTime >= mLifeTime)
            {
                if (onLifeTimeEnd != null)
                    onLifeTimeEnd(this);
            }
        }

        void OnDestroy()
        {
            if (onDestroyed != null)
            {
                onDestroyed(this);
                onDestroyed = null;
                onLifeTimeEnd = null;
                mCurTime = 0;
                mLifeTime = 0;
            }
        }

        public void Init()
        {
            mCurTime = 0;
            mLifeTime = 0;
        }

        public void Play()
        {
            using (var itr = mParticleSystemList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    itr.Current.Stop();
                    itr.Current.Clear();
                    itr.Current.time = 0;
                    itr.Current.Play();
                }
            }
            using (var itr = mAnimatorList.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    AnimatorStateInfo _info = itr.Current.GetCurrentAnimatorStateInfo(0);
                    itr.Current.Play(_info.fullPathHash, 0, 0);
                }
            }
        }

        public Action<UEffect> OnDestroyed
        {
            get { return onDestroyed; }
            set { onDestroyed = value; }
        }

        public Action<UEffect> OnLifeTimeEnd
        {
            get { return onLifeTimeEnd; }
            set
            {
                mLifeTime = 0;

                using (var itr = mParticleSystemList.GetEnumerator())
                {
                    while (itr.MoveNext())
                    {
                        float t = itr.Current.main.duration + itr.Current.main.startDelayMultiplier + itr.Current.main.startLifetimeMultiplier;
                        if (mLifeTime < t)
                        {
                            mLifeTime = t;
                        }
                    }
                }

                onLifeTimeEnd = value;
            }
        }
        
    }

}
