using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp3
{
    enum Months { Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
    enum SortOrder { ascending, descending };
    /// <summary>
    /// AddressBook - класс, который хранит список юзеров (User).
    /// Список пользователей не доступен вне класса.
    /// AddressBook должен иметь методы AddUser, RemoveUser и события UserAdded, UserRemoved.
    /// </summary>
    class AddressBook
    {
        private List<User> users;

        public AddressBook()
        {
            this.users = new List<User>();
        }

        public void FillBookForTest()
        {
            User user1 = new User("Fedorov", "Ivan", new DateTime(1990, 1, 13), "Kiev", "Kurskay 13D", "+3809325874365", Gender.male, "fedor@mail.ru");
            User user2 = new User("Iovcheva", "Irina", new DateTime(2002, 7, 3), "Kiev", "Dobrovolskogo str., 5", "+380678564218", Gender.female, "iovcheva@gmail.com");
            User user3 = new User("Antonov", "Oleg", new DateTime(1985, 5, 30), "Kiev", "Kurskay 1", "+3809376874365", Gender.male, "fedor@mail.ru");
            User user4 = new User("Kirilova", "Anastasia", new DateTime(1931, 6, 3), "Lvov", "Upa str., 5", "+380674564218", Gender.female, "iofadsfsafva@gmail.com");
            User user5 = new User("Hymenuk", "Tanya", new DateTime(2001, 4, 20), "Kiev", "Kurskay 13D", "+3809325873265", Gender.female, "fedor@mail.ru");
            User user6 = new User("Shashkov", "Sergey", new DateTime(1999, 1, 3), "Odessa", "Derebasovskay str., 13", "+38067852328", Gender.male, "iofadsfsafva@gmail.com");

            AddUser(user1);
            AddUser(user2);
            AddUser(user3);
            AddUser(user4);
            AddUser(user5);
            AddUser(user6);
        }

        public void AddUser(User in_user)
        {
            users.Add(in_user);
            UserAdded("User " + in_user.ToString() + " - was added to address book");
        }
        public void RemoveUser(User in_user)
        {
            users.Remove(in_user);
            UserRemoved("User " + in_user.ToString() + " - was removed from address book");
        }
        public delegate void EventHandler(string str);

        public event EventHandler UserAdded;
        public event EventHandler UserRemoved;

        /// <summary>
        /// Метод (используя LINQ - method syntax) выбирает: 
        /// пользователей, у которых Email-адрес имеет заданный домен, 
        /// н/р “gmail.com”
        /// </summary>
        /// <param name="emailDomen"></param>
        /// <returns>IEnumerable или List</returns>
        public List<User> GetUsersByEmailDomen(string emailDomen)
        {
            return this.users.Where(user =>
                user.Email.Contains(emailDomen) == true)
                .ToList<User>();
        }

        /// <summary>
        /// Метод (метод расширения, используя yield) выбирает: 
        /// пользователей, старше определенного возрасти и из заданого города, 
        /// н/р старше 18 и из Киева
        /// <param name="minAge"></param>
        /// <param name="city"></param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<User> GetUsersByAgeAndCity(ushort minAge, string city)
        {
            foreach (User user in users)
            {
                if (String.Compare(user.City, city) == 0
                    & user.Birthdate.CompareTo(DateTime.Now.AddYears(-minAge)) < 0)
                {
                    yield return user;
                }
            }
        }

        /// <summary>
        /// Метод (используя LINQ - query syntax) выбирает: 
        /// пользователей по полу, которые были добавлены за заданное кол-во дней, 
        /// н/р девушки, добавленные за последние 10 дней
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="days"></param>
        /// <returns>IEnumerable или List</returns>
        public List<User> GetUsersByGenderAndAddedPeriod(Gender gender, ushort days)
        {
            return (from user in this.users
                    where user.Gender == gender &
                          user.TimeAdded.CompareTo(DateTime.Now.AddDays(-days)) > 0
                    select user)
                      .ToList();
        }

        /// <summary>
        /// Метод (используя LINQ - method syntax) выбирает: 
        /// пользователей, которые родились в заданом месяце, и при этом имеют заполненые поля адреса и - телефона. 
        /// Список пользователей сотритуется по фамилии исходя из заданого условия.
        /// н/р который родились в январе, с заполнеными полями адреса и телефона и отсортированые по фамилии в обратном порядке
        /// </summary>
        /// <param name="month"></param>
        /// <param name="order"></param>
        /// <returns>IEnumerable или List</returns>
        public IEnumerable<User> /* List<User>*/ GetUsersByBornMonthOrdedByLastName(Months month, SortOrder order)
        {
            if (SortOrder.ascending == order)
            {
                return this.users.Where(user =>
                    user.Birthdate.Month == (int)month
                    & !String.IsNullOrEmpty(user.Address)
                    & !user.Address.Contains("unknown")
                    & !String.IsNullOrEmpty(user.PhoneNumber)
                    & !user.PhoneNumber.Contains("unknown"))
                    .OrderBy(user => user.LastName)
                    /*.ToList<User>()*/;
            }
            else
            {
                return this.users.Where(user =>
                    user.Birthdate.Month == (int)month
                    & !String.IsNullOrEmpty(user.Address)
                    & !user.Address.Contains("unknown")
                    & !String.IsNullOrEmpty(user.PhoneNumber)
                    & !user.PhoneNumber.Contains("unknown"))
                    .OrderByDescending(user => user.LastName)
                    /*.ToList<User>()*/;
            }
        }

        /// <summary>
        /// Метод (используя LINQ - method syntax) возвращает: 
        /// словарь, имеющий два ключа “man” и “woman” и “unknown”
        /// По каждому из ключей словарь должен содержать список пользователей, которые соответствуют ключу словаря;
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<Gender, List<User>> GetUsersSeparatedByGender()
        {
            return users.GroupBy(user => user.Gender).ToDictionary(user => user.Key, user => user.ToList());
        }

        /// <summary>
        /// Метод (используя LINQ - method syntax) возвращает: 
        /// пользователей, передавая произвольное условие (лямбда - выражение) и два параметра - с какого элемента выбирать и по какой (paging).
        /// </summary>
        /// <param name="from"></param>
        /// <param name="till"></param>
        /// <returns>IEnumerable или List</returns>
        public List<User> GetUsersByConditionFromTo(int from, int till)
        {
            return users.Where(user => user.Gender == Gender.male).Skip(from).Take(till).ToList<User>();
        }

        /// <summary>
        /// Метод (используя LINQ - query syntax) возвращает: 
        /// количество пользователей, из заданого города, у которых сегодня день рождения.
        /// </summary>
        /// <param name="city"></param>
        /// <returns>int</returns>
        public int GetNumberUsersFromCityAndTodayBirthDay(string city)
        {
            return (from user in this.users
                    where String.Compare(user.City, city) == 0
                        & user.Birthdate.Day == DateTime.Today.Day
                        & user.Birthdate.Month == DateTime.Today.Month
                    select user).Count();
        }

        /// <summary>
        /// Метод (используя LINQ - method syntax) возвращает: 
        /// пользователей у которых сегодня день рождения
        /// </summary>
        /// <returns>IEnumerable или List</returns>
        public List<User> GetUsersWithTodayBirthDay()
        {
            return users.Where(user => user.Birthdate.Day == DateTime.Today.Day
                        & user.Birthdate.Month == DateTime.Today.Month).ToList<User>();
        }
    }

}
