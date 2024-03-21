
using UnityEngine;

namespace Zeng.Mdx.Runtimes
{
    public class Particle2 : EmittedObject
    {
        public Matrix4x4 worldMatrix = Matrix4x4.identity;

        public int tail = 0;
        public float gravity = 0;
        public Vector3 locationl = Vector3.zero;
        public Vector3 eulerAngles = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Vector3 velocity = Vector3.zero;
        public float facing = 0;

        private MdxParticleSystem2 parameter;
        private Transform transform;

        public Particle2(ParticleEmitter2 emitter) : base(emitter)
        {
        }

        public override void bind(object emitData)
        {
            // 坐标系 -y, z, x
            ParticleEmitter2 emitter = this.emitter as ParticleEmitter2;
            parameter = this.emitter.parameter as MdxParticleSystem2;
            transform = parameter.transform;
            float s = transform.lossyScale.x;
            this.health = parameter.lifeSpan;
            this.gravity = parameter.gravity;
            this.scale = transform.lossyScale;

            float width = parameter.width * 0.5f * s;
            float length = parameter.length * 0.5f * s;
            float variation = parameter.variation;
            float latitude = parameter.latitude;
            float speed = parameter.speed;

            locationl = Vector3.zero;
            locationl.z += Random.Range(-width, width);
            locationl.x += Random.Range(-length, length);

            // World location
            if (!parameter.modelSpace)
            {
                locationl += transform.position;
            }

            velocity = Vector3.zero;
            Vector3 vAngle = Vector3.zero;
            vAngle.x = Random.Range(-latitude, latitude);
            vAngle.y = 90;
            if (!parameter.lineEmitter)
            {
                vAngle.z = Random.Range(-latitude, latitude);
            }

            if (!parameter.modelSpace)
            {
                vAngle += transform.eulerAngles;
            }

            Matrix4x4 m = Matrix4x4.TRS(Vector3.up, Quaternion.Euler(vAngle), Vector3.one * speed * (1 + Random.Range(-variation, variation)));
            velocity = m.MultiplyVector(Vector3.up);

            if (parameter.xZQuad)
            {
                this.facing = Mathf.Atan2(velocity.z, velocity.x) - Mathf.PI + Mathf.PI / 8;
                this.facing *= 100;
            }

            //SettingWorldMatrix();
        }

        public override void update(float dt)
        {
            this.health -= dt;
            if (this.health > 0)
            {
                velocity.y -= gravity * dt;
                locationl += Vector3.Scale(velocity, scale) * dt;
            }
            //SettingWorldMatrix();
        }

        public void SettingWorldMatrix()
        {
            Vector3 position = locationl;
            Vector3 a = Vector3.zero;
            if (parameter.modelSpace)
            {
                position = transform.position + locationl;
                a = transform.eulerAngles;
            }

            Quaternion rotation = Quaternion.Euler(a.x, a.y, a.z);
            worldMatrix = Matrix4x4.TRS(position, rotation, scale);
        }
    }
}
