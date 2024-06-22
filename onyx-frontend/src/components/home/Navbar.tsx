import LoginButton from "@/components/home/LoginButton";
import Brand from "@/components/Logo";

const Navbar = () => {
  return (
    <div className="max-w-1196px mt-6 flex h-30px justify-between bg-background  px-4 md:px-32">
      <div className="flex h-30px cursor-pointer items-center mt-4">
        <Brand className="text-foreground "/>
      </div>
      <div className="z-30">
        <LoginButton />
      </div>
      
    </div>
  );
};
export default Navbar;