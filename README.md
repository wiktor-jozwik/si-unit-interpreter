# Autor
&copy; *Wiktor Jóźwik, Politechnika Warszawska 2022* 


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
  - **[]** - brak jednostki

- Zmienne skalarne, napisy lub booleany
  - `let x: [] = 5`
  - `let y: [] = 5.2`
  - `let z: string = "my string"`
  - `let w: bool = true`
  - `let q: bool = false`

- Zmienne korzystające z jednostki SI
  - `let duration: [s] = 5`
  - `let mass: [kg] = 10`

- Zmienne tworzone przy użyciu złożonych jednostek SI
  - `let speed: [m*s^-1] = 10`
  - `let force: [kg*m*s^-2] = 270`

- Zmienne tworzone przy użyciu zdefiniowanych jednostek przez użytkownika
  ``` 
  unit N: [kg*m*s^-2]
  let force: [N] = 270
  ```

- Przykłady operacji na zmiennych z typami jednostek
  - ```
    let t1: [s] = 5
    let s1: [m] = 12
    let v1: [m*s^-1] = length / duration
    let v2: [m*s^-1] = 10
    let deltaV = v2 - v1
    ```
  - ```
    unit J: [kg*m^2*s^-2]
      
    let energy: [J]
    let speed: [m*s^-1]
    let mass: [kg] = 10
    let duration: [s] = 10
    let distance: [m] = 20
      
    let speed: [m*s^-1] = length / duration
    let energy: [J] = mass * speed^2 / 2

    ```
### Instrukcje warunkowe
- ```
    let time: [s] = 10
    let length: [m] = 50
    let speed: [m*s^-1] = length / speed
    
    if (speed >  5: [m*s^-1]) {
      code block
    } else if (speed <= 0: [m*s^-1]) {
      code block 
    } else {
      code block
    }
    ```

### Pętla while:
  - ```
    let i: [] = 10
  
    while (i > 0) {
      let speed: [m*s^-1] = 10 * i
      print(speed)
      i = i-1
    }
    ```
  
### Funkcje
  - Użycie funkcji z wykorzystaniem argumentów z jednostkami SI oraz zmiennej skalarnej
    ```
    fn calculateVelocityData(v1: [m*s^-1], v2: [m*s^-1], scalar: []) -> [m*s^-1] {
      return (v2-v1) * scalar
    }
    ```
  - Użycie funkcji z resztą zmiennych
    ```
    fn printMass(m: [kg], shouldPrintMass: bool, printText: string) -> void {
      if (shouldPrintMass) {
        print(printText)
        print(m)
      }
    }
    ```
  - Generalna składnia funkcji:
    ```
    fn functionName(arg1: unit, arg2, arg3, ...) -> unit/void/string/bool {
      code block
      return ... // if not void
    }
    ```
  
### Komentarze
- // użycie komentarza w kodzie


### Przykład użycia zawierający wszystkie elementy języka:
- ```
  unit N: [kg*m*s^-2]
  let G: [N*m^2*kg^-2] = 6.6732e-11
  
  fn calculateGForce(m1: [kg], m2: [kg], distance: [m]) -> [N] {
    return G * earthMass * sunMass / earthSunDistance^2
  }
  
  fn printGForceInLoop(gForce: [N], i: [], shouldPrint: bool, printText: string) -> void {
    if (shouldPrint) {
      while (i > 0) {
        print(i)
        print(printText)
        print(gForce)
        i = i-1
      }
    }
  }
  
  
  let earthMass: [kg] = 5.9722e24
  let sunMass: [kg] = 1.989e30
  let earthSunDistance: [m] = 149.24e9
  
  let gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)
  
  let i: [] = 10
  let shouldPrint: bool = true
  let printText: string = "GForce is: "
  
  printGForceInLoop(gForce, i, shouldPrint, printText)
  ```

### Niewłaściwe użycia
- ```
  let duration: [s] = 5
  let length: [m] = 10
    
  let speed: [m*s^-1] = duration / length // Type error: [m*s^-1] unit does not match [s/m].
  ```
- ```
  let s1: [m] = 20
  let t1: [s] = 10
    
  let v: [] = s1 - t1 // Operation error: cannot add [m] and [s]. Its possible to add/subtract only same units.
  ```
- ```
  fn calculateVelocityDelta(v1, v2) {
    return v2 - v1 
  } // Syntax error: v1, v2 and return types are not specified.
  ```
- ```
  fn calculateVelocityDelta(v1: [m*s^-1], v2: [m*s^-1]) -> [m*s^-1] {
    return v2 / v1 // Type error: [] unit does not match with required return type [m*s^-1].
  }
  ```
- ```
  let v: [m*s^-1] = 10
    
  if (v > 5: []) {
    code block
  } // Operation error: cannot compare [m*s^-1] and [].
  ```
- ```
  let v: [m*s^-1] = 10
  if (v > 5) {
    code block
  } // Syntax error: 5 has no type specified
  ```
- ```
  let v1: [m*s^-1] = 5
  let v2: [m*s^-1] = 10
  let speed = v1 / v2 // Syntax error: variable speed does not have type specified.
  ```
- ```
  let x: [] = 5
  let mass: [kg] = 10
  let y: [] = x + mass // Operation error: cannot add [] and [kg]. Its possible to add/subtract only same units.
  ```
- ```
  let x: string = "abc"
  let y: [] = 5
  let z: [] = x + y // Operation error: cannot add string and []
  ```

    
### Operacje na jednostkach
  - Dodawanie i odejmowanie nie wpływa na jednostki 5 m*s^-1 + 5 m*s^-1 = 10 m*s^-1. Nie można odejmować ani dodawać zmiennych z różnymi jednostkami.
  - Mnożenie jednostek sprawia, że jednostka podnoszona jest do potęgi np. 5 m * 5 m = 25 m^2
  - Dzielenie jednostek sprawia, że jednostka jest redukowana np. 10 m / 10 m = 1
  - Potęgowanie zmiennej z jednostką ze zmienną skalarną np. (5 m)^3 = 125 m^3

### O jednostakch słów kilka

Zastosowałem zapis w stylu `unit N: [kg*m*s^-2]` a nie `unit N: [kg*m/s^2]`, ponieważ jednostki fizyczne bardzo często opisywane są przy pomocy
tylko identyfikatorów, operacji mnożenia oraz potęgowania - z racji, że dzielenie można zastąpić ujemną potegą, dlatego
ja również zdecydowałem się na taki krok. [Odnośnik](https://pl.wikipedia.org/wiki/Stałe_fizyczne)

## Formalny opis gramatyki

https://docs.python.org/3/reference/grammar.html
https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/grammar
https://github.com/frenchy64/typescript-parser/blob/master/typescript.ebnf
https://kotlinlang.org/docs/reference/grammar.html#expression
```

--- LITERALS

literal = bool_literal | num_literal | string_literal

bool_literal = "true" | "false"

num_literal = int_literal | float_literal

int_literal = decimal_digits

float_literal = decimal_digits, '.', decimal_digits, [ float_exponent ]
                | decimal_digits, float_exponent

float_exponent = "e", [ "-" ], decimal_digits

decimal_digits = digit, { digit }

digit = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"

string_literal = """, string_literal_character, { string_literal_character } """

string_literal_character = character | escape_character

character = ~["\\\u000D\u000A\u0085\u2028\u2029]     // anything but ", \, and New_Line_Character: : '\u000D' - carriage return, '\u000A' - line feed, '\u0085' - next line, '\u2028' - line separator, '\u2029' - paragraph separator

escape_character =  "\"" | "\n" | "\t" | "\\";

comment = "//", { character }

--- STATEMENTS, KEYWORDS

statement = assign_statement | unit_declaration 
            | function_call | return_statement 
            | function_statement | if_statement 
            | while_statement
            
          
- return_statement = "return", expression

- if_statement = "if", "(" expression, ")", block, [ "else", if_statement_or_block]

if_statement_or_block = if_statement | block
                
- while_statement = "while", "(" expression, ")" block

block = "{" statements "}"
            
statements = statement, { statement }

keyword = "let" | "string" | "bool" | "unit" | 
          "fn" | "if" | "else" | "while" | "void" | "return" | "true" | "false";


--- EXPRESSION

expression = logic_factor, { "||", logic_factor };

logic_factor = relation, { "&&" relation }

relation = [ "!" ], bool_literal | expression_comparison

expression_comparison = additive_expression, comparison_operator, additive_expression

comparison_operator = ">" | "<" | ">=" | "<=" | "==" | "!="

additive_expression = multiplicative_expression, { additive_operator, multiplicative_expression }

additive_operator = "+" | "-"

multiplicative_expression = unary_expression, { multiplicative_operator, unary_expression }

multiplicative_operator = "*" | "/"

unary_expression = [ negate ], num_literal | identifier_or_function_call | "(" expression ")"

negate = "!" | "-"

identifier_or_function_call = identifier, [ "(", args, ")" ]

- function_call = identifier, "(", args, ")"

args = [ arg, { ",", arg } ]

arg = expression


--- ASSIGNMENT VARIABLES

- assign_statement = "let", parameter, "=" expression

parameter = identifier, ":", variable_type

identifier = letter_character, { identifier_character } 

identifier_character = letter_character | "_" | digit

letter_character = [a-zA-Z]


--- VARIABLE TYPES

variable_type = unit_type | literal_type

literal_type = "string" | "bool"

void_type = "void"


--- UNIT 

- unit_declaration = "unit", identifier, ":" unit_type

unit_type = "[" unit_value "]"

unit_value = [ unit_expression ]

unit_expression = identifier, [ unit_power ],  [ "*", unit_expression ]

unit_power = "^", [ "-" ], decimal_digits

--- FUNCTION

- function_statement = "fn", "(", parameters, ")", "->", return_type, block

parameters = [ parameter, { ",", parameter } ]

return_type = variable_type | void_type
```

## Opis techniczny realizacji

### Język oraz framework
C# .NET 6


### Opis uruchomienia
`dotnet run nazwa_pliku`

np.

`dotnet run code_examples/gravity.txt`

### Opis testowania
- Zależności służące do uruchamiania testów: **xUnit**, **Microsoft.NET.Test.Sdk**, **xunit.runner.visualstudio**
- Katalog, w którym będą znajdować się testy: **spec/**
- Aby uruchomić testy w należy uruchomić: **dotnet test**