using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TaskManagementConsoleApp;

namespace TaskManagementConsoleApp.Tests
{
    public class TaskManagementTests
    {
        private TaskManager taskManager;

        [SetUp]
        public void Setup()
        {
            string filePath = Path.Combine("..", "..", "..", "data", "tasks.json");
            taskManager = new TaskManager(filePath);
        }

        private string GetJsonFilePath()
        {
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string jsonFilePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "TaskManagementConsoleApp.Tests", "data", "tasks.json");
            return jsonFilePath;
        }

        [Test]
        public void AddTask_ValidInput_TaskAddedSuccessfully()
        {
            // Arrange
            string title = "Sample Task";
            string description = "This is a sample task";
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(GetJsonFilePath());

            // Act
            taskManager.AddTask(title, description);

            // Assert
            List<Task> tasks = taskManager.GetTasks();
            Assert.AreEqual(1, tasks.Count);

            Task addedTask = tasks[0];
            Assert.AreEqual(title, addedTask.Title);
            Assert.AreEqual(description, addedTask.Description);
            Assert.IsFalse(addedTask.IsDone);


        }
        [Test]
        public void MarkTaskAsCompleted_ValidInput_TaskMarkedAsCompletedSuccessfully()
        {
            // Arrange
            string filePath = GetJsonFilePath();
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(filePath);
            string title = "Sample Task";
            string description = "This is a sample task";
            taskManager.AddTask(title, description);
            int index = 1;

            // Act
            taskManager.MarkTaskAsCompleted(index);

            // Assert
            List<Task> completedTasks = taskManager.GetCompletedTasks();
            Assert.AreEqual(1, completedTasks.Count);
            Assert.AreEqual(title, completedTasks[0].Title);
            Assert.AreEqual(description, completedTasks[0].Description);
            Assert.IsTrue(completedTasks[0].IsDone);
            List<Task> unfinishedTasks = taskManager.GetTasks();
            Assert.AreEqual(0, unfinishedTasks.Count);
        }
        [Test]
        public void EditTask_ValidInput_SuccessfullyEditedTask()
        {
            // Arrange
            string filePath = GetJsonFilePath();
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(filePath);

            string title = "Sample title";
            string description = "Sample description";
            taskManager.AddTask(title, description);

            string updatedTitle = "Updated Task";
            string updatedDescription = "This is the updated task description";
            int index = 1;

            // Act
            taskManager.EditTask(index, updatedTitle, updatedDescription);

            // Assert
            List<Task> tasks = taskManager.GetTasks();
            Task editedTask = tasks[index - 1];
            Assert.AreEqual(updatedTitle, editedTask.Title);
            Assert.AreEqual(updatedDescription, editedTask.Description);
        }
        [Test]
        public void DeleteTask_ValidInput_SuccessfullyDeletedTask()
        {
            // Arrange
            string filePath = GetJsonFilePath();
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(filePath);

            string title = "Sample title";
            string description = "Sample description";
            taskManager.AddTask(title, description);
            taskManager.AddTask(title, description);
            taskManager.AddTask(title, description);
            int index = 1;

            // Act
            taskManager.DeleteTask(index + 2);
            taskManager.DeleteTask(index + 1);
            taskManager.DeleteTask(index);
            // Assert
            List<Task> tasks = taskManager.GetTasks();
            Assert.AreEqual(0, tasks.Count);
        }
        [Test]
        public void ListHelp_ValidInput_SuccessfullyListedHelp()
        {
            // Arrange
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            string expectedOutput = "This application serves as a basic task management system." +
                Environment.NewLine +
                "add - Use this command to add a new task." +
                Environment.NewLine +
                "delete - Use this command to delete a task." +
                Environment.NewLine +
                "edit - Use this command to edit a task." +
                Environment.NewLine +
                "ls - List all the unfinished tasks." +
                Environment.NewLine +
                "mc - Mark a task as completed." +
                Environment.NewLine +
                "lsc - List all finished tasks." +
                Environment.NewLine +
                "resetc - Removes all of the completed tasks." +
                Environment.NewLine +
                "clear - Use this to clear the console." +
                Environment.NewLine +
                "exit - Use this to command to gracefully exit the application." +
                Environment.NewLine +
                "export - Use this command to export to a chosen format." +
                Environment.NewLine;

            // Act
            taskManager.ListHelp();

            // Assert
            string output = sw.ToString();
            Assert.AreEqual(expectedOutput, output);
        }
        [Test]
        public void ListUnfinishedTasksWithDetails_SuccessfullyListed()
        {
            // Arrange
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(GetJsonFilePath());
            string title1 = "Sample title 1";
            string description1 = "Sample description 1";
            string title2 = "Sample title 2";
            string description2 = "Sample description 2";
            string title3 = "Sample title 3";
            string description3 = "Sample description 3";
            taskManager.AddTask(title1, description1);
            taskManager.AddTask(title2, description2);
            taskManager.AddTask(title3, description3);
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            List<Task> tasks = taskManager.GetTasks();
            string expectedOutput = "Unfinished Tasks:" +
                Environment.NewLine +
                $"Title: {tasks[0].Title}" +
                Environment.NewLine +
                $"Description: {tasks[0].Description}" +
                Environment.NewLine +
                Environment.NewLine +
                $"Title: {tasks[1].Title}" +
                Environment.NewLine +
                $"Description: {tasks[1].Description}" +
                Environment.NewLine +
                Environment.NewLine +
                $"Title: {tasks[2].Title}" +
                Environment.NewLine +
                $"Description: {tasks[2].Description}" +
                Environment.NewLine;

            // Act
            taskManager.ListUnfinishedTasksWithDetails();

            // Assert
            string output = sw.ToString();
            Assert.AreEqual(expectedOutput.Trim(), output.Trim());
        }
        [Test]
        public void ListUnfinishedTasks_SuccessfullyListed()
        {
            // Arrange
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(GetJsonFilePath());
            string title1 = "Sample title 1";
            string description1 = "Sample description 1";
            string title2 = "Sample title 2";
            string description2 = "Sample description 2";
            string title3 = "Sample title 3";
            string description3 = "Sample description 3";
            taskManager.AddTask(title1, description1);
            taskManager.AddTask(title2, description2);
            taskManager.AddTask(title3, description3);
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            List<Task> tasks = taskManager.GetTasks();
            string expectedOutput = $"(1) {tasks[0].Title}" +
                Environment.NewLine +
                $"(2) {tasks[1].Title}" +
                Environment.NewLine +
                $"(3) {tasks[2].Title}";

            // Act
            taskManager.ListUnfinishedTasks();

            // Assert
            string output = sw.ToString();
            Assert.AreEqual(expectedOutput.Trim(), output.Trim());
        }
        [Test]
        public void ExportTasks_SuccessfullyExported()
        {
            // Arrange
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(GetJsonFilePath());
            string title1 = "Sample title 1";
            string description1 = "Sample description 1";
            string title2 = "This is a sample title 2";
            string description2 = "This is a sample description 2";
            taskManager.AddTask(title1, description1);
            taskManager.AddTask(title2, description2);
            string commandJson = "json";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string baseFileName = "tasks.json";
            string exportFileNameJson = baseFileName;
            string exportFileNameXml = "tasks.xml";

            string exportFilePathJson = Path.Combine(desktopPath, exportFileNameJson);
            string exportFilePathXml = Path.Combine(desktopPath, exportFileNameXml);

            if (File.Exists(exportFilePathJson))
            {
                int counter = 1;
                string jsonExtension = Path.GetExtension(exportFileNameJson);
                string jsonFileNameWithoutExtension = Path.GetFileNameWithoutExtension(exportFileNameJson);

                while (File.Exists(exportFilePathJson))
                {
                    exportFileNameJson = $"{jsonFileNameWithoutExtension}{counter}{jsonExtension}";
                    exportFilePathJson = Path.Combine(desktopPath, exportFileNameJson);
                    counter++;
                }

                counter = 1;
                string xmlExtension = Path.GetExtension(exportFileNameXml);
                string xmlFileNameWithoutExtension = Path.GetFileNameWithoutExtension(exportFileNameXml);

                while (File.Exists(exportFilePathXml))
                {
                    exportFileNameXml = $"{xmlFileNameWithoutExtension}{counter}{xmlExtension}";
                    exportFilePathXml = Path.Combine(desktopPath, exportFileNameXml);
                    counter++;
                }
            }
            string commandXml = "xml";

            // Act
            taskManager.ExportTasks(exportFilePathJson, commandJson);
            taskManager.ExportTasks(exportFilePathXml, commandXml);

            // Assert
            Assert.IsTrue(File.Exists(exportFilePathJson));
            Assert.IsTrue(File.Exists(exportFilePathXml));
            List<Task> originalTasks = taskManager.GetTasks();
            List<Task> exportedJsonTasks = JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(exportFilePathJson));
            Assert.AreEqual(originalTasks.Count, exportedJsonTasks.Count);

            for (int i = 0; i < originalTasks.Count; i++)
            {
                Assert.AreEqual(originalTasks[i].Title, exportedJsonTasks[i].Title);
                Assert.AreEqual(originalTasks[i].Description, exportedJsonTasks[i].Description);
                Assert.AreEqual(originalTasks[i].IsDone, exportedJsonTasks[i].IsDone);
            }

            List<Task> exportedXmlTasks = new List<Task>();
            XDocument xmlDoc = XDocument.Load(exportFilePathXml);
            foreach (XElement taskElement in xmlDoc.Root.Elements("Task"))
            {
                string title = taskElement.Element("Title").Value;
                string description = taskElement.Element("Description").Value;
                bool isDone = bool.Parse(taskElement.Element("IsDone").Value);
                Task task = new Task();
                task.Title = title;
                task.Description = description;
                task.IsDone = isDone;
                exportedXmlTasks.Add(task);
            }
            Assert.AreEqual(originalTasks.Count, exportedXmlTasks.Count);

            for (int i = 0; i < originalTasks.Count; i++)
            {
                Assert.AreEqual(originalTasks[i].Title, exportedXmlTasks[i].Title);
                Assert.AreEqual(originalTasks[i].Description, exportedXmlTasks[i].Description);
                Assert.AreEqual(originalTasks[i].IsDone, exportedXmlTasks[i].IsDone);
            }
        }
        [Test]
        public void ResetCompletedTaskList_SuccessfullyReset()
        {
            // Arrange
            File.Delete(GetJsonFilePath());
            taskManager.LoadTasksFromFile(GetJsonFilePath());
            string title1 = "Sample title 1";
            string description1 = "Sample description 1";
            string title2 = "This is a sample title 2";
            string description2 = "This is a sample description 2";
            string title3 = "This is a sample title 3";
            string description3 = "This is a sample description 3";
            taskManager.AddTask(title1, description1);
            taskManager.AddTask(title2, description2);
            taskManager.AddTask(title3, description3);
            taskManager.MarkTaskAsCompleted(0);
            taskManager.MarkTaskAsCompleted(1);
            List<Task> originalIncompletedTasks = taskManager.GetTasks();

            // Act
            taskManager.ResetCompletedTaskList();

            // Assert
            List<Task> completedTasks = taskManager.GetCompletedTasks();
            List<Task> incompletedTasks = taskManager.GetTasks();
            Assert.AreEqual(0, completedTasks.Count);
            Assert.AreEqual(originalIncompletedTasks.Count,incompletedTasks.Count);
            Assert.AreEqual(originalIncompletedTasks[0].Title, incompletedTasks[0].Title);
            Assert.AreEqual(originalIncompletedTasks[0].Description, incompletedTasks[0].Description);
        }
        [Test]
        public void SaveTasksToFile_SuccessfullySavedToFile()
        {
            // Arrange
            File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "data", "tasks.json"));
            string filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "data", "tasks.json");
            taskManager.LoadTasksFromFile(filePath);
            string title1 = "Sample title 1";
            string description1 = "Sample description 1";
            string title2 = "This is a sample title 2";
            string description2 = "This is a sample description 2";
            string title3 = "This is a sample title 3";
            string description3 = "This is a sample description 3";
            string title4 = "This is a sample title 4";
            string description4 = "This is a sample description 4";
            string title5 = "This is a sample title 5";
            string description5 = "This is a sample description 5";
            taskManager.AddTask(title1, description1);
            taskManager.AddTask(title2, description2);
            taskManager.AddTask(title3, description3);
            taskManager.AddTask(title4, description4);
            taskManager.AddTask(title5, description5);
            taskManager.MarkTaskAsCompleted(0);
            taskManager.MarkTaskAsCompleted(1);
            taskManager.MarkTaskAsCompleted(3);
            List<Task> originalTasks = taskManager.GetTasks();
            List<Task> originalCompletedTasks = taskManager.GetCompletedTasks();

            // Act
            taskManager.SaveTasksToFile();

            // Assert
            Assert.IsTrue(File.Exists(filePath));

            string fileContent = File.ReadAllText(filePath);
            List<Task> deserializedTasks = JsonConvert.DeserializeObject<List<Task>>(fileContent);
            Assert.AreEqual(originalCompletedTasks.Count, deserializedTasks.Count - originalTasks.Count);
            List<Task> deserializedCompletedTasks = deserializedTasks.Where(task => task.IsDone).ToList();
            for (int i = 0; i < originalCompletedTasks.Count; i++)
            {
                Assert.AreEqual(originalCompletedTasks[i].Title, deserializedCompletedTasks[i].Title);
                Assert.AreEqual(originalCompletedTasks[i].Description, deserializedCompletedTasks[i].Description);
                Assert.AreEqual(originalCompletedTasks[i].IsDone, deserializedCompletedTasks[i].IsDone);
            }
            List<Task> deserializedUncompletedTasks = deserializedTasks.Where(task => !task.IsDone).ToList();
            for (int i = 0; i < originalTasks.Count; i++)
            {
                Assert.AreEqual(originalTasks[i].Title, deserializedUncompletedTasks[i].Title);
                Assert.AreEqual(originalTasks[i].Description, deserializedUncompletedTasks[i].Description);
                Assert.AreEqual(originalTasks[i].IsDone, deserializedUncompletedTasks[i].IsDone);
            }
        }
    }
}