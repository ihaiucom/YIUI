
namespace Zeng.Mdx.Runtimes
{
    public abstract class EmittedObject
    {
        public Emitter emitter;
        public int id = 0;
        public int index = -1;
        public float health =0;

        // 资源是否准备就绪
        public bool ok = false;

        public abstract void bind(object emitData);
        public abstract void update(float dt);

        public EmittedObject(Emitter emitter) { 
            this.emitter = emitter;
        }
    }
}
