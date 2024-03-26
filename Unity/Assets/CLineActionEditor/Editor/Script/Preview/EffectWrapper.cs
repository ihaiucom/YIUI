/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2021 - 2029 All Right Reserved
|
| FILE NAME  : \CLineActionEditor\Editor\Script\Preview\EffectWrapper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    :
|
| SPEC       :
|
| MODIFICATION HISTORY
|
| Ver      Date            By              Details
| -----    -----------    -------------   ----------------------
| 1.0      2021-11-8      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections.Generic;

    public class EffectWrapper : XObject
    {
        private float mSimulateTime = 0f;
        private GameObject mEffect;
        private List<ParticleSystem> mParticleList = new List<ParticleSystem>();
        private List<Animator> mAnimatorList = new List<Animator>();

        public EffectWrapper(EventPlayEffect epe)
        {
            string effectname = epe.EffectName;
            if (epe.UseRandom)
            {
                int idx = UnityEngine.Random.Range(0, epe.RandomEffectList.Count);
                effectname = epe.RandomEffectList[idx];
            }

            GameObject go = ResourceMgr.Instance.LoadObject<GameObject>(effectname);

            switch (epe.DummyType)
            {
                case EEffectDummyType.EEDT_DummyFollow:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(epe.DummyRoot) ? UnitWrapper.Instance.UnitWrapperUnit.transform : Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, epe.DummyRoot);

                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;
                        mEffect.transform.parent = Helper.Find(dummyRoot, epe.DummyAttach);
                        mEffect.transform.localPosition = epe.Position;
                        mEffect.transform.localRotation = Quaternion.Euler(epe.Euler);
                    }
                    break;
                case EEffectDummyType.EEDT_DummyPosition:
                    {
                        Transform dummyRoot = string.IsNullOrEmpty(epe.DummyRoot) ? UnitWrapper.Instance.UnitWrapperUnit.transform : Helper.Find(UnitWrapper.Instance.UnitWrapperUnit.transform, epe.DummyRoot);

                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;
                        Transform tr = Helper.Find(dummyRoot, epe.DummyAttach);

                        mEffect.transform.rotation = Quaternion.Euler(tr.rotation.eulerAngles + epe.Euler);
                        mEffect.transform.position = tr.position + tr.rotation * epe.Position;
                    }
                    break;
                case EEffectDummyType.EEDT_UnitPosition:
                    {
                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;

                        Transform tr = UnitWrapper.Instance.UnitWrapperUnit.transform;
                        mEffect.transform.rotation = Quaternion.Euler(tr.rotation.eulerAngles + epe.Euler);
                        mEffect.transform.position = tr.position + tr.rotation * epe.Position;
                    }
                    break;
                case EEffectDummyType.EEDT_Custom:
                    {
                        mEffect = GameObject.Instantiate(go) as GameObject;
                        mEffect.transform.localScale = Vector3.one;

                        mEffect.transform.rotation = Quaternion.Euler(epe.Euler);
                        mEffect.transform.position = epe.Position;

                    }
                    break;
            }


            if (mEffect)
            {
                mParticleList.AddRange(mEffect.GetComponentsInChildren<ParticleSystem>());
                mAnimatorList.AddRange(mEffect.GetComponentsInChildren<Animator>());

                mParticleList.ForEach((ps) =>
                {
                    if (!ps.proceduralSimulationSupported)
                        ps.useAutoRandomSeed = false;
                });
            }
        }

        public void Tick(float fTick)
        {
            mSimulateTime += fTick;

            mParticleList.ForEach((ps) =>
            {
                ps.Simulate(mSimulateTime);
            });
            mAnimatorList.ForEach((anim) =>
            {
                anim.Update(fTick);
            });
        }

        public float GetEffectTime()
        {
            float time = 0;
            for (int i = 0; i < mParticleList.Count; ++i)
            {
                if (time < (mParticleList[i].main.duration + mParticleList[i].main.startDelayMultiplier))
                {
                    time = (mParticleList[i].main.duration + mParticleList[i].main.startDelayMultiplier);
                }
            }

            return time;
        }

        protected override void OnDispose()
        {
            if (mEffect != null)
            {
                GameObject.DestroyImmediate(mEffect);
                mEffect = null;
            }
        }

    }
}