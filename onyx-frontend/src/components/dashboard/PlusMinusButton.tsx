import { Dispatch, FC, SetStateAction } from "react";

import { Minus, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";

interface PlusMinusButtonProps {
  state: "+" | "-";
  setState: Dispatch<SetStateAction<"+" | "-">>;
}

const PlusMinusButton: FC<PlusMinusButtonProps> = ({ state, setState }) => {
  const handleClick = () => {
    setState((prev) => (prev === "+" ? "-" : "+"));
  };

  return (
    <Button
      type="button"
      onClick={handleClick}
      variant={state === "-" ? "destructive" : "default"}
      size="sm"
      className="h-10"
    >
      {state === "+" ? <Plus /> : <Minus />}
    </Button>
  );
};

export default PlusMinusButton;
