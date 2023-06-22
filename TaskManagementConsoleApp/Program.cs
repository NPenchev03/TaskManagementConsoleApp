using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            TaskManager taskManager = new TaskManager();
            Console.WriteLine("Welcome to the Task Management Application");
            Console.WriteLine("To list the available commands you can use the help prompt, to stop the application you can enter the exit command");
            while (true)
            {
                command = Console.ReadLine()?.Trim() ?? String.Empty;
                int index;
                switch (command)
                {
                    case "add":
                        Console.WriteLine("Enter the title of the task:");
                        string title = Console.ReadLine()?.Trim() ?? String.Empty;
                        Console.WriteLine("Enter the details of the task:");
                        string description = Console.ReadLine()?.Trim() ?? String.Empty;
                        taskManager.AddTask(title, description);
                        break;
                    case "delete":
                        if (taskManager.GetTasks().Count == 0)
                        {
                            Console.WriteLine("There are no unfinished tasks currently, when you any tasks they will be listed here.");
                            break;
                        }
                        taskManager.ListUnfinishedTasks();
                        Console.WriteLine("Using the appropriate number, choose the task you would like to delete.");
                        if (int.TryParse(Console.ReadLine(), out index))
                        {
                            taskManager.DeleteTask(index);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case "edit":
                        if (taskManager.GetTasks().Count == 0)
                        {
                            Console.WriteLine("There are no uncompleted tasks currently, when you any tasks they will be listed here.");
                            break;
                        }
                        taskManager.ListUnfinishedTasks();
                        Console.WriteLine("Using the appropriate number, choose the task you would like to edit.");
                        if (int.TryParse(Console.ReadLine(), out index))
                        {
                            taskManager.EditTask(index);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case "ls": 
                        taskManager.ListUnfinishedTasksWithDetails();
                        break;
                    case "lsc":
                        taskManager.ListCompletedTasks();
                        break;
                    case "mc":
                        if (taskManager.GetCompletedTasks().Count == 0)
                        {
                            Console.WriteLine("There are no tasks currently, when you add any uncompleted tasks they will be listed here.");
                            break;
                        }
                        taskManager.ListUnfinishedTasks();
                        Console.WriteLine("Using the appropriate number, choose the task you would like to mark as completed.");
                        if (int.TryParse(Console.ReadLine(), out index))
                        {
                            taskManager.MarkTaskAsCompleted(index);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                        }
                        break;
                    case "resetc":
                        taskManager.ResetCompletedTaskList();
                        break;
                    case "export":
                        Console.WriteLine("The default export location is the Desktop. Are you sure you wish to proceed? (Y?)");
                        string userResponse = Console.ReadLine()?.Trim() ?? String.Empty;
                        if (userResponse.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string exportFilePath = Path.Combine(desktopPath, "tasks.json");
                            taskManager.ExportTasks(exportFilePath);
                        }
                        else
                        {
                            Console.WriteLine("Export operation canceled by the user.");
                        }
                        break;
                    case "help": taskManager.ListHelp();
                        break;
                    case "clear": taskManager.ClearConsole();
                        break;
                    case "exit": Environment.Exit(0);
                        break;
                    default:Console.WriteLine("Please enter a valid command");
                        break;
                }
            }
        }
    }
}