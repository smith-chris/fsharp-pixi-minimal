module State

type Point = {
  x: float
  y: float
}

type FieldFunc<'a> = {
  data: 'a
  time: float
}

type Field<'a> = {
  value: 'a
  func: FieldFunc<'a>
}

let applyValue (value: float) diff strength =
  value + diff * strength

// export const applyValue = <T>(value: T, diff: T, strength: number): T => {
//   if (typeof value === 'number' && typeof diff === 'number') {
//     // @ts-ignore
//     return value + diff * strength
//   } else if (typeof value === 'object') {
//     // @ts-ignore
//     value = { ...value }
//     for (const key in value) {
//       if (diff[key]) {
//         value[key] = applyValue(value[key], diff[key], strength)
//       }
//     }
//   }
//   return value
// }