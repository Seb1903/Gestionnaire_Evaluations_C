﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gestionnaire_Evaluations
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
        public List<Evaluation> evaluations = new List<Evaluation>();
        public Student (string firstName, string lastName)
            :base (firstName, lastName)
        {
        }
        public void AddEval(Evaluation e)
        {
            evaluations.Add(e);
        }
        public double Average()
        {
            Console.WriteLine(evaluations[0]);
            float Sum = 0;
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
    public class Evaluation //abstract ?
    {
        public float point;
        public Activity activity;
        public Evaluation(Activity activity)
        {
            this.activity = activity;
        }
        //public abstract void setNote(float x);
      //  public abstract void setAppreciation(string x);
        public virtual float Note()
        {
            return this.point; 
        }

    }
    public class Cote : Evaluation
    {
        public new float point;
        public  Cote(Activity activity, float y)
           : base(activity)
        {
            this.point = y;
        }
        public void setNote(float x)
        {
            if (x >= 0 && x <= 20)
            {
                this.point = x;
            }    
        }
        public override float Note()
        {
            return this.point;
        }


    }
    public class Appreciation : Evaluation
    {
        public new float point;
        public Appreciation(string appreciation, Activity activity)
           : base(activity)
        {
            this.point = Equivalent(appreciation);
        }
        public  void setAppreciation(string appreciation)
        {
            this.point = Equivalent(appreciation);
        }
        private float Equivalent(string apr)
        {
            switch (apr)
            {
                case "X":
                    return 20;
                case "TB":
                    return 16;
                case "B":
                    return 12;
                case "C":
                    return 8;
                case "N":
                    return 4;

                default:
                    return 10;
            }
        }
        public override float Note()
        {
            return this.point;
        }
    }

    public class Program
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

            Evaluation e = new Cote(info, 15);
            Console.WriteLine(e.Note());
            //e.setNote(2);
            e.Note();

            marti.AddEval(e);

            Activity chimie = new Activity(6, "chimie", "3BEch", Lurkin);

            Evaluation i = new Cote(chimie, 16);
            Console.WriteLine(i.Note());
            //i.setNote(18);
            i.Note();

            marti.AddEval(i);
            Console.WriteLine(marti.Average());


            marti.Bulletin();

            Evaluation f = new Appreciation("TB", info);
            marti.AddEval(f);
            // f.setAppreciation("TB");
            f.Note();

            marti.Bulletin();

            string jsonData = JsonConvert.SerializeObject(marti);

            File.WriteAllText("db.json",jsonData);

        }

    }
}
