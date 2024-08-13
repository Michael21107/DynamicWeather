﻿using System;
using Rage;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.IO;
using DynamicWeather.Enums;
using DynamicWeather.Models;


namespace DynamicWeather.Helpers
{
    internal class TextureHelper
    {
        internal static void DrawTexture(Rage.Graphics g, List<Texture> textures)
        {
            SizeF size = Game.Resolution;
            for (int index = 0; index < textures.Count; index++)
            {
                Texture texture = textures[index];
                DrawTexture(g,texture, size.Width / (textures.Count + 1) * (index + 1), size.Height / 4, 96, 96);
            }
        }

        internal static void DrawTexture(Rage.Graphics g, Texture texture, float x, float y, float width, float height)
        {
            if (texture != null)
                g.DrawTexture(texture, x, y, width, height);
            else
                return;
        }

        internal static void DrawText(Rage.Graphics g, Text text, float x, float y)
        {
            g.DrawText(text.value, "Lucida Console", text.fontSize, new PointF(x,y), text.color);
        }
        
        internal static void DrawText(Rage.Graphics g, List<Text> texts)
        {
            SizeF size = Game.Resolution;
            for (int index = 0; index < texts.Count; index++)
            {
                Text text = texts[index];
                DrawText(g, text, (size.Width / (texts.Count + 1) * (index + 1)), (size.Height / 4) - 100);
            }
        }
        
        internal static void LoadAllTextures()
        {
            foreach (var texture in Directory.GetFiles(@"Plugins/DynamicWeather/Textures"))
            {
                String filename = Path.GetFileNameWithoutExtension(texture).ToUpper();
                if (!Enum.TryParse(filename, true, out WeatherTypesEnum type))
                {
                    Game.LogTrivial($"Invalid texture name found in directory: {filename}");
                }
                if ((Weathers.WeatherData[type].DayTexture != null) || (Weathers.WeatherData[type].NightTexture != null))
                {
                    Game.LogTrivial($"Duplicate texture types found in directory: {filename}");
                }
                Game.LogTrivial($"Associated {filename} with {Weathers.WeatherData[type].WeatherName}");
                if (filename.Contains("DAY"))
                {
                    Weathers.WeatherData[type].DayTexture = Game.CreateTextureFromFile(texture);
                    if (type == WeatherTypesEnum.Clear)
                    {
                        Weathers.WeatherData[WeatherTypesEnum.ExtraSunny].DayTexture = Game.CreateTextureFromFile(texture);
                        Weathers.WeatherData[WeatherTypesEnum.Neutral].DayTexture = Game.CreateTextureFromFile(texture);
                    }
                }
                else if(filename.Contains("NIGHT"))
                {
                    Weathers.WeatherData[type].NightTexture = Game.CreateTextureFromFile(texture);
                    if (type == WeatherTypesEnum.Clear)
                    {
                        Weathers.WeatherData[WeatherTypesEnum.ExtraSunny].NightTexture = Game.CreateTextureFromFile(texture);
                        Weathers.WeatherData[WeatherTypesEnum.Neutral].NightTexture = Game.CreateTextureFromFile(texture);
                    }
                }
            }
        }
    }

    internal struct Text
    {
        internal string value;
        internal float fontSize;
        internal Color color;

        public Text(string value, float fontSize, Color color)
        {
            this.value = value;
            this.fontSize = fontSize;
            this.color = color;
        }
    }
}
