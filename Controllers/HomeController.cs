using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WordGame.Models;

namespace WordGame.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Words words = new Words();
            words = LoadMatrix();
            //dynamic roles = ((dynamic)System.Web.HttpContext.Current.ApplicationInstance).words;

            return View(words);
        }

        public ActionResult StartNewGame()
        {
            Words words = LoadMatrix();
            return PartialView("Index", words);
        }
        public Words LoadMatrix()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Words words = new Words
            {
                OneOne = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                OneTwo = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                OneThree = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                OneFour = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),

                TwoOne = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                TwoTwo = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                TwoThree = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                TwoFour = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),

                ThreeOne = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                ThreeTwo = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                ThreeThree = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                ThreeFour = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),

                FourOne = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                FourTwo = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                FourThree = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray()),
                FourFour = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray())
            };
            return words;
        }
        [HttpPost]
        public bool CheckWord(Words model)
        {
            WordDictionary dictionary = new WordDictionary() {
                DictionaryFile = "en-US.dic"
            };
            dictionary.Initialize();
            Spelling spelling = new Spelling() {
                Dictionary = dictionary
            };
            if (!spelling.TestWord(model.RandomCharacter)) {
                return false;
            } else {
                string givenWord = model.RandomCharacter;
                model.RandomCharacter = string.Empty;
                Type type = model.GetType();
                var properties = type.GetProperties();
                List<string> characters = new List<string>();
                foreach (var property in properties)
                {
                    if(!string.IsNullOrEmpty(property.GetValue(model).ToString()))
                        characters.Add(property.GetValue(model).ToString());
                }
                List<string> givenWordCharList = new List<string>();
                givenWordCharList.AddRange(givenWord.Select(c => c.ToString()));
                bool result = false;
                foreach (var character in givenWordCharList)
                {
                    if (characters.Contains(character.ToUpper()))
                    {
                        result = true;
                        characters.Remove(character.ToUpper());
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            }
        }

        [HttpPost]
        public ActionResult GetPossibleWords(Words model)
        {
            List<string> possibleWords = new List<string>();
            Type type = model.GetType();
            var properties = type.GetProperties();
            StringBuilder builder = new StringBuilder();
            foreach(var property in properties)
            {
                builder.Append(property.GetValue(model));
            }
            var alphabet = builder.ToString();

            var generatedWords = alphabet.Select(x => x.ToString());
            for (int i = 3; i < 5; i++)
                generatedWords = generatedWords.SelectMany(x => alphabet, (x, y) => x + y);
            List<string> coll = generatedWords.ToList();

            Random random = new Random();
            for(int i = 1; i<=generatedWords.Count(); i++)
            {
                string randomSelection = coll[random.Next(generatedWords.Count())];
                model.RandomCharacter = randomSelection;
                if (!possibleWords.Contains(randomSelection))
                {
                    if (CheckWord(model))
                        possibleWords.Add(randomSelection);
                }
                if (possibleWords.Count() == 5) 
                    break;
            }
            var list = possibleWords;
            return Json(new { possibleWords });
        }
    }
}