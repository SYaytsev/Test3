using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace TestApp3
{
    /// <summary>
    /// Program - класс для тестирования Logger AddressBook.
    /// </summary>
    class Program
    {
        private static Logger log = Logger.Instance(Strategy.RecordToConsole);
        private static AddressBook addressBook = new AddressBook();

        /// <summary>
        /// Get users with today birthday, and provide data for mail sender
        /// </summary>
        /// <param name="state"></param>
        static void Notify(object state)
        {
            SendEmails(addressBook.GetUsersWithTodayBirthDay());
            log.Debug("SendEmails was invoked ");
        }

        /// <summary>
        /// Create messages and send
        /// </summary>
        /// <param name="users"></param>
        static void SendEmails(List<User> users)
        {
            SmtpClient Smtp = new SmtpClient("smtp.ххх.com", 25);
            Smtp.Credentials = new NetworkCredential("name@ххх.com", "pass");

            foreach (User item in users)
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("name@ххх.com");
                Message.To.Add(item.Email);
                Message.Subject = "Happy Birthday ))";
                Message.Body = "Hi, dear " + item.FirstName + " I'm very glad to congratulate .....";
                try
                {
                    Smtp.Send(Message);
                    log.Debug("Message with birthday congratulations was send to " + item.ToString());
                }
                catch (SmtpException)
                {
                    log.Error("Fail to send celebration emails");
                }
            }
        }

        static void Test1()
        {
            foreach (User item in addressBook.GetUsersByEmailDomen("gmail.com"))
            {
                log.Debug("1 method's result: " + item.ToString());
            }
            log.Info("1-st test done");
        }

        static void Test2()
        {
            foreach (User item in addressBook.GetUsersByAgeAndCity(18, "Kiev"))
            {
                log.Debug("2 method's result: " + item.ToString());
            }
            log.Info("2-nd test was done");
        }

        static void Test3()
        {
            foreach (User item in addressBook.GetUsersByGenderAndAddedPeriod(Gender.female, 10))
            {
                log.Debug("3 method's result: " + item.ToString());
            }
            log.Info("3-rd test was done");
        }

        static void Test4()
        {
            foreach (User item in addressBook.GetUsersByBornMonthOrdedByLastName(Months.Jan, SortOrder.descending))
            {
                log.Debug("4 method's result: " + item.ToString());
            }
            log.Info("4-th test was done");
        }

        static void Test5()
        {
            Dictionary<Gender, List<User>> dicUsers = addressBook.GetUsersSeparatedByGender();
            List<User> groupedUsers = new List<User>();
            dicUsers.TryGetValue(Gender.female, out groupedUsers);
            foreach (User item in groupedUsers)
            {
                log.Debug("5 method's result: " + item.ToString());
            }
            log.Info("5-th test was done");
        }
        static void Test6()
        {
            foreach (User item in addressBook.GetUsersByConditionFromTo(1, 1))
            {
                log.Debug("6 method's result: " + item.ToString());
            }
            log.Info("6-th test was done");
        }

        static void Test7()
        {
            log.Debug("7 method's result: " + addressBook.GetNumberUsersFromCityAndTodayBirthDay("Kiev"));
            log.Info("7-th test was done");
        }

        static void Main(string[] args)
        {
            //Test3
            addressBook.UserAdded += log.Info;
            addressBook.UserRemoved += log.Info;

            addressBook.FillBookForTest();
            log.Info("Fill data for tests");

            // 1-st test... по домену “gmail.com”
            Test1();
            // 2-nd test... старше 18 лет в Киеве
            Test2();
            // 3-rd tets... девушки добавленные последние 10 дней
            Test3();
            // 4-th test рожденные в январе, отсортированы по фамилии в обратном порядке
            Test4();
            // 5-th test группа женщин
            Test5();
            Test6();
            Test7();

            Console.ReadLine();

            TimerCallback timerCB = new TimerCallback(Notify);
            Timer timer = new Timer(timerCB, null, 0, 86400000);

            Console.ReadLine();

            log.Dispose();
        }
    }
}
