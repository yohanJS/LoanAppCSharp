using LoanAppCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanAppCSharp.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan) 
        {
            //calculate monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            //create a loop from one to the term
            //and calculate a payment schedule
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            //loop over each month, until I reach the term of the loan
            for (int i = 1; i <= loan.Term; i++)
            {
                monthlyInterest = CalMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;
                monthlyInterest = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;


                LoanPayment loanPayment = new();

                loanPayment.Month = i;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                //push the object into the loan model
                loan.Payments.Add(loanPayment);

            }
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;

            //return the loan to the view
            return loan;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term) 
        {
           
            var monthlyRate = CalcMonthlyRate(rate);

            var rateD = Convert.ToDouble(monthlyRate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * rateD) / (1 - Math.Pow(1+rateD, - term));

            return Convert.ToDecimal(paymentD);
        }

        private decimal CalcMonthlyRate(decimal rate) 
        {
            return rate / 1200;
        }

        private decimal CalMonthlyInterest(decimal balance, decimal monthlyRate) 
        {
            return balance * monthlyRate;
        }
    }
}
