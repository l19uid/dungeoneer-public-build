using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Dungeoneer
{
   public class FileDataHandler
   {
      private string _dataDirPath;
      private string _dataFileName;
      private string _encryptionKey = "&%#@?,:*";
      private bool _useEncryption = true;

      public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption = true)
      {
         _dataDirPath = dataDirPath;
         _dataFileName = dataFileName;
         _useEncryption = useEncryption;
      }

      public GameData Load()
      {
         // Create our full path.
         string fullPath = Path.Combine(_dataDirPath, _dataFileName);
         GameData loadedData = null;

         // If the file exists, try loading it.
         if (File.Exists(fullPath))
         {
            try
            {
               // Read the file.
               string dataToLoad = "";
               using (FileStream fs = new FileStream(fullPath, FileMode.Open))
               {
                  using (StreamReader reader = new StreamReader(fs))
                  {
                     dataToLoad = reader.ReadToEnd();
                  }
               }
               Debug.Log("Data : " + dataToLoad);

               if (_useEncryption)
                  dataToLoad = EncryptDecrypt(dataToLoad);
               // Convert the data to a GameData object.
               loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
               Console.WriteLine("Error occured while loading " + fullPath + ". Error: " + e);
               throw;
            }
         }

         // Return the loaded data.
         return loadedData;
      }

      public void Save(GameData data)
      {
         // Create our full path.
         string fullPath = Path.Combine(_dataDirPath, _dataFileName);
         Debug.Log(fullPath);

         try
         {
            // If the directory doesn't exist, create it.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Convert the GameData object to a string.
            string dataToSave = JsonUtility.ToJson(data, true);

            if (_useEncryption)
               dataToSave = EncryptDecrypt(dataToSave);

            // Write the data to the file.
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
               using (StreamWriter writer = new StreamWriter(fs))
               {
                  writer.Write(dataToSave);
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e);
            throw;
         }
      }
      
      private string EncryptDecrypt(string input)
      {
         string output = "";

         for (int i = 0; i < input.Length; i++)
         {
            output += (char) (input[i] ^ _encryptionKey[i % _encryptionKey.Length]);
         }

         return output;
      }
   }
}