//PTB Prototype - StudyQuest
//Zain Rizwan
//Most recent changes made 28/5/24

//i honestly dont know why i need this many usings but im scared to break it so they stay
using System;
using BetterConsoles.Tables;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using BetterConsoles.Tables.Models;
using BetterConsoles.Tables.Configuration;
using System.Drawing;
using BetterConsoles.Tables.Builders;
using System.Linq;
using BetterConsoles.Core;
using BetterConsoles.Colors.Extensions;
using BetterConsoles.Tables.Builders.Interfaces;
using BetterConsoles.Colors.Builders;
using BetterConsoles.Colors;
using BetterConsoles.Tables.Common;

static void RunProgram() {
    Console.WriteLine("Welcome to StudyQuest!");
    //initialise variables
    
    bool isFinished = false;
    int time = 0;
    int questCount = 0;
    int overdueCount = 0;
    string[,] quests = new string[100,4]; //max no. of quests is 100 due to c# limitations no infinitely expanding arrays (when declared like this)
    while (!isFinished) {
        Console.Clear();
        if (time != 0) {overdueCount = UpdateQuests(ref quests, time, questCount);} //update quests
        DisplayMenu(time, overdueCount);
        //input handling
        Console.ForegroundColor = ConsoleColor.Yellow;
        int input = GetIntInput();
        Console.WriteLine("\n");
        Console.ForegroundColor = ConsoleColor.White;
        //switch case for menu options
        switch (input) {
            case 1:
                if (quests.GetLength(0) <= 100) { //ensure max no. of quests is not exceeded due to earlier limitation
                    AddQuest(ref quests, ref questCount);
                } else {
                    Console.WriteLine("You have reached the maximum number of quests.");
                    Console.WriteLine("\n");
                }
                break;
            case 2:
                ViewQuests(quests, questCount);
                break;
            case 3:
                MarkQuestComplete(ref quests, questCount);
                break;
            case 4:
                time++;
                break;
            case 5:
                isFinished = true;
                break;
            default:
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
                Console.WriteLine("\n");
                break;
        }
    }
}

static void DisplayMenu(int time, int overdueCount) {
    //menu
    Console.Write("Time: ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(time);
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Overdue quests: ");
    if (overdueCount == 0) {
        Console.ForegroundColor = ConsoleColor.Blue;
    } else {
        Console.ForegroundColor = ConsoleColor.Red;
    }
    Console.WriteLine(overdueCount);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("");
    Console.WriteLine("Please choose an option:");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("1. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Add new Quest");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("2. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("View all Quests");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("3. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Mark Quest as complete");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("4. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Increment time");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("5. Exit");
    Console.WriteLine("\n");
    Console.ForegroundColor = ConsoleColor.White;
}

static int GetIntInput() {
    //input validation (type check) - used in multiple places so range check cannot be implemented 
    int inputnum = 0;
    bool isValid = false;
    while (!isValid) {
        string inputString = Console.ReadLine();
        isValid = int.TryParse(inputString, out inputnum);
        if (!isValid) {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
    return inputnum;
}

//add quest to array using ref to modify the original array
static void AddQuest(ref string[,] quests, ref int questCount){
    //input for quest details
    
    
    Console.WriteLine("Enter quest name:");
    string questName = Console.ReadLine();
    Console.WriteLine("Enter quest subject:");
    string questSubject = Console.ReadLine();
    Console.WriteLine("Enter quest due time:");
    int questDueTime = GetIntInput();
    // add quest to 2D array
    quests[questCount, 0] = questName;
    quests[questCount, 1] = questSubject;
    quests[questCount, 2] = questDueTime.ToString(); //convert int to string for array
    quests[questCount, 3] = "Incomplete"; //quest by default is incomplete
    questCount++; //modify quest count using ref to modify original 
    Console.WriteLine("Quest added successfully!");
    Console.WriteLine("\n");
}   

static void ViewQuests(string[,] quests, int questCount) {
    Table table = new TableBuilder()
        .AddColumn("#")
            .RowsFormat()
                .ForegroundColor(Color.SteelBlue)
        .AddColumn("Name")
        .AddColumn("Subject")
        .AddColumn("Due Time")
        .AddColumn("Status")
        .Build();
    for (int i = 0; i < questCount; i++) {
        table.AddRow(i, quests[i, 0], quests[i, 1], quests[i, 2], quests[i, 3]); //add rows to table from array
    }
    table.Config = TableConfig.UnicodeAlt();
    Console.Write(table);
    Console.WriteLine("\n");
    Console.ReadKey();
}

static int UpdateQuests(ref string[,] quests, int time, int questCount) {
    int overdueCount = 0;
    //mark quests as overdue if due time is equal to current time and quest is incomplete
    for (int i = 0; i < questCount; i++) {
        if (time == int.Parse(quests[i, 2]) && quests[i, 3] == "Incomplete") {
            quests[i, 3] = "Overdue";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(quests[i, 0]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" is overdue! \n");
            overdueCount++;
        }
    }
    return overdueCount;
}

//allow user to modify quests as complete (also stops quests from being marked as overdue)
static void MarkQuestComplete(ref string[,] quests, int questCount) {
    ViewQuests(quests, questCount); //displays quests for convienience
    Console.WriteLine("Enter the number of the quest you want to mark as complete:");
    int questNum = GetIntInput();
    quests[questNum, 3] = "Complete";
    Console.WriteLine("Quest marked as complete!");
    Console.WriteLine("\n");
}
    
//no Main() function in new console template so RunProgram() replaces it, putting a Main() function here with RunProgram() call would still work to run program
RunProgram();