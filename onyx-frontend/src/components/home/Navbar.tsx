import LoginButton from "@/components/home/LoginButton";

const Navbar = () => {
  return (
    <div className="max-w-1196px mt-6 flex h-30px justify-between bg-background sm:px-10 md:px-32">
      <div className="flex h-30px cursor-pointer items-center text-xl font-bold leading-7 text-foreground">
        ONYX
      </div>
      <LoginButton />
    </div>
  );
};
export default Navbar;
