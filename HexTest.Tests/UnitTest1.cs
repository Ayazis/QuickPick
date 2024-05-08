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
            List<HexPoint> actualresult = hexCreator.GenerateHexagonalGridFixed(37,false);        

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
    }
}