
using System.Collections.Generic;
using UnityEngine;

namespace Zeng.Mdx.Runtimes
{
    public class EmittedObjectUpdater : MonoBehaviour
    {
        private static EmittedObjectUpdater _i;
        public static EmittedObjectUpdater I
        {
            get
            {
                if (_i == null)
                {
                    GameObject go = new GameObject("[Manager]EmittedObjectUpdater");
                    _i = go.AddComponent<EmittedObjectUpdater>();
                }
                return _i;
            }
        }

        private void Awake()
        {
            if (_i != null && _i != this)
            {
                GameObject.Destroy(_i);
                GameObject.Destroy(_i.gameObject);
            }
            _i = this;
        }



        private void OnEnable()
        {
            if (_i == null) _i = this;
            Reset();
        }

        void Reset()
        {
            objects.Clear();
            alive = 0;
        }

        private void Update()
        {
            update(Time.deltaTime);
        }


        public List<EmittedObject> objects = new List<EmittedObject>();
        public int alive = 0;

        public void add(EmittedObject item)
        {
            if (this.objects.Count < alive + 1)
            {
                for (int i = this.objects.Count; i < alive + 1; i++)
                {
                    this.objects.Add(null);
                }
            }
            this.objects[alive++] = item;
        }

        public void update(float dt)
        {
            for (int i = 0; i < alive; i++)
            {
                EmittedObject item = objects[i];
                item.update(dt * item.emitter.parameter.TimeScale);

                if (item.health <= 0)
                {
                    alive -= 1;
                    item.emitter.kill(item);

                    if (i != this.alive)
                    {
                        objects[i] = objects[this.alive];
                        i -= 1;
                    }
                }
            }
        }

        public void clearEmittedObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].health = 0;
                }
            }
        }


    }
}
