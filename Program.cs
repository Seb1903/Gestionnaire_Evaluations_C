using System;
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
    public abstract class Evaluation
    {
        public float point;
        public Activity activity;
        public Evaluation(Activity activity)
        {
            this.activity = activity;
        }
        public abstract void setNote(float x);
        public abstract void setAppreciation(string x);
        public virtual float Note()
        {
            return this.point; 
        }

    }
    public class Cote : Evaluation
    {
        public new float point;
        public Cote(Activity activity)
           : base(activity)
        {
        }
        public override void setNote(float x)
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
        public override void setAppreciation(string appreciation)
        {
        }


    }
    public class Appreciation : Evaluation
    {
        public new float point;
        public Appreciation(string appreciation, Activity activity)
           : base(activity)
        {
        }
        public override void setAppreciation(string appreciation)
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
        public override void setNote(float x)
        {
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

            Evaluation e = new Cote(info);
            Console.WriteLine(e.Note());
            e.setNote(2);

            marti.AddEval(e);

            Activity chimie = new Activity(6, "chimie", "3BEch", Lurkin);

            Evaluation i = new Cote(chimie);
            Console.WriteLine(i.Note());
            i.setNote(18);

            marti.AddEval(i);
            Console.WriteLine(marti.Average());


            marti.Bulletin();

            Evaluation f = new Appreciation("TB", info);
            marti.AddEval(f);
            f.setAppreciation("TB");

            marti.Bulletin();

            string jsonData = JsonConvert.SerializeObject(marti);

            File.WriteAllText("db.json",jsonData);

            Program.LoadJSON();
        }

        private static void LoadJSON()
        {
            string jsonString = File.ReadAllText("C:/Users/Sebastien/source/repos/Gestionnaire_Évaluations/db.json");

            JObject stud = JObject.Parse(jsonString);
            IList<JToken> students = stud["Students"].Children().ToList();    // ici je cherche tous mes students dans mon JSON 
            IList<Student> studentlist = new List<Student>();
            foreach (JToken result in students)
            {
                Student stu = result.ToObject<Student>();                        // Ici je les créé en objets me semble-t-il
                studentlist.Add(stu);                                     // cette liste me servira à imprimer les bulletins après, c'est une liste d'objets 
            }



            JObject ev = JObject.Parse(jsonString);
            IList<JToken> evals = ev["Evaluations"].Children().ToList();     // ici pareil qu'avec students, on les cherche puis on les crée 
            foreach (JToken result in evals)
            {
                Evaluation iter = result.ToObject<Evaluation>();
            }



            
            foreach (Student s in studentlist)   //je parcours chaque objet Student dans la list d'objets Students
            {
                Console.WriteLine(s.Average());
            }

            
        }
    }
}
