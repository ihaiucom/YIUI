using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A camera.
     */
    public class Camera : AnimatedObject, IMdxDynamicObject
    {
        public string name = "";
        public float[] position = new float[3];
        public float fieldOfView = 0;
        public float farClippingPlane = 0;
        public float nearClippingPlane = 0;
        public float[] targetPosition = new float[3];

        public void ReadMdx(BinaryStream stream, int version)
        {
            int size = (int)stream.ReadUint32();

            this.name = stream.Read(80);
            stream.ReadFloat32Array(this.position);
            this.fieldOfView = stream.ReadFloat32();
            this.farClippingPlane = stream.ReadFloat32();
            this.nearClippingPlane = stream.ReadFloat32();
            stream.ReadFloat32Array(this.targetPosition);

            this.ReadAnimations(stream, size - 120);
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)this.GetByteLength());
            stream.Skip(80 - stream.Write(this.name));
            stream.WriteFloat32Array(this.position);
            stream.WriteFloat32(this.fieldOfView);
            stream.WriteFloat32(this.farClippingPlane);
            stream.WriteFloat32(this.nearClippingPlane);
            stream.WriteFloat32Array(this.targetPosition);

            this.WriteAnimations(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 120 + base.GetByteLength(version);
        }
    }

}
