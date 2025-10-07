using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DataAccess
{
    public class MapSpriteDAC
    {
        public static Sprite LoadMapSprite(string spriteName)
        {
            Sprite result;
            byte[] imageData;
            Texture2D texture = new Texture2D(2, 2);
            Rect rect;
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            string fullPath = GetMapSpritePath();

            fullPath += spriteName.StartsWith("/") ? spriteName : "/" + spriteName;
            imageData = File.ReadAllBytes(fullPath);
            texture.LoadImage(imageData);
            rect = new Rect(0f, 0f, texture.width, texture.height);

            result = Sprite.Create(texture, rect, pivot, texture.height / 8);

            return result;
        }

        public static void SaveMapSprite(string spriteName, string sprite)
        {
            byte[] allBytes = null;
            string fullPath;

            try
            {
                fullPath = GetMapSpritePath();
                Directory.CreateDirectory(fullPath);

                fullPath += spriteName.StartsWith("/") ? spriteName : "/" + spriteName;
                allBytes = Convert.FromBase64String(sprite);
                File.WriteAllBytes(fullPath, allBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetMapSpritePath()
        {
            string fullPath = Application.persistentDataPath;
            if (!fullPath.EndsWith("/") && !fullPath.EndsWith("\\"))
            {
                fullPath += "/";
            }

            // Todo: revisar, es posible que esto ya no sea así.
            // Debido a los múltiples origenes de datos a veces viene "MapSprites" al final y otras veces no.
            if (!fullPath.EndsWith("MapSprites"))
            {
                fullPath += "MapSprites";
            }

            return fullPath;
        }
    }
}
