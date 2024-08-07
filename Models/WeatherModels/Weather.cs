﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DynamicWeather.Enums;
using DynamicWeather.IO;

namespace DynamicWeather.Models
{
    public class Weather
    {
        [XmlIgnore]
        public WeatherType WeatherType { get; set; }
        
        [XmlAttribute]
        public string WeatherName { get; set; }
       
        [XmlIgnore]
        public int Temperature { get; set; }
        
        [XmlAttribute]
        public int MinTemperature { get; set; }
        
        [XmlAttribute]
        public int MaxTemperature { get; set; }
        
        [XmlIgnore]
        public string WeatherTextureName { get; set; }
        

        internal Weather(WeatherType weatherType, string weatherName, int temperature, int minTemperature, int maxTemperature)
        {
            WeatherType = weatherType;
            WeatherName = weatherName.ToUpper();
            Temperature = temperature;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            WeatherTextureName = TextureType.GetTextureName(weatherType);
        }
        public Weather() { }
    }

    [XmlRoot("Weathers", IsNullable = false)]

    public class Weathers
    {
        internal static Dictionary<WeatherType,Weather> WeatherData { get; set; }

        public Weather[] AllWeathers;
        
        internal Weathers() { }
        
        internal static void DeserializeWeathers()
        {
            XMLParser<Weathers> xmlParser = new(@"Plugins/DynamicWeather/Weathers.xml");
            Weathers data = xmlParser.DeserializeXML();
            Random random = new Random(DateTime.Today.Millisecond);
            foreach (Weather weather in data.AllWeathers)
            {
                if (!Enum.TryParse(weather.WeatherName, true, out WeatherType type))
                {
                    throw new InvalidDataException($"Invalid weather name found in xml: {weather.WeatherName}");
                }
                if (WeatherData.ContainsKey(type))
                {
                    throw new InvalidDataException($"Duplicate weather types found in xml: {weather.WeatherName}");
                }
                Weather w = new Weather(type, weather.WeatherName.ToUpper(), 
                    random.Next(weather.MinTemperature, weather.MaxTemperature + 1), weather.MinTemperature, 
                    weather.MaxTemperature);
                WeatherData.Add(type, w);
            }
            
        }

    }
}
