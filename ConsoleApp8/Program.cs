using System;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using static ConsoleApp2.Program;


namespace ConsoleApp2
{

    public class Program
    {
        public class Bank
        {
            private string? name;
            private string? surname;
            private int age;
            private decimal salary;
            public Card BankCard { get; set; }

            public Bank(int id, string? name, string? surname, int age, decimal salary, Card card)
            {
                this.id = id;
                this.name = name;
                this.surname = surname;
                this.age = age;
                this.salary = salary;
                this.BankCard = card;
            }

            private int id;
            public int Id
            {
                get { return id; }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Daxil edilen ID 0-dan kicik olmamalidir !");
                    }
                    id = value;
                }
            }
            public string? Surname
            {
                get { return surname; }
                set
                {
                    if (value != null && value.Length <= 4)
                    {
                        throw new ArgumentException("Minimal soyad 4 symboldan ibaret olmalidir !");
                    }
                    surname = value;
                }
            }
            public string? Name
            {
                get { return name; }
                set
                {
                    if (value != null && value.Length <= 3)
                    {
                        throw new ArgumentException("Minimal ad 3 symboldan ibaret olmalidir !");
                    }
                    name = value;
                }
            }
            public int Age
            {
                get { return age; }
                set
                {
                    if (value < 18)
                    {
                        throw new ArgumentOutOfRangeException("Minimal yas 18 olmalidir !");
                    }
                    age = value;
                }
            }
            public decimal Salary
            {
                get { return salary; }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Daxil edilen balans 1 manatdan az ola bilmez!");
                    }
                    salary = value;
                }
            }
            public bool CardPinControll(string checkPin)
            {
                return BankCard.Pin == checkPin;
            }

            public void CheckBalance()
            {
                Console.Clear();
                Console.WriteLine($"Your Balance: {BankCard.Wallet}");
            }

            public void Withdraw(decimal Withdraw)
            {
                BankCard.Wallet -= Withdraw;
                Console.WriteLine($"Successfully removed {Withdraw} dollars from your account");
            }

            public void TransferMoney(decimal amount, Bank[] users)
            {
                string cardNumber;
                Console.Write("Enter the recipient's 16-digit card number: ");
                cardNumber = Console.ReadLine();

                Bank recipient = null;

                foreach (var user in users)
                {
                    if (user.BankCard.CardNumber == cardNumber)
                    {
                        recipient = user;
                        break;
                    }
                }

                if (recipient != null)
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Invalid transfer amount.");
                        return;
                    }

                    if (BankCard.Wallet >= amount)
                    {
                        BankCard.Wallet -= amount;
                        recipient.BankCard.Wallet += amount;

                        Console.WriteLine($"Successfully transferred {amount:C} to {recipient.Name} {recipient.Surname}");
                        
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance to make the transfer.");
                    }
                }
                else
                {
                    Console.WriteLine("No account found for the card number you entered.");
                }
            }
        }
        public class Card
        {
            public Card(string? cardNumber, string? pin, string? cvc, decimal wallet)
            {
                this.cardNumber = cardNumber;
                this.pin = pin;
                this.cvc = cvc;
                this.wallet = wallet;
            }

            private decimal wallet;
            public decimal Wallet
            {
                get { return wallet; }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("En az 1 manat daxil edilmelidir !");
                    }
                    wallet = value;
                }
            }

            private string? cvc;
            public string? Cvc
            {
                get { return cvc; }
                set
                {
                    if (value.Length > 3)
                    {
                        throw new ArgumentException("CVC kod max 3 reqemli olmalidir !");
                    }
                    cvc = value;
                }
            }

            private string? pin;
            public string? Pin
            {
                get { return pin; }
                set
                {
                    if (value.Length < 4)
                    {
                        throw new ArgumentException("Daxil edilen Pin kod minimal 4 reqemi olmalidir !");
                    }
                }
            }

            private string? cardNumber;
            public string CardNumber
            {
                get { return cardNumber; }
                set
                {
                    if (value.Length < 16) { throw new ArgumentException($"Minimal card 16 reqemli olmalidir !"); }
                    cardNumber = value;
                }
            }
        }


        static void Main(string[] args)
        {
            Bank[] users = new Bank[5];

            users[0] = new Bank(1, "Ilkin", "yasamalli", 19, 1000, new Card("5123762265236552", "4224", "312", 5000));
            users[1] = new Bank(2, "Nihat", "bineqedili", 30, 2000, new Card("5123762265236553", "1234", "456", 3000));
            users[2] = new Bank(3, "Ulvi", "qobustanli", 25, 1500, new Card("5123762265236554", "5678", "789", 4000));
            users[3] = new Bank(4, "Husi", "pusi", 28, 1800, new Card("5123762265236555", "9876", "654", 3500));
            users[4] = new Bank(5, "Balaeli", "Meyxanausdadi", 22, 1200, new Card("5123762265236556", "2468", "135", 2500));

            while (true)
            {
                Console.Write("Enter your PIN: ");
                string enteredPIN = Console.ReadLine();

                Bank user = null;
                foreach (var u in users)
                {
                    if (u.CardPinControll(enteredPIN))
                    {
                        user = u;
                        break;
                    }
                }

                if (user != null)
                {
                    Console.WriteLine("================= A T M =================");
                    Console.WriteLine($"Welcome, {user.Name} {user.Surname}!");
                    Console.WriteLine("Please select an option");
                    Console.WriteLine("[1] Check Balance");
                    Console.WriteLine("[2] Withdraw Cash");
                    Console.WriteLine("[3] Transfer Money");
                    Console.WriteLine("[4] Exit");

                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            user.CheckBalance();
                            break;
                        case 2:
                            Console.Clear();
                            Console.Write("Type the money you want to remove from the account: ");
                            decimal wintdraw = decimal.Parse(Console.ReadLine());
                            user.Withdraw(wintdraw);
                            break;
                        case 3:
                            Console.Clear();
                            Console.Write("Type the money you want to remove from the account: ");
                            decimal wintdraw2 = decimal.Parse(Console.ReadLine());
                            user.TransferMoney(wintdraw2, users);
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid PIN. Please try again.");
                }
            }
        }

    }
}