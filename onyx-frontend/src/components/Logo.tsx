import { cn } from "@/lib/utils";

const Logo = ({ className }: { className?: string }) => {
  return (
    <div
      className={cn(
        "flex flex-col items-center justify-center text-primary-foreground w-52px",
        className,
      )}
    >
      <svg
        id="logo-72"
        width="52"
        height="44"
        viewBox="0 0 53 44"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
        className="fill-current"
      >
        <path d="M23.2997 0L52.0461 28.6301V44H38.6311V34.1553L17.7522 13.3607L13.415 13.3607L13.415 44H0L0 0L23.2997 0ZM38.6311 15.2694V0L52.0461 0V15.2694L38.6311 15.2694Z"></path>
      </svg>
      <div className="w-52px text-center font-bold tracking-widest">ONYX</div>
    </div>
  );
};

export default Logo;
