using MdxLib.Model;
namespace Zeng.MdxImport.mdx.handlers
{
    public class ParticleEmitterHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        public ParticleEmitterHandler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
        }


        public void Imports()
        {

            CObjectContainer<CParticleEmitter> cemitters = cmodel.ParticleEmitters;
            for (int i = 0; i < cemitters.Count; i++)
            {
                CParticleEmitter cemitter = cemitters.Get(i);
                modelHandler.CreateGameObject("ParticleEmitter_" + cemitter.Name, cemitter.PivotPoint, cemitter.Parent.NodeId);
            }

         
        }
    }
}
