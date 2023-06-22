# About the application

This application is a .NET Core 6.0 Console Application, made using C# and Visual Studio. It uses a tasks.json file to store it’s values and have persistent memory. (It is located in bin\Release\net6.0\data folder or bin\Debug\net6.0\data depending on the execution mode.)

•	The program allows the user to export already entered values in json or xml format by using the command <b>“export”</b> and choosing the format of their liking (using the appropriate <b>“json”</b> or <b>“xml”</b> command) to their Desktop by default. If there is already a file on the Desktop using the same name “tasks.json” / “tasks.xml” the program will save it as tasks1.json / tasks1.xml. 

•	The user can also add new tasks to the application by using the <b>“add”</b> command and proceeding in entering the title and description of the task. 

•	A <b>“help”</b> command is available, which shows every other command that can be used. 

•	If a user wants to see every task, he has already added and saved as uncompleted, the command <b>“ls”</b> will show those tasks. 

•	If a user wants to delete a task from the application, he can use the command <b>“delete”</b>. After entering the command, a list of the uncompleted tasks will be shown with indexes. By entering the corresponding index, the appropriate task will be removed forever.

•	If the user has completed a certain task and doesn’t want to delete it, but instead keep it separate from the other tasks, he can use the command <b>“mc”</b> which would let the user choose using indexes, which task to mark as completed. 

•	After a task has been marked as completed it will be separated from the unfinished tasks and can now only be accessed from the <b>“lsc”</b> command, which shows every task that has been marked as completed. 

•	If a user has made a mistake and wants to edit a task, he can do so only to an unfinished one by typing in <b>“edit”</b> and then by choosing the appropriate index. After that the user will be prompted to enter a new title and description. 

•	If there is a lot of clutter on the screen the command <b>“clear”</b> will delete everything on it, except the application’s greeting. 

•	The user can reset the list of completed tasks, which would effectively wipe everything that’s contained in it, by using the command <b>“resetc”</b>. The user will be prompted about whether he is sure about his decision.

•	Last, but not least, the command <b>“exit”</b> can be used to exit the application gracefully.

# Software Requirements

Before compiling and running the application, please ensure that the following requirements are met:

•	.NET Core SDK 6.0 or later: The application is built on the .NET Core Framework. Make sure the appropriate SDK is installed on your machine. (If for some reason the application is not being compiled on a newer version of the .NET Core SDK, please use .NET Core SDK 6.0)

•	IDE: Any IDE that supports .NET Core development, such as Visual Studio, or JetBrains Rider.

# NuGet Packages

The application relies on the following NuGet packages:

•	Newtonsoft.Json: Used for JSON serialization and deserialization.

•	NUnit.Framework: Used for the purpose of unit testing the application. 

•	NUnit3TestAdapter: Required for executing NUnit tests within Visual Studio.

•	coverlet.collector: Used for collecting code coverage information during test execution.

•	Microsoft.NET.Test.SDK: Provides the necessary tools and infrastructure for running tests.

