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

let frameTime = 1000.0 / 60.0

let applyValue (value: float) (diff: float) (time: float) : float =
  value + diff * (time /frameTime)

let applyPointValue (v: Point) (diff: Point) (time: float) : Point =
  let x = applyValue v.x diff.x time
  let y = applyValue v.y diff.y time
  {x = x; y = y}

let getNewPointField (field: Field<Point>) (data: Point) (time: float) : Field<Point> =
  {field with func = {data = data; time = time}; value = (applyPointValue field.value field.func.data time)}


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