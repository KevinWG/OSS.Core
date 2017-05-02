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

            var vistor = new SqlExpression();

            //var sqlUpdateFlag = new SqlVistorFlag(SqlVistorType.InsertOrUpdate);
            //Expression<Func<UserInfoMo, object>> funExpression =
            //    u => new {name = u.nick_name, nick_name = "s" + u.nick_name, email = u.email};
            //vistor.Visit(funExpression, sqlUpdateFlag);

            var sqlWhereFlag = new SqlVistorFlag(SqlVistorType.Where);
            Expression<Func<UserInfoMo, bool>> booExpression =
               u => u.email=="test"&& 1!=2 && false;


            vistor.Visit(booExpression, sqlWhereFlag);


        }
    }
}
