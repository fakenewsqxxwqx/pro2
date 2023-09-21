using System;

namespace pro1{
    class BigMoneyArgs{
        public int accId;
        public double amount;
    }

    delegate void BigMoneyHandle(BigMoneyArgs args);
    class Account{
        //fields
        private int accId;
        private string userName;
        private double balance;

        //properties
        public int AccNo{
            get{return accId;}
            set{accId=value;}
        }
        public string Name{
            get{return userName;}
            set{userName=value;}
        }
        public double Balance{
            get{return balance;}
            set{balance=value;}
        }

        //methods
        public void ShowAccountInfo(){
            Console.WriteLine("Account Id: {0}",accId);
            Console.WriteLine("User Name: {0}",userName);
            Console.WriteLine("Balance: {0}",balance);
        }
        public void Deposit(double amount){
            Random rnd=new Random();
            int num=rnd.Next(1,9);
            if(num<=3){
                throw new BadCashException("Bad Cash");
            }
            balance+=amount;
        }
        virtual public void Withdraw(double amount){
            balance-=amount;
        }
    }

    class Bank{
        //fields
        private string bankName="";
        private List<Account> accounts = new List<Account>();

        //properties
        public string BankName{
            get{return bankName;}
            set{bankName=value;}
        }

        //indexer
        public Account this[int index]{
            get{return accounts[index];}
            set{accounts[index]=value;}
        }

        //methods
        public void withdraw(int accId,double amount){
            foreach(Account acc in accounts){
                if(acc.AccNo==accId){
                    acc.Withdraw(amount);
                    return;
                }
            }
            Console.WriteLine("Account not found");
        }
        public void deposit(int accId,double amount){
            foreach(Account acc in accounts){
                if(acc.AccNo==accId){
                    acc.Deposit(amount);
                    return;
                }
            }
            Console.WriteLine("Account not found");
        }
        public void addAccount(Account acc){
            accounts.Add(acc);
        }
        public void showAllAccounts(){
            foreach(Account acc in accounts){
                acc.ShowAccountInfo();
            }
        }
        public void removeAccount(int accId){
            foreach(Account acc in accounts){
                if(acc.AccNo==accId){
                    accounts.Remove(acc);
                    return;
                }
            }
            Console.WriteLine("Account not found");
        }
        public void BigMoneyWaring(BigMoneyArgs args){
            Console.WriteLine("Big Money Transaction");
            Console.WriteLine("Account Id: {0}",args.accId);
            Console.WriteLine("Amount: {0}",args.amount);
        }
    }

    class BadCashException:Exception{
        public BadCashException(string message):base(message){}
    }

    class ATM{
        private Bank bank=new Bank();
        public event BigMoneyHandle bigMoneyEvent;
        
        public void addAccount(Account acc){
            bank.addAccount(acc);
        }
        public void removeAccount(int accId){
            bank.removeAccount(accId);
        }

        public void deposit(int accId,double amount){
            try{
                bank.deposit(accId,amount);
            }
            catch(BadCashException e){
                Console.WriteLine(e.Message);
            }
        }
        public void withdraw(int accId,double amount){
            if(amount>100000){
                BigMoneyArgs args=new BigMoneyArgs();
                args.accId=accId;
                args.amount=amount;
                bigMoneyEvent+=new BigMoneyHandle(bank.BigMoneyWaring);
                bigMoneyEvent(args);
            }
            bank.withdraw(accId,amount);
        }
    }
    class CredirAccount:Account{
        private int limit;
        public int Limit{
            get{return limit;}
            set{limit=value;}
        }

        public override void Withdraw(double amount){
            if(Balance-amount>=limit){
                base.Withdraw(amount);
            }
            else{
                Console.WriteLine("Insufficient Balance");
            }
        }
    }
    
    class Project{
        static void Main(){
            ATM atm=new ATM();
            Account acc1=new Account();
            acc1.AccNo=1;
            acc1.Name="Rahim";
            acc1.Balance=1000;
            atm.addAccount(acc1);
            atm.deposit(1,1000);
            atm.withdraw(1,1000);
            atm.removeAccount(1);
        }
    }

}