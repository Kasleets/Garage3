﻿namespace Garage3.Models.Entities
{
    public class ParkingRecord
    {
        public int RecordID { get; set; }
        public int VehicleID { get; set; }
        public DateTime ParkTime { get; set; }
        public int MemberID { get; set; }

        // Navigation properties
        public virtual Vehicle Vehicle { get; set; }
        public virtual Member Member { get; set; }
    }

}