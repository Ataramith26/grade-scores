using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransmaxLvl2RequirementsTest
{
    public class GradeScores
    {
        //Constant Declaration
        const int command1 = 13; //Index of end of "grade-scores " begining of file path
        
        /// <summary>
        /// Main - This method is used to launch the application.
        /// The application expects the user to use the grade-scores command followed by the textfile location.
        /// It then sorts the grades and outputs them to a text file with the same name with "-graded.txt" appended.
        /// The new file is created in the same directory that was specified in the grade-scores command.
        /// </summary>
        /// <param name="args"></param>
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
                    convertDataTableToTxtFile(sortScores(scores), filePath.Substring(0, filePath.Length - 4) + "-graded.txt"); //Use correct file name for output
                    Console.WriteLine("Finished: created " + filePath.Substring(0, filePath.Length - 4) + "-graded.txt");
                }

            }
            else
            {
                Console.WriteLine("Invalid command. Exiting program.");
            }

            //Keeps screen open
            Console.ReadLine();



        }

        /// <summary>
        /// mainFileValidation - This method checks some validation rules agaisnt the input text file string and the file itself.
        /// The checks it performs are: That the file exists, that the file is a .txt and that the file is not empty.
        /// </summary>
        /// <param name="filePath">
        /// filePath is a string that contains the filePath of the input text file.
        /// </param>
        /// <returns></returns>
        public static Boolean mainFileValidation(String filePath)
        {

            //File must exist and must be a .txt
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.\nPlease retry with the correct filename.");
                return false;
            }
            //File must be a .txt
            else if (!(filePath.Substring(filePath.Length - 4) == ".txt"))
            {
                Console.WriteLine("File type is not supported.\nPlease use a txt file.");
                return false;
            }
            //File must not be empty
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
        /// <summary>
        /// sortScores - This method is used to sort a correctly formatted set of scores by score, sirname and finally first name.
        /// </summary>
        /// <param name="needSorting">
        /// needSorting is a DataTable that contains a valid set of data with the columns: SirName, FirstName and Score
        /// </param>
        /// <returns></returns>
        public static DataTable sortScores(DataTable needSorting)
        {
            //Use a DataView to sort the datatable then output
            DataTable sortedScores = new DataTable();
            needSorting.DefaultView.Sort = "Score DESC, SirName , FirstName";
            sortedScores = needSorting.DefaultView.ToTable();
            return sortedScores;
        }
        /// <summary>
        /// convertFileToDataTable - This method is used to convert a .txt with the correct columns into a DataTable.
        /// The correct columns are: SirName, FirstName and Score.
        /// </summary>
        /// <param name="filePath">
        /// filePath is a string containing the path of the file to be convereted.
        /// </param>
        /// <returns></returns>
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

        /// <summary>
        /// convertDataTableToTxtFile - This method is used to convert a DataTable into a .txt.
        /// </summary>
        /// <param name="input">
        /// input is the DataTable to be converted to a .txt.
        /// </param>
        /// <param name="fileName">
        /// fileName is the name of the output .txt.
        /// </param>
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


