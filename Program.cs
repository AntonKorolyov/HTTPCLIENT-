using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Httpclient
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Choose();
        }
        static async Task RunAsync(string name, string lname, string id, string choose)
        {
            Person person = new Person();
            person.Name = name;
            person.Fname = lname;

            if (choose == "1")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49654/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await client.PostAsJsonAsync("api/Persons", person);
                }
                GetPersons();
                   Choose();
            }

            if (choose == "2")
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49654/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Persons/" + id);
            if (response.IsSuccessStatusCode)
             {
              Person product = await response.Content.ReadAsAsync<Person>();
              Console.WriteLine("{0}\t{1}", product.Name, product.Fname);
              Choose();     
            }
            }
            if(choose =="3")
            {
                GetPersons();
                Choose();
            }
            if(choose == "4")
            {
                person.ID = Convert.ToInt32(id);
                using (var client = new HttpClient())
                {
                   
                    client.BaseAddress = new Uri("http://localhost:49654/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await client.PutAsJsonAsync("api/Persons", person);
                }
                GetPersons();
                Choose();
            }
            if(choose =="5")
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://localhost:49654/");
                    var response = client.DeleteAsync("api/Persons/" + id).Result; 
                }
                GetPersons();
                Choose();
            }
        }

       public static void Choose()
       {
           Console.WriteLine("choose what you want to do \n Press 1 to add Person" +
                                                        "\n Press 2 Get Person by id" +
                                                        "\n Press 3 Get  All Persons" +
                                                        "\n Press 4 Update Pesron By ID" +
                                                        "\n Press 5 to Delete Person by id");
           string choose = Console.ReadLine();
           if (choose == "1")
           {
               Console.WriteLine("you name");
               string name = Console.ReadLine();
               Console.WriteLine("you last name");
               string lname = Console.ReadLine();
               string id = "0";
               RunAsync(name, lname, id, choose).Wait();
           }
           if (choose == "2")
           {
               string name = "";
               string lname = "";
               Console.WriteLine("select ID");
               string id = Console.ReadLine();
               RunAsync(name, lname, id, choose).Wait();
           }
           if (choose == "3")
           {
               string name = "";
               string lname = "";
               string id = "";
               RunAsync(name, lname, id, choose).Wait();
   
           }
           if (choose == "4")
           {
               Console.WriteLine("ID");
               string id = Console.ReadLine();
               Console.WriteLine("name");
               string name = Console.ReadLine();
               Console.WriteLine("last name");
               string lname = Console.ReadLine();
               RunAsync(name, lname, id, choose).Wait();
           }
           if (choose == "5")
           {
               Console.WriteLine("ID");
               string id = Console.ReadLine();
               string name = "";
               string lname = "";
               RunAsync(name, lname, id, choose).Wait();
           }
       }
        public static void GetPersons()
       {
           List<Person> model = null;
           var clients = new HttpClient();
           var task = clients.GetAsync("http://localhost:49654/api/Persons")
             .ContinueWith((taskwithresponse) =>
             {
                 var response = taskwithresponse.Result;
                 var jsonString = response.Content.ReadAsStringAsync();
                 jsonString.Wait();
                 model = JsonConvert.DeserializeObject<List<Person>>(jsonString.Result);
                 foreach (var item in model)
                 {
                     Console.WriteLine(item.ID + ". Name: " + item.Name + "     Last Name: " + item.Fname + "\n");
                 }

             });
           task.Wait();
       }
    }
}
