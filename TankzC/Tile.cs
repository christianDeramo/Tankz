using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Aiv.Audio;

namespace TankzC
{
    class Tile : Groundable
    {
        protected AudioSource soundEmitter;
        protected AudioClip[] breakSounds;

        public Tile(Vector2 spritePosition, string textureName="crate") : base(spritePosition, textureName, DrawManager.Layer.Playground)
        {
            RigidBody = new RigidBody(sprite.position, this);
            RigidBody.Type = (uint)PhysicsManager.ColliderType.Tile;
            RigidBody.SetCollisionMask((uint)PhysicsManager.ColliderType.Tile);
            RigidBody.IsGravityAffected = true;

            soundEmitter = new AudioSource();

            breakSounds = new AudioClip[2];
            for (int i = 0; i < breakSounds.Length; i++)
            {
                breakSounds[i] = new AudioClip("Assets/");
            }
        }

        public override void OnCollide(Collision collisionInfo)
        {
            base.OnCollide(collisionInfo);

            if (collisionInfo.Collider is Bullet)
            {
                IsActive = false;
                soundEmitter.Pitch = RandomGenerator.GetRandom(3, 10) / 10.0f;
                soundEmitter.Play(breakSounds[RandomGenerator.GetRandom(0, breakSounds.Length)]);
            }
        }
    }
}
