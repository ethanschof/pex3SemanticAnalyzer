:: Creates a Variable for the Output File
@SET file="pex_test_results.txt"

:: Erases Everything Currently In the Output File
type NUL>%file%

:: ----------------------------------------
:: TITLE
:: ----------------------------------------
echo PEX 3 TEST CASES (C1C Ethan Schofield and C1C Joseph Daniel) >> %file%

:: ----------------------------------------
:: GOOD EXAMPLES
:: ----------------------------------------
echo Testing String Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\y_is_not_declared.txt >> %file%
echo. >> %file%

:: ----------------------------------------
:: BAD EXAMPLES
:: ----------------------------------------
echo Running Incorrect Test Cases >> %file%

echo Testing Incorrect Declare 1 >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare_bad_1.txt >> %file%
echo. >> %file%

echo Testing Incorrect Declare 2 >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare_bad_2.txt >> %file%
echo. >> %file%

echo Testing Incorrect Declare 3 >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare_bad_3.txt >> %file%
echo. >> %file%


pause