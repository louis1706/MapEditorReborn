﻿namespace MapEditorReborn.API.Features.Components.ObjectComponents
{
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Extensions;
    using Features;
    using Features.Objects;
    using Interactables.Interobjects.DoorUtils;

    using static API;

    /// <summary>
    /// Component added to spawned DoorObject. Is is used for easier idendification of the object and it's variables.
    /// </summary>
    public class DoorObjectComponent : MapEditorObject
    {
        /// <summary>
        /// Initializes the <see cref="DoorObjectComponent"/>.
        /// </summary>
        /// <param name="doorObject">The <see cref="DoorObject"/> to initialize.</param>
        /// <returns>Instance of this compoment.</returns>
        public DoorObjectComponent Init(DoorObject doorObject)
        {
            Base = doorObject;

            if (TryGetComponent(out DoorVariant doorVariant))
                door = Door.Get(doorVariant);

            Base.DoorType = door.GetDoorTypeByName();
            prevBase.CopyProperties(Base);

            ForcedRoomType = doorObject.RoomType != RoomType.Unknown ? doorObject.RoomType : FindRoom().Type;
            UpdateObject();

            return this;
        }

        /// <summary>
        /// The config-base of the object containing all of it's properties.
        /// </summary>
        public DoorObject Base;

        /// <inheritdoc cref="MapEditorObject.UpdateObject()"/>
        public override void UpdateObject()
        {
            if (prevBase.DoorType != Base.DoorType)
            {
                SpawnedObjects[SpawnedObjects.FindIndex(x => x == this)] = ObjectSpawner.SpawnDoor(Base, transform.position, transform.rotation);
                Destroy();

                return;
            }

            prevBase.CopyProperties(Base);
            door.IsOpen = Base.IsOpen;
            door.ChangeLock(Base.IsLocked ? DoorLockType.SpecialDoorFeature : DoorLockType.None);
            door.RequiredPermissions.RequiredPermissions = Base.KeycardPermissions;
            door.IgnoredDamageTypes = Base.IgnoredDamageSources;
            door.MaxHealth = Base.DoorHealth;
            door.Health = Base.DoorHealth;

            base.UpdateObject();
        }

        private Door door;
        private DoorObject prevBase = new DoorObject();
    }
}