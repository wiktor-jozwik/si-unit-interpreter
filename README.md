## Autor
*Wiktor Jóźwik, Politechnika Warszawska 2022*

## Język, framework i sposób realizacji
C# .NET 6

## Opis zakładanej funkcjonalności i przykłady użycia

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
  - `let mass [kg] = 10`

- Zmienne tworzone przy użyciu złożonych jednostek SI
  - `let speed: [m/s] = 10`
  - `let force: [kg*m/s^2] = 270`

- Zmienne tworzone przy użyciu zdefiniowanych jednostek przez użytkownika
  ``` 
  unit N: [kg*m/s^2]
  let force: [N] = 270
  ```

- Przykłady operacji na zmiennych z typami jednostek
  - ```
    let t1: [s] = 5
    let s1: [m] = 12
    let v1: [m/s] = length / duration
    let v2: [m/s] = 10
    let deltaV = v2 - v1
    ```
  - ```
    unit J: [kg*m^2/s^2]
      
    let energy: [J]
    let speed: [m/s]
    let mass: [kg] = 10
    let duration: [s] = 10
    let distance: [m] = 20
      
    let speed: [m/s] = length / duration
    let energy: [J] = mass * speed^2 / 2

    ```
### Instrukcje warunkowe
- ```
    let time: [s] = 10
    let length: [m] = 50
    let speed: [m/s] = length / speed
    
    if (speed >  5: [m/s]) {
      code block
    } else if (speed <= 0: [m/s]) {
      code block 
    } else {
      code block
    }
    ```

### Pętla while:
  - ```
    let i: [] = 10
  
    while (i > 0) {
      let speed: [m/s] = 10 * i
      print(speed)
      i -= 1
    }
    ```
  
### Funkcje
  - Użycie funkcji z wykorzystaniem argumentów z jednostkami SI oraz zmiennej skalarnej
    ```
    fn calculateVelocityData(v1: [m/s], v2: [m/s], scalar: []) -> [m/s] {
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
  unit N: [kg*m/s^2]
  num G: [N*m^2/kg^2] = 6.6732e-11
  
  fn calculateGForce(m1: [kg], m2: [kg], distance: [m]) -> [N] {
  return G * earthMass * sunMass / earthSunDistance^2
  }
  
  fn printGForceInLoop(gForce: [N], i: [], shouldPrint: bool, printText: string) -> void {
    if (shouldPrint) {
      while (i > 0) {
        print(i)
        print(printText)
        print(gForce)
        i -= 1
      }
    }
  }
  
  
  num earthMass: [kg] = 5.9722e24
  num sunMass: [kg] = 1.989e30
  num earthSunDistance: [m] = 149.24e9
  
  num gForce: [N] = calculateGForce(earthMass, sunMass, earthSunDistance)
  
  let i: [] = 10
  let shouldPrint: bool = true
  let printText: string = "GForce is: "
  
  printGForceInLoop(gForce, i, shouldPrint, printText)
  ```

### Niewłaściwe użycia
- ```
  let duration: [s] = 5
  let length: [m] = 10
    
  let speed: [m/s] = duration / length // Type error: [m/s] unit does not match [s/m].
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
  fn calculateVelocityDelta(v1: [m/s], v2: [m/s]) -> [m/s] {
    return v2 / v1 // Type error: [] unit does not match with required return type [m/s].
  }
  ```
- ```
  let v: [m/s] = 10
    
  if (v > 5: []) {
    code block
  } // Operation error: cannot compare [m/s] and [].
  ```
- ```
  let v: [m/s] = 10
  if (v > 5) {
    code block
  } // Syntax error: 5 has no type specified
  ```
- ```
  let v1: [m/s] = 5
  let v2: [m/s] = 10
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
  - Dodawanie i odejmowanie nie wpływa na jednostki 5 m/s + 5 m/s = 10 m/s. Nie można odejmować ani dodawać zmiennych z różnymi jednostkami.
  - Mnożenie jednostek sprawia, że jednostka podnoszona jest do potęgi np. 5 m * 5 m = 25 m^2
  - Dzielenie jednostek sprawia, że jednostka jest redukowana np. 10 m / 10 m = 1
  - Potęgowanie zmiennej z jednostką ze zmienną skalarną np. (5 m)^3 = 125 m^3


## Gramatyka języka

## Wymagania funkcjonalne


## Opis uruchomienia
`dotnet run nazwa_pliku`

np.

`dotnet run code_examples/gravity.txt`

## Opis testowania
- Zależności służące do uruchamiania testów: **xUnit**, **Microsoft.NET.Test.Sdk**, **xunit.runner.visualstudio**
- Katalog, w którym będą znajdować się testy: **spec/**
- Aby uruchomić testy w należy uruchomić: **dotnet test**