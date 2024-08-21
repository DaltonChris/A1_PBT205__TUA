using Microsoft.VisualStudio.TestTools.UnitTesting;
using PBT_205_A1;

namespace PBT205ChatTests
{
    [TestClass]
    public class ChatLoginTests
    {
        [TestMethod]
        public void TestValidLogin()
        {
            // Arrange
            var loginForm = new ChatLogin();
            string testUsername = "testuser";
            string testPassword = "testpass";

            //bool result = ChatLogin.ValidateLogin(testUsername, testPassword);

            //Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestInvalidLogin_EmptyUsername()
        {
            // Arrange
            var loginForm = new ChatLogin();
            string testUsername = "";
            string testPassword = "testpass";

            //bool result = ChatLogin.ValidateLogin(testUsername, testPassword);

            //Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestInvalidLogin_EmptyPassword()
        {
            // Arrange
            var loginForm = new ChatLogin();
            string testUsername = "testuser";
            string testPassword = "";

            //bool result = ChatLogin.ValidateLogin(testUsername, testPassword);

            //Assert.IsFalse(result);
        }
    }
}
