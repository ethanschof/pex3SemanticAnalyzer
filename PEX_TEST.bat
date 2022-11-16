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

echo Correct 1: Proper Expressions and Variable Declaration >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\good_expr_and_variable_declaration.txt >> %file%
echo. >> %file%

echo Correct 2: Proper If Statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\good_if_expr.txt >> %file%
echo. >> %file%

echo Correct 3: Proper While Statement >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\good_while_expr.txt >> %file%
echo. >> %file%

echo Correct 4: Proper AND OR NOT >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\good_and_or_not.txt >> %file%
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

echo Incorrect 8: if (Expr)… - fails if Expr is not Boolean >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_if_expr_not_bool.txt >> %file%
echo. >> %file%

echo Incorrect 9: while (Expr) … - fails if Expr is not Boolean >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_while_expr_not_bool.txt >> %file%
echo. >> %file%

echo Incorrect 10: Expressions: - fails if type name used instead of variable name >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_expr_type_name_used_not_var_name.txt >> %file%
echo. >> %file%

echo Incorrect 11: Expressions: - fails if undeclared variable used >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_expr_undeclared_var_used.txt >> %file%
echo. >> %file%

echo Incorrect 12: +,-,*,/ fail when types don’t match >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_operators_types_no_match.txt >> %file%
echo. >> %file%

echo Incorrect 13: +,-,*,/ fail when types aren’t integer or float >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_operators_types_not_int_or_float.txt >> %file%
echo. >> %file%

echo Incorrect 14: and/or fail when types don’t match >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_and_or_type_no_match.txt >> %file%
echo. >> %file%

echo Incorrect 15: and/or/not fail when types aren’t Boolean >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_and_or_not_type_no_bool.txt >> %file%
echo. >> %file%

echo Incorrect 16: <,<=,>,>=.=. fail when types don’t match >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_bool_types_no_match.txt >> %file%
echo. >> %file%

echo Incorrect 17: <,<=,>,>=.=. fail when types aren’t integer or float >> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\bad_bool_types_not_int_or_float.txt >> %file%
echo. >> %file%

pause