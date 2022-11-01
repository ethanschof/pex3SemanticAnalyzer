:: Creates a Variable for the Output File
@SET file="pex_test_results.txt"

:: Erases Everything Currently In the Output File
type NUL>%file%

:: ----------------------------------------
:: TITLE
:: ----------------------------------------
echo PEX 2 TEST CASES (C1C Ethan Schofield) >> %file%

:: ----------------------------------------
:: GOOD EXAMPLES
:: ----------------------------------------
echo Testing String Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign1.txt >> %file%
echo. >> %file%

echo Testing Integer Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign2.txt >> %file%
echo. >> %file%

echo Testing Plus Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign3.txt >> %file%
echo. >> %file%

echo Testing Left hand chain OoO Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign4.txt >> %file%
echo. >> %file%

echo Testing Subtract Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign5.txt >> %file%
echo. >> %file%

echo Testing Negative unary operator Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign6.txt >> %file%
echo. >> %file%

echo Testing Unary OoO Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign7.txt >> %file%
echo. >> %file%

echo Testing Multiplication Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign8.txt >> %file%
echo. >> %file%

echo Testing Division Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign9.txt >> %file%
echo. >> %file%

echo Testing Chain OoO Multiplication Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign10.txt >> %file%
echo. >> %file%

echo Testing Chain OoO Division Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign11.txt >> %file%
echo. >> %file%

echo Testing Bit NOT Unary Operator Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign12.txt >> %file%
echo. >> %file%

echo Testing Bit OR Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign13.txt >> %file%
echo. >> %file%

echo Testing Bit AND Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign14.txt >> %file%
echo. >> %file%

echo Testing Chain OoO Multiplication and Parenthesis Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign15.txt >> %file%
echo. >> %file%

echo Testing Float Assign Statements >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assign16.txt >> %file%
echo. >> %file%

echo Testing Constant Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\constant1.txt >> %file%
echo. >> %file%

echo Testing Constant Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\constant2.txt >> %file%
echo. >> %file%

echo Testing Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare1.txt >> %file%
echo. >> %file%

echo Testing Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare2.txt >> %file%
echo. >> %file%

echo Testing Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare3.txt >> %file%
echo. >> %file%

echo Testing Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declare4.txt >> %file%
echo. >> %file%

echo Testing Function Calls >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\func_call1.txt >> %file%
echo. >> %file%

echo Testing Function Calls >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\func_call2.txt >> %file%
echo. >> %file%

echo Testing Function Calls >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\func_call3.txt >> %file%
echo. >> %file%

echo Testing Function Calls >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\func_call4.txt >> %file%
echo. >> %file%

echo Testing Function Calls >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\func_call5.txt >> %file%
echo. >> %file%

echo Testing Function Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\function1.txt >> %file%
echo. >> %file%

echo Testing Function Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\function2.txt >> %file%
echo. >> %file%

echo Testing Function Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\function3.txt >> %file%
echo. >> %file%

echo Testing _if statement and equivalance >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if1.txt >> %file%
echo. >> %file%

echo Testing _if statement and less than >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if2.txt >> %file%
echo. >> %file%

echo Testing _if statement and greater than >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if3.txt >> %file%
echo. >> %file%

echo Testing _if statement and less than equal to >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if4.txt >> %file%
echo. >> %file%

echo Testing _if statement and greater than equal to >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if5.txt >> %file%
echo. >> %file%

echo Testing _if statement and not equal to >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if6.txt >> %file%
echo. >> %file%

echo Testing _if _else statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\if7.txt >> %file%
echo. >> %file%

echo Testing main statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\main1.txt >> %file%
echo. >> %file%


echo Testing while statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\while1.txt >> %file%
echo. >> %file%

echo Testing while statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\while2.txt >> %file%
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