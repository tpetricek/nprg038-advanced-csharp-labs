interface IIncrement {
  inc(): void;
}
interface INumeric {
  value(): number;
}
class ColouredNumber implements IIncrement, INumeric {
  val: number;
  colour: string;
  inc() { this.val++; }
  value() { return this.val; }
  constructor(v: number, c: string) {
    this.val = v; this.colour = c;
  }
}

function incAndPrint(cn: IIncrement & INumeric) {
  cn.inc();
  console.log(cn.value());
}

incAndPrint(new ColouredNumber(5, "red"));

