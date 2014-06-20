﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TheAirline.Model.GeneralModel;

namespace TheAirline.Model.AirportModel
{
    //the class for an expansion of an airport
    [Serializable]
    public class AirportExpansion : ISerializable
    {
        public enum ExpansionType { New_terminal, New_runway, Name, Extra_gates, Close_terminal }
        [Versioning("type")]
        public ExpansionType Type { get; set; }
        [Versioning("date")]
        public DateTime Date { get; set; }
        [Versioning("notify")]
        public Boolean NotifyOnChange { get; set; }
        //for type==name && type==new_terminal (name) && type == new_runway (name) && extra_gates (name) && close_terminal (name)
        [Versioning("name")]
        public string Name { get; set; }
        //for type==new_terminal && type == extra_gates
        [Versioning("gates")]
        public int Gates { get; set; }
        //for type==new_runway
        [Versioning("length")]
        public long Length { get; set; }
        [Versioning("surface")]
        public Runway.SurfaceType Surface { get; set; }
        public AirportExpansion(ExpansionType type, DateTime date, Boolean notifyOnChange)
        {
            this.Type = type;
            this.Date = date;
            this.NotifyOnChange = notifyOnChange;
        }
          private AirportExpansion(SerializationInfo info, StreamingContext ctxt)
        {
            int version = info.GetInt16("version");

            IList<PropertyInfo> props =
                new List<PropertyInfo>(
                    this.GetType()
                        .GetProperties()
                        .Where(
                            p =>
                                p.GetCustomAttribute(typeof(Versioning)) != null
                                && ((Versioning)p.GetCustomAttribute(typeof(Versioning))).AutoGenerated));

            foreach (SerializationEntry entry in info)
            {
                PropertyInfo prop =
                    props.FirstOrDefault(p => ((Versioning)p.GetCustomAttribute(typeof(Versioning))).Name == entry.Name);

                if (prop != null)
                {
                    prop.SetValue(this, entry.Value);
                }
            }

            IEnumerable<PropertyInfo> notSetProps =
                props.Where(p => ((Versioning)p.GetCustomAttribute(typeof(Versioning))).Version > version);

            foreach (PropertyInfo prop in notSetProps)
            {
                var ver = (Versioning)prop.GetCustomAttribute(typeof(Versioning));

                if (ver.AutoGenerated)
                {
                    prop.SetValue(this, ver.DefaultValue);
                }
            }

        }
          public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
          {
              info.AddValue("version", 1);

              Type myType = this.GetType();

              IEnumerable<FieldInfo> fields =
                  myType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                      .Where(p => p.GetCustomAttribute(typeof(Versioning)) != null);

              IList<PropertyInfo> props =
                  new List<PropertyInfo>(
                      myType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                          .Where(p => p.GetCustomAttribute(typeof(Versioning)) != null));

              IEnumerable<MemberInfo> propsAndFields = props.Cast<MemberInfo>().Union(fields.Cast<MemberInfo>());

              foreach (MemberInfo member in propsAndFields)
              {
                  object propValue;

                  if (member is FieldInfo)
                  {
                      propValue = ((FieldInfo)member).GetValue(this);
                  }
                  else
                  {
                      propValue = ((PropertyInfo)member).GetValue(this, null);
                  }

                  var att = (Versioning)member.GetCustomAttribute(typeof(Versioning));

                  info.AddValue(att.Name, propValue);
              }
          }
    }
}