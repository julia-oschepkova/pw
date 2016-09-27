using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Common.DbContext;
using Common.Model;
using NUnit.Framework;

namespace Tests
{
    public class TestConcurrentEditAccounts : TestBase
    {
        private readonly Guid accountId = Guid.NewGuid();
        private readonly Guid userId = Guid.NewGuid();

        public override void SetUp()
        {
            base.SetUp();
            //todo тестовый connection string
            using (var context = new DataContext())
            {
                context.Accounts.Add(new PwAccount() { Id = accountId, UserId = userId, Balance = 100 });
                context.SaveChanges();
            }

        }

        public override void TearDown()
        {
            base.TearDown();
            using (var context = new DataContext())
            {
                var resultAccount = context.Accounts.First(x => x.Id == accountId);

                context.Accounts.Remove(resultAccount);
                context.SaveChanges();
            }
        }

        [Test]
        public void TestAccountWasUpdatedAfterRead()
        {
            using (var context1 = new DataContext())
            {
                var account1 = context1.Accounts.First(x => x.Id == accountId);

                using (var context2 = new DataContext())
                {
                    var account2 = context2.Accounts.First(x => x.Id == accountId);
                    account2.Balance = 79;
                    context2.Entry(account2).State = EntityState.Modified;
                    context2.SaveChanges();
                }

                account1.Balance = 11111;
                account1.UserId = Guid.NewGuid();
                context1.Entry(account1).State = EntityState.Modified;

                Assert.Throws<DbUpdateConcurrencyException>(() => context1.SaveChanges());
            }
            using (var context = new DataContext())
            {
                var resultAccount = context.Accounts.First(x => x.Id == accountId);

                Assert.AreEqual(userId, resultAccount.UserId);
                Assert.AreEqual(79, resultAccount.Balance);
            }
        }
    }
}