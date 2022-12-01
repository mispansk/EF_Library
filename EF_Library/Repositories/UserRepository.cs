using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF_Library.Repositories
{
    public class UserRepository
    {
        /// <summary>
        /// все пользователи
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public void FindAll(AppContext db)
        {
            Console.WriteLine("Пользователи библиотеки:");
            var allUsers = db.Users.ToList();
            foreach (User u in allUsers)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Email}");
            }
        }

        /// <summary>
        /// поиск пользователя по ID
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FindId(AppContext db, int id)
        {
            var IdUser = db.Users.FirstOrDefault(user => user.Id == id);
            return IdUser.Name;
        }

        /// <summary>
        /// добавление пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public User AddUser(AppContext db, string name, string email)
        {
            var userNew = new User { Name = name, Email = email };
            db.Users.Add(userNew);
            db.SaveChanges();
            return userNew;
        }

        /// <summary>
        /// удаление пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        public void DeliteId(AppContext db, int id)
        {
            var IdUser = db.Users.FirstOrDefault(user => user.Id == id);
            Console.WriteLine("\nУдаление пользователя {0}", id);
            if (IdUser != null)
            {
                db.Users.Remove(IdUser);
                db.SaveChanges();
                Console.WriteLine("Удаление прошло успешно");
            }
            else
                Console.WriteLine("\nТакого пользователя нет в БД");         
        }

        /// <summary>
        /// изменение имени пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public void UpdateName(AppContext db, int id, string name)
        {
            var IdUser = db.Users.FirstOrDefault(user => user.Id == id);
            Console.WriteLine("\nИмя {0} успешно изменено на {1}", IdUser.Name, name);
            IdUser.Name = name;
            db.SaveChanges();
        }

        /// <summary>
        /// количество книг на руках у пользователя
        /// </summary>
        /// <param name="db"></param>
        /// <param name="user"></param>
        public void UserBook(AppContext db, string user)
        {
            var courses = db.Users.Where(u => u.Name == user).Include(c => c.Books).ToList();
            foreach (var c in courses)
            {
                Console.WriteLine($"\nУ пользователя: {c.Name} {c.Books.Count()} книг:");
                foreach (Book s in c.Books)
                    Console.WriteLine(s.Name);
            }
            if (courses.Count() == 0)
                Console.WriteLine("\nПользователя {0} не существует", user);
        }
    }
}
