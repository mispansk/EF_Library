using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF_Library.Repositories
{
    public class BookRepository
    {
        /// <summary>
        /// Все книги
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IEnumerable<Book> FindAll(AppContext db)
        {
            var allBooks = db.Books.ToList();
            return allBooks;
        }
        
        /// <summary>
        /// поиск книги по ID
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindId(AppContext db, int id)
        {
            var IdBook = db.Books.FirstOrDefault(u => u.Id == id);
            return IdBook.Name;
        }
        
        /// <summary>
        /// добавление книги
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Book AddBook(AppContext db, string name, string autor, int year, string genre)
        {
            var bookNew = new Book { Name = name, Author = autor, Year = year, Genre = genre };
            db.Books.Add(bookNew);
            db.SaveChanges();
            return bookNew;
        }
        
        /// <summary>
        /// удаление книги
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        public void DeliteId(AppContext db, int id)
        {
            var IdBook = db.Books.FirstOrDefault(u => u.Id == id);
            Console.WriteLine("\nУдаление книги {0}", id);
            if (IdBook != null)
            {
                db.Books.Remove(IdBook);
                db.SaveChanges();
                Console.WriteLine("Удаление прошло успешно");
            }
            else
                Console.WriteLine("Такой книги нет в БД");
        }
        
        /// <summary>
        /// обновление года выпуска книги
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public void UpdateYear(AppContext db, int id, int year)
        {
            var IdBook = db.Books.FirstOrDefault(u => u.Id == id);
            Console.WriteLine("\nГод {0} успешно изменен на {1}", IdBook.Year, year);
            IdBook.Year = year;
            db.SaveChanges();
        }

        /// <summary>
        /// список книг определенного жанра и вышедших между определенными годами
        /// </summary>
        /// <param name="db"></param>
        /// <param name="ganre"></param>
        /// <param name="year1"></param>
        /// <param name="year2"></param>
        /// <returns></returns>
        public IEnumerable<Book> FindBook(AppContext db, string ganre, int year1, int year2)
        {
            var allBooks = db.Books.Where(u => u.Genre == ganre && u.Year >= year1 && u.Year <= year2).ToList();
            return allBooks;
        }

        /// <summary>
        /// количество книг определенного автора в библиотеке
        /// </summary>
        /// <param name="db"></param>
        /// <param name="autor"></param>
        /// <returns></returns>
        public int BookByAutor(AppContext db, string autor)
        {
            var kol = db.Books.Where(u => u.Author == autor).Count();
            return kol;
        }

        /// <summary>
        /// количество книг определенного жанра в библиотеке
        /// </summary>
        /// <param name="db"></param>
        /// <param name="genre"></param>
        /// <returns></returns>
        public int BookByGenre(AppContext db, string genre)
        {
            var kol = db.Books.Where(u => u.Genre == genre).Count();
            return kol;
        }

        /// <summary>
        /// булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке
        /// </summary>
        /// <param name="db"></param>
        /// <param name="autor"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool BookByAutorByName(AppContext db, string autor, string name)
        {
            var kol = db.Books.Any(u => u.Author == autor && u.Name == name);
            return kol;
        }
 
        /// <summary>
        /// есть ли определенная книга на руках у пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name"></param>
        public void BookbyName(AppContext db, string name)
        {
            if (db.Books.Where(a => a.Name == name).Count() != 0)
            {
                var kol = db.Books.Where(u => u.Name == name).Any(c => c.Users.Count != 0);
                if (kol)
                    Console.WriteLine("\nКнига {0} в данный момент у пользователя", name);
                else
                    Console.WriteLine("\nКнига {0} свободна", name);
            }
            else
                Console.WriteLine("\nКниги {0} нет в библиотеке", name);                   
        }
        
        /// <summary>
        /// последняя вышедшая книга
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public Book BookByYear(AppContext db)
        {
            var Book = db.Books.OrderByDescending(u => u.Year).First();
            return Book;
        }

        /// <summary>
        /// список книг, отсортироанных в алфавитном порядке по наименованию
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IEnumerable<Book> AllBook(AppContext db)
        {
            var allBooks = db.Books.OrderBy(u => u.Name).ToList();
            return allBooks;
        }

        /// <summary>
        /// списк всех книг, отсортированного в порядке убывания года их выхода
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IEnumerable<Book> AllBookYear(AppContext db)
        {
            var allBooks = db.Books.OrderByDescending(u => u.Year).ToList();
            return allBooks;
        }
    }
}
