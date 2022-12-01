using EF_Library.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EF_Library
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создаем контекст для добавления данных
            using (var db = new AppContext())
            {
                // Пересоздаем базу
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var user1 = new User { Name = "Ivanov", Email = "Ivanov@mail.ru" };
                var user2 = new User { Name = "Petrov", Email = "Petrov@mail.ru" };
                var user3 = new User { Name = "Sidorov", Email = "Sidorov@mail.ru" };

                var book1 = new Book { Name = "Book1", Year = 1990, Genre = "Genre1", Author = "Autor1" };
                var book2 = new Book { Name = "Book2", Year = 1991, Genre = "Genre2", Author = "Autor1" };
                var book3 = new Book { Name = "Book3", Year = 1992, Genre = "Genre1", Author = "Autor2" };
                var book4 = new Book { Name = "Book4", Year = 1993, Genre = "Genre2", Author = "Autor1" };
                var book5 = new Book { Name = "Book5", Year = 1994, Genre = "Genre2", Author = "Autor2" };
                var book6 = new Book { Name = "Book6", Year = 1995, Genre = "Genre1", Author = "Autor3" };

                db.Users.AddRange(user1, user2, user3);
                db.Books.AddRange(book1, book2, book3, book4, book5, book6);

                user1.Books.Add(book1);
                user1.Books.Add(book2);
                user2.Books.AddRange(new[] { book3, book4});
                user3.Books.Add(book6);

                db.SaveChanges();
            }

            // Создаем контекст для выбора данных (вывод происходит либо в рипозитории, либо тут (для примера))
            using (var db = new AppContext())
            {
                // выбор всех объектов (пользователи)  (пользователи выводятся в репозитории)            
                var allUsers = new UserRepository();
                allUsers.FindAll(db);
                
                // выбор всех объектов (книги) (книги выводятся тут)
                var allBooks = new BookRepository();
                Console.WriteLine("\nВсе книги библиотеки:");
                var resB = allBooks.FindAll(db);
                foreach (Book u in resB)
                {
                    Console.WriteLine($"{u.Id}.{u.Name}");
                }

                // выбор объекта из БД по его идентификатору (например 2 пользователь)
                int i = 2;
                var resu = allUsers.FindId(db, i);
                Console.WriteLine("\nПод номером {0} находится пользователь {1}", i, resu);

                // выбор объекта из БД по его идентификатору (например 4 книга)
                int ii = 4;
                var resb = allBooks.FindId(db, ii);
                Console.WriteLine("\nПод номером {0} находится книга {1}", ii, resb);

                // добавление объекта в БД (user 4)
                var newUser = allUsers.AddUser(db, "Popov", "Popov@mail.ru");
                Console.WriteLine("\nПользователь {0}. {1} - {2} успешно создан", newUser.Id, newUser.Name, newUser.Email);

                // добавление объекта в БД (book 7)
                var newBook = allBooks.AddBook(db, "Book7", "Autor7", 1995, "Genre1");
                Console.WriteLine("\nКнига {0}. {1} - {2} успешно создана", newBook.Id, newBook.Name, newBook.Author);

                // удаление из БД пользователя (удалим 3 пользователя)
                allUsers.DeliteId(db, 3);

                // удаление из БД книги (удалим 6 книгу)
                allBooks.DeliteId(db, 6);

                // обновление имени пользователя(по Id)
                allUsers.UpdateName(db, 1, "Иваницкий");

                // обновление года выпуска книги (по Id)
                allBooks.UpdateYear(db, 1, 1998);

                // список книг определенного жанра и вышедших между определенными годами
                string ganre = "Genre1";
                int year1 = 1990;
                int year2 = 1998;
                Console.WriteLine("\nНайденные книги с фильтром жанр = {0}, год с {1} по {2}:", ganre, year1, year2);
                var Book = allBooks.FindBook(db, ganre, year1, year2);
                foreach (Book b in Book)
                {
                    Console.WriteLine($"{b.Id}.{b.Name} - {b.Year} - {b.Genre}");
                }

                //количество книг определенного автора в библиотеке
                string autor = "Autor1";
                int kolA = allBooks.BookByAutor(db, autor);
                Console.WriteLine("\n{0} - количество книг автора {1}", kolA, autor);

                //количество книг определенного жанра в библиотеке
                string ganre1 = "Genre1";
                int kolG = allBooks.BookByGenre(db, ganre1);
                Console.WriteLine("\n{0} - количество книг жанра {1}", kolG, ganre1);

                //булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке
                string author = "Autor1";
                string name = "Book1";
                var res = allBooks.BookByAutorByName(db, author, name);
                if (res)
                    Console.WriteLine("\nКнига под названием {0} автора {1} существует в библиотеке", name, author);
                else
                    Console.WriteLine("\nКниги под названием {0} автора {1} не существует в библиотеке", name, author);

                //булевый флаг о том, есть ли определенная книга на руках у пользователя
                string findname = "Book4";
                allBooks.BookbyName(db, findname);

                //количество книг на руках у пользователя
                var user = "Popov1";
                allUsers.UserBook(db, user);
                
                //последняя вышедшая книга
                var bookNew = allBooks.BookByYear(db);
                Console.WriteLine("\nПоследняя вышедшая книга - {0} {1} года", bookNew.Name, bookNew.Year);

                //список всех книг, отсортированных в алфавитном порядке по названию
                Console.WriteLine("\nСписок книг отсортированных в алфавитном порядке по названию: ");
                var BookN = allBooks.AllBook(db);
                foreach (Book b in BookN)
                {
                    Console.WriteLine($"{b.Id}.{b.Name}");
                }

                //список всех книг, отсортированных в порядке убывания года их выхода              
                Console.WriteLine("\nСписок книг отсортированный в порядке убыания года их выхода: ");
                var BookY = allBooks.AllBookYear(db);
                foreach (Book b in BookY)
                {
                    Console.WriteLine($"{b.Id}.{b.Name} - {b.Year}");
                }
            }

        }
    }
}
