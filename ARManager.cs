







using System;
using System.Collections.Generic;
using System.Linq;

// Main class to run the application
public class ARManager
{
    // A simple class to represent an Invoice
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid => Amount <= AmountPaid;

        public decimal OutstandingBalance => Amount - AmountPaid;
    }

    // A simple class to represent a Payment
    public class Payment
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    // Static lists to store our data in memory
    private static List<Invoice> invoices = new List<Invoice>();
    private static int nextInvoiceId = 1;

    // The main entry point of the program
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Simple AR Manager!");
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. Create New Invoice");
            Console.WriteLine("2. Record a Payment");
            Console.WriteLine("3. View Outstanding Invoices");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateInvoice();
                    break;
                case "2":
                    RecordPayment();
                    break;
                case "3":
                    ViewOutstandingInvoices();
                    break;
                case "4":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        Console.WriteLine("Exiting program. Goodbye!");
    }

    // Creates a new invoice and adds it to the list
    private static void CreateInvoice()
    {
        Console.Write("Enter invoice amount: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            var newInvoice = new Invoice
            {
                InvoiceId = nextInvoiceId++,
                Amount = amount,
                AmountPaid = 0,
                DueDate = DateTime.Now.AddDays(30) // Due in 30 days
            };
            invoices.Add(newInvoice);
            Console.WriteLine($"Invoice {newInvoice.InvoiceId} created for ${newInvoice.Amount:F2}. Due on {newInvoice.DueDate.ToShortDateString()}.");
        }
        else
        {
            Console.WriteLine("Invalid amount. Please enter a number.");
        }
    }

    // Records a payment against an existing invoice
    private static void RecordPayment()
    {
        Console.Write("Enter Invoice ID to pay: ");
        if (int.TryParse(Console.ReadLine(), out int invoiceId))
        {
            var invoiceToPay = invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
            if (invoiceToPay != null)
            {
                Console.Write("Enter payment amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal paymentAmount))
                {
                    invoiceToPay.AmountPaid += paymentAmount;
                    Console.WriteLine($"Payment of ${paymentAmount:F2} recorded for Invoice {invoiceId}.");
                    Console.WriteLine($"New outstanding balance is ${invoiceToPay.OutstandingBalance:F2}.");

                    if (invoiceToPay.IsPaid)
                    {
                        Console.WriteLine("Invoice is now fully paid.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid payment amount.");
                }
            }
            else
            {
                Console.WriteLine("Invoice not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Invoice ID.");
        }
    }

    // Displays all invoices with an outstanding balance
    private static void ViewOutstandingInvoices()
    {
        var outstanding = invoices.Where(i => !i.IsPaid).ToList();
        if (outstanding.Any())
        {
            Console.WriteLine("--- Outstanding Invoices ---");
            foreach (var inv in outstanding)
            {
                Console.WriteLine($"Invoice ID: {inv.InvoiceId}, Amount Due: ${inv.OutstandingBalance:F2}, Due Date: {inv.DueDate.ToShortDateString()}");
            }
        }
        else
        {
            Console.WriteLine("No outstanding invoices found.");
        }
    }
}
