﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HealthCare_System.entities
{
    public class MergingRenovation : Renovation
    {
        List<Room> rooms;

        public MergingRenovation()
        {
            rooms = new List<Room>();
        }

        public MergingRenovation(Renovation renovation) : base(renovation)
        {
            rooms = new List<Room>();
        }

        public MergingRenovation(int id, DateTime beginningDate, DateTime endingDate) : base(id, beginningDate, endingDate)
        {
            rooms = new List<Room>();
        }

        public MergingRenovation(int id, DateTime beginningDate, DateTime endingDate, List<Room> rooms) : base(id, beginningDate, endingDate)
        {
            this.rooms = rooms;
        }

        public MergingRenovation(MergingRenovation renovation) : base(renovation)
        {
            this.rooms = renovation.Rooms;
        }

        [JsonIgnore]
        public List<Room> Rooms { get => rooms; set => rooms = value; }

        public override string ToString()
        {
            return "MergingRenovation" + base.ToString();
        }
    }
}
