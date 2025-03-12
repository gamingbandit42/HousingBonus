using System;
using System.Collections.Generic;
using System.Linq;
// Dear maintainer: 
// Once you are done trying to 'optimize' this program,
// and have realized what a terrible mistake that was, 
// please increment the following counter as a warning 
// to the next guy: 
//
int total_hours_wasted_here = 500;
// 

namespace GarnersHouses
{
    class Employee
    {
        // creates arrays and shit
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public int PropertiesSold { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }
        public Employee(string name, int employeeId, int propertiesSold)
        {
            Name = name;
            EmployeeId = employeeId;
            PropertiesSold = propertiesSold;
            Commission = propertiesSold * 500m; // £500 per property sold
            Bonus = 0m; // Initially set to 0; we will calculate the bonus later
        }
    }
    class PropertyAgency
    {
        private List<Employee> employees = new List<Employee>();
        private decimal totalCommission = 0;
        private decimal totalBonus = 0;
        private int totalPropertiesSold = 0;
        // Method to input employee details
        public void InputEmployeeData()
        {
            int numEmployees;
            Console.WriteLine("Enter the number of employees: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out numEmployees) && numEmployees > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive number.");
                }
            }
            for (int i = 0; i < numEmployees; i++)
            {
                Console.WriteLine($"Enter details for employee {i + 1}:");
                // Validate employee name to ensure it's letters and spaces only
                string? name;
                while (true)
                {
                    Console.Write("Enter employee name: ");
                    name = Console.ReadLine();
                    if (!IsValidName(name: name))
                    {
                        Console.WriteLine("Invalid name. Please enter a name that contains only letters and spaces.");
                    }
                    else
                    {
                        break;
                    }
                }
                // Validate employee ID (must be unique and valid)
                int employeeId;
                while (true)
                {
                    Console.Write("Enter employee ID: ");
                    if (int.TryParse(Console.ReadLine(), out employeeId) && !EmployeeIdExists(employeeId))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("This ID is already taken or invalid. Please enter a unique valid ID.");
                    }
                }
                // Validate number of properties sold (must be a non-negative integer)
                int propertiesSold;
                while (true)
                {
                    Console.Write("Enter number of properties sold: ");
                    if (int.TryParse(Console.ReadLine(), out propertiesSold) && propertiesSold >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a non-negative number for properties sold. If you have entered a large number, it may be over the 32 bit signed integer limit");
                    }
                }
                // Create and add the employee to the list
                Employee employee = new Employee(name, employeeId, propertiesSold);
                employees.Add(employee);
                totalCommission += employee.Commission;
                totalPropertiesSold += propertiesSold;
            }
        }
        // Method to check if employee ID already exists
        private bool EmployeeIdExists(int employeeId)
        {
            return employees.Any(e => e.EmployeeId == employeeId);
        }
        // Method to sort employees by properties sold (highest to lowest)
        public void SortEmployeesByPropertiesSold()
        {
            employees = employees.OrderByDescending(e => e.PropertiesSold).ToList();
        }
        // Method to calculate bonus for top-selling employee(s)
        public void CalculateBonus()
        {
            if (employees.Count > 0)
            {
                // Find the maximum number of properties sold
                int maxPropertiesSold = employees.Max(e => e.PropertiesSold);

                // Find the employee(s) who sold the maximum number of properties
                var topSellers = employees.Where(e => e.PropertiesSold == maxPropertiesSold).ToList();

                // Only award the bonus to the top seller(s)
                foreach (var topSeller in topSellers)
                {
                    decimal bonus = topSeller.Commission * 0.15m; // 15% bonus for the employee(s) with the most sales
                    topSeller.Bonus = bonus;  // Assign the calculated bonus to the employee
                    totalBonus += bonus; // Add bonus to total bonus for reporting
                    Console.WriteLine($"\nBonus for {topSeller.Name}: £{bonus:F2}");
                }
            }
        }
        // Method to display results
        public void DisplayResults()
        {
            Console.WriteLine("\nEmployee Ranking (Top Seller First):");
            foreach (var employee in employees)
            {
                // Displaying employee's name, properties sold, commission, and bonus together
                Console.WriteLine($"{employee.Name} (ID: {employee.EmployeeId}) - Properties Sold: {employee.PropertiesSold} - Commission: £{employee.Commission:F2} - Bonus: £{employee.Bonus:F2}");
            }
            Console.WriteLine($"\nTotal Commission Paid: £{totalCommission:F2}");
            Console.WriteLine($"Total Bonus Paid: £{totalBonus:F2}");
            Console.WriteLine($"Total Number of Properties Sold: {totalPropertiesSold}");
            // Display the employee(s) who sold the most properties
            var topSellers = employees.Where(e => e.PropertiesSold == employees.Max(emp => emp.PropertiesSold)).ToList();
            Console.WriteLine("\nTop Seller(s):");
            foreach (var topSeller in topSellers)
            {
                Console.WriteLine($"{topSeller.Name} - Properties Sold: {topSeller.PropertiesSold}");
            }
        }
        // Helper method to validate that the name contains only letters and spaces (no regex)
        private bool IsValidName(string name)
        {
            // Check if the name is empty
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            // Check each character in the name to see if it's a letter or space
            foreach (var character in name)
            {
                if (!char.IsLetter(character) && character != ' ')
                {
                    return false;
                }
            }
            return true;
        }
    }

    class Program
    {
        public static void Main(string[] args) // Main Method // Truly a Poundland Java 
        {
            bool continueProgram = true; // loop

            while (continueProgram)
            {
                try
                {
                    PropertyAgency agency = new PropertyAgency();
                    agency.InputEmployeeData();
                    agency.SortEmployeesByPropertiesSold();
                    agency.CalculateBonus(); // Now calculating bonus before displaying results
                    agency.DisplayResults();
                }
                catch (Exception)
                {
                    Console.WriteLine("An error has occurred, Please follow the instructions given to you and try again. If the error still occurs, Please inform the maintainer");
                }
                finally
                {
                    Console.WriteLine("#################################################################################");
                    Console.WriteLine("Do you wish to continue? Y/N [DEFAULT = N]");
                    string? exit = Console.ReadLine();
                    exit = exit?.ToUpper();

                    if (exit == "Y" || exit == "YE" || exit == "YES")
                    {
                        Console.WriteLine("Continuing...");
                        Console.WriteLine("#################################################################################");
                        continueProgram = true; // Continue the loop
                    }
                    else if (exit == "EASTER") // funny but exits
                    {
                        int i = 0;
                        while (i < 20)
                        {
                            Console.WriteLine("lolololololololololololololololololololol");
                            Console.WriteLine();
                            i++;
                        }
                        Console.Beep(900, 2048);
                        Console.WriteLine("You've found the easter egg, a beep");
                        Console.WriteLine("Exiting Program");
                        continueProgram = false; // Exit the loop
                    }
                    else
                    {
                        Console.WriteLine("Exiting Program");
                        continueProgram = false; // Exit the loop 
                        // somedev 11/01/2020: Temporary Fix to allow loop exit
                        // someotherdev 12/03/2025: Temporary my ass
                    }
                }
            }
        }
    }
}
