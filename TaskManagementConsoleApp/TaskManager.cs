using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace TaskManagementConsoleApp
{
    public class TaskManager
    {
        private List<Task> tasks;
        private List<Task> completedTasks;
        private static readonly string _basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? String.Empty;
        private static readonly string _dataFolderPath = Path.Combine(_basePath, "data");
        private static readonly string _filePath = Path.Combine(_dataFolderPath, "tasks.json");


        public List<Task> GetCompletedTasks()
        {
            return completedTasks;
        }

        public List<Task> GetTasks()
        {
            return tasks;
        }
        public TaskManager()
        {
            tasks = new List<Task>();
            completedTasks = new List<Task>();
            LoadTasksFromFile(_filePath);
        }

        public TaskManager(string _filePath)
        {
            tasks = new List<Task>();
            completedTasks = new List<Task>();
            LoadTasksFromFile(_filePath);
        }

        public void LoadTasksFromFile(string _filePath)
        {

            string filePath = _filePath;
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        List<Task> allTasks = JsonConvert.DeserializeObject<List<Task>>(json);
                        tasks = allTasks.Where(task => !task.IsDone).ToList();
                        completedTasks = allTasks.Where(task => task.IsDone).ToList();
                    }
                    else
                    {
                        tasks = new List<Task>();
                        completedTasks = new List<Task>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading tasks from file: " + ex.Message);
                    tasks = new List<Task>();
                    completedTasks = new List<Task>();
                }
            }
            else
            {
                Console.WriteLine("Json file doesn't exist and will be created.");
                tasks = new List<Task>();
                completedTasks = new List<Task>();
            }
        }
        public void SaveTasksToFile()
        {
            List<Task> allTasks = tasks.Concat(completedTasks).ToList();
            string json = JsonConvert.SerializeObject(allTasks, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public void ListHelp()
        {
            Console.WriteLine("This application serves as a basic task management system.");
            Console.WriteLine("add - Use this command to add a new task.");
            Console.WriteLine("delete - Use this command to delete a task.");
            Console.WriteLine("edit - Use this command to edit a task.");
            Console.WriteLine("ls - List all the unfinished tasks.");
            Console.WriteLine("mc - Mark a task as completed.");
            Console.WriteLine("lsc - List all finished tasks.");
            Console.WriteLine("resetc - Removes all of the completed tasks.");
            Console.WriteLine("clear - Use this to clear the console.");
            Console.WriteLine("exit - Use this to command to gracefully exit the application.");
            Console.WriteLine("export - Use this command to export to a chosen format.");

        }
        public void AddTask(string title, string description)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description))
            {
                Task newTask = new Task
                {
                    Title = title,
                    Description = description,
                    IsDone = false
                };
                tasks.Add(newTask);
                SaveTasksToFile();
                Console.WriteLine("The task has been added successfully");
            }
            else
            {
                Console.WriteLine("Invalid task description/title");
            }
        }
        public void ListUnfinishedTasksWithDetails()
        {
            Console.WriteLine("Unfinished Tasks:");
            foreach (Task task in tasks)
            {
                Console.WriteLine($"Title: {task.Title}");
                Console.WriteLine($"Description: {task.Description}");
                Console.WriteLine();
            }
        }
        public void ListUnfinishedTasks()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {tasks[i].Title}");
            }
        }
        public void ListCompletedTasks()
        {
            Console.WriteLine("Completed Tasks:");
            for (int i = 0; i < completedTasks.Count; i++)
            {
                Console.WriteLine($"Title: {completedTasks[i].Title}");
                Console.WriteLine($"Description: {completedTasks[i].Description}");
                Console.WriteLine();
            }
        }
        public void EditTask(int index, string? updatedTitle = null, string? updatedDescription = null)
        {
            if (index > 0 && index <= tasks.Count)
            {
                if (updatedTitle != null)
                {
                    tasks[index - 1].Title = updatedTitle;
                }
                if (updatedDescription != null)
                {
                    tasks[index - 1].Description = updatedDescription;
                    return;
                }

                Console.Write($"Enter the updated title: [{tasks[index - 1].Title}]: ");
                string titleInput = Console.ReadLine()?.Trim()!;
                string title = !string.IsNullOrEmpty(titleInput) ? titleInput : tasks[index - 1].Title;

                Console.Write($"Enter the updated description: [{tasks[index - 1].Description}]: ");
                string descriptionInput = Console.ReadLine()?.Trim()!;
                string description = !string.IsNullOrEmpty(descriptionInput) ? descriptionInput : tasks[index - 1].Description;

                
                if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description))
                {
                    tasks[index - 1].Title = title;
                    tasks[index - 1].Description = description;

                    SaveTasksToFile();
                    Console.WriteLine("Task updated successfully.");
                }
                else
                {
                    Console.WriteLine("Invalid task description/title");
                }
            }
            else
            {
                Console.WriteLine("Invalid task index.");
            }
            
        }
        public void DeleteTask(int index)
        {
            if (index > 0 && index <= tasks.Count)
            {
                tasks.RemoveAt(index - 1);
                SaveTasksToFile();
                Console.WriteLine("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid task index.");
            }
        }

        public void MarkTaskAsCompleted(int index)
        {
            if (index > 0 && index <= tasks.Count)
            {
                Task completedTask = tasks[index - 1];
                completedTask.IsDone = true;
                completedTasks.Add(completedTask);
                tasks.RemoveAt(index - 1);
                SaveTasksToFile();
                Console.WriteLine("The task has been marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task index.");
            }
        }

        
        public void ClearConsole()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Task Management Application");
            Console.WriteLine("To list the available commands you can use the help prompt, to stop the application you can enter the exit command"); 
        }
        public void ExportTasks(string exportFilePath, string? command = null)
        {
            if (tasks.Any())
            {
                Console.WriteLine("Choose the format you would like to export the data in. The available formats are (json and xml) Use the appropriate command \"xml\" or \"json\"");
                Console.WriteLine("If you made a mistake and don't want to export the tasks enter (exit) and you will be returned to the initial screen");
                if (command != null)
                {
                    switch (command)
                    {
                        case "json":
                            ExportTasksToJson(exportFilePath);
                            return;
                        case "xml":
                            ExportTasksToXml(exportFilePath);
                            return;
                        case "exit":
                            return;
                        default:
                            Console.WriteLine("Invalid input.");
                            return;
                    }
                }
                switch (Console.ReadLine())
                {
                    case "json":
                        ExportTasksToJson(exportFilePath);
                        break;
                    case "xml":
                        ExportTasksToXml(exportFilePath);
                        break;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("No tasks to export.");
            }
        }
        private void ExportTasksToJson(string exportFilePath)
        {
            List<Task> allTasks = tasks.Concat(completedTasks).ToList();
            string json = JsonConvert.SerializeObject(allTasks, Formatting.Indented);
            
            string originalExportFilePath = exportFilePath;
            string fileExtension = ".json";
            int fileCounter = 1;

            while (File.Exists(exportFilePath))
            {
                string fileName = Path.GetFileNameWithoutExtension(originalExportFilePath);
                string newFileName = $"{fileName}{fileCounter}{fileExtension}";
                exportFilePath = Path.Combine(Path.GetDirectoryName(originalExportFilePath), newFileName);
                fileCounter++;
            }

            File.WriteAllText(exportFilePath, json);
            Console.WriteLine("Tasks exported to JSON successfully.");
        }
        public void ExportTasksToXml(string exportFilePath)
        {
            try
            {
                List<Task> allTasks = tasks.Concat(completedTasks).ToList();

                XDocument xmlDoc = new XDocument(
               new XElement("Tasks",
                   allTasks.Select(t => new XElement("Task",
                       new XElement("Title", t.Title),
                       new XElement("Description", t.Description),
                       new XElement("IsDone", t.IsDone)
                   ))
               )
           );
                exportFilePath = Path.ChangeExtension(exportFilePath, "xml");
                string originalExportFilePath = exportFilePath;
                string fileExtension = ".xml";
                int fileCounter = 1;
                while (File.Exists(exportFilePath))
                {
                    string fileName = Path.GetFileNameWithoutExtension(originalExportFilePath);
                    string newFileName = $"{fileName}{fileCounter}{fileExtension}";
                    exportFilePath = Path.Combine(Path.GetDirectoryName(originalExportFilePath), newFileName);
                    fileCounter++;
                }

                xmlDoc.Save(exportFilePath);
                Console.WriteLine("Tasks exported to XML successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error exporting tasks to XML: " + ex.Message);
            }
        }
        public void ResetCompletedTaskList()
        {
            if (completedTasks.Count > 0)
            {
                completedTasks.Clear();
                SaveTasksToFile();
                Console.WriteLine("Completed tasks reset successfully.");
            }
            else
            {
                Console.WriteLine("No completed tasks to reset.");
            }
        }
    }
}
