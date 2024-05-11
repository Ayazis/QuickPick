using Newtonsoft.Json;
using System.Security.Cryptography;
using Hexgrid;
using FluentAssertions;

namespace HexTest.Tests
{
    public class Tests
    {
        const string result37AsString = @"[{""Column"":0,""Row"":0,""X"":0,""Y"":0},{""Column"":0,""Row"":-1,""X"":0,""Y"":-1},{""Column"":1,""Row"":-1,""X"":1,""Y"":0},{""Column"":1,""Row"":0,""X"":1,""Y"":0},{""Column"":0,""Row"":1,""X"":0,""Y"":1},{""Column"":-1,""Row"":1,""X"":-1,""Y"":0},{""Column"":-1,""Row"":0,""X"":-1,""Y"":0},{""Column"":0,""Row"":-2,""X"":0,""Y"":-3},{""Column"":1,""Row"":-2,""X"":1,""Y"":-2},{""Column"":2,""Row"":-2,""X"":3,""Y"":-1},{""Column"":2,""Row"":-1,""X"":3,""Y"":0},{""Column"":2,""Row"":0,""X"":3,""Y"":1},{""Column"":1,""Row"":1,""X"":1,""Y"":2},{""Column"":0,""Row"":2,""X"":0,""Y"":3},{""Column"":-1,""Row"":2,""X"":-1,""Y"":2},{""Column"":-2,""Row"":2,""X"":-3,""Y"":1},{""Column"":-2,""Row"":1,""X"":-3,""Y"":0},{""Column"":-2,""Row"":0,""X"":-3,""Y"":-1},{""Column"":-1,""Row"":-1,""X"":-1,""Y"":-2},{""Column"":0,""Row"":-3,""X"":0,""Y"":-5},{""Column"":1,""Row"":-3,""X"":1,""Y"":-4},{""Column"":2,""Row"":-3,""X"":3,""Y"":-3},{""Column"":3,""Row"":-3,""X"":4,""Y"":-2},{""Column"":3,""Row"":-2,""X"":4,""Y"":0},{""Column"":3,""Row"":-1,""X"":4,""Y"":0},{""Column"":3,""Row"":0,""X"":4,""Y"":2},{""Column"":2,""Row"":1,""X"":3,""Y"":3},{""Column"":1,""Row"":2,""X"":1,""Y"":4},{""Column"":0,""Row"":3,""X"":0,""Y"":5},{""Column"":-1,""Row"":3,""X"":-1,""Y"":4},{""Column"":-2,""Row"":3,""X"":-3,""Y"":3},{""Column"":-3,""Row"":3,""X"":-4,""Y"":2},{""Column"":-3,""Row"":2,""X"":-4,""Y"":0},{""Column"":-3,""Row"":1,""X"":-4,""Y"":0},{""Column"":-3,""Row"":0,""X"":-4,""Y"":-2},{""Column"":-2,""Row"":-1,""X"":-3,""Y"":-3},{""Column"":-1,""Row"":-2,""X"":-1,""Y"":-4}]";
        List<HexPoint>? _37;
        [SetUp]
        public void Setup()
        {
            _37 = JsonConvert.DeserializeObject<List<HexPoint>>(result37AsString);
        }

        [Test]
        public void Given_number_37_Places_37_hexes_correct_using_OLD_METHOD()
        {
            List<HexPoint> expectedResult = _37;

            var hexCreator = new HexPositionsCalculator();
            List<HexPoint> actualresult = hexCreator.GenerateHexagonalGridFixed(37, false);

            actualresult.Should().BeEquivalentTo(expectedResult);
        }
        [Test]
        public void Given_number_37_Places_37_hexes_correct_using_NEW_METHOD()
        {
            List<HexPoint>? expectedResult = JsonConvert.DeserializeObject<List<HexPoint>>(result37AsString);

            var hexCreator = new HexPositionsCalculator();
            List<HexPoint> actualresult = hexCreator.GenerateHexagonalGridFixed(37, true);
            actualresult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(19)]
        [TestCase(26)]
        public void Given_ANumber_places_Correct_Number_In_CorrectPlace(int number)
        {
            var expectedResult = _37.Take(number);
            var hexCreator = new HexPositionsCalculator();
            List<HexPoint> actualresult = hexCreator.GenerateHexagonalGridFixed(number, false);

            CollectionAssert.AreEqual(expectedResult, actualresult);

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