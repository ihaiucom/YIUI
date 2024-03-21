using MdxLib.Model;
using UnityEngine;

namespace Zeng.MdxImport.mdx.handlers
{
    public class CollisionShapesHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        public CollisionShapesHandler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
        }


        public void Imports()
        {

            CObjectContainer<CCollisionShape> cshapes = cmodel.CollisionShapes;
            for (int i = 0; i < cshapes.Count; i++)
            {
                CCollisionShape cshape = cshapes.Get(i);
                GameObject gameObject = modelHandler.CreateGameObject("CollisionShape_" + cshape.Name, cshape.PivotPoint, cshape.Parent.NodeId);

                switch (cshape.Type)
                {
                    case ECollisionShapeType.Box:
                        {
                            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                            collider.size = cshape.Vertex2.ToVector3() - cshape.Vertex1.ToVector3();
                            break;
                        }
                    case ECollisionShapeType.Sphere:
                        {
                            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
                            collider.radius = cshape.Radius;
                            break;
                        }
                }
            }
        }
    }
}
