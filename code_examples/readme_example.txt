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