using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FormSubmission.Models;
using DbConnection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FormSubmission.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var errors = HttpContext.Session.GetObjectFromJson<List<string>>("errors");
            if (errors != null)
            {
                ViewBag.errors = errors;   
            }
            return View();
        }
        [HttpPost]
        [Route("newUser")]
        public IActionResult newUser(string firstName, string lastName, string age, string password, string email)
        {
            Console.WriteLine("FUUUUUUUUUUUUU");
            int intAge = Convert.ToInt32(age);
            User NewUser = new User
            {
                firstName = firstName,
                lastName = lastName,
                age = intAge,
                email = email,
                password = password
            };
            if (TryValidateModel(NewUser))
            {
                DbConnection.DbConnector.Execute(string.Format("INSERT INTO User (firstName, lastName, age, password, email) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", NewUser.firstName, NewUser.lastName, NewUser.age, NewUser.password, NewUser.email));
                return RedirectToAction("success");
            }
            else 
            {
                List<string> errors = new List<string>();
                foreach(var error in ModelState.Values)
                {
                    for(int i = 0; i < error.Errors.Count; i++) 
                    {
                        errors.Add(error.Errors[i].ErrorMessage);
                    }
                }
                HttpContext.Session.SetObjectAsJson("errors", errors);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("Success")]

        public IActionResult Success()
        {
            return View();
        }

    }
    // Somewhere in your namespace, outside other classes
    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes theobject to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
