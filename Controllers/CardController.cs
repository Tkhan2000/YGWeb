using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Frameworks;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using YGWeb.Areas.Identity.Data;
using YGWeb.Data;
using YGWeb.Models;
using static YGWeb.Models.Card;

namespace YGWeb.Controllers
{
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<YGWebUser> _userManager;
        private int _lastPage;
        private string _currentCardList = "";
        
        public CardController(ApplicationDbContext db, UserManager<YGWebUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index(string searchString, string cardType, int? page)
        {
            /*
             * Main Page for Card List. Keeps track of Search Keyword, page Number, and other filter keywords 
             * and modifies database accordingly
            */
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);
            int pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(searchString))
            {
                baseCardList = baseCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }
            if (!String.IsNullOrEmpty(cardType))
            {
                baseCardList = baseCardList.Where(card => card.type.Contains(cardType));
            }
            var chunks = baseCardList.Chunk(50);
            _lastPage = chunks.Count();

            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.cardType = cardType;
            ViewBag.lastPage = _lastPage;

            if (_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber - 1);

            
            return View(currentPage);
        }
        [Authorize]
        public IActionResult DeckBuilder(string searchString, int? page)
        {
            IEnumerable<Card> objCardList = _db.Cards;
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Deck> userDecks = _db.Decks.Where(deck => deck.YGWebUserId == userId);

            List<Card> cardList = _db.Cards.ToList();
            foreach(Card card in baseCardList)
            {
                cardList.Remove(card);
            }
            IEnumerable<Card>  tempCardList = cardList.OrderBy(card => card.name);
            int pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(searchString))
            {
                baseCardList = baseCardList.Where(card => card.name.ToLower().Contains(searchString.ToLower()) || card.description.ToLower().Contains(searchString.ToLower()));
            }

            
            var chunks = baseCardList.Chunk(80);
            _lastPage = chunks.Count();

            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.lastPage = _lastPage;

            if (_lastPage == 0)
            {
                return View();
            }

            var currentPage = chunks.ElementAt(pageNumber - 1);

            return View(Tuple.Create(currentPage, tempCardList, userDecks));
        }

        public IActionResult addCard(int id, string s, int p, int c)
        {
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);

            for (int i = 0; i < c; i++)
            {
                // Add back any extra cards that were removed
                _db.Cards.Add(card);
                _db.SaveChanges();
            }
            return RedirectToAction("DeckBuilder", new {searchString = s, page = p});
        }

        public IActionResult removeCard(int id, string s, int p, int c)
        {
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            int count = _db.Cards.Count(card => card.id == id) - c;
            _db.Cards.Remove(card);
            _db.SaveChanges();
            for (int i = 0; i < count; i++)
            {
                // Add back any extra cards that were removed
                _db.Cards.Add(card);
                _db.SaveChanges();
            }
            return RedirectToAction("DeckBuilder", new { searchString = s, page = p });
        }

        public IActionResult clearDeck(string s, int p, string deck)
        {
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);

            List<Card> cardList = _db.Cards.ToList();
            foreach (Card card in baseCardList)
            {
                cardList.Remove(card);
            }
            IEnumerable<Card> tempCardList = cardList.OrderBy(card => card.name);

            foreach(Card obj in tempCardList)
            {
                _db.Cards.Remove(obj);
                _db.SaveChanges();
                _db.Cards.Add(obj);
                _db.SaveChanges();
            }

            if (deck != null)
            {
                IEnumerable<Card> cards = stringToList(deck);
                foreach (Card obj in cards)
                {
                    _db.Cards.Add(obj);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("DeckBuilder", new { searchString = s, page = p });
        }

        [HttpPost]
        public JsonResult saveDeck()
        {
            IEnumerable<Card> tempCardList = getDeckList();
            bool isValid = validateDeck(tempCardList);
            JsonResult result;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Deck> userDecks = _db.Decks.Where(deck => deck.YGWebUserId == userId);
            if (doesDeckExist(userDecks))
            {
                isValid = false;
                return new JsonResult("Deck Exists");
            }
            if (isValid)
            {
                var deck = new Deck();
                string cards = "";
                foreach (Card objCard in tempCardList)
                {
                    cards += objCard.id.ToString() + "/";
                }
                deck.CardList = cards;
                deck.YGWebUserId = userId;
                _db.Decks.Add(deck);
                _db.SaveChanges();
                result = new JsonResult("Success");
            }
            else
            {
                result = new JsonResult("Failure");
            }

            return result;

        } 

        public IActionResult CardDetails(int id, string cardList, string searchString, int? page, int count)
        {
            int pageNumber = page ?? 1;
            ViewBag.cardList = cardList;
            ViewBag.pageNumber = pageNumber;
            ViewBag.searchString = searchString;
            ViewBag.count = count;
            Card card = _db.Cards.FirstOrDefault(card => card.id == id);
            return View(card);
        }

        public IActionResult SavedDecks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Deck> userDecks = _db.Decks.Where(deck => deck.YGWebUserId == userId);
            List<IEnumerable<Card>> fullCardList = new List<IEnumerable<Card>>();

            foreach (var deck in userDecks)
            {
                IEnumerable<Card> cardList = stringToList(deck.CardList);
                fullCardList.Add(cardList);
            }
            return View(Tuple.Create(userDecks, fullCardList));
        }

        public IEnumerable<Card> getDeckList() 
        {
            IEnumerable<Card> baseCardList = _db.Cards.Distinct().OrderBy(card => card.name);

            List<Card> cardList = _db.Cards.ToList();
            foreach (Card card in baseCardList)
            {
                cardList.Remove(card);
            }
            return cardList.OrderBy(card => card.name);
        }

        //Performs full validation of deck and returns boolean
        public bool validateDeck(IEnumerable<Card> deck)
        {
            // Use List of keywords to split the deck into main deck and extra
            List<string> extraDeckMonster = new List<string>(new string[] { "Fusion", "Synchro", "XYZ", "Link" });
            IEnumerable<Card> mainDeck = deck.Where(card => extraDeckMonster.Any(keyword => !card.type.Contains(keyword)));
            IEnumerable<Card> extraDeck = deck.Where(card => extraDeckMonster.Any(keyword => card.type.Contains(keyword)));

            // Validate length of each deck
            int maxMain = 60;
            int minMain = 40;
            int maxExtra = 20;
            if (mainDeck.Count() < minMain || mainDeck.Count() > maxMain || extraDeck.Count() > maxExtra)
            {
                return false;
            }

            // Validate that there are no cards that appear in excess in the full deck
            var cards = deck.GroupBy(x => x);
            foreach (var card in cards)
            {
                int max = 3; //Change later to reflect limited cards
                if (card.Count() > max)
                {
                    return false;
                }
            }

            return true;
        }

        //Given a deck, parse the string of ids and return an IEnumerable<Card>
        public IEnumerable<Card> stringToList(string deck)
        {
            List<string> ids = deck.Split("/").ToList();
            List<int> idList = ids.Select(int.Parse).ToList();
            List<Card> cards = new List<Card>();
            foreach (int id in idList)
            {
                Card card = _db.Cards.FirstOrDefault(c => c.id == id);
                cards.Add(card);
            }
            IEnumerable<Card> newDeck = cards.OrderBy(card => card.name);
            return cards.OrderBy(card => card.name);
        }

        public string listToString(IEnumerable<Card> cardList)
        {
            string cards = "";
            foreach (Card objCard in cardList)
            {
                cards += objCard.id.ToString() + "/";
            }
            return cards.Remove(cards.Length - 1);
        }

        //Check the user already has a deck that matches the current deck
        public bool doesDeckExist(IEnumerable<Deck> decks)
        {
            string currentDeck = listToString(getDeckList());
            foreach (Deck deck in decks)
            {
                if (deck.CardList.Equals(currentDeck))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
