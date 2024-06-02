import { Button } from "@/components/ui/button";

const Navbar =()=>{
    return (
            <div className="flex max-w-1196px h-30px mt-6 justify-between md:px-32 sm:px-10 bg-background">
                <div className="font-bold text-xl leading-7 h-30px flex items-center text-foreground cursor-pointer">ONYX</div>
                <Button variant="link" className="h-30px font-bold text-xl leading-7 flex items-center text-foreground w-20">Sign in</Button>                 
            </div>
    )}
export default Navbar;