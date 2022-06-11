# Autor

&copy; *Wiktor Jóźwik, Politechnika Warszawska 2022*

# Dokumentacja

## Opis funkcjonalny i przykłady użycia

### Zmienne i dedykowane typy

- Zdefiniowane jednostki SI wbudowane w język **unit**:
    - **[s]** - czas
    - **[m]** - długość
    - **[kg]** - masa
    - **[A]** - natężenie prądu elektrycznego
    - **[K]** - temperatura
    - **[mol]** - liczność materii
    - **[cd]** - światłość źródła światła
    - **[]** - brak jednostki(zmienna skalarna)

- Typy danych:
    - **unit** (zrzesza typy całkowite **int** oraz zmiennoprzecinkowe **float**)
    - **string**
    - **bool**

- Zmienne skalarne, napisy lub booleany
    - `let x: [] = 5`
    - `let y: [] = 5.2`
    - `let z: string = "my string"`
    - `let w: bool = true`
    - `let q: bool = false`

- Zmienne korzystające z jednostki SI
    - `let duration: [s] = 5 [s]`
    - `let mass: [kg] = 10 [kg]`

- Zmienne tworzone przy użyciu złożonych jednostek SI
    - `let speed: [m*s^-1] = 10 [m*s^-1]`
    - `let force: [kg*m*s^-2] = 270 [kg*m*s^-2]`

- Zmienne tworzone przy użyciu zdefiniowanych jednostek przez użytkownika
  ``` 
  unit N: [kg*m*s^-2]
  let force: [N] = 270 [N]
  ```

- Przykłady operacji na zmiennych z typami jednostek
    - ```
      let t1: [s] = 5 [s]
      let s1: [m] = 12 [m]
      let v1: [m*s^-1] = s1 / t1 let v2: [m*s^-1] = 10 [m*s^-1]
      let deltaV = v2 - v1
  
    - ```
      unit J: [kg*m^2*s^-2]
    
      let mass: [kg] = 10 [kg]
      let duration: [s] = 10 [s]
      let distance: [m] = 20 [m]
    
      let speed: [m*s^-1] = distance / duration let energy: [J] = mass * speed*speed / 2
      ```

### Komentarze

- // użycie komentarza w kodzie

### Instrukcje warunkowe

- ```
    let time: [s] = 10 [s]
    let length: [m] = 50 [m]
    let speed: [m*s^-1] = length / time
    
    if (speed >  5 [m*s^-1]) {
      code block
    } else if (speed <= 0 [m*s^-1]) {
      code block 
    } else {
      code block
    }
    ```

### Pętla while:

- ```
    let i: [] = 10
  
    while (i > 0) {
      let speed: [m*s^-1] = 10 [m*s^-1] * i
      print(speed)
      i = i - 1
    }
    ```

### Funkcje

- Użycie funkcji z wykorzystaniem argumentów z jednostkami SI oraz zmiennej skalarnej
  ```
  calculateVelocityData(v1: [m*s^-1], v2: [m*s^-1], scalar: []) -> [m*s^-1] {
    return (v2-v1) * scalar
  }
  ```
- Użycie funkcji z resztą zmiennych
  ```
  printMass(m: [kg], shouldPrintMass: bool, printText: string) -> void {
    if (shouldPrintMass) {
      print(printText)
      print(m)
    }
  }
  ```
- Generalna składnia funkcji:
  ```
  functionName(arg1: unit|string|bool, arg2: unit|string|bool, arg3: unit|string|bool, ...) -> unit|void|string|bool {
    code block
    ...
    return var // or just return
  }
  ```

### Wbudowane funkcje

- `print(argument)`
    - w przypadku gdy przekazana zmienna z typem **unit** wypisana zostanie wartość oraz typ jednostki np.

```
  main() -> void {
    let x: [m] = 20 [m]
    print("x equals")
    print(x)
  }
  // would print:
  // x equals:
  // 20 [m]
``` 

- w przypadku gdy przekazana funkcja jako argument ze zwracanym typem **unit** także zostanie wypisana jednostka

```
  getTime() -> [s] {
    return 25 [s]
  }
  main() -> void {
    print("variable from getTime():")
    print(getTime())
  }
  // would print:
  // variable from getTime():
  // 25 [s]
```

- w przyadku gdy przekazane Expression np. `print(y + x)` i `y` i `x` mają typy **unit** nie zostanie wypisana jednostka

```
  main() -> void {
    let x: [m] = 20 [m]
    let y: [m] = -5 [m]
    print("x + y")
    print(x+y)
  }
  // would print:
  // x + y
  // 15
```

### Przykłady użycia zawierające elementy języka:

- ```
  unit N: [kg*m*s^-2]
  
  calculateGForce(earthMass: [kg], sunMass: [kg], earthSunDistance: [m]) -> [N] {
    let G: [N*m^2*kg^-2] = 6.6732e-11 [N*m^2*kg^-2]
    return G * earthMass * sunMass / (earthSunDistance * earthSunDistance)
  }
  
  printGForceInLoop(gForce: [N], i: [], shouldPrint: bool, printText: string) -> void {
    if (shouldPrint) {
      while (i > 0) {
        print(i)
        print(printText)
        print(gForce)
        i = i-1
      }
    }
  }
  
  main() -> void {
  let earthMass: [kg] = 5.9722e24 [kg]
  let sunMass: [kg] = 1.989e30 [kg]
  let earthSunDistance: [m] = 149.24e9 [m]
  
  let gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)
  
  let i: [] = 3
  let shouldPrint: bool = true
  let printText: string = "GForce is: "
  
  printGForceInLoop(gForce, i, shouldPrint, printText)
  }
  ```

- ```
  getDistance(x: []) -> [m] {
    // 1/2
    return 5 [m] / x
  }
  getDuration(x: []) -> [s] {
    // 25
    return 5 [s] * x
  }
  
  calculateVelocity(distance: [m], duration: [s]) -> [m*s^-1] {
  // 1/2 / 25
  return distance / duration
  }
  
  main() -> void {
    let x: [m] = getDistance(10)
    let y: [s] = getDuration(5)
    let v1: [m*s^-1] = calculateVelocity(getDistance(10), getDuration(5))
    let v2: [m*s^-1] = calculateVelocity(x, y)
    
    print(v1)
    print(v2)
  }
  ```

- więcej przykładów użycia w folderze `code_exmaples`

### Dozwolone operacje

- _same_unit_ **+** **-** **==** **!=** **>** **<** **>=** **<=** _same_unit_
- _unit_ **/** * _unit_

- _string_ **+** _string_
- _string_ **==** **!=** _string_

- _bool_ **||** **&&** **!=** **==** _bool_
- **!** _bool_
- **-** _unit_

(same_unit - gdy oba typy **unit** mają tą samą jednostkę np. [s] i [s])

- Reszta operacji jest niedozwolona

#### Błędy

- Lexer
    - CommentExceededLengthException
    - DecimalPlacesExceededAmountException
    - ExponentPartExceededSizeException
    - IdentifierExceededLengthException
    - NumberExceededSizeException
    - TextEndingQuoteNotFoundException
    - TextExceededLengthException
    - UnknownEscapeCharException
- Parser
    - FunctionAlreadyDefinedException
    - ParserException
    - UnitAlreadyDefinedException
- SemanticAnalyzer
    - FunctionUndeclaredException
    - NotUniqueParametersNamesException
    - NotValidReturnTypeException
    - TypeMismatchException
    - UnitUndeclaredException
    - UnpermittedOperationException
    - VariableRedeclarationException
    - VariableUndeclaredException
    - WrongNumberOfArgumentsException
- Interpreter
    - LackOfMainFunctionException
    - LackOfValidReturnException
    - MaxNumberIterationReachedException

### Przykłady niewłaściwego użycia

###### W większości przypadków pominięta została funkcja `main`

- ```
  let x: bool = true
  let y: string = "rts --> TextEndingQuoteNotFoundException
  ```

- ```
  calculateVelocityDelta(v1: [m/s], v2: [m/s]) {
    return v2 - v1 
  } --> ParserException: Expected RETURN_ARROW token but received LEFT_CURLY_BRACE on row x and column y
  ```

- ```
  let v1: [m*s^-1] = 5
  let v2: [m*s^-1] = 10
  let speed = v1 / v2 --> ParserException: Expected COLON token but received ASSIGNMENT_OPERATOR on row x and column y
  ```

- ```
  unit N: [kg*m/s^2] --> ParserException: Expected RIGHT_SQUARE_BRACKET or MULTIPLICATION_OPERATOR or POWER_OPERATOR 
  token but received DIVISION_OPERATOR on row x and column y
  ```

- ```
  myFun() -> void { }
  myFun(x: []) -> [] { return x } --> FunctionAlreadyDefinedException: 'myFun' function is already defined
  ```

- ```
  unit N: [kg*m*s^-2]
  unit N: [kg*m*s^-2] --> UnitAlreadyDefinedException: 'N' unit is already defined
  ```

- ```
  calculateVelocityDelta(v1, v2) -> [m*s^-1] {
    return v2 - v1 
  } --> VariableUndeclaredException: // SyntaxError: 'v1' is not defined
    --> VariableUndeclaredException: // SyntaxError: 'v2' is not defined
  ```

- ```
  let x: [] = 5

  let z: [] = x * y --> VariableUndeclaredException: 'y' is not defined
  ```

- ```
  let z: [] = fun() --> FunctionUndeclaredException: 'fun' is not defined
  ```

- ```
  let force: [N] = 20 [N] --> UnitUndeclaredException: 'N' is not defined
  ```

- ```
  myFun2(x: [], x: [m]) -> void {} --> NotUniqueParametersNamesException: 'myFun2' got two or more 'x' parameters
  ```

- ```
  calculateVelocityDelta(v1: [m*s^-1], v2: [m*s^-1]) -> [m*s^-1] {
    return v2 / v1 --> NotValidReturnTypeException: 'calculateVelocityDelta' return requires [m*s^-1] type but returned []
  }
  ```

- ```
  let var: [] = 5
  let var: [m] = 10 [m] --> VariableRedeclarationException: 'var' is already declared
  ```

- ```
  calcFun(x: []) -> [] { return x }
  main() -> void {
    let x: [] = calcFun() --> WrongNumberOfArgumentsException: 'calcFun' function was invoked with 0 argument(s) but expected 1 argument(s)
  }
  ```

- ```
  myFun2(x: [s], y: [m]) -> void {}
  main() -> {
    myFun2(10 [s], 20 [s])
  } --> TypeMismatchException: 'y' requires [m] type but received [s]
  ```

- ```
  let duration: [s] = 5 [s]
  let length: [m] = 10 [m]
    
  let speed: [m*s^-1] = duration / length --> TypeMismatchException: 'speed' requires [m*s^-1] type but received [s*m^-1]
  ```

- ```
  let duration: [s] = 5 --> TypeMismatchException: 'duration' requires [s] type but received []
  ```

- ```
  let s1: [m] = 5 [m]
  let t1: [s] = 10 [s]
    
  let v: [] = s1 - t1 --> UnpermittedOperationException: Unsupported operator '-' for [m] and [s]
  ```

- ```
  let v: [m*s^-1] = 10 [m*s^-1]
    
  if (v > 5) {
    code block
  } --> UnpermittedOperationException: Unsupported operator '>' for [m*s^-1] and [] 
  ```

- ```  
  if (10 <= 5 [m*s^-1]) {
    code block
  } --> UnpermittedOperationException: Unsupported operator '>' for [] and [m*s^-1]
  ```

- ```
  let x: [] = 5
  let mass: [kg] = 10 [kg]
  let y: [] = x - mass --> UnpermittedOperationException: Unsupported operator '-' for [] and [kg]
  ```

- ```
  let x: string = "abc"
  let y: [] = 5
  let z: [] = x + y --> UnpermittedOperationException: Unsupported operator '+' for string and []
  ```

- ```
  let x: string = "abc"
  let y: string = "xyz"
  
  let z: string = x / y --> UnpermittedOperationException: Unsupported operator '/' for string and string
  ```

- ```
  let x: string = "str"
  let y: string "rts"
   
  let z: bool = x > y --> UnpermittedOperationException: Unsupported operator '>' for string and string
  ```

- ```
  let x: bool = true
  let y: string = "rts"
   
  let z: bool = x + y --> UnpermittedOperationException: Unsupported operator '+' for bool and string
  ```

- ```
  let str: string = "str"
   
  let z: bool = !str --> UnpermittedOperationException: Unsupported operator '!' for string type
  ```

- ```
  let b: bool = true
   
  let z: bool = -b --> UnpermittedOperationException: Unsupported operator '-' for bool type
  ```

- ```
  let x: [] = 5
  let y: [] = 0
  
  let z: [] = x / y --> ZeroDivisionError
  ```

- ```
  funWithoutValidReturn() -> [m] {
    let x: [m] = 20 [m]
    let y: [m] = 30 [m]
    let z: [m] = y - x --> LackOfValidReturnException
  }
  
  main() -> void {
    let x: [m] = funWithoutValidReturn() 
  }
  ```

## Formalny opis gramatyki EBNF

```bash
--- STATEMENTS
top_level                   = { top_level_statement };

top_level_statement         = unit_declaration      | 
                              function_statement    | 
                              
statement                   = assign_statement      |
                              variable_declaration  | 
                              function_call         | 
                              return_statement      | 
                              if_statement          | 
                              while_statement;                              
          
return_statement            = "return", [ expression ];

if_statement                = "if", "(" expression, ")", block, [ "else", if_statement_or_block];

if_statement_or_block       = if_statement | 
                              block;
                
while_statement             = "while", "(" expression, ")" block;

block                       = "{" statements "}";
            
statements                  = statement, { statement };


--- EXPRESSION

expression                  = or_expression

or_expression               = and_expression, { "||", and_expression };

and_expression              = expression_comparison, { "&&" expression_comparison };

expression_comparison       = additive_expression, [ comparison_operator, additive_expression ];

comparison_operator         = ">"   | 
                              "<"   | 
                              ">="  | 
                              "<="  | 
                              "=="  | 
                              "!=";

additive_expression         = multiplicative_expression, { additive_operator, multiplicative_expression };

additive_operator           = "+" | 
                              "-";

multiplicative_expression   = unary_expression, { multiplicative_operator, unary_expression };

multiplicative_operator     = "*" | 
                              "/";

unary_expression            = [ negate ], primary_expression;

negate                      = "!" | 
                              "-";
          
primary_expression          = literal                     |
                              identifier_or_function_call |
                              "(" expression ")"

identifier_or_function_call = identifier, [ "(", args, ")" ];

args                        = [ arg, { ",", arg } ];

arg                         = expression;


--- FUNCTION

function_call               = identifier, "(", args, ")";

function_statement          = identifier, "(", parameters, ")", "->", return_type, block;

parameters                  = [ parameter, { ",", parameter } ];

return_type                 = variable_type | 
                              void_type;
                              
                              
--- VARIABLES ASSIGNMENT

variable_declaration            = "let", parameter, "=" expression;

parameter                       = identifier, ":", variable_type;

assign_statement                = identifier, "=", expression

--- VARIABLE TYPES

variable_type               = unit_type | 
                              literal_type;

literal_type                = "string" | 
                              "bool";

void_type                   = "void";


--- UNIT 

unit_declaration            = "unit", identifier, ":" unit_type;

unit_type                   = "[" unit_value "]";

unit_value                  = [ unit_expression ];

unit_expression             = unit_unary_expression, { "*", unit_unary_expression };

unit_unary_expression       = identifier, [ unit_power ]

unit_power                  = "^", [ "-" ], int_literal;


--- LITERALS

literal                     = bool_literal  | 
                              num_literal   | 
                              string_literal;

bool_literal                = "true" | 
                              "false";
                              
num_literal                 = int_or_float_literal, [ unit_type ]

int_or_float_literal        = int_literal   | 
                              float_literal;

int_literal                 = non_zero_digit, { decimal_digits };

float_literal               = decimal_digits, float_dot_part_or_exponent

float_dot_part_or_exponent  = float_dot_part | float_exponent
                              
float_dot_part              = '.', decimal_digits, [ float_exponent ]

float_exponent              = "e", [ "-" ], decimal_digits;

decimal_digits              = digit, { digit };

digit                       = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";

non_zero_digit              = "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9";

string_literal              = """, { string_literal_character } """;

string_literal_character    = character | escape_character;

character                   = ~["\\\u000D\u000A\u0085\u2028\u2029];     // anything but ", \, and New_Line_Character: : \u000D - carriage return, \u000A - line feed, \u0085 - next line, \u2028 - line separator, \u2029 - paragraph separator

escape_character            =  "\"" | "\n" | "\t" | "\\";


--- IDENTIFIER

identifier                  = letter_character, { identifier_character }; 

identifier_character        = letter_character | "_" | digit;

letter_character            = [a-zA-Z];

comment                     = "//", { character };

--- KEYWORD

keyword                     = "let"     | 
                              "string"  | 
                              "bool"    | 
                              "unit"    | 
                              "fn"      | 
                              "void"    | 
                              "return"  | 
                              "if"      | 
                              "else"    | 
                              "while"   | 
                              "true"    | 
                              "false";
```

## Opis realizacji

### Język oraz framework

C# .NET 6

### Wejście oraz wyjście programu

Komenda służąca do uruchomienia:

`dotnet run nazwa_pliku`

np.

- `dotnet run code_examples/g_force.txt`

Wejście programu to ścieżka do pliku z kodem w wyżej zdefiniowanym języku, a wyjściem konsola, ukazany wynik będzie
zależał od tego co znajdzie się w kodzie dostarczonym przez użytkownika. Błędy obsłużone przez interpreter również będą
pokazywane w konsoli.

### Opis działania systemu

Po uruchomieniu programu z podaniem ścieżki zostaje wywołana klasa `Program`, która to wywołuje `MainProcessor`
przekazując do niej podane argumenty, przekazane przez użytkownika. Klasa ta powołuje do życia `Lexer`, a w
zadazie `CommentFilteredLexer`,
(klasa `CommentFilteredLexer` niczym oprócz odfiltrowania komentarzy nie różni się od klasy `Lexer`) oraz `Parser`, do
którego instancja klasy `CommentFilteredLexer` zostaje przekazana. Plik zostaje wczytany przy pomocy klasy wbudowanej w
C# `StreamReader` i podany do `Parser`a. `Parser` pyta `Lexer` o kolejny `Token` i buduje z nich odpowiednie obiekty. Po
przeanalizowaniu całego kodu zwracane jest drzewo obiektów programu. Drzewo to, którego korzeniem jest obiekt
klasy `TopLeveL` przekazywane jest dalej do klasy `Interpreter`. Tutaj odbywa się analiza semantyczna kodu - to czy
zdefiniowane działania przez użytkownika mają rzeczywiście sens. Powołany zostaje do życia obiekt
klasy `SemanticAnalyzerVisitor`, który analizuje kod pod względem semantycznym. Został zastosowany wzorzec wizytatora,
aby w łatwy sposób móc przejść po drzewie obiektów. Gdy analiza semantyczna powiedzie się bez błędów to
obiekt `TopLevel` zostaje przekazany do klasy `InterpreterVisitor` i tutaj odbywa się interpretowanie i działanie na
kodzie, który dostarczył użytkownik.

### Opis elementów systemu

- Lexer - tworzy tokeny na podstawie dostarczonych znaków
- Parser - otrzymuje kolejne tokeny po dostarczeniu kodu Lexerowi, buduje strukture obiektów przekazywaną do analizatora
  semantycznego i interpretera
- Analizator semantyczny - weryfikuje poprawność semantyczną dostarczonego kodu (analizuje czy kod ma sens)
- Interpreter - realizuje funkcjonalności języka na podstawie dostarczonej struktury obiektów przez Parser

### Opis testowania

- Zależności służące do uruchamiania testów: **xUnit**, **MiŚcrosoft.NET.Test.Sdk**, **xunit.runner.visualstudio**
- Katalog, w którym będą znajdować się testy: **spec/**
- Do uruchomienia testów służy komenda: **dotnet test**
- Każdy z elementów: Lexer, Parser, SemanticAnalyzer oraz Interpreter posiadają swoje testy
- Zaimplementowane zostało kilka testów akceptacyjnych, które znajdują się w folderze `code_examples`
- Testy sprawdzają poprawność wykonywanych operacji, strukturę obiektów, obsługują przypadki pozytywne jak i negatywne,
  sprawdzają czy są rzucane błędy, gdy powinny być rzucane. Sprawdzają przypadki progowe tzw. edge case'y np.
    - czy zmienna przekazywana jest przez wartość
    - czy zmienna jest widoczna tylko w danym scopie,
    - czy instrukcje po instrukcji return nie są wykonywane
    - czy jest limit iteracji pętli
    - czy działa rekursja
    - czy typ zwracany się zgadza

### Założenia techniczne

#### Językowe

- silne i statyczne typowanie
- parametry przekazywane do funkcji przez wartość
- zmienne widoczne tylko w danym scopie
- zmienne mutowalne

### Założenia funkcjonalne

#### Działania na jednostkach

- Dodawanie i odejmowanie nie wpływa na jednostki 5 m*s^-1 + 5 m*s^-1 = 10 m*s^-1.
- Nie można odejmować ani dodawać zmiennych z różnymi jednostkami.
- Dzielenie i mnożenie jednostek odbywa się na takich samych zasadach jak dzielenie i mnożenie wartości skalarnych np. 5
  m / 5 s = 1 m*s^-1 lub 1 m*s^-1 * 10 s = 10 m)
- Mnożenie takich samych jednostek sprawia, że jednostka podnoszona jest do potęgi np. 5 m * 5 m = 25 m^2
- Dzielenie takich samych jednostek sprawia, że jednostka jest redukowana np. 10 m / 10 m = 1

#### Zapis jednostek

Zastosowany został zapis w stylu `unit N: [kg*m*s^-2]` a nie `unit N: [kg*m/s^2]`, ponieważ jednostki fizyczne bardzo
często opisywane są przy pomocy tylko identyfikatorów, operacji mnożenia oraz potęgowania - z racji, że dzielenie można
zastąpić ujemną potegą.
[Odnośnik](https://pl.wikipedia.org/wiki/Stałe_fizyczne)

