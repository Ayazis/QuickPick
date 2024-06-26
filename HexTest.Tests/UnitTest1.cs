using Newtonsoft.Json;
using System.Security.Cryptography;
using Hexgrid;
using FluentAssertions;

namespace HexTest.Tests
{
    public class Tests
    {
            
        string _61Hexes = """[{"Column":0,"Row":0,"X":0,"Y":0},{"Column":1,"Row":-1,"X":1,"Y":0},{ "Column":1,"Row":0,"X":1,"Y":0},{ "Column":0,"Row":1,"X":0,"Y":1},{ "Column":-1,"Row":1,"X":-1,"Y":0},{ "Column":-1,"Row":0,"X":-1,"Y":0},{ "Column":0,"Row":-1,"X":0,"Y":-1},{ "Column":1,"Row":-2,"X":1,"Y":-2},{ "Column":2,"Row":-2,"X":3,"Y":-1},{ "Column":2,"Row":-1,"X":3,"Y":0},{ "Column":2,"Row":0,"X":3,"Y":1},{ "Column":1,"Row":1,"X":1,"Y":2},{ "Column":0,"Row":2,"X":0,"Y":3},{ "Column":-1,"Row":2,"X":-1,"Y":2},{ "Column":-2,"Row":2,"X":-3,"Y":1},{ "Column":-2,"Row":1,"X":-3,"Y":0},{ "Column":-2,"Row":0,"X":-3,"Y":-1},{ "Column":-1,"Row":-1,"X":-1,"Y":-2},{ "Column":0,"Row":-2,"X":0,"Y":-3},{ "Column":1,"Row":-3,"X":1,"Y":-4},{ "Column":2,"Row":-3,"X":3,"Y":-3},{ "Column":3,"Row":-3,"X":4,"Y":-2},{ "Column":3,"Row":-2,"X":4,"Y":0},{ "Column":3,"Row":-1,"X":4,"Y":0},{ "Column":3,"Row":0,"X":4,"Y":2},{ "Column":2,"Row":1,"X":3,"Y":3},{ "Column":1,"Row":2,"X":1,"Y":4},{ "Column":0,"Row":3,"X":0,"Y":5},{ "Column":-1,"Row":3,"X":-1,"Y":4},{ "Column":-2,"Row":3,"X":-3,"Y":3},{ "Column":-3,"Row":3,"X":-4,"Y":2},{ "Column":-3,"Row":2,"X":-4,"Y":0},{ "Column":-3,"Row":1,"X":-4,"Y":0},{ "Column":-3,"Row":0,"X":-4,"Y":-2},{ "Column":-2,"Row":-1,"X":-3,"Y":-3},{ "Column":-1,"Row":-2,"X":-1,"Y":-4},{ "Column":0,"Row":-3,"X":0,"Y":-5},{ "Column":1,"Row":-4,"X":1,"Y":-6},{ "Column":2,"Row":-4,"X":3,"Y":-5},{ "Column":3,"Row":-4,"X":4,"Y":-4},{ "Column":4,"Row":-4,"X":6,"Y":-3},{ "Column":4,"Row":-3,"X":6,"Y":-1},{ "Column":4,"Row":-2,"X":6,"Y":0},{ "Column":4,"Row":-1,"X":6,"Y":1},{ "Column":4,"Row":0,"X":6,"Y":3},{ "Column":3,"Row":1,"X":4,"Y":4},{ "Column":2,"Row":2,"X":3,"Y":5},{ "Column":1,"Row":3,"X":1,"Y":6},{ "Column":0,"Row":4,"X":0,"Y":6},{ "Column":-1,"Row":4,"X":-1,"Y":6},{ "Column":-2,"Row":4,"X":-3,"Y":5},{ "Column":-3,"Row":4,"X":-4,"Y":4},{ "Column":-4,"Row":4,"X":-6,"Y":3},{ "Column":-4,"Row":3,"X":-6,"Y":1},{ "Column":-4,"Row":2,"X":-6,"Y":0},{ "Column":-4,"Row":1,"X":-6,"Y":-1},{ "Column":-4,"Row":0,"X":-6,"Y":-3},{ "Column":-3,"Row":-1,"X":-4,"Y":-4},{ "Column":-2,"Row":-2,"X":-3,"Y":-5},{ "Column":-1,"Row":-3,"X":-1,"Y":-6},{ "Column":0,"Row":-4,"X":0,"Y":-6}]""";
        
        [SetUp]
        public void Setup()
        {
        }



        [Test]
        public void HexPositionsCalculator_Given61_Creates_61_Correct_HexPoints()
        {
            var expectedResult = JsonConvert.DeserializeObject<List<HexPoint>>(_61Hexes);
            var actualResult = new HexPositionsCalculator().GenerateHexagonalGridFixed(61);
            CollectionAssert.AreEqual(expectedResult, actualResult );
        }


        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        [TestCase(5, 1)]
        [TestCase(6, 1)]
        [TestCase(7, 2)]
        [TestCase(8, 2)]
        [TestCase(9, 2)]
        [TestCase(10, 2)]
        [TestCase(11, 2)]
        [TestCase(12, 2)]
        [TestCase(13, 2)]
        [TestCase(14, 2)]
        [TestCase(15, 2)]
        [TestCase(16, 2)]
        [TestCase(17, 2)]
        [TestCase(18, 2)]
        [TestCase(19, 3)]
        [TestCase(20, 3)]
        [TestCase(21, 3)]
        [TestCase(22, 3)]
        [TestCase(23, 3)]
        [TestCase(24, 3)]
        [TestCase(25, 3)]
        [TestCase(26, 3)]
        [TestCase(27, 3)]
        [TestCase(28, 3)]
        [TestCase(29, 3)]
        [TestCase(30, 3)]
        [TestCase(31, 3)]
        [TestCase(32, 3)]
        [TestCase(33, 3)]
        [TestCase(34, 3)]
        [TestCase(35, 3)]
        [TestCase(36, 3)]
        [TestCase(37, 4)]
        [TestCase(38, 4)]
        public void Calculate_RingNumber_ReturnsCorrectValue(int hexNumber, int expectedResult)
        {
            var calculator = new HexPositionsCalculator();
            int actualResult = calculator.CalculateRingNumber(hexNumber);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(1, 1, 0)]
        [TestCase(2, 1, 1)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 1, 3)]
        [TestCase(5, 1, 4)]
        [TestCase(6, 1, 5)]


        [TestCase(7, 2, 0)]
        [TestCase(8, 2, 0)]

        [TestCase(9, 2, 1)]
        [TestCase(10, 2, 1)]

        [TestCase(11, 2, 2)]
        [TestCase(12, 2, 2)]

        [TestCase(13, 2, 3)]
        [TestCase(14, 2, 3)]

        [TestCase(15, 2, 4)]
        [TestCase(16, 2, 4)]

        [TestCase(17, 2, 5)]
        [TestCase(18, 2, 5)]

        [TestCase(19, 3, 0)]
        [TestCase(20, 3, 0)]
        [TestCase(21, 3, 0)]

        [TestCase(22, 3, 1)]
        [TestCase(23, 3, 1)]
        [TestCase(24, 3, 1)]

        [TestCase(25, 3, 2)]
        [TestCase(26, 3, 2)]
        [TestCase(27, 3, 2)]

        [TestCase(28, 3, 3)]
        [TestCase(29, 3, 3)]
        [TestCase(30, 3, 3)]

        [TestCase(31, 3, 4)]
        [TestCase(32, 3, 4)]
        [TestCase(33, 3, 4)]

        [TestCase(34, 3, 5)]
        [TestCase(35, 3, 5)]
        [TestCase(36, 3, 5)]

        [TestCase(37, 4, 0)]
        [TestCase(38, 4, 0)]
        public void Calculate_Side_Index_ReturnsCorrectValue(int hexNumber, int ringNumber, int expectedResult)
        {
            var calculator = new HexPositionsCalculator();
            int actualResult = calculator.GetSideIndex(ringNumber, hexNumber);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}