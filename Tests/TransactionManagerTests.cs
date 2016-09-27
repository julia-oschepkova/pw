using System;
using System.Collections.Generic;
using System.Linq;
using Common.DbContext;
using Common.Exceptions;
using Common.Managers;
using Common.Model;
using NUnit.Framework;
using PwTransferApp.ContainerConfig;

namespace Tests
{
    public class TransferManagerTests : TestBase
    {
        private TransferManager testee;
        private readonly Guid sourceId = Guid.NewGuid();
        private readonly Guid destId = Guid.NewGuid();

        public override void SetUp()
        {
            base.SetUp();
            testee = Container.Resolve<TransferManager>();
            WriteTestData();
        }

        private void WriteTestData()
        {
            using (var context = new DataContext())
            {
                context.Accounts.AddRange(new List<PwAccount>()
                {
                    new PwAccount() {Id = sourceId, UserId = sourceId, Balance = 500, IsDefault = true},
                    new PwAccount() {Id = destId, UserId = destId, Balance = 500, IsDefault = true},
                });
                context.SaveChanges();
            }
        }

        public override void TearDown()
        {
            base.TearDown();
            using (var context = new DataContext())
            {
                context.Accounts.ToList().ForEach(x => context.Accounts.Remove(x));
                context.Transactions.ToList().ForEach(x => context.Transactions.Remove(x));
                context.SaveChanges();
            }
        }

        [Test]
        public void TestNotEnoughtPwToMakeTransfer()
        {
            Assert.Throws<NotEnoughtPwForTransferException>(() => testee.Transfer(sourceId, destId, 1000, "desc"));

            using (var context = new DataContext())
            {
                var transfer = context.Transactions.First(
                    x => x.SourceAccountId == sourceId && x.DestinationAccountId == destId);
                Assert.AreEqual(TransferStatus.Failed, transfer.Status);

                var source = context.Accounts.First(x => x.Id == sourceId);
                var destination = context.Accounts.First(x => x.Id == destId);
                Assert.AreEqual(500, source.Balance);
                Assert.AreEqual(500, destination.Balance);
            }
        }

        [Test]
        public void TestSuccessfulTransfer()
        {
            var result = testee.Transfer(sourceId, destId, 100, "desc");

            using (var context = new DataContext())
            {
                var transfer = context.Transactions.First(x => x.Id == result.Item1.Id);
                Assert.AreEqual(TransferStatus.Successed, transfer.Status);

                var source = context.Accounts.First(x => x.Id == sourceId);
                var destination = context.Accounts.First(x => x.Id == destId);
                Assert.AreEqual(400, source.Balance);
                Assert.AreEqual(600, destination.Balance);
            }
        }
    }
}