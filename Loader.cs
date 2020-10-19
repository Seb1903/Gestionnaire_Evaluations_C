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
    public class Loader
    {
        public static void LoadJSON()
        {
            string jsonString = File.ReadAllText("C:/Users/Sebastien/source/repos/Gestionnaire_Évaluations/db.json");

            JObject teacher = JObject.Parse(jsonString);
            IList<JToken> t = teacher["Teachers"].Children().ToList();     // ici pareil qu'avec students, on les cherche puis on les crée 
            foreach (JToken result in t)
            {
                Teacher tc = result.ToObject<Teacher>();
            }

            
            JObject ac = JObject.Parse(jsonString);
            IList<JToken> act = ac["Activities"].Children().ToList();     // ici pareil qu'avec students, on les cherche puis on les crée 
            foreach (JToken result in act)
            {
                Activity ex = result.ToObject<Activity>();
            }

            JObject ev = JObject.Parse(jsonString);
            IList<JToken> evals = ev["Evaluations"].Children().ToList();     // ici pareil qu'avec students, on les cherche puis on les crée 
            foreach (JToken result in evals)
            {
                //Evaluation iter = result.ToObject<Evaluation>();
               // Console.WriteLine(iter.activity.Code);
            }


            JObject stud = JObject.Parse(jsonString);
            IList<JToken> students = stud["Students"].Children().ToList();    // ici je cherche tous mes students dans mon JSON 
            IList<Student> studentlist = new List<Student>();
            List<Student> deserializedstudents = JsonConvert.DeserializeObject<List<Student>>(jsonString,
    new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

            Console.WriteLine(deserializedstudents);

            foreach (JObject result in students)
            {
                Student stu = result.ToObject<Student>();                       // Ici je les créé en objets me semble-t-il
                studentlist.Add(stu);                                     // cette liste me servira à imprimer les bulletins après, c'est une liste d'objets 
            }

            JObject co = JObject.Parse(jsonString);
            IList<JToken> cotes = co["Cotes"].Children().ToList();     // ici pareil qu'avec students, on les cherche puis on les crée 
            foreach (JToken result in cotes)
            {
                Cote iter = result.ToObject<Cote>();
            }



            




            foreach (Student s in studentlist)   //je parcours chaque objet Student dans la list d'objets Students
            {
                Console.WriteLine(s.Average());
            }


        }

    }

}