﻿namespace MapEditorReborn.API
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Mirror;
    using UnityEngine;

    /// <summary>
    /// Component added to all MapEditorReborn objects. It contains properties that are common for most of the objects.
    /// </summary>
    public class MapEditorObject : MonoBehaviour
    {
        /// <summary>
        /// Updates object properties after they were changed.
        /// </summary>
        public virtual void UpdateObject()
        {
            NetworkServer.UnSpawn(gameObject);
            NetworkServer.Spawn(gameObject);
        }

        /// <summary>
        /// Gets the relative position of the object to the <see cref="Room"/> it is currently in.
        /// </summary>
        public Vector3 RelativePosition
        {
            get
            {
                if (currentRoom == null)
                    currentRoom = Map.FindParentRoom(gameObject);

                return currentRoom.Type == RoomType.Surface ? transform.position : currentRoom.transform.InverseTransformPoint(transform.position);
            }
        }

        /// <summary>
        /// Gets the relative rotation of the object to the <see cref="Room"/> it is currently in.
        /// It will also take into account if the object had a random rotation.
        /// </summary>
        public Vector3 RelativeRotation
        {
            get
            {
                if (currentRoom == null)
                    currentRoom = Map.FindParentRoom(gameObject);

                Vector3 rotation = currentRoom.Type == RoomType.Surface ? transform.eulerAngles : transform.eulerAngles - currentRoom.transform.eulerAngles;

                if (gameObject.TryGetComponent(out ObjectRotationComponent rotationComponent))
                {
                    if (rotationComponent.XisRandom)
                        rotation.x = -1f;

                    if (rotationComponent.YisRandom)
                        rotation.y = -1f;

                    if (rotationComponent.ZisRandom)
                        rotation.z = -1f;
                }

                return rotation;
            }
        }

        /// <summary>
        /// Gets the room type of the object.
        /// </summary>
        public RoomType RoomType
        {
            get
            {
                if (currentRoom == null)
                    currentRoom = Map.FindParentRoom(gameObject);

                return currentRoom.Type;
            }
        }

        /// <summary>
        /// Gets the scale of the object.
        /// </summary>
        public Vector3 Scale => transform.localScale;

        /// <summary>
        /// Destroys the object.
        /// </summary>
        public void Destroy() => Destroy(gameObject);

        private Room currentRoom;
    }
}