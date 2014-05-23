﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using TheAirline.Model.AirlineModel;
using TheAirline.Model.AirlinerModel;
using TheAirline.Model.GeneralModel;

namespace TheAirline.Model.PilotModel
{
    //the class for a pilot
    [Serializable]
    public class Pilot : ISerializable
    {
        public Boolean OnTraining {get {return this.Training != null;} set {}}
        [Versioning("training",Version=3)]
        public PilotTraining Training { get; set; }
        [Versioning("aircrafts",Version=3)]
        public List<string> Aircrafts { get; set; }
        [Versioning("pilotrating",Version=2)]
        public PilotRating Rating { get; set; }
        [Versioning("profile")]
        public PilotProfile Profile { get; set; }
        [Versioning("airline")]
        public Airline Airline { get; set; }
        [Versioning("signeddate")]
        public DateTime AirlineSignedDate { get; set; }
        [Versioning("education")]
        public DateTime EducationTime { get; set; }
        [Versioning("airliner")]
        public FleetAirliner Airliner { get; set; }
        public const int RetirementAge = 55;
         public Pilot(PilotProfile profile, DateTime educationTime, PilotRating rating)
        {
            this.Profile = profile;
            this.EducationTime = educationTime;
            this.Rating = rating;
            this.Aircrafts = new List<string>();
             
    
        }
        //adds an airliner type family which the pilot expirence
         public void addAirlinerFamily(string family)
         {
             this.Aircrafts.Add(family);
         }
        //sets the airline for a pilot
        public void setAirline(Airline airline, DateTime signDate)
        {
            this.Airline = airline;
            this.AirlineSignedDate = signDate;
        }
        private Pilot(SerializationInfo info, StreamingContext ctxt)
        {
            int version = info.GetInt16("version");

            var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.GetCustomAttribute(typeof(Versioning)) != null);

            IList<PropertyInfo> props = new List<PropertyInfo>(this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.GetCustomAttribute(typeof(Versioning)) != null));

            var propsAndFields = props.Cast<MemberInfo>().Union(fields.Cast<MemberInfo>());

            foreach (SerializationEntry entry in info)
            {
                MemberInfo prop = propsAndFields.FirstOrDefault(p => ((Versioning)p.GetCustomAttribute(typeof(Versioning))).Name == entry.Name);


                if (prop != null)
                {
                    if (prop is FieldInfo)
                        ((FieldInfo)prop).SetValue(this, entry.Value);
                    else
                        ((PropertyInfo)prop).SetValue(this, entry.Value);
                }
            }

            var notSetProps = propsAndFields.Where(p => ((Versioning)p.GetCustomAttribute(typeof(Versioning))).Version > version);

            foreach (MemberInfo notSet in notSetProps)
            {
                Versioning ver = (Versioning)notSet.GetCustomAttribute(typeof(Versioning));

                if (ver.AutoGenerated)
                {
                    if (notSet is FieldInfo)
                        ((FieldInfo)notSet).SetValue(this, ver.DefaultValue);
                    else
                        ((PropertyInfo)notSet).SetValue(this, ver.DefaultValue);

                }

            }
            if (version == 1)
            {
                this.Rating = GeneralHelpers.GetPilotRating();
            }
            if (version < 3)
            {
                this.Aircrafts = GeneralHelpers.GetPilotAircrafts(this);
                this.Training = null;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("version", 3);

            Type myType = this.GetType();

            var fields = myType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.GetCustomAttribute(typeof(Versioning)) != null);

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(p => p.GetCustomAttribute(typeof(Versioning)) != null));

            var propsAndFields = props.Cast<MemberInfo>().Union(fields.Cast<MemberInfo>());

            foreach (MemberInfo member in propsAndFields)
            {
                object propValue;

                if (member is FieldInfo)
                    propValue = ((FieldInfo)member).GetValue(this);
                else
                    propValue = ((PropertyInfo)member).GetValue(this, null);

                Versioning att = (Versioning)member.GetCustomAttribute(typeof(Versioning));

                info.AddValue(att.Name, propValue);
            }

        }
    }
    //the list of pilots
    public class Pilots
    {
        private static List<Pilot> pilots = new List<Pilot>();
        //adds a pilot to the list
        public static void AddPilot(Pilot pilot)
        {
            lock (pilots)
            {
                if (pilot != null)
                    pilots.Add(pilot);
            }
        }
        //clears the list of pilots
        public static void Clear()
        {
            pilots.Clear();
        }
        //returns all pilots
        public static List<Pilot> GetPilots()
        {
            return pilots;
        }
        //returns all unassigned pilots
        public static List<Pilot> GetUnassignedPilots()
        {
            var unassigned = pilots.FindAll(p => p.Airline == null);

            if (unassigned.Count < 5)
            {
                GeneralHelpers.CreatePilots(10);

                return GetUnassignedPilots();
            }

            return unassigned;

        }
        public static List<Pilot> GetUnassignedPilots(Predicate<Pilot> match)
        {
            return GetUnassignedPilots().FindAll(match);
        }
        //removes a pilot from the list
        public static void RemovePilot(Pilot pilot)
        {
            pilots.Remove(pilot);
        }
        //counts the number of unassigned pilots
        public static int GetNumberOfUnassignedPilots()
        {
            return GetUnassignedPilots().Count;
        }
        //counts the number of pilots
        public static int GetNumberOfPilots() 
        {
            return pilots.Count;
        }
       
    }
}
