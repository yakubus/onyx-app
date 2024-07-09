import { FC } from "react";

import { Minus, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";

interface PlusMinusButtonProps {
  state: "+" | "-";
  setState: (state: "+" | "-") => void;
}

const PlusMinusButton: FC<PlusMinusButtonProps> = ({ state, setState }) => {
  const handleClick = () => {
    setState(state === "+" ? "-" : "+");
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
