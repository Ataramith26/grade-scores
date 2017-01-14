using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransmaxLvl2RequirementsTest
{
    class Program
    {
        const int command1 = 13; //Index of end of "grade-scores " begining of file path
        static void Main(string[] args)
        {
            //Read in the console command typed
            string commandEntered = Console.ReadLine();
            //Correct command must be given
            if (commandEntered.StartsWith("grade-scores"))
            {
                string filePath = commandEntered.Substring(command1);
                if (mainFileValidation(filePath) == true)
                {
                    DataTable scores = new DataTable();
                    scores = convertFileToDataTable(filePath);
                    convertDataTableToTxtFile(sortScores(scores), filePath.Substring(0, filePath.Length - 4) + "-graded.txt"); //Use correct file anme for output
                }

            }
            else
            {
                Console.WriteLine("Invalid command. Exiting program.");
            }

            //Keeps screen open
            Console.ReadLine();



        }

        public static Boolean mainFileValidation(String filePath)
        {

            //File must exist and must be a .txt
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.\nPlease retry with the correct filename.");
                return false;
            }
            else if (!(filePath.Substring(filePath.Length - 4) == ".txt"))
            {
                Console.WriteLine("File type is not supported.\nPlease use a txt file.");
                return false;
            }
            else if (new FileInfo(filePath).Length == 0)
            {
                Console.WriteLine("File is empty.\nFile cannot be used.");
                return false;
            }
            else
            {
                return true;
            }

        }

        //This method sorts the scores of a valid DataTable
        public static DataTable sortScores(DataTable needSorting)
        {
            //Use a DataView to sort the datatable then output
            DataTable sortedScores = new DataTable();
            needSorting.DefaultView.Sort = "Score DESC, SirName , FirstName";
            sortedScores = needSorting.DefaultView.ToTable();
            return sortedScores;
        }

        public static DataTable convertFileToDataTable(String filePath)
        {
            //Setup Columns
            DataTable ConvertedTable = new DataTable();
            ConvertedTable.Columns.Add(new DataColumn("SirName", typeof(string)));
            ConvertedTable.Columns.Add(new DataColumn("FirstName", typeof(string)));
            ConvertedTable.Columns.Add(new DataColumn("Score", typeof(int)));
            int numCols = 3; // 3 Columns
            bool addRow = true; //Flag for if row is ok to add to table

            //Read in Lines
            string[] lines = System.IO.File.ReadAllLines(filePath);

            //Split the lines by comma delimiter and populate the DataRows to populate the DataTable
            foreach (string line in lines)
            {

                var spiltLine = line.Split(',');
                DataRow toBeAdded = ConvertedTable.NewRow();

                for (int i = 0; i < numCols; i++)
                {
                    try
                    {
                        toBeAdded[i] = spiltLine[i].Replace(" ", ""); //remove whitespace
                        addRow = true;
                    }
                    catch (Exception e)
                    {
                        if (e is System.IndexOutOfRangeException || e is System.ArgumentException)
                        {
                            Console.WriteLine("Line " + i + " has invalid data and will be ignored!\n");
                            i++;
                            addRow = false;
                        }

                    }
                }
                if (addRow)
                {
                    ConvertedTable.Rows.Add(toBeAdded);
                }

            }

            return ConvertedTable;
        }

        public static void convertDataTableToTxtFile(DataTable input, string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
                //Move through each row and print in correct format
                foreach (DataRow row in input.Rows)
                {
                    object[] arrayOfRowData = row.ItemArray;
                    for (int i = 0; i < arrayOfRowData.Length; i++)
                    {
                        //First 2 columns have ", " after them
                        if (i != arrayOfRowData.Length - 1)
                        {
                            outputFile.Write(arrayOfRowData[i] + ", ");
                        }
                        else
                        {
                            outputFile.Write(arrayOfRowData[i]);
                        }
                    }
                    //Write line return character
                    outputFile.WriteLine();
                }
        }

    }


}


