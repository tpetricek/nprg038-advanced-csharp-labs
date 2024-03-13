type [<Measure>] row
type [<Measure>] col

let row = 1<row>
let col = 5<col>

let lookup (r:int<row>) (c:int<col>) = 
  if r = 1<_> && c = 5<_> then 100
  else 0

lookup row col
lookup col row
