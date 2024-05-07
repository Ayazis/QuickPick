using Newtonsoft.Json;
using System.Security.Cryptography;
using Hexgrid;

namespace HexTest.Tests
{
    public class Tests
    {
        const string result37AsString = @"[{""Column"":0,""Row"":0,""X"":0,""Y"":0},{""Column"":0,""Row"":-1,""X"":0,""Y"":-1},{""Column"":1,""Row"":-1,""X"":1,""Y"":0},{""Column"":1,""Row"":0,""X"":1,""Y"":0},{""Column"":0,""Row"":1,""X"":0,""Y"":1},{""Column"":-1,""Row"":1,""X"":-1,""Y"":0},{""Column"":-1,""Row"":0,""X"":-1,""Y"":0},{""Column"":0,""Row"":-2,""X"":0,""Y"":-3},{""Column"":1,""Row"":-2,""X"":1,""Y"":-2},{""Column"":2,""Row"":-2,""X"":3,""Y"":-1},{""Column"":2,""Row"":-1,""X"":3,""Y"":0},{""Column"":2,""Row"":0,""X"":3,""Y"":1},{""Column"":1,""Row"":1,""X"":1,""Y"":2},{""Column"":0,""Row"":2,""X"":0,""Y"":3},{""Column"":-1,""Row"":2,""X"":-1,""Y"":2},{""Column"":-2,""Row"":2,""X"":-3,""Y"":1},{""Column"":-2,""Row"":1,""X"":-3,""Y"":0},{""Column"":-2,""Row"":0,""X"":-3,""Y"":-1},{""Column"":-1,""Row"":-1,""X"":-1,""Y"":-2},{""Column"":0,""Row"":-3,""X"":0,""Y"":-5},{""Column"":1,""Row"":-3,""X"":1,""Y"":-4},{""Column"":2,""Row"":-3,""X"":3,""Y"":-3},{""Column"":3,""Row"":-3,""X"":4,""Y"":-2},{""Column"":3,""Row"":-2,""X"":4,""Y"":0},{""Column"":3,""Row"":-1,""X"":4,""Y"":0},{""Column"":3,""Row"":0,""X"":4,""Y"":2},{""Column"":2,""Row"":1,""X"":3,""Y"":3},{""Column"":1,""Row"":2,""X"":1,""Y"":4},{""Column"":0,""Row"":3,""X"":0,""Y"":5},{""Column"":-1,""Row"":3,""X"":-1,""Y"":4},{""Column"":-2,""Row"":3,""X"":-3,""Y"":3},{""Column"":-3,""Row"":3,""X"":-4,""Y"":2},{""Column"":-3,""Row"":2,""X"":-4,""Y"":0},{""Column"":-3,""Row"":1,""X"":-4,""Y"":0},{""Column"":-3,""Row"":0,""X"":-4,""Y"":-2},{""Column"":-2,""Row"":-1,""X"":-3,""Y"":-3},{""Column"":-1,""Row"":-2,""X"":-1,""Y"":-4}]";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_number_37_Places_37_hexes_correct()
        {
            List<HexPoint>? expectedResult = JsonConvert.DeserializeObject<List<HexPoint>>(result37AsString);

            var hexCreator = new HexPositionsCalculator();
            List<HexPoint> actualresult = hexCreator.GenerateHexagonalGridFixed(37);
            var actualJson = JsonConvert.SerializeObject(actualresult);

            Assert.That(actualJson, Is.EqualTo(result37AsString));

            Assert.That(actualresult.Count, Is.EqualTo(37));
        }
    }
}