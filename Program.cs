using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waterbill 
{
    internal class Program
    {
        static void ShowMenu()
        {
            Console.WriteLine("___Menu___");
            Console.WriteLine("1. Household customer");
            Console.WriteLine("2. Administrative agency, public services");
            Console.WriteLine("3. Production units");
            Console.WriteLine("4. Business services");
            Console.WriteLine("5. Exit");
        }

        static string CustomerChoice()
        {
            Console.WriteLine("\nEnter your choice:");
            string choice = Console.ReadLine();

            while (choice != "1" && choice != "2" && choice != "3" && choice != "4" && choice != "5")
            {
                Console.WriteLine("Error! Please re-enter: ");
                choice = Console.ReadLine();
            }

            return choice;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
                string choice = CustomerChoice();

                if (choice == "5")
                {
                    break;
                }

                double waterUsage = GetWaterUsage();
                CalculateBill(choice, waterUsage);
            }
        }
        static double GetWaterUsage()
        {
            Console.Write("\nName customer: ");
            string name = Console.ReadLine();

            double lastMonth, thisMonth;

            Console.Write("\nWater meter reading last month: ");
            while (!double.TryParse(Console.ReadLine(), out lastMonth))
            {
                Console.WriteLine("Error. Please re-enter.");
                Console.Write("\nWater meter reading last month: ");
            }

            Console.Write("\nWater meter reading this month: ");
            while (!double.TryParse(Console.ReadLine(), out thisMonth) || lastMonth > thisMonth)
            {
                Console.WriteLine("Error. Please re-enter.");
                Console.Write("\nWater meter reading this month: ");
            }

            double waterUsage = thisMonth - lastMonth;
            Console.WriteLine($"\nWater used (m3): {waterUsage} m3");

            return waterUsage;
        }
        static void CalculateBill(string choice, double waterUsage)
        {
            switch (choice)
            {
                case "1":
                    CalculateHouseholdBill(waterUsage);
                    break;
                case "2":
                    CalculateBillForType(waterUsage, 9.955);
                    break;
                case "3":
                    CalculateBillForType(waterUsage, 11.615);
                    break;
                case "4":
                    CalculateBusinessBill(waterUsage);
                    break;
            }
        }
        static void CalculateHouseholdBill(double waterUsage)
        {
            Console.Write("How many people in your family: ");
            int numPeople;
            while (!int.TryParse(Console.ReadLine(), out numPeople))
            {
                Console.WriteLine("Error. Please re-enter.");
                Console.Write("How many people in the household: ");
            }

            double waterPerPerson = waterUsage / numPeople;
            double totalCost = 0;
            double remainingWater = waterPerPerson;
            double[] waterRates = { 5.973, 7.052, 8.699, 15.929 };

            for (int i = 0; i < 3 && remainingWater > 0; i++)
            {
                double currentWaterUsage = Math.Min(10, remainingWater);
                totalCost += currentWaterUsage * waterRates[i];
                remainingWater -= currentWaterUsage;
            }

            if (remainingWater > 0)
            {
                totalCost += remainingWater * waterRates[3];
            }

            double totalForAll = totalCost * numPeople;
            double environmentalTax = totalForAll / 10;
            double vatTax = (totalForAll + environmentalTax) / 10;
            double finalTotal = totalForAll + environmentalTax + vatTax;

            Console.WriteLine($"Cost per person: {totalCost.ToString("0.000")} VND");
            Console.WriteLine($"Total cost for {numPeople} people: {totalForAll.ToString("#,#.000")} VND");
            Console.WriteLine($"VAT: {vatTax.ToString("0.000")} VND");
            Console.WriteLine($"Environmental tax: {environmentalTax.ToString("0.000")} VND");
            Console.WriteLine($"Total after tax: {finalTotal.ToString("#,#.000")} VND");
        }

        static void CalculateBillForType(double waterUsage, double rate)
        {
            double total = waterUsage * rate;
            double vatTax = total / 10;
            double environmentalTax = total / 10;
            double finalTotal = total + vatTax + environmentalTax;

            Console.WriteLine($"Cost before tax: {total.ToString("0.000")} VND");
            Console.WriteLine($"VAT: {vatTax.ToString("0.000")} VND");
            Console.WriteLine($"Environmental tax: {environmentalTax.ToString("0.000")} VND");
            Console.WriteLine($"Total after tax: {finalTotal.ToString("#,#.000")} VND");
        }

        static void CalculateBusinessBill(double waterUsage)
        {
            double totalBeforeTax = waterUsage * 22.068;
            double totalAfterVAT = totalBeforeTax + (totalBeforeTax / 10);
            double finalTotal = totalAfterVAT + (totalAfterVAT / 10);

            Console.WriteLine($"Cost before tax: {totalBeforeTax.ToString("#,#.000")} VND");
            Console.WriteLine($"Cost after VAT: {totalAfterVAT.ToString("#,#.000")} VND");
            Console.WriteLine($"Total after environmental tax: {finalTotal.ToString("#,#.000")} VND");
        
        }
    }
}
