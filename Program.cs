﻿using bank_system.Models;

internal class Program
{
    public static void Login()
    {
      string? id;
      string? password;

      Console.WriteLine("Enter the CPF or CNPJ:");
      id = Console.ReadLine();

      Console.WriteLine("Enter the password:");
      password = Console.ReadLine();
    }

    public static void NewAccount()
    {
      string? id;
      string? password;
      string? passwordConfirmed;
      string? pin;

      Console.WriteLine("Enter a CPF or a CNPJ:");
      id = Console.ReadLine();
      Console.WriteLine("Enter a password:");
      password = Console.ReadLine();
      Console.WriteLine("Confirm the password:");
      passwordConfirmed = Console.ReadLine();
      Console.WriteLine("Enter a PIN:");
      pin = Console.ReadLine();
    }

    private static void Main(string[] args)
    {
      bool systemOn = true;

      do
      {
          int choice;

          Console.WriteLine("Welcome to Cantoni Bank!\n" +
                            "Choose one option below:\n" +
                            "1 - Login\n" +
                            "2 - Create an account\n" +
                            "3 - Exit");

          choice = Convert.ToInt32(Console.ReadLine());

          if (choice == 1)
          {
              // Send to Login screen
              Login();
          }
          else if (choice == 2)
          {
              // Send to new account screen
              NewAccount();
          }
          else
          {
              // Exit the program
              System.Environment.Exit(0);
          }
      } while (systemOn);
    }
}