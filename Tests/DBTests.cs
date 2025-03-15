using Xunit;
using Moq;
using System.Data;
using SCMS.Models;

public class DBTests
{
   [Fact]
   public void IsValidUser_WithCorrectCredentials_ReturnTrue()
   {
       var email = "test@mail.com";
       var password = "password";
       var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

       var mockConnection = new Mock<IDbConnection>();
       var mockCommand = new Mock<IDbCommand>();
       var mockParamCollection = new Mock<IDataParameterCollection>();

       mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(hashedPassword);
       mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParamCollection.Object);
       mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

       var dbModel = new DBModel();

       var result = dbModel.IsValidUser(email, password);

       Assert.True(result);
   }


   [Fact]
   public void IsValidUser_WithInvalidEmail_ReturnFalse()
   {
       var email = "wrong@mail.com";
       var password = "password";

       var mockConnection = new Mock<IDbConnection>();
       var mockCommand = new Mock<IDbCommand>();
       var mockParamCollection = new Mock<IDataParameterCollection>();

       mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(string.Empty);
       mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParamCollection.Object);
       mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

       var dbModel = new DBModel();

       var result = dbModel.IsValidUser(email, password);

       Assert.False(result);
   }


   [Fact]
   public void IsValidUser_WithInvalidPassword_ReturnFalse()
   {
       var email = "test@mail.com";
       var password = "wrongpassword";
       var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");

       var mockConnection = new Mock<IDbConnection>();
       var mockCommand = new Mock<IDbCommand>();
       var mockParamCollection = new Mock<IDataParameterCollection>();

       mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(hashedPassword);
       mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParamCollection.Object);
       mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

       var dbModel = new DBModel();

       var result = dbModel.IsValidUser(email, password);

       Assert.False(result);
   }

}
