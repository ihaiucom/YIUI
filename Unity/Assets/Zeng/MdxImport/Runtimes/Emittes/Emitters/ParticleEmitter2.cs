
namespace Zeng.Mdx.Runtimes
{
    public class ParticleEmitter2 : Emitter
    {
        public MdxParticleSystem2 emitterParameter;

        public int lastEmissionKey = -1;
        public ParticleEmitter2(MdxParticleSystem2 emitterParameter) : base(emitterParameter)
        {
            this.emitterParameter = emitterParameter;
        }

        public override EmittedObject createObject()
        {
            EmittedObject item = new Particle2(this);
            return item;
        }

        public override void emit()
        {
            if (emitterParameter.head)
            {
                this.emitObject(0);
            }

            if (emitterParameter.tail)
            {
                this.emitObject(1);
            }


        }

        public override void updateEmission(float dt)
        {
            if (emitterParameter.allowParticleSpawn)
            {
                if (emitterParameter.squirt)
                {

                    if (emitterParameter.emissionRateKey != this.lastEmissionKey)
                    {
                        this.currentEmission += emitterParameter.emissionRate;
                    }

                    this.lastEmissionKey = emitterParameter.emissionRateKey;

                }
                else
                {
                    this.currentEmission += emitterParameter.emissionRate * dt * 0.5f;
                }
            }
        }
    }
}
