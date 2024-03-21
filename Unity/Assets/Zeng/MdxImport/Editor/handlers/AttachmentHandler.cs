using MdxLib.Model;
namespace Zeng.MdxImport.mdx.handlers
{
    public class AttachmentHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        public AttachmentHandler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
        }


        public void Imports()
        {
            CObjectContainer<CAttachment> cattachments = cmodel.Attachments;
            for (int i = 0; i < cattachments.Count; i++)
            {
                CAttachment cattachment = cattachments.Get(i);
                modelHandler.CreateGameObject("Attachment_" + cattachment.Name, cattachment.PivotPoint, cattachment.Parent.NodeId);
            }
        }
    }
}
