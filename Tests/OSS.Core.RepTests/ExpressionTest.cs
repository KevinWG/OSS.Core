using System;
using System.Linq.Expressions;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.RepDapper.OrmExtention;
using Xunit;

namespace OSS.Core.RepTests
{
    public class ExpressionTest
    {
        [Fact]
        public void Test()
        {
            var sqlFlag = new SqlVistorFlag(SqlVistorType.InsertOrUpdate);
            var vistor = new SqlExpression();

            Expression<Func<UserInfoMo, object>> funExpression =
                u => new {name = u.nick_name, nick_name = "s" + u.nick_name, email = u.email};
            vistor.Visit(funExpression, sqlFlag);


        }
    }
}
