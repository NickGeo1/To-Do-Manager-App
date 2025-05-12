# To-Do Manager App

## About
A small Desktop application where multiple users can manage their to-do tasks.
Each user is able to generate a list of to-do tasks which have the following details:
1. The title of the task
2. The description of the task
3. The priority of the task in a scale 1-10

4. The deadline date of the task

Users are also able to change the status of the task from **Pending** to **Completed** and the other way around.<br>

## Features

The application supports **Login** and **Register** system and stores the tasks in a local **MySQL database**.<br> 
The features of the current version of the application consist of:
1. Adding a new task
2. Deleting a current task
3. Modifying the details of a current task
4. Sorting tasks by date (undoable)

5. Sorting tasks by priority (undoable)

## Installation
1. Download **MySQL Installer**, setup and install **MySQL Server** and **MySQL Workbench** if you don't already have them, [here](https://dev.mysql.com/downloads/installer/) (you may prefer to choose full installation).
3. Download **To-Do-App.zip** from the latest [Release version](https://github.com/NickGeo1/To-Do-Manager-App/releases/tag/v1.0.0).
4. Open ``To-Do App\db\tododb.mwb`` file in **MySQL Workbench**. You will notice the database structure.
5. At the toolbar located at the top of the window, navigate to **Database -> Forward Engineer** and hit **Next** to all steps.
   
6. Open your connection and you will notice the **tododb** schema listed at the leftmost **SCHEMAS** list

**Steps 6-10 are optional** in case you want to add some data beforehand:

6. At the toolbar, navigate to **Server -> Data Import**
7. At **Import Options** choose **Import from Self-Contained File** and then browse ``To-Do App\db\data.sql``
8. As **Default Target Schema** choose **tododb**
9. Hit **Start Import** button at the botom right.

10. To test if data were successfully imported go to **tododb -> Tables -> Right click on tasks/users -> Select Rows - Limit 1000**
11. Run the application at ``To-Do App\To-Do Management System\To-Do Management System.exe``
12. Import your database connection details and hit **Connect**
13. Enjoy!



## Screenshots

![image](https://github.com/user-attachments/assets/a801e3f2-9570-4245-b79c-defc1a7e25c1)

![image](https://github.com/user-attachments/assets/13cffb5e-8b2b-4088-bdc0-d37027bb03c6)

![image](https://github.com/user-attachments/assets/0a9961d9-8181-4ffd-9782-8fff49fb80f3)

