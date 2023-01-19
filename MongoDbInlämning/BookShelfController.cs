using MongoDB.Driver;
using MongoDbInlämning.BookshelfDAO;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;
using MongoDbInlämning.QuotesAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MongoDbInlämning
{
    public class BookShelfController : IBookshelfController
    {
        private IBookDAO bookDAO;
        private IComicDAO comicDAO;
        private IUI UI;
        private IQuotesApiClient apiClient;
        public BookShelfController(IBookDAO bookDAO, IComicDAO comicDAO, IUI uI, IQuotesApiClient apiClient)
        {
            this.bookDAO = bookDAO;
            this.comicDAO = comicDAO;
            this.UI = uI;
            this.apiClient = apiClient;
        }
        public void StartUp() //Sätter igång en while-loop med menyval.
        {
            bool ValidConnections = true;
            try //Testar kontakten med databaserna, ifall det inte fungerar så får man information om problemet samt avslutar programmet
            {
                bookDAO.TestConnection();
                comicDAO.TestConnection();
            }
            catch (MongoAuthenticationException)
            {
                UI.PrintString("Could not authenticate connection to database");
                ValidConnections = false;
            }
            catch (Exception ex)
            {
                UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                ValidConnections = false;
            }
            while (ValidConnections)
            {

                UI.PrintString("Bookshelf database:");

                try
                {
                    UI.PrintString(GetQuote()); //Denna metod hämtar ett citat från ett API.
                }
                catch (Exception ex)
                {
                    UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                }
                UI.PrintString("1.List Books\n2.List Comics\n3.Add item to bookshelf\n4.Update item in bookshelf\n5.Search for item\n6.Delete item from database\n7.Exit");
                string choice = UI.GetStringInput();
                UI.Clear();
                switch (choice)
                {
                    case "1":
                        try
                        {
                            ListBooks(); //Hämtar och skriver ut alla böcker.
                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace); //Göra en metod med detta istället?
                        }
                        Console.ReadKey();
                        break;
                    case "2":
                        try
                        {
                            ListComics(); //Hämtar och skriver ut alla serietidningar..
                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                        }
                        Console.ReadKey();
                        break;
                    case "3":
                        try
                        {
                            AddItem();

                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                        }
                        Console.ReadKey();
                        break;
                    case "4":
                        try
                        {
                            UpdateItem();

                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                        }
                        Console.ReadKey();
                        break;
                    case "5":
                        try
                        {
                            SearchItem();
                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                        }
                        Console.ReadKey();
                        break;
                    case "6":
                        try
                        {
                            DeleteItem();
                        }
                        catch (Exception ex)
                        {
                            UI.PrintString("Något fel inträffade: \n" + ex.Message + "\n\n" + ex.StackTrace);
                        }
                        Console.ReadKey();
                        break;
                    case "7":
                        Environment.Exit(0);
                        break;
                    default:
                        UI.PrintString("Wrong input, try again.");
                        Console.ReadKey();
                        break;
                }
                UI.Clear();
            }
            Console.ReadKey();
            Environment.Exit(0);
        }
        private List<Book> ListBooks()
        {
            /*
             * Här har jag ett return på List<Book> så kan den antingen användas av sig självt, 
             * men även i andra metoder om jag skulle behöva listan.
             */
            List<Book> books = new List<Book>();
            int indexOfItems = 1;
            books = bookDAO.ReadAll();
            foreach (Book book in books)
            {

                UI.PrintString($"{indexOfItems}.{book.Title}\n{book.Author}\nPages: {book.Pages}\n");
                indexOfItems++;
            }
            return books;

        }
        private List<Comic> ListComics()
        {
            int indexOfItems = 1;
            List<Comic> comics = new List<Comic>();

            comics = comicDAO.ReadAll();
            foreach (Comic comic in comics)
            {
                UI.PrintString($"{indexOfItems}.{comic.Title}\n{comic.Vol}\n{comic.Author}\n");
                indexOfItems++;
            }
            return comics;
        }
        public void AddItem()
        {
            /* Man får här välja om man ska lägga till en bok eller serietidning.
             * Detta gör man genom att använda sig av AddBook/AddComic som retunerar
             * en ny bok eller serietidning.
             */
            UI.PrintString("1. Book\n2. Comic");
            string choice = UI.GetStringInput();
            switch (choice)
            {
                case "1":
                    bookDAO.Create(AddBook());
                    break;
                case "2":
                    comicDAO.Create(AddComic());
                    break;
                default:
                    UI.PrintString("Wrong input.");
                    break;
            }
        }
        private Comic AddComic()
        {
            string title, vol, author;
            UI.PrintStringSameLine("Title: ");
            title = CheckStringChoice(); //CheckStringChoice retunerar en string som är kontrollerad för null och whitespaces.
            UI.PrintStringSameLine("Volume: ");
            vol = CheckStringChoice();
            UI.PrintStringSameLine("Author: ");
            author = CheckStringChoice();
            return new Comic(title, vol, author);
        }
        private Book AddBook()
        {
            string title, author;
            int pages;
            UI.PrintStringSameLine("Title: ");
            title = CheckStringChoice();
            UI.PrintStringSameLine("Author: ");
            author = CheckStringChoice();
            UI.PrintStringSameLine("Pages: ");
            bool ValidParse;
            do
            {
                ValidParse = Int32.TryParse(UI.GetStringInput(), out pages);
                if (ValidParse == false) UI.PrintString("Wrong input");
            } while (ValidParse == false);
            return new Book(title, author, pages);

        }
        public void UpdateItem()
        {

            UI.PrintString("1.Book\n2.Comic");
            switch (UI.GetStringInput())
            {
                /*
                 * Använder mig här av "CheckIntInput" för att kolla så att inmatningen håller sig inom index.
                 * Den retunerar en int som jag tar -1 för att det ska överensstämma med index.
                 */
                case "1":
                    List<Book> bookList = ListBooks();
                    UI.PrintString("Input the number of the book you want to update");
                    UpdateBook(bookList, CheckIntInput(bookList.Count) - 1);
                    break;
                case "2":
                    List<Comic> comicsList = ListComics();
                    UI.PrintString("Input the number of the comic you want to update");
                    UpdateComic(comicsList, CheckIntInput(comicsList.Count) - 1);
                    break;
                default:
                    UI.PrintString("Wrong input");
                    break;
            }
        }
        private void UpdateBook(List<Book> collectionOfBooks, int choiceOfBook)
        {
            /*
             * Skickar in objektId för boken jag vill uppdatera och sedan fältet och till sist ändringen.
             */
            string update;
            int pages;
            UI.Clear();
            UI.PrintString($"What do you want to update?\n1.Titel: {collectionOfBooks[choiceOfBook].Title}\n2.Author: {collectionOfBooks[choiceOfBook].Author}\n3.Pages: {collectionOfBooks[choiceOfBook].Pages}");
            switch (UI.GetStringInput())
            {
                case "1":
                    UI.PrintStringSameLine("Titel: ");
                    update = CheckStringChoice();
                    bookDAO.Update(collectionOfBooks[choiceOfBook].databaseId, "Title", update);
                    break;
                case "2":
                    UI.PrintStringSameLine("Author: ");
                    update = CheckStringChoice();
                    bookDAO.Update(collectionOfBooks[choiceOfBook].databaseId, "Author", update);
                    break;
                case "3":
                    UI.PrintStringSameLine("Pages: ");
                    if (Int32.TryParse(UI.GetStringInput(), out pages))
                    {
                        bookDAO.Update(collectionOfBooks[choiceOfBook].databaseId, "Pages", pages);
                    }
                    else
                    {
                        UI.PrintString("Wrong input");
                    }
                    break;
                default:
                    UI.PrintString("Wrong input");
                    break;
            }
        }
        private void UpdateComic(List<Comic> collectionOfComic, int choiceOfComic)
        {
            string update;
            UI.Clear();
            UI.PrintString($"What do you want to update?\n1.Titel: {collectionOfComic[choiceOfComic].Title}\n2.Volume: {collectionOfComic[choiceOfComic].Vol}\n3.Author: {collectionOfComic[choiceOfComic].Author}");
            switch (UI.GetStringInput())
            {
                case "1":
                    UI.PrintStringSameLine("Titel: ");
                    update = CheckStringChoice();
                    comicDAO.Update(collectionOfComic[choiceOfComic].databaseId, "Title", update);
                    break;
                case "2":
                    UI.PrintStringSameLine("Volume: ");
                    update = CheckStringChoice();
                    comicDAO.Update(collectionOfComic[choiceOfComic].databaseId, "Vol", update);
                    break;
                case "3":
                    UI.PrintStringSameLine("Author: ");
                    update = CheckStringChoice();
                    comicDAO.Update(collectionOfComic[choiceOfComic].databaseId, "Author", update);
                    break;
                default:
                    UI.PrintString("Wrong input");
                    break;
            }
        }
        public void DeleteItem()
        {
            UI.PrintString("1.Book\n2.Comic");
            switch (UI.GetStringInput())
            {
                case "1":
                    UI.Clear();
                    List<Book> bookList = ListBooks();
                    UI.PrintString("Input the number of the book you want to delete");
                    bookDAO.Delete(bookList[CheckIntInput(bookList.Count) - 1].databaseId);
                    break;
                case "2":
                    UI.Clear();
                    List<Comic> comicsList = ListComics();
                    UI.PrintString("Input the number of the comic you want to delete");
                    comicDAO.Delete(comicsList[CheckIntInput(comicsList.Count) - 1].databaseId);
                    break;
                default:
                    UI.PrintString("Wrong input");
                    break;
            }
        }

        public void SearchItem()
        {
            /*Här väljer jag vilket fält jag ska söka på, har valt gemensamma nämnare som titel och författare.
             *Sökvärdet samt fältet stoppas in i en read i DAO:erna som med ett regex filter söker av.
             *Matchningar retuneras tillbaka i en IBookShelfItem vilket både comic och book bygger på.
             *Så skrivs resultaten ut av PrintIBookshelfItems.
             */
            UI.Clear();
            UI.PrintStringSameLine("Choose search field:\n1.Title\n2.Author\nInput:");
            string searchValue;
            switch (UI.GetStringInput())
            {
                case "1":
                    UI.PrintStringSameLine("Title:");
                    searchValue = CheckStringChoice();
                    PrintIBookshelfItems(SearchCollections("Title", searchValue));
                    break;
                case "2":
                    UI.PrintStringSameLine("Author:");
                    searchValue = CheckStringChoice();
                    PrintIBookshelfItems(SearchCollections("Author", searchValue));
                    break;
                default:
                    UI.PrintString("Wrong input");
                    break;
            }
        }
        private void PrintIBookshelfItems(List<IBookshelfItem> returnedList)
        {
            if (returnedList.Count != 0)
            {
                UI.PrintString("\nFound the following results:\n");
                foreach (IBookshelfItem item in returnedList)
                {
                    if (item.GetType() == typeof(Book))
                    {
                        Book bookItem = (Book)item;
                        UI.PrintString($"Book:\nTitle: {bookItem.Title}\nAuthor: {bookItem.Author}\nPages: {bookItem.Pages}\n");
                    }
                    else if (item.GetType() == typeof(Comic))
                    {
                        Comic comicItem = (Comic)item;
                        UI.PrintString($"Comic:\nTitle: {comicItem.Title}\nVol: {comicItem.Vol}\nAuthor: {comicItem.Author}\n");
                    }
                    else
                    {
                        UI.PrintString($"{item.Title}\n{item.Author}\n{item.GetType().Name}\n");
                    }
                }
            }
            else
            {
                UI.PrintString("No matching result found");
            }
        }

        private string CheckStringChoice()
        {
            string InputText = UI.GetStringInput();
            while (string.IsNullOrWhiteSpace(InputText))
            {
                UI.PrintString("Empty space is not valid input, please make new input.");
                InputText = UI.GetStringInput();
            }

            return InputText;
        }
        private int CheckIntInput(int NumberOfChoices)
        {
            int userInput = 0;
            bool ValidInput = false;
            while (ValidInput == false)
            {
                if (ValidInput = Int32.TryParse(UI.GetStringInput(), out userInput))
                {
                    if (userInput > NumberOfChoices || userInput < 1)
                    {
                        UI.PrintString("Incorrect input" + ((NumberOfChoices == 1) ? ".\nNew input:" : $", must be a number between 1 - {NumberOfChoices}\nNew input:")); //Om det bara finns 1 val att göra så får man ett mer talade svar.
                        ValidInput = false;
                    }
                }
                else UI.PrintString("Incorrect input\nNew input:");
            }
            return userInput;
        }
        private string GetQuote()
        {
            string markering = new string('-', 40);
            BookQuote? quote = apiClient.Read();
            return quote == null ? "Inget citat tillgängligt" : $"\n{markering}\n\"{quote.Text}\"\n- {quote.Author}\n{markering}\n";
        }
        private List<IBookshelfItem> SearchCollections(string searchField, string searchValue)
        {
            List<IBookshelfItem> returnList = new List<IBookshelfItem>();
            returnList.AddRange(comicDAO.Read(searchField, searchValue));
            returnList.AddRange(bookDAO.Read(searchField, searchValue));
            return returnList;
        }


    }
}
