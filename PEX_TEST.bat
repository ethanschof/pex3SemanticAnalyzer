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
echo Running Correct Test Cases >> %file%

echo Correct 1: Proper Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\good_proper_variable_declaration.txt >> %file%
echo. >> %file%

:: ----------------------------------------
:: BAD EXAMPLES
:: ----------------------------------------
echo Running Incorrect Test Cases >> %file%

echo Incorrect 1: y x; – fails if x already declared >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_x_already_declared.txt >> %file%
echo. >> %file%

echo Incorrect 2: y x; – fails if y is not declared >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_y_is_not_declared.txt >> %file%
echo. >> %file%

echo Incorrect 3: y x; – fails if y is declared, but not a type >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_y_is_declared_not_a_type.txt >> %file%
echo. >> %file%

echo Incorrect 4: final y x=z; - fails if x already declared >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_final_x_already_declared.txt >> %file%
echo. >> %file%

echo Incorrect 5: final y x=z; - fails if y is not declared >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_final_y_not_declared.txt >> %file%
echo. >> %file%

echo Incorrect 6: final y x=z; - fails if y is declared, but not a type >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_final_y_declared_not_a_type.txt >> %file%
echo. >> %file%

echo Incorrect 7: final y x=z; - fails if types of y, z don’t match >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_final_types_y_z_no_match.txt >> %file%
echo. >> %file%

pause