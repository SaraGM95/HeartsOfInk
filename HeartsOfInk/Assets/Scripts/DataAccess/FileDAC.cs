using HeartsOfInk.SharedLogic;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.DataAccess
{
    /// <summary>
    /// Carga y guardado del fichero de opciones.
    /// </summary>
    public class FileDAC<T>
    {
        public static T LoadJsonFile(string path)
        {
            if (File.Exists(path))
            {
                return JsonCustomUtils<T>.ReadObjectFromFile(path);

            } 
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Guarda el contenido serializado en json en un fichero. Si ya existe lo sobrescribe.
        /// </summary>
        /// <param name="fileContent"> Contenido del fichero.</param>
        /// <param name="path"> Ruta.</param>
        public static void SaveJsonFile(T fileContent, string path)
        {
            JsonCustomUtils<T>.SaveObjectIntoFile(fileContent, path);
        }
    }

    public class FileDAC
    {
        public static void SaveBase64File(string fileContentbase64, string path)
        {
            try
            {
                byte[] fileContentBytes = Convert.FromBase64String(fileContentbase64);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllBytes(path, fileContentBytes);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}