using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ПП._2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Login_CorrectDirectorCredentials_ReturnsDirectorRole()
        {
            var service = new AuthService();
            string result = service.Login("admin", "123");

            Assert.AreEqual("директор", result);
        }
        [TestMethod]
        public void Login_CorrectManagerCredentials_ReturnsManagerRole()
        {
            var service = new AuthService();
            string result = service.Login("manager", "111");

            Assert.AreEqual("менеджер", result);
        }
        [TestMethod]
        public void Register_NewUser_ReturnsTrue()
        {
            var service = new AuthService();
            bool result = service.Register("newuser", "pass", "a@mail.com");

            Assert.IsTrue(result);
        }
        [TestMethod]
        public void Login_WrongPassword_ReturnsNull()
        {
            var service = new AuthService();
            string result = service.Login("admin", "wrong");

            Assert.IsNull(result);
        }
        [TestMethod]
        public void Register_EmptyFields_ReturnsFalse()
        {
            var service = new AuthService();
            bool result = service.Register("", "", "");

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void Register_ExistingLogin_ReturnsFalse()
        {
            var service = new AuthService();
            bool result = service.Register("admin", "222", "new@mail.com");

            Assert.IsFalse(result);
        }
    }
}
