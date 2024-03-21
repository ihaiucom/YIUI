using MdxLib.Model;
namespace Zeng.MdxImport.mdx.handlers
{
    public class EventHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        public EventHandler(ModelHandler modelHandler) {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
        }


        public void Imports()
        {
            CObjectContainer<CEvent> cevents = cmodel.Events;
            for (int i = 0; i < cevents.Count; i++)
            {
                CEvent cevent = cevents.Get(i);
                modelHandler.CreateGameObject("Event_" + cevent.Name, cevent.PivotPoint, cevent.Parent.NodeId);
            }
        }
    }
}
