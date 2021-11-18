using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Account
{

    public class TransactionEventArgs : EventArgs
    {
        public int TransactionAmount { get; set; }
        public string TransactionType { get; set; }

        public TransactionEventArgs(int amount, string trans_type)
        {
            TransactionAmount = amount;
            TransactionType = trans_type;
        }
    }


    public delegate void TransactionHandler(object sender, TransactionEventArgs e); // delegate definition

    public class Account
    {
        public event TransactionHandler TransactionMade; // Event Definition

        private int m_balance_amount;
        public int BalanceAmount
        { get { return m_balance_amount; } }

        public Account(int amount)
        {
            this.m_balance_amount = amount;
        }

        public void Debit(int amount)
        {
            if (amount < m_balance_amount)
            {
                m_balance_amount = m_balance_amount - amount;
                TransactionEventArgs e = new TransactionEventArgs(amount, "Debited");
                OnTransactionMade(e);
            }
        }

        public void Credit(int amount)
        {
            m_balance_amount = m_balance_amount + amount;
            TransactionEventArgs e = new TransactionEventArgs(amount, "Credited");
            OnTransactionMade(e);
        }

        private void OnTransactionMade(TransactionEventArgs e)
        {
            if (TransactionMade != null)
            {
                TransactionMade(this, e); // Raise the event
            }
        }

    }

    public class Program
    {
        static void Main()
        {
            Account acnt1 = new Account(10000);
            Account acnt2 = new Account(20000);

            acnt1.TransactionMade += TransactionHandler;
            acnt2.TransactionMade += TransactionHandler;

            acnt1.Debit(2000);
            acnt2.Credit(4000);
        }

        static void TransactionHandler(object sender, TransactionEventArgs e)
        {
            Console.WriteLine($"{e.TransactionAmount} {e.TransactionType}");
        }

    }
}

