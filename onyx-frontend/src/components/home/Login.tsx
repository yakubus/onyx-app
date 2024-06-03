import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
  } from "@/components/ui/dialog"
  

  const Login = () => {
    return (
        <Dialog>
        <DialogTrigger asChild>
          <Button variant="outline" className="w-56 h-16 rounded-full mt-16 font-semibold text-base mx-auto lg:mx-0">Login</Button>
        </DialogTrigger>
        <DialogContent className="sm:max-w-[450px]">
          <DialogHeader className="divide-y-2">
            <DialogTitle className="text-center font-bold text-3xl my-2">Sign In</DialogTitle>
            <DialogDescription>
                <p className="mb-4 mt-6 text-foreground">Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae perferendis labore.</p>
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4">
            <div className="grid grid-cols-4 items-center gap-4">             
              <Input id="name" value="login" className="col-span-4 rounded-lg h-12 text-foreground border-black " />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Input id="username" value="password" className="col-span-4 rounded-lg h-12 text-foreground border-black " />
            </div>
          </div>
          <DialogFooter>                       
            <Button type="submit" className="w-full h-14 rounded-full font-semibold text-base mx-auto">Sign in</Button>                                
          </DialogFooter>
        </DialogContent>
      </Dialog>
    );
};
export default Login;