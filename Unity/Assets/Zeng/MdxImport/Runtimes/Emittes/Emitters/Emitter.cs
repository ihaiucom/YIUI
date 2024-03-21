
using System.Collections.Generic;
using System.Diagnostics;
using static UnityEditor.Progress;

namespace Zeng.Mdx.Runtimes
{
    public abstract class Emitter
    {
        protected int GID = 0;
        public IEmitterParameter parameter;
        public List<EmittedObject> objects = new List<EmittedObject>();
        public int alive = 0;
        public float currentEmission = 0;

        public abstract EmittedObject createObject();
        public abstract void updateEmission(float dt);
        public abstract void emit();

        public Emitter(IEmitterParameter emitterParameter)
        {
            this.parameter = emitterParameter;
        }

        public virtual void update(float dt)
        {
            this .updateEmission(dt);

            float currentEmission = this .currentEmission;
            if (currentEmission >= 1) { 
                for(int i = 0; i < currentEmission; i += 1)
                {
                    this.emit ();
                }
            }
        }

        public void clear()
        {
            List<EmittedObject> objects = this.objects;
            for(int i = 0, l = this.alive; i < l; i ++)
            {
                EmittedObject item = objects[i];
                item.health = 0;
            }

            this.currentEmission = 0;
        }

        public EmittedObject emitObject(object emitData) {
            if (this.alive == objects.Count) {
                objects.Add(this.createObject());
            }

            EmittedObject item = objects[this.alive];

            item.id = GID++;
            //if (GID > 2000) GID = 0;



            item.index = this.alive;
            item.bind(emitData);


            this.alive += 1;
            this.currentEmission -= 1;

            this.parameter.emittedObjectUpdater.add(item);

            return item;
        }

        public void kill(EmittedObject item)
        {
            this.alive -= 1;
            EmittedObject otherItem = objects[this.alive];
            objects[item.index] = otherItem;
            objects[this.alive] = item;


            otherItem.index = item.index;
            item.index = -1;
        }


    }
}