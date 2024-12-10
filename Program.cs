using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Abstract superclass
abstract class Applicant
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Barangay { get; set; }


    public Applicant(string name, int age, string address, string contactNumber, string barangay)
    {
        Name = name;
        Age = age;
        Address = address;
        ContactNumber = contactNumber;
        Barangay = barangay;
    }

    // Abstract method for applying to the system
    public abstract void Apply();
}

// SeniorCitizen class inheriting from Applicant
class SeniorCitizen : Applicant
{
    public double AllowanceAmount { get; set; }
    public DateTime ApplicationTime { get; set; }
    public bool Received { get; set; }
    public DateTime distributionDateTime { get; set; }


    public SeniorCitizen(string name, int age, string address, string contactNumber, string barangay)
        : base(name, age, address, contactNumber, barangay)
    {

    }

    public override void Apply()
    {
        // Senior Citizen specific application logic can be added here
        Console.WriteLine("Applying as a senior citizen.");
        // Additional logic for senior citizen application
    }
}

// Student class inheriting from Applicant
class Student : Applicant
{
    public string SchoolName { get; set; }
    public string Course { get; set; }
    public double AllowanceAmount { get; set; }
    public bool Received { get; set; }
    public DateTime ApplicationTime { get; set; }
    public DateTime distributionDateTime { get; set; }

    public Student(string name, int age, string address, string contactNumber, string schoolName, string course, string barangay)
        : base(name, age, address, contactNumber, barangay)
    {
        SchoolName = schoolName;
        Course = course;
    }

    public override void Apply()
    {
        // Student specific application logic can be added here
        Console.WriteLine("Applying as a student.");
        // Additional logic for student application
    }
}

// Inheriting from the superclass and implementing file handling interface
class FINHELPHUBSYSTEM : Applicant, IFileHandler
{
    public string SchoolName { get; set; }
    public string Course { get; set; }
    public DateTime ApplicationTime { get; set; }
    public DateTime distributionDateTime { get; set; }
    public double AllowanceAmount { get; set; }
    public int PriorityNumber { get; set; }
    public string Barangay { get; set; }
    public bool ReceivedAllowance { get; set; } = false;


    public static List<FINHELPHUBSYSTEM> Applications = new List<FINHELPHUBSYSTEM>();

    public FINHELPHUBSYSTEM(string name, int age, string address, string contactNumber)
    : base(name, age, address, contactNumber, "DefaultBarangay")
    {
        // Additional initialization specific to FINHELPHUB SYSTEM, if needed
        // You can set any default value for Barangay here if needed
    }


    public FINHELPHUBSYSTEM(string name, int age, string address, string contactNumber, string barangay)
        : base(name, age, address, contactNumber, barangay)
    {
        Barangay = barangay;
    }

    public static void AddApplicant(FINHELPHUBSYSTEM applicant)
    {
        Applications.Add(applicant);
    }
           public override void Apply()
    {
        Console.WriteLine("Applying for the system:");

        // Common information for both student and senior citizen
        Console.Write("Enter name: ");
        Name = Console.ReadLine();
        Console.Write("Enter age: ");
        try
        {
            Age = Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException)
        {

            Console.WriteLine("Invalid format: Please enter a valid number for age.");
            return;
        }
        Console.Write("Enter address: ");
        Address = Console.ReadLine();
        Console.Write("Enter Barangay: ");
        Barangay = Console.ReadLine();
        Console.Write("Enter contact number: ");
        ContactNumber = Console.ReadLine();

        // Capture the application date
        ApplicationTime = DateTime.Now;

        if (Age >= 60)
        {
            // Senior Citizen
            Console.WriteLine("You are applying as a Senior Citizen.");
            AllowanceAmount = 6000; // Fixed allowance amount for senior citizens
            PriorityNumber = FINHELPHUBSYSTEM.Applications.Count + 1;

            // Create a new instance to add to the list
            FINHELPHUBSYSTEM seniorApplicant = new FINHELPHUBSYSTEM(Name, Age, Address, ContactNumber, Barangay)
            {
                AllowanceAmount = AllowanceAmount,
                PriorityNumber = PriorityNumber,
                ApplicationTime = ApplicationTime
            };

            // Add to the static Applications list
            FINHELPHUBSYSTEM.AddApplicant(seniorApplicant);
        }
        else if (Age >= 18 && Age <= 25)
        {
            // Student
            Console.WriteLine("You are applying as a Student.");
            Console.Write("Enter school: ");
            SchoolName = Console.ReadLine();
            Console.Write("Enter course: ");
            Course = Console.ReadLine();
            AllowanceAmount = 1000; // Fixed allowance amount for students
            PriorityNumber = FINHELPHUBSYSTEM.Applications.Count + 1;

            // Create a new instance to add to the list
            FINHELPHUBSYSTEM studentApplicant = new FINHELPHUBSYSTEM(Name, Age, Address, ContactNumber, Barangay)
            {
                SchoolName = SchoolName,
                Course = Course,
                AllowanceAmount = AllowanceAmount,
                PriorityNumber = PriorityNumber,
                ApplicationTime = ApplicationTime
            };

            // Add to the static Applications list
            FINHELPHUBSYSTEM.AddApplicant(studentApplicant);
        }
        else
        {
            Console.WriteLine("Invalid age. Only seniors (60 and above), students (18-25).");
            return;
        }

        // Save to file after successful application
        SaveApplicantToFile();

        // Confirm application
        Console.WriteLine($"Application successful for {Name}!");
    }

    private void SaveApplicantToFile()
    {
        string filePath = "applicant_information.csv";

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Ensure all required fields are written
                writer.WriteLine($"{Name},{Age},{Address},{ContactNumber},{Barangay},{SchoolName ?? "N/A"},{Course ?? "N/A"},{ApplicationTime},{AllowanceAmount},{PriorityNumber},false");
            }
            Console.WriteLine("Applicant information written to file successfully.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while writing to the file: {e.Message}");
        }
    }

    public void ReviewApplications()
    {
        Console.WriteLine("Reviewing applications");
        foreach (var applicant in Applications)
        {
            Console.WriteLine("Name: " + applicant.Name);
            Console.WriteLine("Age: " + applicant.Age);
            Console.WriteLine("Address: " + applicant.Address);
            Console.WriteLine("Barangay: " + applicant.Barangay);
            Console.WriteLine("Contact Number: " + applicant.ContactNumber);

            // Update the review logic based on the new requirements
            if (applicant.Age >= 60)
            {
                Console.WriteLine("Approved for senior: " + applicant.Name);
            }
            else if (applicant.Age >= 18 && applicant.Age <= 25)
            {
                Console.WriteLine("Approved for student: " + applicant.Name);
            }
            else
            {
                Console.WriteLine("Denied: " + applicant.Name);
            }

            Console.WriteLine("---------------------------------------");
        }
    }

    public static void UpdateDetails()
    {
        Console.Write("Enter the name of the applicant to edit: ");
        string name = Console.ReadLine();
        Console.WriteLine();

        FINHELPHUBSYSTEM applicant = Applications.Find(a => a.Name == name);

        if (applicant != null)
        {
            Console.Write("Enter new Name: ");
            string newName = Console.ReadLine();
            applicant.Name = newName;

            Console.Write("Enter new Age: ");
            int newAge;
            if (int.TryParse(Console.ReadLine(), out newAge))
            {
                applicant.Age = newAge;
            }
            else
            {
                Console.WriteLine("Invalid format: Please enter a valid number for age.");
                return;
            }

            Console.Write("Enter new Address: ");
            string newAddress = Console.ReadLine();
            applicant.Address = newAddress;

            Console.Write("Enter new Barangay: ");
            string newBarangay = Console.ReadLine();
            applicant.Barangay = newBarangay;

            Console.Write("Enter new Contact Number: ");
            string newContactNumber = Console.ReadLine();
            applicant.ContactNumber = newContactNumber;

            Console.WriteLine($"Applicant information updated successfully!");

            FINHELPHUBSYSTEM FINHELPHUBSYSTEM = new FINHELPHUBSYSTEM("", 0, "", "");
            FINHELPHUBSYSTEM.WriteFile();
        }
        else
        {
            Console.WriteLine("Applicant not found.");
        }
    }


    public static void DeleteApplicant()
    {
        Console.Write("Enter the name of the applicant to delete: ");
        string name = Console.ReadLine();

        FINHELPHUBSYSTEM applicant = Applications.Find(a => a.Name == name);

        if (applicant != null)
        {
            // Remove applicant from the list
            Applications.Remove(applicant);
            Console.WriteLine($"Applicant deleted successfully.");

            // Remove distribution data from the file
            RemoveDistributionDataFromFile("distribution_data.csv", applicant.Name);

            // Rewrite both applicant and distribution files
            FINHELPHUBSYSTEM FINHELPHUBSYSTEM = new FINHELPHUBSYSTEM("", 0, "", "");
            FINHELPHUBSYSTEM.WriteFile();
            FINHELPHUBSYSTEM.WriteDistributionFile();
        }
        else
        {
            Console.WriteLine("Applicant not found.");
        }
    }

    public void WriteDistributionFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("distribution_data.csv"))
            {
                foreach (var applicant in Applications)
                {
                    if (applicant.distributionDateTime != DateTime.MinValue)
                    {
                        writer.WriteLine($"{applicant.Name},{applicant.AllowanceAmount},{applicant.PriorityNumber},{applicant.distributionDateTime},{applicant.ReceivedAllowance}");
                       
                    }
                }
            }
           
            Console.WriteLine("Distribution file written successfully");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while writing the distribution file: {e.Message}");
        }
    }


    private static void RemoveDistributionDataFromFile(string filePath, string applicantName)
    {
        try
        {
            // Read all lines from the distribution file
            string[] lines = File.ReadAllLines(filePath);

            // Filter out lines with the specified applicant name
            var filteredLines = lines.Where(line => !line.StartsWith(applicantName, StringComparison.OrdinalIgnoreCase));

            // Write the filtered lines back to the distribution file
            File.WriteAllLines(filePath, filteredLines);

            Console.WriteLine("Distribution data removed from file successfully");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while removing distribution data from the file: {e.Message}");
        }
    }


    public static void SearchApplicantDynamically()
    {
        Console.Write("Enter the search query: ");
        string searchQuery = Console.ReadLine();

        try
        {
            List<FINHELPHUBSYSTEM> searchResults = Applications
                .Where(applicant => applicant.Name.StartsWith(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (searchResults.Count == 0)
            {
                Console.WriteLine($"No applicants found with the specified search query: '{searchQuery}'.");
            }
            else
            {
                Console.WriteLine($"{searchResults.Count} applicants found with names starting with '{searchQuery}'.");
                DisplayApplicantInformation(searchResults);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while searching for applicants: {e.Message}");
        }
    }


    private static void DisplayApplicantInformation(List<FINHELPHUBSYSTEM> applicants)
    {
        // Display the header
        Console.WriteLine("=============================================================================================================================================================");
        Console.WriteLine("| Name              | Age | School          | Course          | Address         | Contact Number  | Barangay         | Date            | Received Allowance |");
        Console.WriteLine("=============================================================================================================================================================");
        foreach (var applicant in applicants)
        {
            // Display each applicant's information in a tabular format
            Console.WriteLine($"| {applicant.Name,-17} | {applicant.Age,-3} | {applicant.SchoolName,-17} | {applicant.Course,-15} | {applicant.Address,-17} | {applicant.ContactNumber,-15} | {applicant.Barangay,-15} | {applicant.ApplicationTime,-15} | {applicant.ReceivedAllowance,-17} |");
        }

        // Display the footer
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=============================================================================================================================================================");
        Console.ResetColor();
    }

    public static void DisplayAllApplicants()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("All Applicants:");
        Console.WriteLine("=============================================================================================================================================================");
        Console.WriteLine("| Name              | Age | School          | Course          | Address         | Contact Number  | Barangay         | Date            | Received Allowance |");
        Console.WriteLine("=============================================================================================================================================================");
        Console.ResetColor();

        foreach (var applicant in FINHELPHUBSYSTEM.Applications)
        {
            // Display each applicant's information in a tabular format
            Console.WriteLine($"| {applicant.Name,-17} | {applicant.Age,-3} | {applicant.SchoolName,-17} | {applicant.Course,-15} | {applicant.Address,-17} | {applicant.ContactNumber,-15} | {applicant.Barangay,-15} | {applicant.ApplicationTime,-15} | {applicant.ReceivedAllowance,-17} |");
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=============================================================================================================================================================");
        Console.ResetColor();

    }


    public static void SearchByBarangay(string barangay)
    {
        Console.WriteLine($"Searching for applicants in Barangay '{barangay}':");
        List<FINHELPHUBSYSTEM> searchResults = Applications
            .Where(applicant => applicant.Barangay.Equals(barangay, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (searchResults.Count == 0)
        {
            Console.WriteLine($"No applicants found in Barangay '{barangay}'.");
        }
        else
        {
            Console.WriteLine($"{searchResults.Count} applicants found in Barangay '{barangay}'.");
            DisplayApplicantInformation(searchResults);
        }
    }



    public void ReadFile()
    {
        // Clear existing applications before reading from file
        Applications.Clear();

        try
        {
            if (!File.Exists("applicant_information.csv"))
            {
                Console.WriteLine("Applicant information file does not exist.");
                return;
            }

            string[] lines = File.ReadAllLines("applicant_information.csv");

            foreach (var line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length >= 10)
                {
                    try
                    {
                        FINHELPHUBSYSTEM applicant = new FINHELPHUBSYSTEM(
                            values[0],                      // Name
                            Convert.ToInt32(values[1]),     // Age
                            values[2],                      // Address
                            values[3],                      // Contact Number
                            values[4]                       // Barangay
                        )
                        {
                            SchoolName = values[5] != "N/A" ? values[5] : null,
                            Course = values[6] != "N/A" ? values[6] : null,
                            ApplicationTime = Convert.ToDateTime(values[7]),
                            AllowanceAmount = Convert.ToDouble(values[8]),
                            PriorityNumber = Convert.ToInt32(values[9]),
                            ReceivedAllowance = values.Length > 10 ? Convert.ToBoolean(values[10]) : false
                        };

                        // Add to the static Applications list
                        Applications.Add(applicant);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error converting data on line: {line}. {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"Successfully loaded {Applications.Count} applicants from file.");
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred while reading the file: " + e.Message);
        }
    }

    public void WriteFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("applicant_information.csv"))
            {
                foreach (var applicant in Applications)
                {
                    writer.WriteLine($"{applicant.Name},{applicant.Age},{applicant.Address},{applicant.ContactNumber},{applicant.Barangay},{applicant.SchoolName ?? "N/A"},{applicant.Course ?? "N/A"},{applicant.ApplicationTime},{applicant.AllowanceAmount},{applicant.PriorityNumber},{applicant.ReceivedAllowance}");
                }
            }
            Console.WriteLine("File written successfully");
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred while writing the file: " + e.Message);
        }
    }

    public void DistributeAllowance()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        if (Age >= 60)
        {
            // Senior Citizen
            AllowanceAmount = 1000; // Fixed allowance amount for senior citizens
            PriorityNumber = Applications.Count + 1;
        }
        else if (Age >= 18 && Age <= 25)
        {
            // Student
            AllowanceAmount = 2000; // Fixed allowance amount for students
            PriorityNumber = Applications.Count + 1;
        }
        else
        {
            Console.WriteLine("Invalid age. Only seniors (60 and above) and students (18-25) are eligible.");
            return;
        }
    }

    public static class BarangayAnalytics
    {
        public static void GenerateBarangayApplicantAnalytics()
        {
            // Group applicants by barangay
            var barangayGroups = FINHELPHUBSYSTEM.Applications
                .GroupBy(a => a.Barangay)
                .Select(g => new BarangayAnalyticsData
                {
                    Barangay = g.Key,
                    TotalApplicants = g.Count(),
                    StudentApplicants = g.Count(a => a.Age >= 18 && a.Age <= 25),
                    SeniorApplicants = g.Count(a => a.Age >= 60)
                })
                .ToList();

            DisplayBarangayStatistics(barangayGroups);
            DrawBarangayAnalyticsChart(barangayGroups);
        }

        private class BarangayAnalyticsData
        {
            public string Barangay { get; set; }
            public int TotalApplicants { get; set; }
            public int StudentApplicants { get; set; }
            public int SeniorApplicants { get; set; }
        }

        private static void DisplayBarangayStatistics(List<BarangayAnalyticsData> barangayGroups)
        {
            Console.WriteLine("\nBarangay Applicant Analytics:");
            Console.WriteLine("===================================================");
            Console.WriteLine("{0,-20} {1,10} {2,10} {3,10} {4,15} {5,15}",
                "Barangay", "Total", "Students", "Seniors", "% Student", "% Senior");

            foreach (var group in barangayGroups)
            {
                double studentPercentage = group.TotalApplicants > 0
                    ? (double)group.StudentApplicants / group.TotalApplicants * 100
                    : 0;

                double seniorPercentage = group.TotalApplicants > 0
                    ? (double)group.SeniorApplicants / group.TotalApplicants * 100
                    : 0;

                Console.WriteLine("{0,-20} {1,10} {2,10} {3,10} {4,15:F2}% {5,15:F2}%",
                    group.Barangay,
                    group.TotalApplicants,
                    group.StudentApplicants,
                    group.SeniorApplicants,
                    studentPercentage,
                    seniorPercentage);
            }
            Console.WriteLine("===================================================");
        }

        private static void DrawBarangayAnalyticsChart(List<BarangayAnalyticsData> barangayGroups)
        {
            const int chartWidth = 50;
            const char studentBar = '▓';
            const char seniorBar = '░';

            Console.WriteLine("\nBarangay Applicant Percentage Chart");
            Console.WriteLine("Barangay".PadRight(20) + "Students (White) | Seniors (Gray)");
            Console.WriteLine(new string('-', 70));

            foreach (var group in barangayGroups)
            {
                double totalApplicants = group.TotalApplicants;
                double studentPercentage = totalApplicants > 0
                    ? (double)group.StudentApplicants / totalApplicants * 100
                    : 0;
                double seniorPercentage = totalApplicants > 0
                    ? (double)group.SeniorApplicants / totalApplicants * 100
                    : 0;

                int studentBarLength = (int)(studentPercentage / 100 * chartWidth);
                int seniorBarLength = (int)(seniorPercentage / 100 * chartWidth);

                string chart = new string(studentBar, studentBarLength) +
                               new string(seniorBar, seniorBarLength);

                Console.WriteLine("{0,-20} {1,-50} St:{2,6:F2}% Sr:{3,6:F2}%",
                    group.Barangay,
                    chart,
                    studentPercentage,
                    seniorPercentage);
            }
        }

        public static void ShowBarangayAnalytics()
        {
            Console.Clear();
            Console.WriteLine("Generating Barangay Applicant Analytics...");

            try
            {
                GenerateBarangayApplicantAnalytics();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating analytics: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

}

interface IFileHandler
{
    void ReadFile();
    void WriteFile();
}

class Program
{
    public static List<FINHELPHUBSYSTEM> Applicants = new List<FINHELPHUBSYSTEM>();

    static void Main(string[] args)
    {
        FINHELPHUBSYSTEM FINHELPHUBSYSTEM = new FINHELPHUBSYSTEM("", 0, "", "");

        FINHELPHUBSYSTEM.ReadFile();


        // Method to display the welcome message on a separate screen
        void DisplayWelcomeMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===================================================");
            Console.WriteLine("     *** Welcome to the FINHELPHUB SYSTEM ***");
            Console.WriteLine("===================================================");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to proceed...");
            Console.ReadKey(); // Waits for user input before proceeding
            Console.Clear();   // Clears the screen before moving to the main menu
        }

        // Method to display the main menu on a separate screen
        void DisplayMainMenu()
        {
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----------------------------");
            Console.WriteLine("|  1. Applicant            |");
            Console.WriteLine("|  2. Administrator        |");
            Console.WriteLine("|  3. Distribution         |");
            Console.WriteLine("|  4. Exit                 |");
            Console.WriteLine("----------------------------");
            Console.ResetColor();
            Console.Write("Enter your choice: ");
        }

        bool exit = false;

        // Display the welcome message on a separate screen
        DisplayWelcomeMessage();


        while (!exit)
        {
            // Display main menu on a different screen
            DisplayMainMenu();

            try
            {
                int initialChoice = Convert.ToInt32(Console.ReadLine());

                switch (initialChoice)
                {
                    case 1:
                        Console.Clear();
                        FINHELPHUBSYSTEM newApplicant = new FINHELPHUBSYSTEM("", 0, "", "");
                        newApplicant.Apply();
                        //FINHELPHUBSYSTEM.Applications.Add(newApplicant);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        if (AdministratorLogin())
                        {
                            AdministratorOptions();
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        break;
                    case 3:
                        Console.Clear();
                        DistributionOptions();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        Console.WriteLine("Thank you for using FINHELPHUB SYSTEM.");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format: Please enter a valid number.");
            }
        }
    }

    static bool AdministratorLogin()
    {
        Console.Clear();
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = ReadPassword();

        // For demonstration purposes, the username is 'admin' and the password is 'pass'.
        // Can be replaced with a proper authentication mechanism.
        return (username == "admin" && password == "pass");
    }

    static void AdministratorOptions()
    {
        bool backToMainMenu = false;

        while (!backToMainMenu)
        {
            Console.Clear();
            Console.WriteLine("Administrator Menu");
            Console.WriteLine("1. Review applications");
            Console.WriteLine("2. Update information");
            Console.WriteLine("3. Delete information");
            Console.WriteLine("4. Search applicants");
            Console.WriteLine("5. Search Barangay");
            Console.WriteLine("6. Display all applicants");
            Console.WriteLine("7. Barangay Applicant Analytics");
            Console.WriteLine("8. Go back to the main menu");
            
            Console.Write("Enter your choice: ");

            try
            {
                int adminChoice = Convert.ToInt32(Console.ReadLine());

                switch (adminChoice)
                {
                    case 1:
                        Console.Clear();
                        foreach (var applicant in FINHELPHUBSYSTEM.Applications)
                        {
                            Console.WriteLine($"Name: {applicant.Name}");
                            Console.WriteLine($"Age: {applicant.Age}");
                            Console.WriteLine($"Address: {applicant.Address}");
                            Console.WriteLine($"Barangay: {applicant.Barangay}");
                            Console.WriteLine($"Contact Number: {applicant.ContactNumber}");

                            if (applicant.Age >= 60)
                            {
                                Console.WriteLine("Approved for senior: " + applicant.Name);
                            }
                            else if (applicant.Age >= 18 && applicant.Age <= 25)
                            {
                                Console.WriteLine("Approved for student: " + applicant.Name);
                            }
                            else
                            {
                                Console.WriteLine("Denied: " + applicant.Name);
                            }

                            Console.WriteLine("----------------------------------------------");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("----- Update details/information of the Applicant -----");
                        FINHELPHUBSYSTEM.UpdateDetails();
                        Console.WriteLine("----------------------------------------------");
                        break;
                    case 3:
                        Console.Clear();
                        FINHELPHUBSYSTEM.DeleteApplicant();
                        break;
                    case 4:
                        Console.Clear();
                        FINHELPHUBSYSTEM.SearchApplicantDynamically();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write("Enter the Barangay to search: ");
                        string searchBarangay = Console.ReadLine();
                        FINHELPHUBSYSTEM.SearchByBarangay(searchBarangay);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        Console.Clear();
                        FINHELPHUBSYSTEM.DisplayAllApplicants();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 7:
                        FINHELPHUBSYSTEM.BarangayAnalytics.ShowBarangayAnalytics();
                        break;
                    case 8:
                        backToMainMenu = true;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid format: Please enter a valid number.");
            }
        }
    }

    static void DistributionOptions()
    {
        Console.WriteLine("Are you a 'student', or 'senior' ? Enter the word: ");
        string userType = Console.ReadLine().ToLower();

        if (userType == "student" || userType == "senior")
        {
            Console.Write("Enter your name: ");
            string applicantName = Console.ReadLine();

            // Find the applicant based only on name
            FINHELPHUBSYSTEM applicant = FINHELPHUBSYSTEM.Applications.Find(a =>
                a.Name.Equals(applicantName, StringComparison.OrdinalIgnoreCase));

            if (applicant != null)
            {
                // Verify eligibility based on user type
                bool isEligible = (userType == "student" && applicant.Age >= 18 && applicant.Age <= 25) ||
                                  (userType == "senior" && applicant.Age >= 60);

                if (isEligible)
                {
                    // The system shows the automatically assigned priority number
                    Console.WriteLine($"Your priority number is: {applicant.PriorityNumber}");

                    // Mark as received
                    applicant.ReceivedAllowance = true;
                    applicant.distributionDateTime = DateTime.Now;

                    // Ensure distribution_data.csv directory exists
                    string distributionFilePath = "distribution_data.csv";

                    // Write to distribution file
                    using (StreamWriter writer = new StreamWriter(distributionFilePath, true))
                    {
                        writer.WriteLine($"{applicant.Name},{applicant.AllowanceAmount},{applicant.PriorityNumber},{applicant.distributionDateTime},{applicant.ReceivedAllowance}");
                    }

                    Console.WriteLine($"Allowance of {applicant.AllowanceAmount} has been received.");
                    Console.WriteLine($"Distribution Date: {applicant.distributionDateTime}");
                    Console.WriteLine($"Claim your allowance at the Finance Office");

                    // Update the main applicant information file
                    FINHELPHUBSYSTEM finHelp = new FINHELPHUBSYSTEM("", 0, "", "");
                    finHelp.WriteFile();
                }
            }
            else
            {
                Console.WriteLine("Applicant not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid user type.");
        }
    }


    static void DistributeStudentAllowance(string applicantName)
    {
        FINHELPHUBSYSTEM studentApplicant = FINHELPHUBSYSTEM.Applications.Find(applicant =>
            applicant.Name == applicantName && applicant.Age >= 18 && applicant.Age <= 25);

        if (studentApplicant != null)
        {
            Console.WriteLine($"Allowance: {studentApplicant.AllowanceAmount}");
            Console.WriteLine($"Priority number: {studentApplicant.PriorityNumber}");

            // Capture the distribution date and time separately
            DateTime distributionDateTime = DateTime.Now.AddDays(31); // Example: Distribution after 31 days

            // Prompt for Received Allowance
            Console.Write("Has the allowance been received? (yes/no): ");
            string receivedResponse = Console.ReadLine().ToLower();
            bool receivedAllowance = receivedResponse == "yes" || receivedResponse == "y";

            // Call the method to store distribution data in a file
            DistributeDataToFile("distribution_data.csv",
                $"{studentApplicant.Name},{studentApplicant.AllowanceAmount},{studentApplicant.PriorityNumber},{distributionDateTime},{receivedAllowance}");

            // Update the applicant's ReceivedAllowance status
            studentApplicant.ReceivedAllowance = receivedAllowance;

            FINHELPHUBSYSTEM financialAidSystem = new FINHELPHUBSYSTEM("", 0, "", "");
            financialAidSystem.WriteFile();
        }
        else
        {
            Console.WriteLine("Student not found or not eligible.");
        }
    }

    static void DistributeSeniorAllowance(string applicantName)
    {
        FINHELPHUBSYSTEM seniorApplicant = FINHELPHUBSYSTEM.Applications.Find(applicant =>
            applicant.Name == applicantName && applicant.Age >= 60);

        if (seniorApplicant != null)
        {
            Console.WriteLine($"Allowance: {seniorApplicant.AllowanceAmount}");
            Console.WriteLine($"Priority number: {seniorApplicant.PriorityNumber}");

            // Capture the distribution date and time separately
            DateTime distributionDateTime = DateTime.Now.AddDays(31); // Example: Distribution after 31 days

            // Prompt for Received Allowance
            Console.Write("Has the allowance been received? (yes/no): ");
            string receivedResponse = Console.ReadLine().ToLower();
            bool receivedAllowance = receivedResponse == "yes" || receivedResponse == "y";

            // Call the method to store distribution data in a file
            DistributeDataToFile("distribution_data.csv",
                $"{seniorApplicant.Name},{seniorApplicant.AllowanceAmount},{seniorApplicant.PriorityNumber},{distributionDateTime},{receivedAllowance}");

            // Update the applicant's ReceivedAllowance status
            seniorApplicant.ReceivedAllowance = receivedAllowance;

            FINHELPHUBSYSTEM financialAidSystem = new FINHELPHUBSYSTEM("", 0, "", "");
            financialAidSystem.WriteFile();
        }
        else
        {
            Console.WriteLine("Senior not found or not eligible.");
        }
    }


    static void DistributeDataToFile(string filePath, string data)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }

            Console.WriteLine("Distribution data written to file successfully");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while writing the distribution file: {e.Message}");
        }
    }


    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo info = Console.ReadKey(true);
        while (info.Key != ConsoleKey.Enter)
        {
            if (info.Key != ConsoleKey.Backspace)
            {
                Console.Write("*");
                password += info.KeyChar;
            }
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (!string.IsNullOrEmpty(password))
                {
                    // remove one character from the list of password characters
                    password = password.Substring(0, password.Length - 1);
                    // get the location of the cursor
                    int pos = Console.CursorLeft;
                    // move the cursor to the left by one character
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    // replace it with space
                    Console.Write(" ");
                    // move the cursor to the left by one character again
                    Console.SetCursorPosition(pos - 1, Console.CursorTop);
                }
            }
            info = Console.ReadKey(true);
        }
        Console.WriteLine();
        return password;
    }
}


