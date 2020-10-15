using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Gestionnaire_Évaluations
{
    public class Person
    {
        public String firstName;
        public String lastName;
        public Person (string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;

        }
        public void DisplayName()                // public override string ToString()
        {
            Console.WriteLine(string.Format("{0} {1}", firstName, lastName));
        }
    }

    public class Student : Person
    {
        public Evaluation eval;
        public List<Evaluation> evaluations = new List<Evaluation>();
        public Student (string firstName, string lastName)
            :base (firstName, lastName)
        {
        }
        public void AddEval(Evaluation e)
        {
            eval = e;
            evaluations.Add(e); 
        }
        public double Average()
        {
            int Sum = 0;
            int credits = 0;
            for (int i = 0; i < evaluations.Count ; i++)
            {
                Sum += evaluations[i].Note() * evaluations[i].activity.ECTS;
                credits += evaluations[i].activity.ECTS;
            }

            return Sum / (credits);
        }
        public String Bulletin()
        {
            string bulletin ="";
            bulletin += string.Format("\n {0} {1} \n \n", firstName, lastName) ;
            for (int i = 0; i < evaluations.Count ; i++)
            {
                bulletin += string.Format("[{0}] {1} ({2} {3}, {4}ECTS): {5} \n", evaluations[i].activity.Code, evaluations[i].activity.Name, evaluations[i].activity.teacher.firstName, evaluations[i].activity.teacher.lastName, evaluations[i].activity.ECTS, evaluations[i].Note());     
            }
            bulletin += String.Format("\n AVERAGE : {0}", Average());
            Console.WriteLine(bulletin);
            return bulletin; 
        }

    }
    public class Teacher : Person
    {
        public int Salary; 
        public Teacher(string firstName, string lastName,int Salary)
            : base(firstName, lastName)
        {
            this.Salary = Salary ;
        }
    }
    public class Activity
    {
        public int ECTS;
        public string Name;
        public string Code;
        public Teacher teacher;
        public Activity(int ECTS, string Name, string Code, Teacher teacher)
        {
            this.teacher = teacher;
            this.ECTS = ECTS;
            this.Name = Name;
            this.Code = Code; 
        }
    }
    public abstract class Evaluation
    {
        public int point;
        public Activity activity;
        public Evaluation(Activity activity)
        {
            this.activity = activity;

         // surement pas juste voir Note()        
        }
        public abstract void setNote(int x);
        public abstract void setAppreciation(string x);
        public virtual int Note()
        {
            return this.point; 
        }

    }
    public class Cote : Evaluation
    {
        public new int point;
        public Cote(Activity activity)
           : base(activity)
        {
        }
        public override void setNote(int x)
        {
            this.point = x;
        }
        public override int Note()
        {
            return this.point;
        }
        public override void setAppreciation(string appreciation)
        {
        }


    }
    public class Appreciation : Evaluation
    {
        public new int point;
        public int x; 
        public Appreciation(string appreciation, Activity activity)
           : base(activity)
        {
            x = Equivalent(appreciation);
        }
        public override void setAppreciation(string appreciation)
        {
            this.point = Equivalent(appreciation);
        }
        private int Equivalent(string apr)
        {
            switch (apr)
            {
                case "TB":
                    return 20;
                case "B":
                    return 15;
                case "N":
                    return 10;
                case "PB":
                    return 5;
                case "Nul":
                    return 0;

                default:
                    return 10;
            }
        }
        public override int Note()
        {
            return this.point;
        }
        public override void setNote(int x)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Person Moi = new Person("Sebastien", "Martinez");
            Moi.DisplayName(); 

            Student marti = new Student("Marti", "McFLy");
            marti.DisplayName();

            Teacher Lurkin = new Teacher("Q", "Lur", 10000);
            Console.WriteLine(Lurkin.Salary);
            Lurkin.DisplayName();

            Activity info = new Activity(6, "informatique", "3BEee", Lurkin);
            //Console.WriteLine((info.teacher).DisplayName());  ne marche pas ? 

            Evaluation e = new Cote(info);
            Console.WriteLine(e.Note());
            e.setNote(2);

            marti.AddEval(e);

            Activity chimie = new Activity(6, "chimie", "3BEch", Lurkin);
            //Console.WriteLine((info.teacher).DisplayName());  ne marche pas ? 

            Evaluation i = new Cote(chimie);
            Console.WriteLine(i.Note());
            i.setNote(18); 

            marti.AddEval(i);
            Console.WriteLine(marti.Average());


            marti.Bulletin();

            Evaluation f = new Appreciation("TB",info);
            marti.AddEval(f);
            f.setAppreciation("TB");

            marti.Bulletin();



        }
    }
}
