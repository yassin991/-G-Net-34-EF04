using AssignmentEF04.Models;
using System;

namespace AssignmentEF04
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            using var context = new ApplicationDBcontext();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== BANK SYSTEM ====");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Open Account");
                Console.WriteLine("3. Update Account Status");
                Console.WriteLine("4. Remove Account from Customer");
                Console.WriteLine("5. List Customers");
                Console.WriteLine("0. Exit");

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddCustomer(context); break;
                    case "2": OpenAccount(context); break;
                    case "3": UpdateStatus(context); break;
                    case "4": RemoveAccount(context); break;
                    case "5": ListCustomers(context); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid!"); break;
                }

                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
        }


        static void AddCustomer(ApplicationDBcontext context)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("National ID: ");
            var nid = Console.ReadLine();

            Console.Write("DOB: ");
            DateTime dob = DateTime.Parse(Console.ReadLine());

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Phone: ");
            var phone = Console.ReadLine();

            Console.Write("Address: ");
            var address = Console.ReadLine();

            Console.Write("Type (Individual/Business): ");
            var type = Console.ReadLine();

            var customer = new Customer
            {
                FullName = name,
                NationalId = nid,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = type
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine("Customer Added!");
        }


        static void OpenAccount(ApplicationDBcontext context)
        {
            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            Console.Write("Account Type: ");
            var type = Console.ReadLine();

            Console.Write("Branch Code: ");
            var branchCode = Console.ReadLine();

            var branch = context.Branches.Find(branchCode);
            if (branch == null)
            {
                Console.WriteLine("Branch not found!");
                return;
            }

            Console.Write("Customer Id: ");
            int custId = int.Parse(Console.ReadLine());

            var customer = context.Customers.Find(custId);
            if (customer == null)
            {
                Console.WriteLine("Customer not found!");
                return;
            }

            var account = new Account
            {
                AccountNumber = accNum,
                AccountType = type,
                OpeningDate = DateTime.Now,
                CurrentBalance = 0,
                BranchCode = branchCode
            };

            context.Accounts.Add(account);

            var link = new CustomerAccount
            {
                CustomerId = custId,
                AccountNumber = accNum,
                OwnershipStartDate = DateTime.Now,
                OwnershipType = "Primary",
                AccountStatus = "Active"
            };

            context.CustomerAccounts.Add(link);

            context.SaveChanges();

            Console.WriteLine("Account Created!");
        }


        static void UpdateStatus(ApplicationDBcontext context)
        {
            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            Console.Write("Customer Id: ");
            int custId = int.Parse(Console.ReadLine());

            var link = context.CustomerAccounts
                .FirstOrDefault(x => x.AccountNumber == accNum && x.CustomerId == custId);

            if (link == null)
            {
                Console.WriteLine("Not found!");
                return;
            }

            link.AccountStatus = link.AccountStatus == "Active" ? "Closed" : "Active";

            context.SaveChanges();

            Console.WriteLine("Status Updated!");
        }


        static void RemoveAccount(ApplicationDBcontext context)
        {
            Console.Write("Account Number: ");
            var accNum = Console.ReadLine();

            Console.Write("Customer Id: ");
            int custId = int.Parse(Console.ReadLine());

            var link = context.CustomerAccounts
                .FirstOrDefault(x => x.AccountNumber == accNum && x.CustomerId == custId);

            if (link == null)
            {
                Console.WriteLine("Not found!");
                return;
            }

            context.CustomerAccounts.Remove(link);
            context.SaveChanges();

            Console.WriteLine("Removed!");
        }

        static void ListCustomers(ApplicationDBcontext context)
        {
            var customers = context.Customers
                .Select(c => new
                {
                    c.FullName,
                    Accounts = c.CustomerAccounts.Select(ca => ca.Account.AccountNumber)
                })
                .ToList();

            foreach (var c in customers)
            {
                Console.WriteLine($"Customer: {c.FullName}");
                foreach (var acc in c.Accounts)
                {
                    Console.WriteLine($"   Account: {acc}");
                }
            }




        }
    }
}
