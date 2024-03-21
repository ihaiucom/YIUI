using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{


    /**
     * A collision shape.
     */
    public class CollisionShape : GenericObject
    {
        public enum Shape
        {
            Box = 0,
            Plane = 1,
            Sphere = 2,
            Cylinder = 3
        }

        public Shape type = Shape.Box;
        public float[][] vertices = { new float[3], new float[3] };
        public float boundsRadius = 0;

        public CollisionShape() : base(0x2000)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            base.ReadMdx(stream, version);

            type = (Shape)stream.ReadUint32();

            stream.ReadFloat32Array(vertices[0]);

            if (type != Shape.Sphere)
            {
                stream.ReadFloat32Array(vertices[1]);
            }

            if (type == Shape.Sphere || type == Shape.Cylinder)
            {
                boundsRadius = stream.ReadFloat32();
            }
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            base.WriteMdx(stream, version);

            stream.WriteUint32((uint)type);
            stream.WriteFloat32Array(vertices[0]);

            if (type != Shape.Sphere)
            {
                stream.WriteFloat32Array(vertices[1]);
            }

            if (type == Shape.Sphere || type == Shape.Cylinder)
            {
                stream.WriteFloat32(boundsRadius);
            }
        }

        public override int GetByteLength(int version = 0)
        {
            int size = 16 + base.GetByteLength(version);

            if (type != Shape.Sphere)
            {
                size += 12;
            }

            if (type == Shape.Sphere || type == Shape.Cylinder)
            {
                size += 4;
            }

            return size;
        }
    }

}
