using GroceryCo.Domain;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCo.Tests
{
    [TestFixture]
    public class GroceryCoValidatorTests
    {
        [Test]
        public void ValidateSelectionTest()
        {
            int value = -1;
            Assert.IsTrue(GroceryCoValidator.ValidateSelection("0", 3, out value));
            Assert.IsTrue(GroceryCoValidator.ValidateSelection("2", 3, out value));

            Assert.IsFalse(GroceryCoValidator.ValidateSelection("3", 3, out value));
            Assert.IsFalse(GroceryCoValidator.ValidateSelection("-1", 3, out value));
            Assert.IsFalse(GroceryCoValidator.ValidateSelection("a", 3, out value));
            Assert.IsFalse(GroceryCoValidator.ValidateSelection("z", 3, out value));

            Assert.IsTrue(GroceryCoValidator.ValidateSelection("1", 3, out value));
            Assert.AreEqual(1, value);
        }

        [Test]
        public void ValidateDecimalTest()
        {
            decimal value = -1;
            Assert.IsTrue(GroceryCoValidator.ValidateDecimal("0", out value));
            Assert.IsTrue(GroceryCoValidator.ValidateDecimal("2", out value));
            Assert.IsTrue(GroceryCoValidator.ValidateDecimal("1.25", out value));
            Assert.IsTrue(GroceryCoValidator.ValidateDecimal("10.4", out value));

            Assert.IsFalse(GroceryCoValidator.ValidateDecimal("-1", out value));
            Assert.IsFalse(GroceryCoValidator.ValidateDecimal("a", out value));
            Assert.IsFalse(GroceryCoValidator.ValidateDecimal("abc", out value));

            Assert.IsTrue(GroceryCoValidator.ValidateDecimal("21.5", out value));
            Assert.AreEqual(21.5, value);
        }

        [Test]
        public void ValidatePercentTest()
        {
            int value = -1;
            Assert.IsTrue(GroceryCoValidator.ValidatePercent("0", out value));
            Assert.IsTrue(GroceryCoValidator.ValidatePercent("1", out value));
            Assert.IsTrue(GroceryCoValidator.ValidatePercent("50", out value));
            Assert.IsTrue(GroceryCoValidator.ValidatePercent("100", out value));

            Assert.IsFalse(GroceryCoValidator.ValidatePercent("-1", out value));
            Assert.IsFalse(GroceryCoValidator.ValidatePercent("a", out value));
            Assert.IsFalse(GroceryCoValidator.ValidatePercent("101", out value));
            Assert.IsFalse(GroceryCoValidator.ValidatePercent("10.5", out value));

            Assert.IsTrue(GroceryCoValidator.ValidatePercent("21", out value));
            Assert.AreEqual(21, value);
        }
    }
}
