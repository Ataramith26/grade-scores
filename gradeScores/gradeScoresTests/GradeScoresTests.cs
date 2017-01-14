using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransmaxLvl2RequirementsTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TransmaxLvl2RequirementsTest.Tests
{
    [TestClass()]
    public class GradeScoresTests
    {

        //mainFileValidation Unit Tests
        [TestMethod()]
        public void mainFileValidationTestIsEmpty()
        {
            string filePath = @"";
            bool test = GradeScores.mainFileValidation(filePath);
            Assert.IsFalse(test);
        }
        [TestMethod()]
        public void mainFileValidationTestExists()
        {
            string filePath = @"c:\jimmy\not\here.txt";
            bool test = GradeScores.mainFileValidation(filePath);
            Assert.IsFalse(test);
        }

        [TestMethod()]
        public void mainFileValidationTestFileExtension()
        {
            string filePath = @"GradeScoresTest.cs";
            bool test = GradeScores.mainFileValidation(filePath);
            Assert.IsFalse(test);
        }

        [TestMethod()]
        public void mainFileValidationTestSuccess()
        {
            string filePath = @"UTfile2.txt";
            bool test = GradeScores.mainFileValidation(filePath);
            Assert.IsTrue(test);
        }

        //convertFileToDataTable Unit tests
        [TestMethod()]
        public void convertFileToDataTableTestSuccess()
        {
            string filePath = @"UTfile2.txt";
            DataTable scores = new DataTable();
            scores = GradeScores.convertFileToDataTable(filePath);
            Assert.IsNotNull(scores);
        }

        [TestMethod()]
        public void convertFileToDataTableTestExceptionsHandled()
        {
            string filePath = @"UTExceptionsHandled.txt";
            DataTable scores = new DataTable();
            scores = GradeScores.convertFileToDataTable(filePath);
            Assert.IsNotNull(scores);
        }

        //Score sorting Unit Tests
        [TestMethod()]
        public void sortScoresTestHighScore()
        {
            string filePath = @"UTfile2.txt";
            DataTable scores = new DataTable();
            scores = GradeScores.convertFileToDataTable(filePath);
            DataTable sorted = new DataTable();
            sorted = GradeScores.sortScores(scores);
            Object field = sorted.Rows[0][2];
            Assert.AreEqual(field,88);
        }

        [TestMethod()]
        public void sortScoresTestSirName()
        {
            string filePath = @"UTfile2.txt";
            DataTable scores = new DataTable();
            scores = GradeScores.convertFileToDataTable(filePath);
            DataTable sorted = new DataTable();
            sorted = GradeScores.sortScores(scores);
            Object field = sorted.Rows[1][0];
            Assert.AreEqual(field, "KING");
        }

        [TestMethod()]
        public void sortScoresTestFirstName()
        {
            string filePath = @"UTfile2.txt";
            DataTable scores = new DataTable();
            scores = GradeScores.convertFileToDataTable(filePath);
            DataTable sorted = new DataTable();
            sorted = GradeScores.sortScores(scores);
            Object field = sorted.Rows[3][1];
            Assert.AreEqual(field, "FRANCIS");
        }

        //
    }
}