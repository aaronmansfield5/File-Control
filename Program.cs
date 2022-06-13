using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSharpLib;

namespace File_Control
{
    class Program
    {
        static bool ChangeFile(string fileName, string changed)
        {
            string[] readData = File.ReadAllText("File Control Changes.txt").Split(";");
            foreach(var container in readData)
            {
                List<string> data = container.Split("=").ToList();
                if(data[0] == fileName)
                {
                    string toSave = $"{fileName}={changed}={DateTime.Now}";
                    foreach (var dataPiece in readData.ToList().Where(f => f != container))
                    {
                        toSave += $";{dataPiece}";
                    }
                    using(StreamWriter writer = new StreamWriter("File Control Changes.txt"))
                    {
                        writer.Write(toSave);
                        writer.Close();
                    }
                    return true;
                }
            }
            if (File.ReadAllText("File Control Changes.txt").Length == 0)
            {
                using (StreamWriter writer = new StreamWriter("File Control Changes.txt"))
                {
                    writer.Write($"{fileName}={changed}={DateTime.Now}");
                    writer.Close();
                }
                return true;
            }
            else
            {
                using (StreamWriter writer = new StreamWriter("File Control Changes.txt", true))
                {
                    writer.Write($";{fileName}={changed}={DateTime.Now}");
                    writer.Close();
                }
                return true;
            }
        }
        static string GetDetails(string fName)
        {
            string[] readData = File.ReadAllText("File Control Changes.txt").Split(";");
            foreach (var container in readData)
            {
                List<string> data = container.Split("=").ToList();
                if (data[0] == fName)
                {
                    return $" - {fName} | {data[1]} at {data[2]}";
                }
            }
            return "";
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Would you like to create or edit a file?");
            var userChoice = Console.ReadLine().ToLower();
            Console.Clear();
            if (userChoice.Contains("edit")) {
                string[] Files = Directory.GetFiles(".").ToList().Where(f => !f.ToString().ToLower().Contains("file control") && !f.ToString().ToLower().Contains("csharplib")).Select(f => f.Replace(".\\", "")).ToArray();
                if(Files.Length >= 1)
                {
                    Console.WriteLine("Which file would you like to edit?");
                    foreach (var file in Files)
                    {
                        string fileName = file.ToString();
                        Console.WriteLine($"{GetDetails(fileName)} | {new FileInfo(fileName).FormatBytes()}");
                    }
                    string toEdit = Console.ReadLine();
                    if(Files.Contains(toEdit))
                    {
                        Console.WriteLine("Would you like to delete the following file or edit it's contents?");
                        string editChoice = Console.ReadLine().ToLower();
                        if (editChoice.Contains("delete"))
                        {
                            File.Delete(toEdit);
                            Console.WriteLine($"'{toEdit}' was successfully deleted.");
                        } else if (editChoice.Contains("edit")) {
                            Console.WriteLine("What would you like to set the file's contents to?");
                            string contents = Console.ReadLine();
                            using (StreamWriter Writer = new StreamWriter(toEdit))
                            {
                                Writer.Write(contents);
                                Writer.Close();
                            }
                            Console.WriteLine($"Your file named '{toEdit}' was written to!");
                            ChangeFile(toEdit, "Edited");
                        } else {
                            Console.WriteLine($"'{editChoice}' isn't a valid option.");
                        }
                    } else {
                        Console.WriteLine("That file does not exist!");
                    }
                } else {
                    Console.WriteLine("No user created files currently exist.");
                }
            } else if (userChoice.Contains("create")) {
                Console.WriteLine("What would you like to call your file? Please provide the file name extension.");
                string fName = Console.ReadLine();
                File.Create(fName).Close();
                Console.WriteLine($"Your file named '{fName}' was created.\nWhat would you like to set the file's contents to? Leave empty if not applicable.");
                ChangeFile(fName, "Created");
                string contents = Console.ReadLine();
                if(contents != "")
                {

                    using(StreamWriter Writer = new StreamWriter(fName))
                    {
                        Writer.Write(contents);
                        Writer.Close();
                    }
                    Console.WriteLine($"Your file named '{fName} was written to!");
                } else {
                    Console.WriteLine("Your file was not written to.");
                }
            } else {
                Console.WriteLine($"'{userChoice}' isn't a valid option.");
            }
        }
    }
}

/* You need to create an interface that can use this code as well as meet additional 
 * client requirements.
 * 
 * User friendly interface inc personalisation for user.
 * Options screen
 * Choice of Create,edit,search,delete individual files
 * Security features
 * Maintainance comments
 * Exception handling.
 * Any additional enhancement features
*/
