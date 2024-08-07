﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DynamicWeather.Helpers;
using DynamicWeather.Models;
using Rage;
using Rage.Exceptions;
using Rage.Native;

[assembly: Rage.Attributes.Plugin("DynamicWeather", Author = "Roheat",PrefersSingleInstance = true,Description = "More variety in the sky")]
namespace DynamicWeather
{
    internal static class EntryPoint
    {
        internal static bool drawing = false;
        internal static Forecast currentForecast = null;
        internal static void Main()
        {
            Weathers.DeserializeAndValidateXML();
            TextureHelper.LoadAllTextures();
            currentForecast = new Forecast(1);
            while (true)
            {
                GameFiber.Yield();
                if (Game.IsKeyDownRightNow(Keys.Tab) && !drawing) 
                {
                    Start();
                    drawing = true;
                }
                else if (!Game.IsKeyDownRightNow(Keys.Tab) && drawing)
                {
                    Stop();
                    drawing = false;
                }

            }
        }
 
        internal static void Start()
        {
            Game.RawFrameRender += FrameRender;
        }
        
        internal static void Stop()
        {
            Game.RawFrameRender -= FrameRender;
        }
        
        private static void FrameRender(object sender, GraphicsEventArgs e)
        {
            currentForecast.DrawForecast(e.Graphics);
        }
    }
}   