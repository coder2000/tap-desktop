﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheAirline.Model.AirportModel
{
    //the class for a runway at an airport
    public class Runway
    {
        public string Name { get; set; }
        public long Length { get; set; }

        public enum SurfaceType { Asphalt, Concrete, Grass, Dirt, Gravel, Ice, Salt }
        public SurfaceType Surface { get; set; }
        public Runway(string name, long length, SurfaceType surface)
        {
            this.Name = name;
            this.Length = length;
            this.Surface = surface;
        }
    }
}
