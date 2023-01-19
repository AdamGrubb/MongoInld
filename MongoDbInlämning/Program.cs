
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbInlämning;
using MongoDbInlämning.BookshelfDAO;
using MongoDbInlämning.Interface;
using MongoDbInlämning.Models;
using MongoDbInlämning.QuotesAPI;
using System.Linq.Expressions;
using static System.Reflection.Metadata.BlobBuilder;

IUI ui;
IBookDAO bookDAO;
IComicDAO comicDAO;
IQuotesApiClient apiClient;


    ui = new KonsolTextUI();
    bookDAO = new MongoBookDAO(SecretClass.MongoDbConnectionString, "Bookshelf");
    comicDAO = new MongoComicDAO(SecretClass.MongoDbConnectionString, "Bookshelf");
    apiClient = new GOTQuotesAPI();
    BookShelfController bookShelfController = new BookShelfController(bookDAO, comicDAO, ui, apiClient);
    bookShelfController.StartUp();
