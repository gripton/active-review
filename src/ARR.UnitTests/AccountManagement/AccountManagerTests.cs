using System;
using ARR.AccountManagement;
using ARR.AccountManagement.Exceptions;
using ARR.Data.Entities;
using ARR.Repository;

using Moq;
using PracticalCode.WebSecurity.Infrastructure.Membership;
using PracticalCode.WebSecurity.Infrastructure.Policies.Exceptions;
using Raven.Client;
using Xunit;

namespace ARR.UnitTests.ReviewManagement
{
    public class AccountManagerTests : BaseUnitTest
    {
        [Fact]
        public void CreateNew_Succeeds()
        {
            var mockSession = new Mock<IDocumentSession>();
            var accountRepo = new Mock<AbstractRepository<Account>>(mockSession.Object);
            var passwordManager = new Mock<IPasswordManager>();

            passwordManager
                .Setup(p => p.EncryptPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Func<string, string, string>>()))
                .Returns("encrypted");

            // Create a new session to work with
            var account = NewAccount();
            Account nullAccount = null;

            // setup mocks
            accountRepo.Setup(a => a.GetByName(It.IsAny<string>())).Returns(nullAccount);

            var manager = new AccountManager(accountRepo.Object, passwordManager.Object);
            Assert.DoesNotThrow(() => manager.CreateNew(account));
        }

        [Fact]
        public void CreateNew_Fails_AlreadyExists()
        {
            var mockSession = new Mock<IDocumentSession>();
            var accountRepo = new Mock<AbstractRepository<Account>>(mockSession.Object);
            var passwordManager = new Mock<IPasswordManager>();

            // Create a new session to work with
            var account = NewAccount();

            // setup mocks
            accountRepo.Setup(a => a.GetByName(It.IsAny<string>())).Returns(account);
            
            var manager = new AccountManager(accountRepo.Object, passwordManager.Object);

            Assert.Throws<UserAlreadyExistsException>(() => manager.CreateNew(account));
        }

        [Fact]
        public void CreateNew_Fails_InvalidUserName()
        {
            var mockSession = new Mock<IDocumentSession>();
            var accountRepo = new Mock<AbstractRepository<Account>>(mockSession.Object);
            var passwordManager = new Mock<IPasswordManager>();
            
            // Create a new session to work with
            var account = NewAccount();
            account.Username = "FAIL!";
            Account nullAccount = null;

            // setup mocks
            accountRepo.Setup(a => a.GetByName(It.IsAny<string>())).Returns(nullAccount);

            var manager = new AccountManager(accountRepo.Object, passwordManager.Object);

            Assert.Throws<InvalidUsernameException>(() => manager.CreateNew(account));
        }

        [Fact]
        public void CreateNew_Fails_InvalidEmail()
        {
            var mockSession = new Mock<IDocumentSession>();
            var accountRepo = new Mock<AbstractRepository<Account>>(mockSession.Object);
            var passwordManager = new Mock<IPasswordManager>();

            // Create a new session to work with
            var account = NewAccount();
            account.EmailAddress = "FAIL!";
            Account nullAccount = null;

            // setup mocks
            accountRepo.Setup(a => a.GetByName(It.IsAny<string>())).Returns(nullAccount);

            var manager = new AccountManager(accountRepo.Object, passwordManager.Object);

            Assert.Throws<InvalidEmailAddressException>(() => manager.CreateNew(account));
        }
    }
}