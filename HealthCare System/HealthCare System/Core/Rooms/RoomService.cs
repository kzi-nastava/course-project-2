﻿using HealthCare_System.Core.Appointments;
using HealthCare_System.Core.Appointments.Model;
using HealthCare_System.Core.Equipments.Model;
using HealthCare_System.Core.EquipmentTransfers;
using HealthCare_System.Core.EquipmentTransfers.Model;
using HealthCare_System.Core.Renovations;
using HealthCare_System.Core.Renovations.Model;
using HealthCare_System.Core.Rooms.Model;
using HealthCare_System.Core.Rooms.Repository;
using System;
using System.Collections.Generic;


namespace HealthCare_System.Core.Rooms
{
    public class RoomService : IRoomService
    {
        IMergingRenovationService mergingRenovationService;
        ISimpleRenovationService simpleRenovationService;
        IEquipmentTransferService equipmentTransferService;
        ISplittingRenovationService splittingRenovationService;
        IAppointmentService appointmentService;

        IRoomRepo roomRepo;

        public RoomService(IMergingRenovationService mergingRenovationService, ISimpleRenovationService simpleRenovationService,
            IEquipmentTransferService equipmentTransferService, ISplittingRenovationService splittingRenovationService,
            IAppointmentService appointmentService, IRoomRepo roomRepo)
        {
            this.mergingRenovationService = mergingRenovationService;
            this.simpleRenovationService = simpleRenovationService;
            this.equipmentTransferService = equipmentTransferService;
            this.splittingRenovationService = splittingRenovationService;
            this.appointmentService = appointmentService;
            this.roomRepo = roomRepo;
        }

        public IRoomRepo RoomRepo { get => roomRepo; }
       
        public List<Room> Rooms()
        {
            return roomRepo.Rooms;
        }

        public Room Storage()
        {
            return roomRepo.GetStorage();
        }

        public bool IsRoomAvailableAtAllMerging(Room room)
        {
            bool available = true;
            foreach (MergingRenovation mergingRenovation in mergingRenovationService.MergingRenovations())
            {
                foreach (Room roomInMerging in mergingRenovation.Rooms)
                {
                    if (room == roomInMerging)
                    {
                        available = false;
                        return available;
                    }
                }
            }
            return available;
        }

        public bool IsRoomAvailableAtTimeMerging(Room room, DateTime time)
        {
            bool available = true;
            foreach (MergingRenovation mergingRenovation in mergingRenovationService.MergingRenovations())
            {
                foreach (Room roomInMerging in mergingRenovation.Rooms)
                {
                    if (room == roomInMerging && time.AddMinutes(15) >= mergingRenovation.BeginningDate)
                    {
                        available = false;
                        return available;
                    }
                }
            }
            return available;
        }

        public void Create(string name, TypeOfRoom type, Dictionary<Equipment, int> equipmentAmount)
        {
            if (name.Length > 30 || name.Length < 5)
                throw new Exception();

            Room newRoom = new Room(roomRepo.GenerateId(), name, type, equipmentAmount);
            roomRepo.Add(newRoom);
        }

        public void Create(RoomDto roomDto)
        {
            if (roomDto.Name.Length > 30 || roomDto.Name.Length < 5)
                throw new Exception();

            Room newRoom = new Room(roomDto);
            roomRepo.Add(newRoom);
        }

        public void Update(Room room, string name, TypeOfRoom type)
        {
            if (name.Length > 30 || name.Length < 5)
                throw new Exception();
            room.Name = name;
            room.Type = type;
            roomRepo.Serialize();
        }

        public void Update(Room room, RoomDto roomDto)
        {
            if (roomDto.Name.Length > 30 || roomDto.Name.Length < 5)
                throw new Exception();
            room.Name = roomDto.Name;
            room.Type = roomDto.Type;
            roomRepo.Serialize();
        }

        public void Delete(Room room)
        {
            equipmentTransferService.MoveEquipmentToStorage(room);
            roomRepo.Delete(room);
        }

        public bool IsRoomAvailableAtAllSimple(Room room)
        {
            bool available = true;
            foreach (SimpleRenovation simpleRenovation in simpleRenovationService.SimpleRenovations())
            {
                if (room == simpleRenovation.Room)
                {
                    available = false;
                    return available;
                }
            }
            return available;
        }

        public bool IsRoomAvailableAtTimeSimple(Room room, DateTime time)
        {
            bool available = true;
            foreach (SimpleRenovation simpleRenovation in simpleRenovationService.SimpleRenovations())
            {
                if (room == simpleRenovation.Room && time.AddMinutes(15) >= simpleRenovation.BeginningDate)
                {
                    available = false;
                    return available;
                }
            }
            return available;
        }

        public bool IsRoomAvailableAtAllSplitting(Room room)
        {
            bool available = true;
            foreach (SplittingRenovation splittingRenovation in splittingRenovationService.SplittingRenovations())
            {
                if (room == splittingRenovation.Room)
                {
                    available = false;
                    return available;
                }
            }
            return available;
        }

        public bool IsRoomAvailableAtTimeSplitting(Room room, DateTime time)
        {
            bool available = true;
            foreach (SplittingRenovation splittingRenovation in splittingRenovationService.SplittingRenovations())
            {
                if (room == splittingRenovation.Room && time.AddMinutes(15) >= splittingRenovation.BeginningDate)
                {
                    available = false;
                    return available;
                }
            }
            return available;
        }

        public bool IsRoomAvailable(Room room)
        {
            bool available = true;
            foreach (Transfer transfer in equipmentTransferService.Transfers())
            {
                if (room == transfer.FromRoom || room == transfer.ToRoom)
                {
                    available = false;
                    break;
                }
            }
            return available;
        }

        public bool IsRoomAvailableForChange(Room room)
        {
            bool available = true;

            available = IsRoomAvailableAppointments(room);
            if (!available)
            {
                return available;
            }

            available = IsRoomAvailable(room);
            if (!available)
            {
                return available;
            }

            return available;
        }

        public void RemoveRoom(Room room)
        {
            foreach (Appointment appointment in appointmentService.Appointments())
            {
                if (appointment.Room == room)
                    appointment.Room = null;
            }
            appointmentService.AppointmentRepo.Serialize();
            Delete(room);
        }

        public bool IsRoomAvailableAppointments(Room room)
        {
            bool available = true;
            foreach (Appointment appointment in appointmentService.Appointments())
            {
                if (room == appointment.Room && appointment.Status != AppointmentStatus.FINISHED)
                {
                    available = false;
                    break;
                }
            }
            return available;
        }

        public bool IsRoomAvailableRenovationsAtAll(Room room)
        {
            bool available = true;
            available = IsRoomAvailableAtAllSimple(room);
            if (!available)
            {
                return available;
            }

            available = IsRoomAvailableAtAllMerging(room);
            if (!available)
            {
                return available;
            }

            available = IsRoomAvailableAtAllSplitting(room);
            if (!available)
            {
                return available;
            }
            return available;
        }

        public bool IsRoomAvailableRenovationsAtTime(Room room, DateTime time)
        {
            bool available = true;
            available = IsRoomAvailableAtTimeSimple(room, time);
            if (!available)
            {
                return available;
            }

            available = IsRoomAvailableAtTimeMerging(room, time);
            if (!available)
            {
                return available;
            }

            available = IsRoomAvailableAtTimeSplitting(room, time);
            if (!available)
            {
                return available;
            }
            return available;
        }
    }


}
